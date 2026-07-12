using Cortex.Module.Issues.Application.Abstraction;
using MediatR;
namespace Cortex.Module.Issues.Application.WorkSpaces.GetWorkSpaces
{
    public class GetWorkSpaceQueryHandler :IRequestHandler<GetWorkSpaceQuery, List<WorkSpaceDto>>
    {
        private readonly IWorkspaceRepository _workspaceRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;

        public GetWorkSpaceQueryHandler(
            IWorkspaceRepository workspaceRepository,
            IWorkSpaceMemberRepository memberRepository)
        {
            _workspaceRepository = workspaceRepository;
            _memberRepository = memberRepository;
        }
        public async Task<List<WorkSpaceDto>> Handle(GetWorkSpaceQuery request, CancellationToken cancellationToken)
        {
            var workspaces = await _workspaceRepository.GetByUserIdAsync(
                request.UserId, cancellationToken);
            var result = new List<WorkSpaceDto>();
            foreach (var workspace in workspaces)
            {
                var member = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                    request.UserId, workspace.Id, cancellationToken);

                result.Add(new WorkSpaceDto
                {
                    Id = workspace.Id,
                    Name = workspace.Name,
                    RoleName = member?.WorkSpaceRole.Name ?? "Unknown"
                });
            }

            return result;
        }
    }
}
