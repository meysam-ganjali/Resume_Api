using Microsoft.Extensions.DependencyInjection;
using Resume.Application.AppImp;
using Resume.Application.AppInterfaces;
using Resume.Repositories.Application.Imp;
using Resume.Repositories.Application.Interfaces;

namespace Resume.IOC.ServiceContainer;

public static class ApplicationServicesBootstrapper
{
    public static IServiceCollection ServiceConfing(this IServiceCollection services)
    {

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IAuthRepository,AuthRepository>();
        services.AddScoped<IAuthService,   AuthService>();

        return services;
    }
}