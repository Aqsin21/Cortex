using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortex.Module.Issues.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly IssuesDbContext _context;

        public ProjectRepository(IssuesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken)
        {
            await _context.Projects.AddAsync(project, cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Project>> GetByMemberUserIdAsync(string userId, Guid workspaceId, CancellationToken cancellationToken)
        {
            return await _context.Projects
                .Where(p => p.WorkspaceId == workspaceId &&
                            p.Members.Any(pm => pm.WorkspaceMember.UserId == userId))
                .ToListAsync(cancellationToken);
        }
        public async Task AddMemberAsync(ProjectMember projectMember, CancellationToken cancellationToken)
        {
            await _context.ProjectMembers.AddAsync(projectMember, cancellationToken);
        }
        public void Delete(Project project)
        {
            _context.Projects.Remove(project);
        }
    }
}