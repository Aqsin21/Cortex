using Cortex.Module.Issues.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Issues.Application.Issues.GetIssues
{
    public class GetIssuesQueryHandler : IRequestHandler<GetIssuesQuery, List<IssueDto>>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IWorkSpaceMemberRepository _memberRepository;

        public GetIssuesQueryHandler(
            IIssueRepository issueRepository,
            IWorkSpaceMemberRepository memberRepository)
        {
            _issueRepository = issueRepository;
            _memberRepository = memberRepository;
        }

        public async Task<List<IssueDto>> Handle(GetIssuesQuery request, CancellationToken cancellationToken)
        {
          
            var member = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.UserId, request.WorkspaceId, cancellationToken);

            if (member is null)
                return new List<IssueDto>();

            var issues = await _issueRepository.GetByProjectIdAsync(
                request.ProjectId,
                request.Status,
                request.Priority,
                cancellationToken);


            return issues.Select(i => new IssueDto
            {
                Id = i.Id,
                Title = i.Title,
                Description = i.Description,
                Status = i.Status,
                Priority = i.Priority,
                CreatedDate = i.CreatedDate,
                DueDate = i.DueDate,
                AssigneeId = i.AssigneeId
            }).ToList();
        }
    }
}