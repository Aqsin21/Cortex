using Cortex.Module.Issues.Application.Abstraction;
using MediatR;
namespace Cortex.Module.Issues.Application.Projects.UpdateProject
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, UpdateProjectResult>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectCommandHandler(
            IProjectRepository projectRepository,
            IWorkSpaceMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateProjectResult> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, request.WorkspaceId, cancellationToken);

            if (requester is null || requester.WorkSpaceRole.Name != "TeamLead")
            {
                return new UpdateProjectResult
                {
                    Succeeded = false,
                    Error = "Only a TeamLead can update a project."
                };
            }

            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project is null)
            {
                return new UpdateProjectResult { Succeeded = false, Error = "Project not found." };
            }

            project.Name = request.Name;
            project.Description = request.Description;
            project.EndDate = request.EndDate;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new UpdateProjectResult { Succeeded = true };
        }
    }
}