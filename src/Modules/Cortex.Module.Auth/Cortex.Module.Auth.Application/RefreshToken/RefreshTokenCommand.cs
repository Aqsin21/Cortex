using MediatR;
namespace Cortex.Module.Auth.Application.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResult>
    {
        public required string Token { get; set; }
    }

    public class RefreshTokenResult
    {
        public bool Succeeded { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Error { get; set; }
    }
}
