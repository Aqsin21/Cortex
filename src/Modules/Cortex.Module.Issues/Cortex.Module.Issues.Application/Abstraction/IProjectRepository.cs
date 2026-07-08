using Cortex.Module.Issues.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortex.Module.Issues.Application.Abstraction
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project, CancellationToken cancellationToken);
        Task AddMemberAsync(ProjectMember projectMember, CancellationToken cancellationToken);
        Task<Project> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Project>> GetByMemberUserIdAsync(string userId, Guid workSpaceId, CancellationToken cancellationToken);
        void Delete(Project project);
    }
}
