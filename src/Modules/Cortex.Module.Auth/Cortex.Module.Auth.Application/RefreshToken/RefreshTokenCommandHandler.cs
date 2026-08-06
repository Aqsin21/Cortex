using Cortex.Module.Auth.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Auth.Application.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
    {
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<RefreshTokenResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RefreshTokenAsync(request.Token);

            return new RefreshTokenResult
            {
                Succeeded = result.Succeeded,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                Error = result.Error
            };
        }
    }
}