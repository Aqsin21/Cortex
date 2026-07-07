namespace Cortex.Module.Issues.Application.Abstraction
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
