using MediatR;
using System;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using UserService.Infrastructure;
using UserService.Infrastructure.Keycloak;
using UserService.Models;

namespace UserService.Features.CreateUser;

public record CreateUserCommand(string Email) : IRequest<Result<Guid>>;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly UserDbContext _userDbContext;
    private readonly IKeycloakAdminService _keycloakAdminService;

    public CreateUserHandler(UserDbContext userDbContext, IKeycloakAdminService keycloakAdminService)
    {
        _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
        _keycloakAdminService = keycloakAdminService ?? throw new ArgumentNullException(nameof(keycloakAdminService));
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed, "Email is required.",
                    "Email is required"));
            }

            var email = request.Email.Trim().ToLowerInvariant();

            var existingUser = await _userDbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (existingUser != null)
            {
                return Result<Guid>.Failure(Error.Conflict(ErrorCode.UserAlreadyExists,
                    $"User already exists with {existingUser.Id}.", "User already exists"));
            }

            var keycloakResult = await _keycloakAdminService.CreateUserAsync(email);
            if (keycloakResult.IsFailure)
            {
                return Result<Guid>.Failure(keycloakResult.Error!);
            }
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                KeycloakUserId = keycloakResult.Value,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _userDbContext.Users.Add(user);
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