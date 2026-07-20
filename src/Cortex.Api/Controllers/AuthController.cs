using Cortex.Module.Auth.Application.ChangePassword;
using Cortex.Module.Auth.Application.Login;
using Cortex.Module.Auth.Application.Register;
using Cortex.Module.Auth.Application.UpdateProfile;
using Cortex.Module.Auth.Application.Users.SearchByEmail;
using Cortex.Module.Auth.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Cortex.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IMediator mediator, UserManager<AppUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
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
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;

            var user = await _userManager.FindByIdAsync(userId!);
            if (user is null) return NotFound();

            return Ok(new
            {
                userId,
                email,
                firstName = user.FirstName,
                lastName = user.LastName
            });
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
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var result = await _mediator.Send(new UpdateProfileCommand
            {
                UserId = userId,
                FirstName = request.FirstName,
                LastName = request.LastName
            });

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return Ok();
        }
        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId is null) return Unauthorized();

            var result = await _mediator.Send(new ChangePasswordCommand
            {
                UserId = userId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            });

            if (!result.Succeeded)
                return BadRequest(new { error = result.Error });

            return Ok();
        }

        public class UpdateProfileRequest
        {
            public required string FirstName { get; set; }
            public required string LastName { get; set; }
        }

        public class ChangePasswordRequest
        {
            public required string CurrentPassword { get; set; }
            public required string NewPassword { get; set; }
        }

    }
}
