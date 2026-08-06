namespace Cortex.Module.Auth.Application.Abstraction
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email);
        string GenerateRefreshToken();
    }
}
