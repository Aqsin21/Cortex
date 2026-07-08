using MediatR;

namespace Cortex.Module.Issues.Application.Projects.GetProjects
{
    public class GetProjectsQuery : IRequest<List<ProjectDto>>
    {
        public Guid WorkspaceId { get; set; }
        public required string UserId { get; set; }
    }

    public class ProjectDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}