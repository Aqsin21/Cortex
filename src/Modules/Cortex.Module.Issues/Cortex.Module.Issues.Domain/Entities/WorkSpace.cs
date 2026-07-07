namespace Cortex.Module.Issues.Domain.Entities
{
    public class Workspace
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string OwnerId { get; set; }  
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public ICollection<WorkSpaceMember> Members { get; set; } = new List<WorkSpaceMember>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
