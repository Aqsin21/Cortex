namespace Cortex.Module.Issues.Domain.Entities
{
    public class ProjectMember
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid WorkspaceMemberId { get; set; }
        public Project Project { get; set; } = null!;
        public WorkSpaceMember WorkspaceMember { get; set; } = null!;
    }
}
