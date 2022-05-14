using System.Text.Json.Serialization;
using Application.Configurations;
using Application.Extensions;
using ChatHub.API.Filters;
using ChatHub.API.Middleware;
using ChatHub.API.SignalR;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var services=builder.Services.AddSwaggerGen();
var configuration = builder.Configuration;
// binding
services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));

services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;

});
// 


// Application
services.AddApplicationServices();
// Infrastructure
services.AddInfrastructureServices(configuration);
// PresenceTracker
services.AddSingleton<PresenceTracker>();
// swagger gen
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "ChatApp API", Version = "v1" });
    // Signalr live documentation
    options.AddSignalRSwaggerGen();
    options.OperationFilter<SwaggerSkipPropertyFilter>();
    var securitySchema = new OpenApiSecurityScheme()
    {
        Description = "JWT Auth Bearer Schema",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Type = SecuritySchemeType.Http,
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme,
        }
    };
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement()
    {
        {securitySchema, new List<string>() {JwtBearerDefaults.AuthenticationScheme} }
    };
    options.AddSecurityRequirement(securityRequirement);
   
});
//cors
services.AddCors(options =>
{
    //development cors
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowCredentials().AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
    // production  cors
    options.AddPolicy("ProdCorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowCredentials().AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
    // staging cors
    options.AddPolicy("StagingCorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowCredentials().AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
});


var app = builder.Build();

//seeding
await using var scope = app.Services.CreateAsyncScope();
var servicesScope = scope.ServiceProvider;
try
{
    var context = servicesScope.GetRequiredService<ChatAppContext>();
    var userManager = servicesScope.GetRequiredService<UserManager<AppUser>>();
    var roleManager = servicesScope.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager,context);
}
catch (Exception e)
{
    var logger = servicesScope.GetRequiredService<ILogger<Program>>();
    logger.LogError(e, "An error occurred during migration");
}



// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("DevCorsPolicy");
}
else if (app.Environment.IsStaging())
{
    app.UseCors("StagingCorsPolicy");
}
else
{
    app.UseCors("ProdCorsPolicy");
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.Run();

