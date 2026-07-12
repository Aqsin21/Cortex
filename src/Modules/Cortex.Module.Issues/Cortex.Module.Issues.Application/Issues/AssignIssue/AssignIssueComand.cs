using MediatR;
namespace Cortex.Module.Issues.Application.Issues.AssignIssue
{
    public class AssignIssueCommand : IRequest<AssignIssueResult>
    {
        public Guid IssueId { get; set; }
        public Guid WorkspaceId { get; set; }
        public required string RequestedByUserId { get; set; }
        public Guid? AssigneeWorkSpaceMemberId { get; set; }  // null = atamayı kaldır
    }

    public class AssignIssueResult
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}