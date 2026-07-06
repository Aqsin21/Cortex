using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cortex.Module.Auth.Application.Login
{
    public class LoginCommand : IRequest<LoginResult>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string? Token { get; set; }
        public string? Error { get; set; }
    }
}
