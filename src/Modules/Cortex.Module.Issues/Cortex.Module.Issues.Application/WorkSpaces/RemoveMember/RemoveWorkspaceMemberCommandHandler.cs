using Cortex.Module.Issues.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Issues.Application.Workspaces.RemoveMember
{
    public class RemoveWorkspaceMemberCommandHandler : IRequestHandler<RemoveWorkspaceMemberCommand, RemoveWorkspaceMemberResult>
    {
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveWorkspaceMemberCommandHandler(
            IWorkSpaceMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RemoveWorkspaceMemberResult> Handle(RemoveWorkspaceMemberCommand request, CancellationToken cancellationToken)
        {
           
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, request.WorkspaceId, cancellationToken);

            if (requester is null || requester.WorkSpaceRole.Name != "TeamLead")
                return new RemoveWorkspaceMemberResult
                {
                    Succeeded = false,
                    Error = "Only a TeamLead can remove a member."
                };

            
            var targetMember = await _memberRepository.GetByIdAsync(
                request.TargetMemberId, cancellationToken);

            if (targetMember is null || targetMember.WorkspaceId != request.WorkspaceId)
                return new RemoveWorkspaceMemberResult
                {
                    Succeeded = false,
                    Error = "Member not found in this workspace."
                };

            
            if (targetMember.UserId == request.RequestedByUserId)
                return new RemoveWorkspaceMemberResult
                {
                    Succeeded = false,
                    Error = "TeamLead cannot remove themselves from the workspace."
                };

            _memberRepository.Delete(targetMember);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RemoveWorkspaceMemberResult { Succeeded = true };
        }
    }
}