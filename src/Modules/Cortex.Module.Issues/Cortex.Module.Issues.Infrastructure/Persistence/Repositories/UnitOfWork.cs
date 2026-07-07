using Cortex.Module.Issues.Application.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cortex.Module.Issues.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IssuesDbContext _context;

        public UnitOfWork(IssuesDbContext context)
        {
            _context = context;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
