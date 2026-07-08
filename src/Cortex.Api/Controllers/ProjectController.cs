using Cortex.Module.Issues.Application.Projects.CreateProject;
using Cortex.Module.Issues.Application.Projects.DeleteProject;
using Cortex.Module.Issues.Application.Projects.GetProjects;
using Cortex.Module.Issues.Application.Projects.UpdateProject;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Cortex.Api.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var command = new CreateProjectCommand
            {
                WorkspaceId = request.WorkspaceId,
                RequestedByUserId = userId,
                Name = request.Name,
                Description = request.Description,
                EndDate = request.EndDate
            };

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return Ok(new { projectId = result.ProjectId });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid workspaceId)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var query = new GetProjectsQuery
            {
                WorkspaceId = workspaceId,
                UserId = userId
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{projectId}")]
        public async Task<IActionResult> Update(Guid projectId, [FromBody] UpdateProjectRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var command = new UpdateProjectCommand
            {
                ProjectId = projectId,
                WorkspaceId = request.WorkspaceId,
                RequestedByUserId = userId,
                Name = request.Name,
                Description = request.Description,
                EndDate = request.EndDate
            };

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return Ok();
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> Delete(Guid projectId, [FromQuery] Guid workspaceId)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var command = new DeleteProjectCommand
            {
                ProjectId = projectId,
                WorkspaceId = workspaceId,
                RequestedByUserId = userId
            };

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return NoContent();
        }
    }

    public class CreateProjectRequest
    {
        public Guid WorkspaceId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class UpdateProjectRequest
    {
        public Guid WorkspaceId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public DateTime EndDate { get; set; }
    }
}