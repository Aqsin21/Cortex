using Cortex.Module.Issues.Domain.Entities;
namespace Cortex.Module.Issues.Application.Abstraction
{
    public interface IWorkSpaceMemberRepository
    {
        Task AddAsync(WorkSpaceMember member, CancellationToken cancellationToken);
        Task<WorkSpaceMember?> GetByUserIdAndWorkspaceAsync(string userId, Guid workspaceId, CancellationToken cancellationToken);
        Task<List<WorkSpaceMember>> GetByWorkspaceIdAsync(Guid workspaceId, CancellationToken cancellationToken);
        Task<WorkSpaceMember?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        void Delete(WorkSpaceMember member);
    }
}
