using MediatR;
namespace Cortex.Module.Issues.Application.Projects.UpdateProject
{
    public class UpdateProjectCommand : IRequest<UpdateProjectResult>
    {
        public Guid ProjectId { get; set; }
        public Guid WorkspaceId { get; set; }
        public required string RequestedByUserId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UpdateProjectResult
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}