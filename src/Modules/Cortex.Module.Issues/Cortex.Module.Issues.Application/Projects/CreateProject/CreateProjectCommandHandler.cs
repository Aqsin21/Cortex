using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Domain.Entities;
using MediatR;

namespace Cortex.Module.Issues.Application.Projects.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CreateProjectResult>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectCommandHandler(
            IProjectRepository projectRepository,
            IWorkSpaceMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _projectRepository = projectRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateProjectResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, request.WorkspaceId, cancellationToken);

            if (requester is null || requester.WorkSpaceRole.Name != "TeamLead")
            {
                return new CreateProjectResult
                {
                    Succeeded = false,
                    Error = "Only a TeamLead can create a project."
                };
            }

            
            var project = new Project
            {
                Id = Guid.NewGuid(),
                WorkspaceId = request.WorkspaceId,
                Name = request.Name,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow,
                EndDate = request.EndDate,
                LeadId = requester.Id
            };

            await _projectRepository.AddAsync(project, cancellationToken);
            var projectMember = new ProjectMember
            {
                Id = Guid.NewGuid(),
                ProjectId = project.Id,
                WorkspaceMemberId = requester.Id
            };
            _context.ProjectMembers.Add(projectMember);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateProjectResult
            {
                Succeeded = true,
                ProjectId = project.Id
            };
        }
    }
}