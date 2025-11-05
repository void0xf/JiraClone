using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using UserService.Infrastructure;
using UserService.Infrastructure.Keycloak;
using UserService.Models;

namespace UserService.Features.VerifyEmailCode;

public record VerifyEmailCodeCommand(string Email, string Code) : IRequest<Result<VerifyEmailCodeResponse>>;

public record VerifyEmailCodeResponse(string KeycloakRegistrationUrl);

public class VerifyEmailCodeHandler : IRequestHandler<VerifyEmailCodeCommand, Result<VerifyEmailCodeResponse>>
{
    private readonly UserDbContext _userDbContext;
    private readonly IKeycloakAdminService _keycloakAdminService;
    private readonly IConfiguration _configuration;
    private readonly IVerificationCodeService _verificationCodeService;

    public VerifyEmailCodeHandler(UserDbContext userDbContext, IKeycloakAdminService keycloakAdminService, IConfiguration configuration, IVerificationCodeService verificationCodeService)
    {
        _userDbContext = userDbContext;
        _keycloakAdminService = keycloakAdminService;
        _configuration = configuration;
        _verificationCodeService = verificationCodeService;
    }

    public async Task<Result<VerifyEmailCodeResponse>> Handle(VerifyEmailCodeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Code))
            {
                return Result<VerifyEmailCodeResponse>.Failure(
                    Error.Validation(ErrorCode.ValidationFailed, "Email and code are required", "Invalid input"));
            }

            var email = request.Email.Trim().ToLowerInvariant();

            // Get verification code from memory
            var (storedCode, expiry) = _verificationCodeService.GetCode(email);

            // if (string.IsNullOrEmpty(storedCode))
            // {
            //     return Result<VerifyEmailCodeResponse>.Failure(
            //         Error.NotFound(ErrorCode.UserNotFound, "No verification code found for this email", "Code not found"));
            // }

            // // Check if verification code is valid
            // if (storedCode != request.Code)
            // {
            //     return Result<VerifyEmailCodeResponse>.Failure(
            //         Error.Validation(ErrorCode.InvalidVerificationCode, "Invalid verification code", "Invalid code"));
            // }

            // // Check if code has expired (valid for 15 minutes)
            // if (expiry < DateTime.UtcNow)
            // {
            //     _verificationCodeService.RemoveCode(email);
            //     return Result<VerifyEmailCodeResponse>.Failure(
            //         Error.Validation(ErrorCode.VerificationCodeExpired, "Verification code has expired", "Code expired"));
            // }

            // Find user in database
            var user = await _userDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
            {
                return Result<VerifyEmailCodeResponse>.Failure(
                    Error.NotFound(ErrorCode.UserNotFound, "User not found", "User not found"));
            }

            // Mark email as verified in our database
            user.IsEmailVerified = true;
            user.UpdatedAt = DateTime.UtcNow;

            // Mark email as verified in Keycloak
            var verifyResult = await _keycloakAdminService.VerifyUserEmail(user.KeycloakUserId);
            if (verifyResult.IsFailure)
            {
                return Result<VerifyEmailCodeResponse>.Failure(verifyResult.Error!);
            }

            await _userDbContext.SaveChangesAsync(cancellationToken);

            // Remove verification code from memory after successful verification
            _verificationCodeService.RemoveCode(email);

            // Generate Keycloak registration URL
            var keycloakBaseUrl = _configuration["Keycloak:BaseUrl"]; // http://localhost:8080
            var realm = _configuration["Keycloak:RealmName"]; // jira-clone
            var clientId = "jira-clone-frontend"; // jira-clone-frontend
            var redirectUri = "http://localhost:4200/jira/software/sign-up/profile-setup";

            // ✅ THIS IS THE URL YOU NEED
            var registrationUrl = $"{keycloakBaseUrl}/realms/{realm}/protocol/openid-connect/registrations?" +
                                $"client_id={clientId}&" +
                                $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
                                $"response_type=code&" +
                                $"scope=openid%20profile%20email&" +
                                $"login_hint={Uri.EscapeDataString(email)}";
            return Result<VerifyEmailCodeResponse>.Success(new VerifyEmailCodeResponse(registrationUrl));
        }
        catch (Exception ex)
        {
            return Result<VerifyEmailCodeResponse>.Failure(
                Error.Unexpected(ErrorCode.UnknownError, "An error occurred while verifying code", null, ex));
        }
    }
}
