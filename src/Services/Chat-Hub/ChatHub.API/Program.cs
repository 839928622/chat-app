using Application.Extensions;
using ChatHub.API.Middleware;
using ChatHub.API.SignalR;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
var services=builder.Services.AddSwaggerGen();
var configuration = builder.Configuration;
services.AddControllers();
services.AddSignalR();

services.AddApplicationServices();
services.AddInfrastructureServices(configuration);

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
}
app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
app.Run();

