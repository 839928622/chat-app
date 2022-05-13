using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Application.Attributes;

namespace ChatHub.API.Filters
{/// <summary>
///  Hiding unnecessary input from the Swagger documentation
/// discussion: https://github.com/domaindrivendev/Swashbuckle.WebApi/issues/1230
/// cf. https://dejanstojanovic.net/aspnet/2019/october/ignoring-properties-from-controller-action-model-in-swagger-using-jsonignore/ 
/// </summary>
    public class SwaggerSkipPropertyFilter : IOperationFilter
    {
        
        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            
                var ignoredProperties = context.MethodInfo.GetParameters()
                    .SelectMany(p => p.ParameterType.GetProperties()
                        .Where(prop => prop.GetCustomAttribute<SwaggerIgnoreAttribute>() != null)
                    );
               
                    foreach (var property in ignoredProperties)
                    {
                        operation.Parameters = operation.Parameters
                            .Where(p => !p.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }
        }
    }
}
