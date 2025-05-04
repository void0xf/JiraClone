using Carter;
using Issues.API.Features.UpdateIssue.DTO;
using Issues.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Text.Json;

namespace Issues.API.Features.UpdateIssue;

public class UpdateIssueEndpoint : CarterModule
{
    public UpdateIssueEndpoint() : base("api/v1") { }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("issue/{id}", async ([FromRoute] Guid id, [FromBody] JsonElement requestBody, ISender sender) =>
        {
            // Get the properties that were actually provided in the request JSON
            var explicitlySetProperties = new HashSet<string>();
            
            // Deserialize to our request model
            var options = new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            };
            
            var request = JsonSerializer.Deserialize<UpdateIssueRequest>(requestBody.GetRawText(), options);
            
            // Record which properties were explicitly set in the JSON
            foreach (var property in requestBody.EnumerateObject())
            {
                // Convert from camelCase to PascalCase for property matching
                var pascalCaseName = char.ToUpper(property.Name[0]) + property.Name.Substring(1);
                explicitlySetProperties.Add(pascalCaseName);
            }
            
            var command = new UpdateIssueCommand(
                id,
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
                request.DueDate,
                explicitlySetProperties
            );
            
            var result = await sender.Send(command);
            
            if (result.IsSuccess)
            {
                var response = new UpdateIssueResponse(result.Value.IssueId);
                return ApiResponse<UpdateIssueResponse>.Success(response).ToMinimalApiResult();
            }
            
            return result.ToApiResponse().ToMinimalApiResult();
        })
        .WithName("UpdateIssue")
        .Produces<ApiResponse<UpdateIssueResponse>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update Issue");
    }
} 