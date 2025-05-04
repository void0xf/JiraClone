using Carter;
using Issues.API.Features.GetIssueById.DTO;
using MediatR;
using SharedKernel;

namespace Issues.API.Features.GetIssueById;

public class GetIssueByIdEndpoint : CarterModule
{
    public GetIssueByIdEndpoint() : base("api/v1") { }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/issue/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetIssueByIdQuery(id));
            
            if (result.IsSuccess)
            {
                var issue = result.Value.Issue;
                
                // Map domain model to DTO
                var response = new GetIssueByIdResponse(
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
                );
                
                return ApiResponse<GetIssueByIdResponse>.Success(response).ToMinimalApiResult();
            }
            
            return result.ToApiResponse().ToMinimalApiResult();
        })
        .WithName("GetIssueById")
        .Produces<ApiResponse<GetIssueByIdResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Get Issue By ID");
    }
} 