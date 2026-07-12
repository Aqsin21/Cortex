using Cortex.Module.Issues.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Issues.Application.Workspaces.GetWorkspaceMembers
{
    public class GetWorkspaceMembersQueryHandler : IRequestHandler<GetWorkspaceMembersQuery, List<WorkspaceMemberDto>>
    {
        private readonly IWorkSpaceMemberRepository _memberRepository;

        public GetWorkspaceMembersQueryHandler(IWorkSpaceMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<List<WorkspaceMemberDto>> Handle(GetWorkspaceMembersQuery request, CancellationToken cancellationToken)
        {
           
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.UserId, request.WorkspaceId, cancellationToken);

            if (requester is null)
                return new List<WorkspaceMemberDto>();

            var members = await _memberRepository.GetByWorkspaceIdAsync(
                request.WorkspaceId, cancellationToken);

            return members.Select(m => new WorkspaceMemberDto
            {
                WorkSpaceMemberId = m.Id,
                FullName = m.FullName,
                RoleName = m.WorkSpaceRole.Name
            }).ToList();
        }
    }
}