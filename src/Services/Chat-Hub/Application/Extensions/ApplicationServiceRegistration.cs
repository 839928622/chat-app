﻿
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Extensions
{
   public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {


            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
