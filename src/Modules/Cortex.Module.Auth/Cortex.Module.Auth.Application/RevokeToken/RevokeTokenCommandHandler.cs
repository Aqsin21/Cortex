using Cortex.Module.Auth.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Auth.Application.RevokeToken
{
    public class RevokeTokenCommandHandler :IRequestHandler<RevokeTokenCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public RevokeTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.RevokeTokenAsync(request.Token);
        }
    }
}
