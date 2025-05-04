using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using UserService.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace UserService.Features.DeleteUser;

public record DeleteUserCommand() : IRequest<Result<bool>>;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
{
    private readonly UserDbContext _userDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteUserHandler(UserDbContext userDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
    
    public async Task<Result<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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
            
            _userDbContext.Users.Remove(user);
            var result = await _userDbContext.SaveChangesAsync(cancellationToken);
            
            if(result <= 0)
            {
                return Result<bool>.Failure(Error.Unexpected(ErrorCode.UnknownError, 
                    "An error occurred while deleting the user", null, null));
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