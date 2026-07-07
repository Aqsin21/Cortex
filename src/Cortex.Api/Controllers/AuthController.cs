using Cortex.Module.Auth.Application.Login;
using Cortex.Module.Auth.Application.Register;
using Cortex.Module.Auth.Application.Users.SearchByEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Cortex.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new { userId = result.UserId });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Succeeded)
                return Unauthorized(new { error = result.Error });

            return Ok(new { token = result.Token });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            return Ok(new { userId, email });
        }
        [HttpGet("users/search")]
        [Authorize]
        public async Task<IActionResult> SearchUser([FromQuery] string email)
        {
            var result = await _mediator.Send(new SearchUserByEmailQuery { Email = email });

            if (!result.Found)
                return NotFound();

            return Ok(new { userId = result.UserId, fullName = result.FullName });
        }
    }
}
