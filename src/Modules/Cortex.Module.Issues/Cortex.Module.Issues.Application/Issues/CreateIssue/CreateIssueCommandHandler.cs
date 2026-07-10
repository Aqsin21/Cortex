using Cortex.Module.Issues.Application.Abstraction;
using Cortex.Module.Issues.Application.Issues.CreateIssue.Cortex.Module.Issues.Application.Issues.CreateIssue;
using Cortex.Module.Issues.Domain.Entities;
using Cortex.Module.Issues.Domain.Enums;
using MediatR;
namespace Cortex.Module.Issues.Application.Issues.CreateIssue
{
    public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, CreateIssueResult>
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IProjectRepository _projectRepository; // YENİ: Projeyi doğrulamak için eklendi
        private readonly IWorkSpaceMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateIssueCommandHandler(
            IIssueRepository issueRepository,
            IProjectRepository projectRepository,
            IWorkSpaceMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _issueRepository = issueRepository;
            _projectRepository = projectRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateIssueResult> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
        {
            
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);
            if (project is null)
            {
                return new CreateIssueResult { Succeeded = false, Error = "Project not found." };
            }

          
            var requester = await _memberRepository.GetByUserIdAndWorkspaceAsync(
                request.RequestedByUserId, project.WorkspaceId, cancellationToken);

            if (requester is null || requester.WorkSpaceRole.Name != "TeamLead")
            {
                return new CreateIssueResult
                {
                    Succeeded = false,
                    Error = "Only a TeamLead can create an issue."
                };
            }

          
            if (request.AssigneeWorkSpaceMemberId.HasValue)
            {
                
                var isProjectMember = project.Members.Any(pm => pm.WorkspaceMemberId == request.AssigneeWorkSpaceMemberId.Value);

                if (!isProjectMember)
                {
                    return new CreateIssueResult
                    {
                        Succeeded = false,
                        Error = "The assignee must be a member of this specific project."
                    };
                }
            }

            
            var issue = new Issue
            {
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                Status = IssueStatus.ToDo,
                CreatedDate = DateTime.UtcNow,
                DueDate = request.DueDate,
                ReporterId = requester.Id,
                AssigneeId = request.AssigneeWorkSpaceMemberId 
            };

            await _issueRepository.AddAsync(issue, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CreateIssueResult
            {
                Succeeded = true,
                IssueId = issue.Id
            };
        }
    }
}