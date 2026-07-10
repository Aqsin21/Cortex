using Cortex.Module.Issues.Domain.Enums;
using MediatR;

namespace Cortex.Module.Issues.Application.Issues.CreateIssue
{
    namespace Cortex.Module.Issues.Application.Issues.CreateIssue
    {
        public class CreateIssueCommand : IRequest<CreateIssueResult>
        {
            public Guid ProjectId { get; set; }
            public required string RequestedByUserId { get; set; }
            public required string Title { get; set; }
            public required string Description { get; set; }
            public IssuePriority Priority { get; set; } = IssuePriority.Medium;
            public DateTime? DueDate { get; set; }
            public Guid? AssigneeWorkSpaceMemberId { get; set; }
        }
    }

    public class CreateIssueResult
    {
        public bool Succeeded { get; set; }
        public Guid? IssueId { get; set; }
        public string? Error { get; set; }
    }
}