namespace Cortex.Module.Issues.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public Guid WorkspaceId { get; set; }      
        public Workspace Workspace { get; set; } = null!;

        // Time management
        public DateTime CreatedDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid LeadId { get; set; }
        public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
        public ICollection<Issue> Issues { get; set; } = new List<Issue>();
    }
}
