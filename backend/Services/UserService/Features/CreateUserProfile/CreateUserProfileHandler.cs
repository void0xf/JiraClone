using System;
using System.Collections.Generic;
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

    public CreateUserProfileHandler(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
    }

    public async Task<Result<Guid>> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Request is null)
            {
                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed, "Request payload is required.",
                    "Request payload is required"));
            }

            if (string.IsNullOrWhiteSpace(request.Request.Email))
            {
                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed, "Email is required.",
                    "Email is required"));
            }

            var email = request.Request.Email.Trim().ToLowerInvariant();

            if (request.Request.FullName is null || string.IsNullOrWhiteSpace(request.Request.FullName))
            {
                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed, "Full name is required.",
                    "Full name is required"));
            }

            if (request.Request.PublicName is null || string.IsNullOrWhiteSpace(request.Request.PublicName))
            {
                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed, "Public name is required.",
                    "Public name is required"));
            }

            var user = await _userDbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user == null)
            {
                return Result<Guid>.Failure(Error.NotFound(ErrorCode.UserNotFound,
                    $"User with email {email} does not exist", "User does not exist"));
            }

            if (user.FullName != null || user.PublicName != null)
            {
                return Result<Guid>.Failure(Error.Conflict(ErrorCode.UserAlreadyExists,
                    $"Profile already exists for user {user.Id}", "Profile already exists"));
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
