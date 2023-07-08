using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Resume.IOC.SwaggerContainer;

public static class SwaggerBootstrapper
{
    public static IServiceCollection AddOurSwagger(this IServiceCollection service)
    {
        service.AddSwaggerGen(opt => {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Sample In Asp.Net Core 6", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
        return service;
    }
}