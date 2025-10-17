using System;
using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using UserService.Features.CreateUserProfile.DTO;
using UserService.Infrastructure;
using UserService.Models;

namespace UserService.Features.CreateUserProfile;

public record CreateUserProfileCommand(CreateUserProfileRequest Request) : IRequest<Result<Guid>>;

public class CreateUserProfileHandler : IRequestHandler<CreateUserProfileCommand, Result<Guid>>
{
    private readonly UserDbContext _userDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public CreateUserProfileHandler(UserDbContext userDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<Guid>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            var keycloakUserId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            if(keycloakUserId == null) 
            {
                return Result<Guid>.Failure(Error.Unauthorized(ErrorCode.Unauthorized,
                    "User is not authenticated or token is invalid.",
                    "Authentication required"));
            }
            if (request.Request is null)
            {
                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed,
                    "Request payload is required.",
                    "Request payload is required"));
            }

            if (request.Request.FullName is null || string.IsNullOrWhiteSpace(request.Request.FullName))
            {
                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed,
                    "Full name is required.",
                    "Full name is required"));
            }

            if (request.Request.PublicName is null || string.IsNullOrWhiteSpace(request.Request.PublicName))
            {
                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed,
                    "Public name is required.",
                    "Public name is required"));
            }

            var user = await _userDbContext.Users
                .FirstOrDefaultAsync(u => u.KeycloakUserId == keycloakUserId, cancellationToken);

            if (user == null)
            {
                return Result<Guid>.Failure(Error.Unauthorized(ErrorCode.UserNotFound,
                    $"User dont exists", "User dont exists"));
            }

            user.FullName = new PrivacySetting
            {
                Value = request.Request.FullName,
                WhoCanSee = PrivacyLevel.Private
            };

            user.PublicName = new PrivacySetting
            {
                Value = request.Request.PublicName,
                WhoCanSee = PrivacyLevel.Public
            };
            user.KeycloakUserId = keycloakUserId;
            user.HeaderImageUrl = String.Empty;
            user.ProfilePictureUrl = String.Empty;
            user.WorkedOn = new List<Guid>();
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userDbContext.SaveChangesAsync(cancellationToken);
            if (result <= 0)
            {
                return Result<Guid>.Failure(Error.Unexpected(ErrorCode.UnknownError,
                    "An error occurred while processing your request", null, null));
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
