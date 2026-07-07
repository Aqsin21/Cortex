using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Domain.Entities;
using MediatR;
namespace Cortex.Module.Issues.Application.WorkSpaces.CreateWorkSpace
{
    public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, CreateWorkspaceResult>
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IWorkSpaceRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWorkspaceCommandHandler(
            IWorkspaceRepository workspaceRepository,
            IWorkSpaceMemberRepository memberRepository,
            IWorkSpaceRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _workspaceRepository = workspaceRepository;
            _memberRepository = memberRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateWorkspaceResult> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            var teamLeadRole = await _roleRepository.GetByNameAsync("TeamLead", cancellationToken);

            if (teamLeadRole is null)
            {
                teamLeadRole = new WorkSpaceRole
                {
                    Id = Guid.NewGuid(),
                    Name = "TeamLead",
                    Description = "The workspace administrator can manage projects and members."
                };
                await _roleRepository.AddAsync(teamLeadRole, cancellationToken);
            }

            var workspace = new Workspace
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                OwnerId = request.OwnerId,
                CreatedDate = DateTime.UtcNow
            };
            await _workspaceRepository.AddAsync(workspace, cancellationToken);

            var member = new WorkSpaceMember
            {
                Id = Guid.NewGuid(),
                UserId = request.OwnerId,
                WorkspaceId = workspace.Id,
                FullName = request.OwnerFullName,
                WorkSpaceRolId = teamLeadRole.Id
            };
            await _memberRepository.AddAsync(member, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateWorkspaceResult
            {
                WorkspaceId = workspace.Id,
                WorkSpaceMemberId = member.Id
            };
        }
    }
}
