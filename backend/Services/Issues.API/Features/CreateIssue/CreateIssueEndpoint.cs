using Carter;
using Issues.API.Features.CreateIssue.DTO;
using Issues.API.Models;
using MediatR;
using SharedKernel;

namespace Issues.API.Features.CreateIssue;

public class CreateIssueEndpoint : CarterModule
{
    public CreateIssueEndpoint() : base("api/v1") { }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("issue", async (CreateIssueRequest request, ISender sender) =>
        {
            var command = new CreateIssueCommand(
                request.Key,
                request.ProjectId,
                request.IssueType,
                request.ParentIssueId,
                request.Summary,
                request.Description,
                request.Status,
                request.Priority,
                request.AssigneeId,
                request.ReporterId,
                request.SprintId,
                request.Labels?.Select(l => new Label { Name = l.Name }).ToList(),
                request.EstimatedStoryPoints,
                request.IsArchived,
                request.InBacklog,
                request.DueDate
            );

            var result = await sender.Send(command);

            if (result.IsSuccess)
            {
                var response = new CreateIssueResponse(result.Value.IssueId, result.Value.Key);
                return ApiResponse<CreateIssueResponse>.Success(response)
                    .ToCreatedResult($"api/v1/issue/{result.Value.IssueId}");
            }

            return result.ToApiResponse().ToMinimalApiResult();
        })
        .WithName("CreateIssue")
        .Produces<ApiResponse<CreateIssueResponse>>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Issue");
    }
}