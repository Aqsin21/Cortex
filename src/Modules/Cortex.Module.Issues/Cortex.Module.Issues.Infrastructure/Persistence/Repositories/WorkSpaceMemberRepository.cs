using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortex.Module.Issues.Infrastructure.Persistence.Repositories
{
    public class WorkSpaceMemberRepository : IWorkSpaceMemberRepository
    {
        private readonly IssuesDbContext _context;

        public WorkSpaceMemberRepository(IssuesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(WorkSpaceMember member, CancellationToken cancellationToken)
        {
            await _context.WorkSpaceMembers.AddAsync(member, cancellationToken);
        }

        public async Task<WorkSpaceMember?> GetByUserIdAndWorkspaceAsync(string userId, Guid workspaceId, CancellationToken cancellationToken)
        {
            return await _context.WorkSpaceMembers
                .Include(m => m.WorkSpaceRole)
                .FirstOrDefaultAsync(m => m.UserId == userId && m.WorkspaceId == workspaceId, cancellationToken);
        }
        public async Task<List<WorkSpaceMember>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken)
        {
            return await _context.WorkSpaceMembers
                .Include(m => m.WorkSpaceRole)
                .Where(m => m.WorkspaceId == workspaceId)
                .ToListAsync(cancellationToken);
        }
        public async Task<WorkSpaceMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.WorkSpaceMembers
                .Include(m => m.WorkSpaceRole)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public void Delete(WorkSpaceMember member)
        {
            _context.WorkSpaceMembers.Remove(member);
        }
    }
}
