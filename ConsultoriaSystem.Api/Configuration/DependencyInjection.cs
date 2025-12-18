using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ConsultoriaSystem.Api.Repositories;
using ConsultoriaSystem.Api.Services;

namespace ConsultoriaSystem.Api.Configuration
{
    public static class DependencyInjection
    {
        //Servicios de negocio
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IConsultoresService, ConsultoresService>();
            services.AddScoped<IPaquetesService, PaquetesService>();
            services.AddScoped<IReportesService, ReportesService>();
            services.AddScoped<IConsultorPaqueteService, ConsultorPaqueteService>();

            return services;
        }

        // Repositorios (acceso a la bd, sp)
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsuariosRepository, UsuariosRepository>();
            services.AddScoped<IConsultoresRepository, ConsultoresRepository>();
            services.AddScoped<IPaquetesRepository, PaquetesRepository>();
            services.AddScoped<IReportesRepository, ReportesRepository>();
            services.AddScoped<IConsultorPaqueteRepository, ConsultorPaqueteRepository>();

            return services;
        }

        // Autenticación JWT
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSection["Issuer"],
                        ValidAudience = jwtSection["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            return services;
        }

        public static IServiceCollection AddCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();
            return services;
        }
    }
}
