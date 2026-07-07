using Cortex.Module.Issues.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortex.Module.Issues.Application.Abstraction
{
    public interface IWorkSpaceRoleRepository
    {
        Task<WorkSpaceRole?> GetByNameAsync(string name, CancellationToken cancellationToken);
        Task AddAsync(WorkSpaceRole role, CancellationToken cancellationToken);
    }
}
