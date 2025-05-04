using Carter;
using MediatR;
using SharedKernel;
using UserService.Features.UpdateUser.DTO;

namespace UserService.Features.UpdateUser;

public class UpdateUserEndpoint : CarterModule
{
    public UpdateUserEndpoint() : base("api/v1")
    {
        this.RequireAuthorization();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/user", async (UpdateUserRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateUserCommand(request));
            return result.ToApiResponse();
        });
    }
} 