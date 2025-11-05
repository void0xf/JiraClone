using System;
using Carter;
using MediatR;
using SharedKernel;
using UserService.Features.CreateUser.DTO;

namespace UserService.Features.CreateUser;

public class SignUpEndpoint : CarterModule
{
    public SignUpEndpoint() : base("api/v1")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/signup", async (CreateUserRequest request, ISender sender) =>
        {
            if (request is null)
            {
                var failure = Result<CreateUserResponse>.Failure(Error.Validation(ErrorCode.ValidationFailed,
                    "Request payload is required.", "Request payload is required"));
                return failure.ToApiResponse().ToMinimalApiResult();
            }

            var result = await sender.Send(new SignUpCommand(request.Email));
            return result.ToApiResponse().ToMinimalApiResult();
        }).AllowAnonymous();
    }
}