using Microsoft.Extensions.DependencyInjection;
using Resume.Application.AppImp;
using Resume.Application.AppInterfaces;
using Resume.Repositories.Application.Imp;
using Resume.Repositories.Application.Interfaces;

namespace Resume.IOC.ServiceContainer;

public static class ApplicationServicesBootstrapper {
    public static IServiceCollection ServiceConfing(this IServiceCollection services) {

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<ISkillTypeRepository, SkillTypeRepository>();
        services.AddScoped<ISkillTypeService, SkillTypeService>();

        services.AddScoped<IUserSkillRepository, UserSkillRepository>();
        services.AddScoped<IUserSkillService, UserSkillService>();

        services.AddScoped<IUserSocialMediaRepository, UserSocialMediaRepository>();
        services.AddScoped<ISocialMediaService, SocialMediaService>();

        services.AddScoped<IWorkExperienceRepository, WorkExperienceRepository>();
        services.AddScoped<IWorkExperienceService, WorkExperienceService>();

        services.AddScoped<IDegreeEducationRepository, DegreeEducationRepository>();
        services.AddScoped<IDegreeEducationService, DegreeEducationService>();

        return services;
    }
}