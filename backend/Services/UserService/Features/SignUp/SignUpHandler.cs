using MediatR;
using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using UserService.Infrastructure;
using UserService.Infrastructure.Keycloak;
using UserService.Models;

namespace UserService.Features.CreateUser;

public record SignUpCommand(string Email) : IRequest<Result<Guid>>;

public class SignUpHandler : IRequestHandler<SignUpCommand, Result<Guid>>
{
    private readonly UserDbContext _userDbContext;
    private readonly IKeycloakAdminService _keycloakAdminService;

    public SignUpHandler(UserDbContext userDbContext, IKeycloakAdminService keycloakAdminService)
    {
        _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
        _keycloakAdminService = keycloakAdminService ?? throw new ArgumentNullException(nameof(keycloakAdminService));
    }

    public async Task<Result<Guid>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed, "Email is required.",
                    "Email is required"));
            }

            var email = request.Email.Trim().ToLowerInvariant();

            var isEmailVerifiedResult = await _keycloakAdminService.IsEmailVerified(email);
            if (isEmailVerifiedResult.IsSuccess && isEmailVerifiedResult.Value)
            {
                return Result<Guid>.Failure(Error.Conflict(ErrorCode.UserAlreadyExists,
                    $"User with email {email} already exists and is verified.",
                    "Account for this email already exists"));
            }

            var isEmailExistsAndUnverifiedResult = await _keycloakAdminService.IsEmailExistsAndUnverified(email);
            if (isEmailExistsAndUnverifiedResult.IsSuccess && isEmailExistsAndUnverifiedResult.Value)
            {
                var resendResult = await _keycloakAdminService.ResendVerificationEmailAsync(email);
                if (resendResult.IsFailure)
                {
                    return Result<Guid>.Failure(resendResult.Error!);
                }

                var existingUser = await _userDbContext.Users.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

                if (existingUser != null)
                {
                    return Result<Guid>.Success(existingUser.Id);
                }

                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed,
                    "Verification email has been resent. Please check your inbox.",
                    "Verification email has been resent"));
            }

            var createKeycloakUserResult = await _keycloakAdminService.CreateUserAsync(email);
            if (createKeycloakUserResult.IsFailure)
            {
                return Result<Guid>.Failure(createKeycloakUserResult.Error!);
            }

            var keycloakUserId = createKeycloakUserResult.Value;

            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                KeycloakUserId = keycloakUserId,
                Email = email,
                FullName = null,
                PublicName = null,
                HeaderImageUrl = string.Empty,
                ProfilePictureUrl = string.Empty,
                WorkedOn = new List<Guid>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _userDbContext.Users.Add(user);
            var result = await _userDbContext.SaveChangesAsync(cancellationToken);

            if (result <= 0)
            {
                return Result<Guid>.Failure(Error.Unexpected(ErrorCode.UnknownError,
                    "An error occurred while creating user record", null, null));
            }

            return Result<Guid>.Success(user.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<Guid>.Failure(Error.Unexpected(ErrorCode.UnknownError,
                "An error occurred while processing your request", null, null));
        }
    }
}