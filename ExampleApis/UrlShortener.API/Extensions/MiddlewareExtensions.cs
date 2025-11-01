using Microsoft.OpenApi.Models;
using System.Reflection;

namespace UrlShortener.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Url Shortener API",
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
                options.SwaggerEndpoint("/api4/swagger/v1/swagger.json", "Url Shortener API");
                options.DocumentTitle = "Url Shortener API";
            });
            return app;
        }
    }
}
