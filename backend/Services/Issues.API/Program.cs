using System.Text.Json;
using Carter;
using Issues.API.Data;
using Issues.API.Features.CreateIssue;
using Marten;
using Microsoft.AspNetCore.Http.Json;

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IIssueRepository, IssueRepository>();

builder.Services.AddCarter();

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("DefaultConnection")!);
}).UseLightweightSessions();

var app = builder.Build();

app.MapCarter();
app.Run();