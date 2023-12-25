using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Infrastructure.Data;
using BuildCleanArchitecture.Services;

namespace BuildCleanArchitecture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<IUser, CurrentUser>();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            return services;
        }
    }
}
