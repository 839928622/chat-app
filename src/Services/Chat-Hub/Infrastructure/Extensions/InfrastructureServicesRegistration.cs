using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Token;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;


namespace Infrastructure.Extensions
{
  public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<ChatAppContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ChatApp")));

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
            return services;
        }
    }
}
