using MediatR;

namespace Cortex.Module.Issues.Application.WorkSpaces.CreateWorkSpace
{
    public class CreateWorkspaceCommand : IRequest<CreateWorkspaceResult>
    {
        public required string Name { get; set; }
        public required string OwnerId { get; set; }
        public required string OwnerFullName { get; set; }
    }

    public class CreateWorkspaceResult
    {
        public Guid WorkspaceId { get; set; }
        public Guid WorkSpaceMemberId { get; set; }
    }
}
