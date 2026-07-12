using Cortex.Module.Issues.Application.Abstraction;
using MediatR;

namespace Cortex.Module.Issues.Application.Roles.GetAllRoles
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
    {
        private readonly IWorkSpaceRoleRepository _roleRepository;

        public GetAllRolesQueryHandler(IWorkSpaceRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllAsync(cancellationToken);

            return roles.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToList();
        }
    }
}