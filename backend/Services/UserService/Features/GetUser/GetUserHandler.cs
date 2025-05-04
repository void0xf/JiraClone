using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using UserService.Features.GetUser.DTO;
using UserService.Infrastructure;
using UserService.Models;

namespace UserService.Features.GetUser;

public record GetUserQuery(Guid UserId) : IRequest<Result<GetUserResponse>>;

public class GetUserHandler : IRequestHandler<GetUserQuery, Result<GetUserResponse>>
{
    private readonly UserDbContext _userDbContext;

    public GetUserHandler(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
    }
    
    public async Task<Result<GetUserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userDbContext.Users
                .AsNoTracking()
                .Include(user => user.FullName)
                .Include(user => user.PublicName)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
                
            if (user == null)
            {
                return Result<GetUserResponse>.Failure(
                    Error.NotFound(ErrorCode.NotFound, $"User with ID {request.UserId} does not exist"));
            }
            
            var response = new GetUserResponse
            {
                Id = user.Id,
                FullName = MapToPrivacySettingDto(user.FullName),
                PublicName = MapToPrivacySettingDto(user.PublicName),
                WorkedOn = user.WorkedOn,
                Email = user.Email,
                HeaderImageUrl = user.HeaderImageUrl,
                ProfilePictureUrl = user.ProfilePictureUrl,
            };
            
            return Result<GetUserResponse>.Success(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<GetUserResponse>.Failure(
                Error.Unexpected(ErrorCode.UnknownError, "An error occurred while processing your request", null, null));
        }
    }
    
    private PrivacySettingDto MapToPrivacySettingDto(PrivacySetting setting)
    {
        if (setting == null)
            return null;
            
        return new PrivacySettingDto
        {
            Value = setting.Value,
            WhoCanSee = setting.WhoCanSee
        };
    }
} 