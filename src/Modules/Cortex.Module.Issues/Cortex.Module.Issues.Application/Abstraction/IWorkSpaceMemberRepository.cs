using Cortex.Module.Issues.Domain.Entities;
namespace Cortex.Module.Issues.Application.Abstraction
{
    public interface IWorkSpaceMemberRepository
    {
        Task AddAsync(WorkSpaceMember member, CancellationToken cancellationToken);
        Task<WorkSpaceMember?> GetByUserIdAndWorkspaceAsync(string userId, Guid workspaceId, CancellationToken cancellationToken);
    }
}
