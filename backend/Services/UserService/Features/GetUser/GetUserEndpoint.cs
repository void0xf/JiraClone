using Carter;
using MediatR;
using SharedKernel;
using UserService.Features.GetUser.DTO;

namespace UserService.Features.GetUser;

public class GetUserEndpoint : CarterModule
{
    public GetUserEndpoint() : base("api/v1")
    {
        this.RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/user/{userId}", async (Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetUserQuery(userId));
            return result.ToApiResponse();
        });
    }
} 