using System.Linq.Expressions;

namespace protocol.rabbitqm.shared.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<List<TEntity>> GetAllAsync();
        Task<bool> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate);
        // Paginacao
        Task<List<TEntity>> GetPaginationAsync(int page, int pageSize);
    }
}
