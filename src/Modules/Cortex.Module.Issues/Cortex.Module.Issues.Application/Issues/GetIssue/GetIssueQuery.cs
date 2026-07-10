using Cortex.Module.Issues.Domain.Enums;
using MediatR;

namespace Cortex.Module.Issues.Application.Issues.GetIssues
{
    public class GetIssuesQuery : IRequest<List<IssueDto>>
    {
        public Guid ProjectId { get; set; }
        public Guid WorkspaceId { get; set; }
        public required string UserId { get; set; }
    }

    public class IssueDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public IssueStatus Status { get; set; }
        public IssuePriority Priority { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? AssigneeId { get; set; }
    }
}