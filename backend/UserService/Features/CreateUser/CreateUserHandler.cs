using MediatR;
using SharedKernel;
using UserService.Features.CreateUser.DTO;
using UserService.Infrastructure;
using UserService.Models;
using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace UserService.Features.CreateUser;
using Microsoft.AspNetCore.Http;

public record CreateUserCommand() : IRequest<Result<Guid>>;

    
public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly UserDbContext _userDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateUserHandler(UserDbContext userDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            
            var principal = _httpContextAccessor.HttpContext?.User;
            if(principal == null)
                return Result<Guid>.Failure(Error.Conflict(ErrorCode.Forbidden, "User is not authenticated.", 
                    "User is not authenticated"));
 
            
            var keycloakUserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = principal.FindFirstValue(ClaimTypes.Email);
            var firstName = principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = principal.FindFirstValue(ClaimTypes.Surname);

            var existingUser = await _userDbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.KeycloakUserId == keycloakUserId);

            if (existingUser != null)
            {
                return Result<Guid>.Failure(Error.Conflict(ErrorCode.Conflict, 
                    $"User already exists with {existingUser.Id}.", "User already exists"));
            }
            
            var userId = Guid.NewGuid();
            var user = new User()
            {
                Id = userId,
                KeycloakUserId = keycloakUserId,
                Email = email,
                FullName = new PrivacySetting
                {
                    Value = firstName,
                    WhoCanSee = PrivacyLevel.Public
                },
                PublicName = new PrivacySetting
                {
                    Value = lastName,
                    WhoCanSee = PrivacyLevel.Public
                },
                WorkedOn = new List<Guid>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            _userDbContext.Users.Add(user);
            var result = await _userDbContext.SaveChangesAsync(cancellationToken);
            if(result <= 0)
            {
                return Result<Guid>.Failure(Error.Unexpected(ErrorCode.UnknownError, 
                    "An error occurred while processing your request", null, null ));
            }
            
            return Result<Guid>.Success(user.Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<Guid>.Failure(Error.Unexpected(ErrorCode.UnknownError, 
                "An error occurred while processing your request", null, null ));        }
    }
}