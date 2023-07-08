using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Resume.Data;

namespace Resume.IOC.DbContainer;

public static class DatabaseBootsrapper
{
    public static IServiceCollection DataBaseConfig(this IServiceCollection services, string conStr)
    {
        services.AddDbContext<ResumeDbContext>(op => op.UseSqlServer(conStr));
        return services;
    }
}