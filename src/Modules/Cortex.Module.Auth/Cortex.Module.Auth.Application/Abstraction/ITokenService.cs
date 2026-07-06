using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cortex.Module.Auth.Application.Abstraction
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string email);
    }
}
