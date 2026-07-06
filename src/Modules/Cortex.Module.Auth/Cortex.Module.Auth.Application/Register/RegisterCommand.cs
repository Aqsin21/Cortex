using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cortex.Module.Auth.Application.Register
{
    public class RegisterCommand : IRequest<RegisterResult>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
    public class RegisterResult
    {
        public bool Succeeded { get; set; }
        public string? UserId { get; set; }
        public IEnumerable<string> Errors { get; set; } = [];
    }
}
