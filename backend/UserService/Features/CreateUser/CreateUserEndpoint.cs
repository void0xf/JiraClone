using Carter;
using MediatR;
using SharedKernel;
using UserService.Features.CreateUser.DTO;

namespace UserService.Features.CreateUser;

public class CreateUserEndpoint : CarterModule
{
    public CreateUserEndpoint() : base("api/v1")
    {
        this.RequireAuthorization();
    }

    public  override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user", async (ISender sender) =>
        {
            var result = await sender.Send(new CreateUserCommand());
            if (!result.IsSuccess)
            {
                return result.ToApiResponse().ToCreatedResult($"/user/{Guid.Empty}");
            }

            return result.ToApiResponse().ToCreatedResult($"/user/{result.Value}");


        });
    }
}