using MediatR;

namespace Cortex.Module.Auth.Application.RevokeToken
{
    public class RevokeTokenCommand : IRequest<bool>
    {
        public required string Token { get; set; }
    }
}