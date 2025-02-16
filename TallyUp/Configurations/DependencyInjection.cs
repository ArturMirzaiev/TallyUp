

using TallyUp.Application.Interfaces;
using TallyUp.Application.Mapping;
using TallyUp.Application.Services;
using TallyUp.Domain.Interfaces;
using TallyUp.Infrastructure.Repositories;

namespace TallyUp.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddSingleton<IJwtService, JwtService>();

        services.AddScoped<IPollRepository, PollRepository>();
        services.AddScoped<IPollService, PollService>();
        
        return services;
    }
}