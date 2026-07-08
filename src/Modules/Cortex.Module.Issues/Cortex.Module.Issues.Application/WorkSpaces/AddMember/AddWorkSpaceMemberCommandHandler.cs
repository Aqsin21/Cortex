using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Application.WorkSpaces.AddMember;
using Cortex.Module.Issues.Domain.Entities;
using MediatR;

namespace Cortex.Module.Issues.Application.Workspaces.AddMember
{
    public class AddWorkspaceMemberCommandHandler : IRequestHandler<AddWorkspaceMemberCommand, AddWorkspaceMemberResult>
    {
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IWorkSpaceRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddWorkspaceMemberCommandHandler(
            IWorkSpaceMemberRepository memberRepository,
            IWorkSpaceRoleRepository roleRepository,
            IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AddWorkspaceMemberResult> Handle(AddWorkspaceMemberCommand request, CancellationToken cancellationToken)
        {
            // 1. Yetki kontrolü: isteği yapan kişi bu workspace'te TeamLead mi?
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, request.WorkspaceId, cancellationToken);

            if (requester is null || requester.WorkSpaceRole.Name != "TeamLead")
            {
                return new AddWorkspaceMemberResult
                {
                    Succeeded = false,
                    Error = "You do not have permission to perform this action. Team Lead member options only."
                };
            }

            // 2. Hedef kullanıcı zaten bu workspace'te üye mi?
            var existingMember = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.TargetUserId, request.WorkspaceId, cancellationToken);

            if (existingMember is not null)
            {
                return new AddWorkspaceMemberResult
                {
                    Succeeded = false,
                    Error = "This user is already a member of the workspace."
                };
            }

            // 3. Rolü bul
            var role = await _roleRepository.GetByNameAsync(request.RoleName, cancellationToken);
            if (role is null)
            {
                return new AddWorkspaceMemberResult
                {
                    Succeeded = false,
                    Error = $"'{request.RoleName}' No role with that name was found."
                };
            }

            
            var newMember = new WorkSpaceMember
            {
                Id = Guid.NewGuid(),
                UserId = request.TargetUserId,
                WorkspaceId = request.WorkspaceId,
                FullName = request.TargetFullName,
                WorkSpaceRolId = role.Id
            };

            await _memberRepository.AddAsync(newMember, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AddWorkspaceMemberResult
            {
                Succeeded = true,
                WorkSpaceMemberId = newMember.Id
            };
        }
    }
}