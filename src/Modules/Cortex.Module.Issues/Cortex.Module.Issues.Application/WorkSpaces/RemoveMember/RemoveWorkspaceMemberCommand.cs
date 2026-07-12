using MediatR;

namespace Cortex.Module.Issues.Application.Workspaces.RemoveMember
{
    public class RemoveWorkspaceMemberCommand : IRequest<RemoveWorkspaceMemberResult>
    {
        public Guid WorkspaceId { get; set; }
        public Guid TargetMemberId { get; set; }
        public required string RequestedByUserId { get; set; }
    }

    public class RemoveWorkspaceMemberResult
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}