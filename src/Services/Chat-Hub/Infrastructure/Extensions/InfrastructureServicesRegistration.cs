using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Infrastructure.EventBusConsumers;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Token;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.MQ.RemoveCacheByKey;
using StackExchange.Redis;


namespace Infrastructure.Extensions
{
  public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ChatAppContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ChatApp"))
                , ServiceLifetime.Scoped); // default ServiceLifetime is Scoped

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUserPhotoRepository, UserPhotoRepository>();

            // identity service
            services.AddIdentityService(configuration);


            services.AddTransient<ITokenService, TokenService>();

            // redis distributed cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration
                    .GetConnectionString("RedisConnectionString");
            });
            // StackExchange.Redis,business usage
            services.AddSingleton<IConnectionMultiplexer, ConnectionMultiplexer>(options =>
            {
                var configurationString = ConfigurationOptions
                    .Parse(configuration.GetConnectionString("RedisConnectionString"));
                return ConnectionMultiplexer.Connect(configurationString);
            });
            // MassTransit.AspNetCore
            services.AddMassTransit(options =>
            {
                options.UsingRabbitMq((context, configure) =>
                {
                    configure.Host(configuration.GetConnectionString("EventBusHostAddress"));

                    configure.ReceiveEndpoint(nameof(RemoveUserCache),config =>
                    {
                        config.ConfigureConsumer<RemoveUserCacheConsumer>(context);
                    });
                });
                // consumers
                options.AddConsumer<RemoveUserCacheConsumer>();

            });
            return services;
        }
    }
}
