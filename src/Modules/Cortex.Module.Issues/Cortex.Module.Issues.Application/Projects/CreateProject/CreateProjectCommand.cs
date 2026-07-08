using MediatR;
namespace Cortex.Module.Issues.Application.Projects.CreateProject
{
    public class CreateProjectCommand : IRequest<CreateProjectResult>
    {
        public Guid WorkspaceId { get; set; }
        public required string RequestedByUserId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateProjectResult
    {
        public bool Succeeded { get; set; }
        public Guid? ProjectId { get; set; }
        public string? Error { get; set; }
    }
}
