using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using protocol.rabbitmq.shared;
using protocol.rabbitmq.shared.Interfaces;
using System.Linq.Expressions;

namespace protocol.rabbitmq.data.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly IServiceLog log;
        private readonly DbSet<TEntity> _dbSet;
        private BulkConfig bulkConfig;


        public Repository(DbContext context, IServiceLog log)
        {
            this._context = context;
            this.log = log;
            this._dbSet = context.Set<TEntity>();

            //TODO: Prevent Degradation of performance
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            // Configuracao do BulkConfig
            bulkConfig = new BulkConfig
            {
                BatchSize = 10000, // Tamanho do lote a Ser Processado EX: Se enviar 25000 Envia 10000, 10000 e 5000
                UseTempDB = true, // Usar TempDB para melhorar a velocidade
                SetOutputIdentity = false, // Desativar se IDs gerados pelo banco não são necessários
                CalculateStats = false, // Evitar cálculos de estatísticas desnecessárias
                BulkCopyTimeout = 600 // Tempo de espera para o BulkCopy
            };
            _dbSet = context.Set<TEntity>();
        }

        public async Task<Result<TEntity>> GetByIdAsync(int id)
        {
            try
            {
                var response = await _dbSet.FindAsync(id);
                return Result<TEntity>.Success(response);
            }
            catch (Exception e)
            {
                log.Error(e);
                return Result<TEntity>.Fail("Erro ao buscar dados por id");
            }
        }

        public async Task<Result<List<TEntity>>> GetAllAsync()
        {
            try
            {
                var response = await _dbSet.ToListAsync();
                return Result<List<TEntity>>.Success(response);
            }
            catch (Exception e)
            {
                log.Error(e);
                return Result<List<TEntity>>.Fail("Erro ao buscar dados");
            }
        }

        public async Task<Result<bool>> AddAsync(TEntity entity)
        {
            try
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success();
            }
            catch (Exception e)
            {
                log.Error(e);
                return Result<bool>.Fail("Erro ao adicionar dados");
            }
        }

        public async Task<Result<List<TEntity>>> AddBatchAsync(List<TEntity> entity)
        {
            // TODO : Usa transação para garantir a integridade
            using (var transaction = _context.Database.BeginTransaction())
            {
                await _context.BulkInsertAsync(entity);
                await transaction.CommitAsync();
            }
            return await Result<List<TEntity>>.SuccessAsync(entity);
        }


        public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Result<TEntity>.Success(entity);
            }
            catch (Exception e)
            {
                log.Error(e);
                return Result<TEntity>.Fail("Erro ao atualizar dados");
            }
        }

        public async Task<Result<bool>> DeleteAsync(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success();
            }
            catch (Exception e)
            {
                log.Error(e);
                return Result<bool>.Fail("Erro ao Excluir");
            }
        }

        public async Task<Result<TEntity>> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var response = await _dbSet.FirstOrDefaultAsync(predicate);
                return Result<TEntity>.Success(response);
            }
            catch (Exception e)
            {
                log.Error(e);
                return Result<TEntity>.Fail("Erro buscar informação");
            }
        }

        public async Task<Result<List<TEntity>>> ListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var responseCollection = await _dbSet.Where(predicate).ToListAsync();
                return Result<List<TEntity>>.Success(responseCollection);
            }
            catch (Exception e)
            {
                log.Error(e);
                return Result<List<TEntity>>.Fail("Erro listar informação");
            }
        }

        public async Task<Result<List<TEntity>>> GetPaginationAsync(int page, int pageSize)
        {
            try
            {
                var responseCollection = await _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                return Result<List<TEntity>>.Success(responseCollection);
            }
            catch (Exception e)
            {
                log.Error(e);
                return Result<List<TEntity>>.Fail("Erro listar informação");
            }


        }
    }

}
