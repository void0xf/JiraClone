using Carter;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims; // For ClaimTypes


var assembly = typeof(Program).Assembly;
var builder = WebApplication.CreateBuilder(args);
var keycloakConfig = builder.Configuration.GetSection("Keycloak");

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}
builder.Services.AddCarter();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(assembly);
});
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakConfig["Authority"];
        options.Audience = keycloakConfig["Audience"];
        options.RequireHttpsMetadata = keycloakConfig.GetValue<bool>("RequireHttpsMetadata");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = keycloakConfig["Authority"],
            ValidateAudience = true,
            ValidAudience = keycloakConfig["Audience"],
            ValidateLifetime = true,
            NameClaimType = ClaimTypes.Name, 
            RoleClaimType = ClaimTypes.Role 
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapCarter();

/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.UseAuthentication(); 
app.UseAuthorization(); 


app.UseHttpsRedirection();

app.Run();
