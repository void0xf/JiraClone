using System;
using Carter;
using MediatR;
using SharedKernel;
using UserService.Features.CreateUser.DTO;

namespace UserService.Features.CreateUser;

public class CreateUserEndpoint : CarterModule
{
    public CreateUserEndpoint() : base("api/v1")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user", async (CreateUserRequest request, ISender sender) =>
        {
            if (request is null)
            {
                var failure = Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed,
                    "Request payload is required.", "Request payload is required"));
                return failure.ToApiResponse().ToCreatedResult($"/user/{Guid.Empty}");
            }

            var result = await sender.Send(new CreateUserCommand(request.Email));
            if (!result.IsSuccess)
            {
                return result.ToApiResponse().ToCreatedResult($"/user/{Guid.Empty}");
            }

            return result.ToApiResponse().ToCreatedResult($"/user/{result.Value}");


        });
    }
}