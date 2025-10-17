using Carter;
using MediatR;
using SharedKernel;
using UserService.Features.ResendVerifyMail.DTO;

namespace UserService.Features.ResendVerifyMail;

public class ResendVerificationEmailEndpoint : CarterModule
{
    public ResendVerificationEmailEndpoint() : base("api/v1")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user/resend-verification", async (ResendVerificationEmailRequest request, ISender sender) =>
        {
            if (request is null)
            {
                var failure = Result<bool>.Failure(Error.Validation(ErrorCode.ValidationFailed,
                    "Request payload is required.", "Request payload is required"));
                return failure.ToApiResponse();
            }

            var result = await sender.Send(new ResendVerificationEmailCommand(request.Email));
            return result.ToApiResponse();
        })
        .WithName("ResendVerificationEmail")
        .WithTags("User Authentication")
        .Produces<ApiResponse<bool>>(StatusCodes.Status200OK)
        .Produces<ApiResponse<bool>>(StatusCodes.Status400BadRequest)
        .Produces<ApiResponse<bool>>(StatusCodes.Status404NotFound);
    }
}
