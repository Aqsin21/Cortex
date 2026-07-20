using MediatR;

namespace Cortex.Module.Auth.Application.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ChangePasswordResult>
    {
        public required string UserId { get; set; }
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }

    public class ChangePasswordResult
    {
        public bool Succeeded { get; set; }
        public string? Error { get; set; }
    }
}