using Cortex.Module.Issues.Application.Roles.GetAllRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cortex.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllRolesQuery(), cancellationToken);
            return Ok(result);
        }
    }
}