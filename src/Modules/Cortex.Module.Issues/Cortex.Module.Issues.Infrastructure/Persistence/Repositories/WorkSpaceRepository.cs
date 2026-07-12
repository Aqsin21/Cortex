using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortex.Module.Issues.Infrastructure.Persistence.Repositories
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private readonly IssuesDbContext _context;

        public WorkspaceRepository(IssuesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Workspace workspace, CancellationToken cancellationToken)
        {
            await _context.Workspaces.AddAsync(workspace, cancellationToken);
        }

        public async Task<Workspace?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Workspaces.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }
        public void Delete(Workspace workspace)
        {
            _context.Workspaces.Remove(workspace);
        }
        public async Task<List<Workspace>> GetByUserIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await _context.Workspaces
                .Where(w => w.Members.Any(m => m.UserId == userId))
                .ToListAsync(cancellationToken);
        }
    }
}
