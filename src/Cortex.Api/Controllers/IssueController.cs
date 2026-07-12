using Cortex.Module.Issues.Application.Issues.AssignIssue;
using Cortex.Module.Issues.Application.Issues.CreateIssue;
using Cortex.Module.Issues.Application.Issues.CreateIssue.Cortex.Module.Issues.Application.Issues.CreateIssue;
using Cortex.Module.Issues.Application.Issues.DeleteIssue;
using Cortex.Module.Issues.Application.Issues.GetIssues;
using Cortex.Module.Issues.Application.Issues.UpdateIssueStatus;
using Cortex.Module.Issues.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Cortex.Api.Controllers
{
    [ApiController]
    [Route("api/issues")]
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IssueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIssueRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var command = new CreateIssueCommand
            {
                ProjectId = request.ProjectId,
                RequestedByUserId = userId,
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                DueDate = request.DueDate,
                AssigneeWorkSpaceMemberId = request.AssigneeWorkSpaceMemberId
            };

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return Ok(new { issueId = result.IssueId });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
        [FromQuery] Guid projectId,
        [FromQuery] Guid workspaceId,
        [FromQuery] IssueStatus? status,
        [FromQuery] IssuePriority? priority)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var query = new GetIssuesQuery
            {
                ProjectId = projectId,
                WorkspaceId = workspaceId,
                UserId = userId,
                Status = status,
                Priority = priority
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPatch("{issueId}/status")]
        public async Task<IActionResult> UpdateStatus(Guid issueId, [FromBody] UpdateIssueStatusRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var command = new UpdateIssueStatusCommand
            {
                IssueId = issueId,
                WorkspaceId = request.WorkspaceId,
                RequestedByUserId = userId,
                NewStatus = request.NewStatus
            };

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return Ok();
        }

        [HttpDelete("{issueId}")]
        public async Task<IActionResult> Delete(Guid issueId, [FromQuery] Guid workspaceId)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var command = new DeleteIssueCommand
            {
                IssueId = issueId,
                WorkspaceId = workspaceId,
                RequestedByUserId = userId
            };

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return NoContent();
        }
        [HttpPatch("{issueId}/assign")]
        public async Task<IActionResult> Assign(Guid issueId, [FromBody] AssignIssueRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var command = new AssignIssueCommand
            {
                IssueId = issueId,
                WorkspaceId = request.WorkspaceId,
                RequestedByUserId = userId,
                AssigneeWorkSpaceMemberId = request.AssigneeWorkSpaceMemberId
            };

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return NoContent();
        }
    }

    public class CreateIssueRequest
    {
        public Guid ProjectId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public IssuePriority Priority { get; set; } = IssuePriority.Medium;
        public DateTime? DueDate { get; set; }
        public Guid? AssigneeWorkSpaceMemberId { get; set; }
    }

    public class UpdateIssueStatusRequest
    {
        public Guid WorkspaceId { get; set; }
        public IssueStatus NewStatus { get; set; }
    }
    public class AssignIssueRequest
    {
        public Guid WorkspaceId { get; set; }
        public Guid? AssigneeWorkSpaceMemberId { get; set; }
    }
}