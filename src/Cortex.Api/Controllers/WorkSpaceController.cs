using Cortex.Module.Issues.Application.WorkSpaces.CreateWorkSpace;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Cortex.Api.Controllers
{
    [Route("api/workspace")]
    [ApiController]
    [Authorize]
    public class WorkSpaceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorkSpaceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWorkspaceRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (userId is null)
                return Unauthorized();

            var command = new CreateWorkspaceCommand
            {   
                Name = request.Name,
                OwnerId = userId,
                OwnerFullName = request.OwnerFullName
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
    public class CreateWorkspaceRequest
    {
        public required string Name { get; set; }
        public required string OwnerFullName { get; set; }
    }
}
