using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Store.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Store API",
                    Version = "v1",
                    Description = "Simple RESTful API built with ASP.NET Core to show how to create RESTful services using a service-oriented architecture."
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/api3/swagger/v1/swagger.json", "Store API");
                options.DocumentTitle = "Store API";
            });
            return app;
        }
    }
}
