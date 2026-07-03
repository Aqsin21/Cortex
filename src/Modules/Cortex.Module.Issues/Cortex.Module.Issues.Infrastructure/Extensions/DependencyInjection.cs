using Cortex.Module.Issues.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Cortex.Module.Issues.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIssuesInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("IssuesModuleConnection");
            services.AddDbContext<IssuesDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
