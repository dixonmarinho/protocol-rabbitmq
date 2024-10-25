using System.Linq.Expressions;

namespace protocol.rabbitmq.shared.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<Result<TEntity>> GetByIdAsync(int id);
        Task<Result<List<TEntity>>> GetAllAsync();
        Task<Result<bool>> AddAsync(TEntity entity);
        Task<Result<List<TEntity>>> AddBatchAsync(List<TEntity> entity);
        Task<Result<TEntity>> UpdateAsync(TEntity entity);
        Task<Result<bool>> DeleteAsync(TEntity entity);
        Task<Result<TEntity>> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<Result<List<TEntity>>> ListAsync(Expression<Func<TEntity, bool>> predicate);
        // Paginacao
        Task<Result<List<TEntity?>>> GetPaginationAsync(int page, int pageSize);
    }
}
