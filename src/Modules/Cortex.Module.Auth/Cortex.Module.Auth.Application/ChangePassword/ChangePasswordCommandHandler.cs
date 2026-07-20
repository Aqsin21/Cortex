using Cortex.Module.Auth.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Auth.Application.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordResult>
    {
        private readonly IIdentityService _identityService;

        public ChangePasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<ChangePasswordResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ChangePasswordAsync(
                request.UserId, request.CurrentPassword, request.NewPassword);

            return new ChangePasswordResult
            {
                Succeeded = result.Succeeded,
                Error = result.Errors.FirstOrDefault()
            };
        }
    }
}