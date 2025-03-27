using Carter;
using Issues.API.Features.DeleteIssue.DTO;
using MediatR;
using SharedKernel;

namespace Issues.API.Features.DeleteIssue;

public class DeleteIssueEndpoint : CarterModule
{
    public DeleteIssueEndpoint() : base("api/v1") { }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("issue/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteIssueCommand(id));
            
            if (result.IsSuccess)
            {
                var response = new DeleteIssueResponse(result.Value.IssueId);
                return ApiResponse<DeleteIssueResponse>.Success(response).ToMinimalApiResult();
            }
            
            return result.ToApiResponse().ToMinimalApiResult();
        })
        .WithName("DeleteIssue")
        .Produces<ApiResponse<DeleteIssueResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete Issue");
    }
} 