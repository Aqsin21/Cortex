using MediatR;

namespace Cortex.Module.Issues.Application.Roles.GetAllRoles
{
    public class GetAllRolesQuery : IRequest<List<RoleDto>> { }

    public class RoleDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}