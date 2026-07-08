using Cortex.Module.Issues.Application.Abstraction;
using MediatR;
namespace Cortex.Module.Issues.Application.Projects.GetProjects
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, List<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectsQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
        
            var projects = await _projectRepository.GetByWorkspaceIdAsync(
                request.WorkspaceId, cancellationToken);

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedDate = p.CreatedDate,
                EndDate = p.EndDate
            }).ToList();
        }
    }
}