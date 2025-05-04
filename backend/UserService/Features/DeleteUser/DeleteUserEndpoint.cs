using Carter;
using MediatR;
using SharedKernel;

namespace UserService.Features.DeleteUser;

public class DeleteUserEndpoint : CarterModule
{
    public DeleteUserEndpoint() : base("api/v1")
    {
        this.RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/user", async (ISender sender) =>
        {
            var result = await sender.Send(new DeleteUserCommand());
            return result.ToApiResponse();
        });
    }
} 