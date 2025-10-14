using System;
using Carter;
using MediatR;
using SharedKernel;
using UserService.Features.CreateUserProfile.DTO;

namespace UserService.Features.CreateUserProfile;

public class CreateUserProfileEndpoint : CarterModule
{
    public CreateUserProfileEndpoint() : base("api/v1") => RequireAuthorization();
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/profile", async (CreateUserProfileRequest request, ISender sender) =>
        {
            if (request is null)
            {
                var failure = Result<Guid>.Failure(Error.Validation(ErrorCode.ValidationFailed,
                    "Request payload is required.", "Request payload is required"));
                return failure.ToApiResponse().ToCreatedResult($"/user/{Guid.Empty}");
            }

            var result = await sender.Send(new CreateUserProfileCommand(request));
            if (!result.IsSuccess)
            {
                return result.ToApiResponse().ToCreatedResult($"/user/{Guid.Empty}");
            }

            return result.ToApiResponse().ToCreatedResult($"/user/{result.Value}");
        });
    }
}
