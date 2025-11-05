using Carter;
using MediatR;
using SharedKernel;

namespace UserService.Features.VerifyEmailCode;

public class VerifyEmailCodeEndpoint : CarterModule
{
    public VerifyEmailCodeEndpoint() : base("api/v1")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        // TODO in feature
        // app.MapPost("/user/verify-email-code", async (VerifyEmailCodeRequest request, ISender sender) =>
        // {
        //     if (request is null)
        //     {
        //         var failure = Result<VerifyEmailCodeResponse>.Failure(Error.Validation(ErrorCode.ValidationFailed,
        //             "Request payload is required.", "Request payload is required"));
        //         return failure.ToApiResponse();
        //     }

        //     var result = await sender.Send(new VerifyEmailCodeCommand(request.Email, request.Code));

        //     return result.ToApiResponse();
        // }).AllowAnonymous();
    }
}

public record VerifyEmailCodeRequest(string Email, string Code);
