using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cortex.Module.Auth.Application.Users.SearchByEmail
{
    public class SearchUserByEmailQuery : IRequest<SearchUserResult>
    {
        public required string Email { get; set; }
    }

    public class SearchUserResult
    {
        public bool Found { get; set; }
        public string? UserId { get; set; }
        public string? FullName { get; set; }
    }
}
