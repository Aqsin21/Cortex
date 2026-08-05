using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Application.Behaviors;
using Cortex.Module.Issues.Application.WorkSpaces.CreateWorkSpace;
using Cortex.Module.Issues.Infrastructure.Persistence;
using Cortex.Module.Issues.Infrastructure.Persistence.Repositories;
using FluentValidation;
using MediatR;
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
            services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
            services.AddScoped<IWorkSpaceMemberRepository, WorkSpaceMemberRepository>();
            services.AddScoped<IWorkSpaceRoleRepository, WorkSpaceRoleRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IIssueRepository, IssueRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddValidatorsFromAssembly(typeof(CreateWorkspaceCommand).Assembly);
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(CreateWorkspaceCommand).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
            });
            return services;
        }
    }
}
    