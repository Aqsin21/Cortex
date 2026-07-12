using Cortex.Module.Issues.Domain.Entities;
using Cortex.Module.Issues.Domain.Enums;
namespace Cortex.Module.Issues.Application.Abstraction
{
    public interface IIssueRepository
    {
            Task AddAsync(Issue issue, CancellationToken cancellationToken);
            Task<Issue?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
            Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken);
        Task<List<Issue>> GetByProjectIdAsync(Guid projectId, IssueStatus? status, IssuePriority? priority, CancellationToken cancellationToken);
        void Delete(Issue issue);
    }
}
