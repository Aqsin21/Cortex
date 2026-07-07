using Cortex.Module.Issues.Domain.Entities;
namespace Cortex.Module.Issues.Application.Abstraction
{
    public interface IWorkspaceRepository
    {
        Task AddAsync(Workspace workspace, CancellationToken cancellationToken);
        Task<Workspace?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
