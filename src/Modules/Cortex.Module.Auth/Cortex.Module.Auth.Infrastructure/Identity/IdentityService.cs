using Cortex.Module.Auth.Application.Abstraction;
using Cortex.Module.Auth.Domain.Entities;
using Cortex.Module.Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cortex.Module.Auth.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly AuthDbContext _context;

        public IdentityService(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            AuthDbContext context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<IdentityOperationResult> RegisterAsync(
            string email, string password, string firstName, string lastName)
        {
            var user = new AppUser
            {
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await _userManager.CreateAsync(user, password);

            return new IdentityOperationResult
            {
                Succeeded = result.Succeeded,
                UserId = result.Succeeded ? user.Id : null,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<LoginOperationResult> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return new LoginOperationResult { Succeeded = false, Error = "Email or password is wrong." };

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                return new LoginOperationResult { Succeeded = false, Error = "Email or password is wrong." };

            if (!user.IsActive)
                return new LoginOperationResult { Succeeded = false, Error = "Account is not active." };

            return new LoginOperationResult
            {
                Succeeded = true,
                UserId = user.Id,
                Email = user.Email
            };
        }

        public async Task<UserLookupResult?> FindByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return null;

            return new UserLookupResult
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}"
            };
        }

        public async Task<IdentityOperationResult> UpdateProfileAsync(string userId, string firstName, string lastName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return new IdentityOperationResult { Succeeded = false, Errors = ["User not found."] };

            user.FirstName = firstName;
            user.LastName = lastName;

            var result = await _userManager.UpdateAsync(user);
            return new IdentityOperationResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<IdentityOperationResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return new IdentityOperationResult { Succeeded = false, Errors = ["User not found."] };

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return new IdentityOperationResult
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<string> GenerateRefreshTokenAsync(string userId)
        {
            // Eskimiş refresh token'ları temizle
            var oldTokens = await _context.RefreshTokens
                .Where(r => r.UserId == userId && !r.IsRevoked && !r.IsUsed)
                .ToListAsync();

            foreach (var old in oldTokens)
                old.IsRevoked = true;

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = _tokenService.GenerateRefreshToken(),
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<RefreshTokenResult> RefreshTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (token is null || !token.IsActive)
                return new RefreshTokenResult { Succeeded = false, Error = "Invalid or expired refresh token." };

            // Eski token'ı kullanıldı olarak işaretle
            token.IsUsed = true;

            // Yeni token'lar üret
            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = _tokenService.GenerateRefreshToken(),
                UserId = token.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            var newAccessToken = _tokenService.GenerateToken(token.UserId, token.User.Email!);

            return new RefreshTokenResult
            {
                Succeeded = true,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == refreshToken);

            if (token is null || !token.IsActive)
                return false;

            token.IsRevoked = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}