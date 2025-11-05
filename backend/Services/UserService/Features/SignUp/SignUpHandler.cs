using MediatR;
using Microsoft.Extensions.Configuration;
using SharedKernel;
using System;
using UserService.Features.CreateUser.DTO;
using UserService.Infrastructure.Keycloak;

namespace UserService.Features.CreateUser;

public record SignUpCommand(string Email) : IRequest<Result<CreateUserResponse>>;

public class SignUpHandler : IRequestHandler<SignUpCommand, Result<CreateUserResponse>>
{
    private readonly IKeycloakAdminService _keycloakAdminService;
    private readonly IConfiguration _configuration;

    public SignUpHandler(IKeycloakAdminService keycloakAdminService, IConfiguration configuration)
    {
        _keycloakAdminService = keycloakAdminService ?? throw new ArgumentNullException(nameof(keycloakAdminService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<Result<CreateUserResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Result<CreateUserResponse>.Failure(Error.Validation(ErrorCode.ValidationFailed, "Email is required.",
                    "Email is required"));
            }

            var email = request.Email.Trim().ToLowerInvariant();

            var isEmailVerifiedResult = await _keycloakAdminService.IsEmailVerified(email);
            if (isEmailVerifiedResult.IsSuccess && isEmailVerifiedResult.Value)
            {
                return Result<CreateUserResponse>.Failure(Error.Conflict(ErrorCode.UserAlreadyExists,
                    $"User with email {email} already exists and is verified.",
                    "Account for this email already exists"));
            }

            var keycloakBaseUrl = _configuration["Keycloak:BaseUrl"] ?? "http://localhost:8080";
            var realm = _configuration["Keycloak:RealmName"] ?? "jira-clone";
            var clientId = _configuration["Keycloak:ClientId"] ?? "jira-clone-frontend";
            var redirectUri = _configuration["Frontend:PostRegistrationRedirect"] ?? "http://localhost:4200/jira/software/for-you";

            var username = email.Contains('@') ? email[..email.IndexOf('@')] : email;

            var registrationUrl = $"{keycloakBaseUrl}/realms/{realm}/protocol/openid-connect/registrations?" +
                                   $"client_id={Uri.EscapeDataString(clientId)}&" +
                                   $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
                                   $"response_type=code&" +
                                   $"scope=openid%20profile%20email&" +
                                   $"kc_locale=en&" +
                                   $"login_hint={Uri.EscapeDataString(username)}&" +
                                   $"username={Uri.EscapeDataString(username)}&" +
                                   $"user.attributes.username={Uri.EscapeDataString(username)}&" +
                                   $"email={Uri.EscapeDataString(email)}&" +
                                   $"user.attributes.email={Uri.EscapeDataString(email)}";

            return Result<CreateUserResponse>.Success(new CreateUserResponse(registrationUrl));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<CreateUserResponse>.Failure(Error.Unexpected(ErrorCode.UnknownError,
                "An error occurred while processing your request", null, null));
        }
    }
}