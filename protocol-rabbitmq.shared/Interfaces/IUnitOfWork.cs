using Microsoft.EntityFrameworkCore;

namespace protocol.rabbitmq.shared.Interfaces
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        TContext Context { get; }
        int Complete();
        Task<int> CompleteAsync();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        IRepository<TEntity> NewRepository<TEntity>() where TEntity : class;

        // Se a base esta Acessivel
        Task<bool> IsAvailableAsync(string connectionstring = "");
    }
}
