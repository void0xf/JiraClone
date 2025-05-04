using System.Text.Json.Serialization;
using Newtonsoft.Json.Serialization;
using ProjectService.API.Models;

namespace ProjectService.API.Features.CreateProject.DTO;

public record CreateProjectRequest()
{
    
    public string Name { get; set; } 
    [JsonConverter(typeof(JsonStringEnumConverter<ProjectTemplate>))]
    public ProjectTemplate ProjectTemplate { get; set; } 
    public  string ProjectKey { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter<AccessLevel>))]
    public AccessLevel AccessLevel { get; set; }
} 