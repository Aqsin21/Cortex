namespace Cortex.Module.Issues.Domain.Entities
{
    public class WorkSpaceMember
    {
        public Guid Id { get; set; }
        public required string UserId { get; set; }
        public required string FullName { get; set; }
        public Guid WorkSpaceRolId {  get; set; }
        public WorkSpaceRole WorkSpaceRole { get; set; } = null!;
        public ICollection<ProjectMember> ProjectMemberships { get; set; } = new List<ProjectMember>();
        public ICollection<Issue> ReportedIssues { get; set; } = new List<Issue>();
        public ICollection<Issue> AssignedIssues { get; set; } = new List<Issue>();
    }
}
