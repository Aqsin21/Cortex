using Cortex.Module.Auth.Application.Abstraction;
using Cortex.Module.Auth.Application.Register;
using Cortex.Module.Auth.Domain.Entities;
using Cortex.Module.Auth.Infrastructure.Identity;
using Cortex.Module.Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            return services;
        }
    }
}
