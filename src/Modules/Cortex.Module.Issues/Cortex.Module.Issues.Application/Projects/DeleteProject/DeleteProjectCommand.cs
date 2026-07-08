using MediatR;

namespace Cortex.Module.Issues.Application.Projects.DeleteProject
{
    public class DeleteProjectCommand : IRequest<DeleteProjectResult>
    {
        public Guid ProjectId { get; set; }
        public Guid WorkspaceId { get; set; }
        public required string RequestedByUserId { get; set; }
    }

    public class DeleteProjectResult
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}