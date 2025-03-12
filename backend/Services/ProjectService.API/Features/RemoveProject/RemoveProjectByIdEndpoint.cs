namespace ProjectService.API.Features.RemoveProject;

public class RemoveProjectByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("projects/{id}", async (ISender sender, Guid id) =>
        {
            var response = await sender.Send(new RemoveProjectCommand(id));
            return response;
        });
    }
}