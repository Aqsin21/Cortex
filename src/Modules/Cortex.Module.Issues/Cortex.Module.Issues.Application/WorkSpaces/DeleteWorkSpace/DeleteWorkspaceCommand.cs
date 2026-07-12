using MediatR;

namespace Cortex.Module.Issues.Application.Workspaces.DeleteWorkspace
{
    public class DeleteWorkspaceCommand : IRequest<DeleteWorkspaceResult>
    {
        public Guid WorkspaceId { get; set; }
        public required string RequestedByUserId { get; set; }
    }

    public class DeleteWorkspaceResult
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}