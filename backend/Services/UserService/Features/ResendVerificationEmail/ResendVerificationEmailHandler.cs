using MediatR;
using SharedKernel;
using UserService.Infrastructure.Keycloak;

namespace UserService.Features.ResendVerifyMail;

public record ResendVerificationEmailCommand(string Email) : IRequest<Result<bool>>;

public class ResendVerificationEmailHandler : IRequestHandler<ResendVerificationEmailCommand, Result<bool>>
{
    private readonly IKeycloakAdminService _keycloakAdminService;

    public ResendVerificationEmailHandler(IKeycloakAdminService keycloakAdminService)
    {
        _keycloakAdminService = keycloakAdminService ?? throw new ArgumentNullException(nameof(keycloakAdminService));
    }

    public async Task<Result<bool>> Handle(ResendVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Result<bool>.Failure(Error.Validation(ErrorCode.ValidationFailed, 
                    "Email is required.", "Email is required"));
            }

            var email = request.Email.Trim().ToLowerInvariant();

            var isEmailVerifiedResult = await _keycloakAdminService.IsEmailVerified(email);
            if (isEmailVerifiedResult.IsSuccess && isEmailVerifiedResult.Value)
            {
                return Result<bool>.Failure(Error.Conflict(ErrorCode.Conflict,
                    $"User with email {email} is already verified.",
                    "This email is already verified"));
            }

            var isEmailExistsAndUnverifiedResult = await _keycloakAdminService.IsEmailExistsAndUnverified(email);
            if (isEmailExistsAndUnverifiedResult.IsFailure)
            {
                return Result<bool>.Failure(isEmailExistsAndUnverifiedResult.Error!);
            }

            if (!isEmailExistsAndUnverifiedResult.Value)
            {
                return Result<bool>.Failure(Error.NotFound(ErrorCode.UserNotFound,
                    $"No unverified user found with email {email}",
                    "User not found or already verified"));
            }

            // Resend verification email
            var resendResult = await _keycloakAdminService.ResendVerificationEmailAsync(email);
            if (resendResult.IsFailure)
            {
                return Result<bool>.Failure(resendResult.Error!);
            }

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result<bool>.Failure(Error.Unexpected(ErrorCode.UnknownError,
                "An error occurred while processing your request", null, null));
        }
    }
}
