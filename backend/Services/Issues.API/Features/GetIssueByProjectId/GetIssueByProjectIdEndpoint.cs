using Carter;
using Issues.API.Features.GetIssueByProjectId.DTO;
using MediatR;
using SharedKernel;

namespace Issues.API.Features.GetIssueByProjectId
{
    public class GetIssueByProjectIdEndpoint : CarterModule
    {
        public GetIssueByProjectIdEndpoint() : base("api/v1") { }

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/issue/project/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new GetIssueByProjectIdQuery(id));

                if (result.IsSuccess)
                {
                    var issuesDto = result.Value.Issues.Select(issue => new IssueDto(
                        issue.Id,
                        issue.Key,
                        issue.ProjectId,
                        issue.IssueType,
                        issue.ParentIssueId,
                        issue.Summary,
                        issue.Description,
                        issue.Status,
                        issue.Priority,
                        issue.AssigneeId,
                        issue.ReporterId,
                        issue.SprintId,
                        issue.Labels.Select(l => new LabelDto(l.Id, l.Name)).ToList(),
                        issue.EstimatedStoryPoints,
                        issue.IsArchived,
                        issue.InBacklog,
                        issue.DueDate,
                        issue.CreatedAt,
                        issue.UpdatedAt
                    )).ToList();

                    return ApiResponse<GetIssueByProjectIdResponse>.
                        Success(new GetIssueByProjectIdResponse(issuesDto)).ToMinimalApiResult();
                }

                return result.ToApiResponse().ToMinimalApiResult();

               
            })
            .WithName("GetIssuesByProjectId") // Name for the endpoint
            .Produces<ApiResponse<GetIssueByProjectIdResponse>>(StatusCodes.Status200OK) // Successful response type
            .ProducesProblem(StatusCodes.Status404NotFound) // Error response type
            .WithSummary("Get Issues By Project ID"); // Summary for the endpoint
        }
    }
}
