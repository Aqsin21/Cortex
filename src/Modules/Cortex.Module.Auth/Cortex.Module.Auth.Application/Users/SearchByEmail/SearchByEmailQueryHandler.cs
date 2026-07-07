using Cortex.Module.Auth.Application.Abstraction;
using MediatR;
namespace Cortex.Module.Auth.Application.Users.SearchByEmail
{
    public class SearchUserByEmailQueryHandler : IRequestHandler<SearchUserByEmailQuery, SearchUserResult>
    {
        private readonly IIdentityService _identityService;

        public SearchUserByEmailQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<SearchUserResult> Handle(SearchUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmailAsync(request.Email);

            if (user is null)
                return new SearchUserResult { Found = false };

            return new SearchUserResult
            {
                Found = true,
                UserId = user.UserId,
                FullName = user.FullName
            };
        }
    }
}
