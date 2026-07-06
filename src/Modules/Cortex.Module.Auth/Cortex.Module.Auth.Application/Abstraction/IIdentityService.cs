using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cortex.Module.Auth.Application.Abstraction
{
    public interface IIdentityService
    {
        Task<IdentityOperationResult> RegisterAsync(string email, string password, string firstName, string lastName);
        Task<LoginOperationResult> ValidateCredentialsAsync(string email, string password);
    }
    public class IdentityOperationResult
    {
        public bool Succeeded { get; set; }
        public string? UserId { get; set; }
        public IEnumerable<string> Errors { get; set; } = [];
    }
    public class LoginOperationResult
    {
        public bool Succeeded { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Error { get; set; }
    }

}
