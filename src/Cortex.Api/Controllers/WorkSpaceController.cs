using Cortex.Module.Issues.Application.WorkSpaces.AddMember;
using Cortex.Module.Issues.Application.WorkSpaces.CreateWorkSpace;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Cortex.Api.Controllers
{
    [ApiController]
    [Route("api/Createworkspaces")]
    [Authorize]
    public class WorkspaceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkspaceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkspaceRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var command = new CreateWorkspaceCommand
            {
                Name = request.Name,
                OwnerId = userId,
                OwnerFullName = request.OwnerFullName
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("Addmembers")]
        public async Task<IActionResult> AddMember([FromBody] AddWorkspaceMemberRequest request)
        {
            var requestedByUserId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (requestedByUserId is null) return Unauthorized();

            var command = new AddWorkspaceMemberCommand
            {
                WorkspaceId = request.WorkspaceId,
                RequestedByUserId = requestedByUserId,
                TargetUserId = request.TargetUserId,
                TargetFullName = request.TargetFullName,
                RoleName = request.RoleName
            };

            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return Ok(new { workSpaceMemberId = result.WorkSpaceMemberId });
        }
    }

    public class CreateWorkspaceRequest
    {
        public required string Name { get; set; }
        public required string OwnerFullName { get; set; }
    }

    public class AddWorkspaceMemberRequest
    {
        public Guid WorkspaceId { get; set; } 
        public required string TargetUserId { get; set; }
        public required string TargetFullName { get; set; }
        public required string RoleName { get; set; }
    }
}