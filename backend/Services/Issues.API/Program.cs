using System.Text.Json;
using Carter;
using Marten;
using Microsoft.AspNetCore.Http.Json;

var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("DefaultConnection")!);
}).UseLightweightSessions();

var app = builder.Build();

app.MapCarter();
app.Run();