using Cortex.Module.Issues.Domain.Enums;
namespace Cortex.Module.Issues.Domain.Entities
{
    public class Issue
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }

        // Enum Connections
        public IssueStatus Status { get; set; } = IssueStatus.ToDo;
        public IssuePriority Priority { get; set; } = IssuePriority.Medium;

        // Time Management
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; } 

        // Relationship
        public Guid ProjectId { get; set; }
        public Guid ReporterId { get; set; } 
        public Guid? AssigneeId { get; set; }

        // Navigation Properties 
        public Project Project { get; set; } = null!;
        public WorkSpaceMember Reporter { get; set; } = null!;
        public WorkSpaceMember? Assignee { get; set; }
    }
}
