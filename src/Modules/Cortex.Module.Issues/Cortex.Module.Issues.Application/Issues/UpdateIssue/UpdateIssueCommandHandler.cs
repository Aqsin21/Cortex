using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Domain.Enums;
using MediatR;

namespace Cortex.Module.Issues.Application.Issues.UpdateIssueStatus
{
    public class UpdateIssueStatusCommandHandler : IRequestHandler<UpdateIssueStatusCommand, UpdateIssueStatusResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateIssueStatusCommandHandler(
            IIssueRepository issueRepository,
            IWorkSpaceMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _issueRepository = issueRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateIssueStatusResult> Handle(UpdateIssueStatusCommand request, CancellationToken cancellationToken)
        {
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, request.WorkspaceId, cancellationToken);

            if (requester is null)
                return new UpdateIssueStatusResult { Succeeded = false, Error = "Workspace member not found." };

            var issue = await _issueRepository.GetByIdAsync(request.IssueId, cancellationToken);
            if (issue is null)
                return new UpdateIssueStatusResult { Succeeded = false, Error = "Issue not found." };

            var isTeamLead = requester.WorkSpaceRole.Name == "TeamLead";
            var isAssignee = issue.AssigneeId == requester.Id;
            var isQA = requester.WorkSpaceRole.Name == "QA";

           
            var allowed = false;

            if (isTeamLead)
            {
                allowed = true;
            }
            else if (isAssignee)
            {
                allowed = true; 
            }
            else
            {
                allowed = false; 
            }
            issue.Status = request.NewStatus;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateIssueStatusResult { Succeeded = true };
        }
    }
}