using Cortex.Module.Auth.Application.Abstraction;
using Cortex.Module.Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace Cortex.Module.Auth.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;

        public IdentityService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
            {
                return new LoginOperationResult { Succeeded = false, Error = "Email or Password is wrong." };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
            {
                return new LoginOperationResult { Succeeded = false, Error = "Email or Password is wrong." };
            }

            if (!user.IsActive)
            {
                return new LoginOperationResult { Succeeded = false, Error = "Account is not active." };
            }
            return new LoginOperationResult
            {
                Succeeded = true,
                UserId = user.Id,
                Email = user.Email
            };
        }
    }
}