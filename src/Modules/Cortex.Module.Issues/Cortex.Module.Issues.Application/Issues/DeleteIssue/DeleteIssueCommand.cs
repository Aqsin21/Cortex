using MediatR;

namespace Cortex.Module.Issues.Application.Issues.DeleteIssue
{
    public class DeleteIssueCommand : IRequest<DeleteIssueResult>
    {
        public Guid IssueId { get; set; }
        public Guid WorkspaceId { get; set; }
        public required string RequestedByUserId { get; set; }
    }

    public class DeleteIssueResult
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}