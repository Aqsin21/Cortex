using MediatR;

namespace Cortex.Module.Issues.Application.Workspaces.GetWorkspaceMembers
{
    public class GetWorkspaceMembersQuery : IRequest<List<WorkspaceMemberDto>>
    {
        public Guid WorkspaceId { get; set; }
        public required string UserId { get; set; }
    }

    public class WorkspaceMemberDto
    {
        public Guid WorkSpaceMemberId { get; set; }
        public required string FullName { get; set; }
        public required string RoleName { get; set; }
    }
}