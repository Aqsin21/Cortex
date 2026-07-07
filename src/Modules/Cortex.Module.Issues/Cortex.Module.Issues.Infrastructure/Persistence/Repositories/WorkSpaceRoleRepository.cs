using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortex.Module.Issues.Infrastructure.Persistence.Repositories
{
    public class WorkSpaceRoleRepository : IWorkSpaceRoleRepository
    {
        private readonly IssuesDbContext _context;

        public WorkSpaceRoleRepository(IssuesDbContext context)
        {
            _context = context;
        }

        public async Task<WorkSpaceRole?> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.WorkSpaceRoles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
        }

        public async Task AddAsync(WorkSpaceRole role, CancellationToken cancellationToken)
        {
            await _context.WorkSpaceRoles.AddAsync(role, cancellationToken);
        }
    }
}
