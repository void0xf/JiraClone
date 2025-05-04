using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using UserService.Features.UpdateUser.DTO;
using UserService.Infrastructure;
using UserService.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace UserService.Features.UpdateUser;

public record UpdateUserCommand(UpdateUserRequest Request) : IRequest<Result<bool>>;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<bool>>
{
    private readonly UserDbContext _userDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateUserHandler(UserDbContext userDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
    
    public async Task<Result<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get user identity from token
            var principal = _httpContextAccessor.HttpContext?.User;
            if(principal == null)
                return Result<bool>.Failure(Error.Conflict(ErrorCode.Forbidden, "User is not authenticated.", 
                    "User is not authenticated"));
            
            var keycloakUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // Find user by Keycloak ID
            var user = await _userDbContext.Users
                .FirstOrDefaultAsync(u => u.KeycloakUserId == keycloakUserId, cancellationToken);
                
            if (user == null)
            {
                return Result<bool>.Failure(
                    Error.NotFound(ErrorCode.NotFound, $"User with Keycloak ID {keycloakUserId} does not exist"));
            }

            // Update user information
            user.FullName = new PrivacySetting
            {
                Value = request.Request.FullName.Value,
                WhoCanSee = request.Request.FullName.WhoCanSee
            };
            
            user.PublicName = new PrivacySetting
            {
                Value = request.Request.PublicName.Value,
                WhoCanSee = request.Request.PublicName.WhoCanSee
            };
            
            user.HeaderImageUrl = request.Request.HeaderImageUrl;
            user.ProfilePictureUrl = request.Request.ProfilePictureUrl;
            user.WorkedOn = request.Request.WorkedOn;
            user.UpdatedAt = DateTime.UtcNow;
            
            var result = await _userDbContext.SaveChangesAsync(cancellationToken);
            
            if(result <= 0)
            {
                return Result<bool>.Failure(Error.Unexpected(ErrorCode.UnknownError, 
                    "An error occurred while updating the user", null, null));
            }
            
            return Result<bool>.Success(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<bool>.Failure(
                Error.Unexpected(ErrorCode.UnknownError, "An error occurred while processing your request", null, null));
        }
    }
} 