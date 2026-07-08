using Cortex.Module.Issues.Application.Abstraction;
using MediatR;
namespace Cortex.Module.Issues.Application.Projects.DeleteProject
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, DeleteProjectResult>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProjectCommandHandler(
            IProjectRepository projectRepository,
            IWorkSpaceMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteProjectResult> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            // Yetki kontrolü
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, request.WorkspaceId, cancellationToken);

            if (requester is null || requester.WorkSpaceRole.Name != "TeamLead")
            {
                return new DeleteProjectResult
                {
                    Succeeded = false,
                    Error = "Only a TeamLead can delete a project."
                };
            }

            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project is null)
            {
                return new DeleteProjectResult { Succeeded = false, Error = "Project not found." };
            }

            _projectRepository.Delete(project);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DeleteProjectResult { Succeeded = true };
        }
    }
}