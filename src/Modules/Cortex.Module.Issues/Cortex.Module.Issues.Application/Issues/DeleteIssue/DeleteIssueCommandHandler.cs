using Cortex.Module.Issues.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Issues.Application.Issues.DeleteIssue
{
    public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand, DeleteIssueResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteIssueCommandHandler(
            IIssueRepository issueRepository,
            IWorkSpaceMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _issueRepository = issueRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteIssueResult> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
        {
            
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, request.WorkspaceId, cancellationToken);

            if (requester is null || requester.WorkSpaceRole.Name != "TeamLead")
                return new DeleteIssueResult { Succeeded = false, Error = "Only a TeamLead can delete an issue." };

            var issue = await _issueRepository.GetByIdAsync(request.IssueId, cancellationToken);
            if (issue is null)
                return new DeleteIssueResult { Succeeded = false, Error = "Issue not found." };

            _issueRepository.Delete(issue);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteIssueResult { Succeeded = true };
        }
    }
}