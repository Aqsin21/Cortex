using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cortex.Module.Issues.Infrastructure.Persistence.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly IssuesDbContext _context;

        public IssueRepository(IssuesDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Issue issue, CancellationToken cancellationToken)
        {
            await _context.Issues.AddAsync(issue, cancellationToken);
        }

        public async Task<Issue?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Issues
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public async Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken)
        {
            return await _context.Issues
                .Where(i => i.ProjectId == projectId)
                .ToListAsync(cancellationToken);
        }

        public void Delete(Issue issue)
        {
            _context.Issues.Remove(issue);
        }
    }
}