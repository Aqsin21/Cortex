using Cortex.Module.Auth.Application.Abstraction;
using Cortex.Module.Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
