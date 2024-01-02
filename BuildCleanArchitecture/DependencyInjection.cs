using BuildCleanArchitecture.Application.Common.Interfaces;
using BuildCleanArchitecture.Infrastructure.Data;
using BuildCleanArchitecture.Services;
using Microsoft.OpenApi.Models;

namespace BuildCleanArchitecture
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<IUser, CurrentUser>();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                // Đây là cấu hình để Swagger không yêu cầu xác thực
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
