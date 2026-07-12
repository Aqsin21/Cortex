using Cortex.Module.Issues.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Issues.Application.Workspaces.DeleteWorkspace
{
    public class DeleteWorkspaceCommandHandler : IRequestHandler<DeleteWorkspaceCommand, DeleteWorkspaceResult>
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWorkspaceCommandHandler(
            IWorkspaceRepository workspaceRepository,
            IWorkSpaceMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _workspaceRepository = workspaceRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteWorkspaceResult> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
        {
          
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, request.WorkspaceId, cancellationToken);

            if (requester is null || requester.WorkSpaceRole.Name != "TeamLead")
                return new DeleteWorkspaceResult
                {
                    Succeeded = false,
                    Error = "Only a TeamLead can delete a workspace."
                };

            var workspace = await _workspaceRepository.GetByIdAsync(
                request.WorkspaceId, cancellationToken);

            if (workspace is null)
                return new DeleteWorkspaceResult { Succeeded = false, Error = "Workspace not found." };
             
            var members = await _memberRepository.GetByWorkspaceIdAsync(
                        request.WorkspaceId, cancellationToken);

            foreach (var member in members)
                _memberRepository.Delete(member);

            _workspaceRepository.Delete(workspace);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteWorkspaceResult { Succeeded = true };
        }
    }
}