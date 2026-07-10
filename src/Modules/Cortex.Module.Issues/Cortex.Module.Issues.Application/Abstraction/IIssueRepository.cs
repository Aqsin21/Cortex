using Cortex.Module.Issues.Domain.Entities;
namespace Cortex.Module.Issues.Application.Abstraction
{
    public interface IIssueRepository
    {
            Task AddAsync(Issue issue, CancellationToken cancellationToken);
            Task<Issue?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
            Task<List<Issue>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken);
            void Delete(Issue issue);
    }
}
