using Cortex.Module.Auth.Application.Abstraction;
using Cortex.Module.Auth.Application.Register;
using Cortex.Module.Auth.Domain.Entities;
using Cortex.Module.Auth.Infrastructure.Identity;
using Cortex.Module.Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace Cortex.Module.Auth.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AuthModuleConnection");

           
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentityCore<AppUser>(options =>
            {
                
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddRoles<IdentityRole>() 
            .AddEntityFrameworkStores<AuthDbContext>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, JwtTokenService>();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));
            services.AddAuthInfrastructureAuthentication(configuration);
            return services;
        }
        private static IServiceCollection AddAuthInfrastructureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
                };
            });

            services.AddAuthorization();
            return services;
        }
    }
}
