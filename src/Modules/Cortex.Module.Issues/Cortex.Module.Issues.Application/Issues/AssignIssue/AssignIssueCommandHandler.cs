using Cortex.Module.Issues.Application.Abstraction;
using MediatR;
namespace Cortex.Module.Issues.Application.Issues.AssignIssue
{
    public class AssignIssueCommandHandler : IRequestHandler<AssignIssueCommand, AssignIssueResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignIssueCommandHandler(
            IIssueRepository issueRepository,
            IWorkSpaceMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _issueRepository = issueRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AssignIssueResult> Handle(AssignIssueCommand request, CancellationToken cancellationToken)
        {
            
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, request.WorkspaceId, cancellationToken);

            if (requester is null || requester.WorkSpaceRole.Name != "TeamLead")
                return new AssignIssueResult
                {
                    Succeeded = false,
                    Error = "Only a TeamLead can assign an issue."
                };

            var issue = await _issueRepository.GetByIdAsync(request.IssueId, cancellationToken);
            if (issue is null)
                return new AssignIssueResult { Succeeded = false, Error = "Issue not found." };

            issue.AssigneeId = request.AssigneeWorkSpaceMemberId;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AssignIssueResult { Succeeded = true };
        }
    }
}