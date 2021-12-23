using Application.Contracts.Infrastructure;
using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Token;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


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
            // identity service
            services.AddIdentityService(configuration);


            services.AddTransient<ITokenService, TokenService>();
            return services;
        }
    }
}
