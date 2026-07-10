using Cortex.Module.Issues.Domain.Enums;
using MediatR;

namespace Cortex.Module.Issues.Application.Issues.UpdateIssueStatus
{
    public class UpdateIssueStatusCommand : IRequest<UpdateIssueStatusResult>
    {
        public Guid IssueId { get; set; }
        public Guid WorkspaceId { get; set; }
        public required string RequestedByUserId { get; set; }
        public IssueStatus NewStatus { get; set; }
    }

    public class UpdateIssueStatusResult
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}