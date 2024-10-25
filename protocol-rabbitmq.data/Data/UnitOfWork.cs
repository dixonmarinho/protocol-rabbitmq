﻿using Microsoft.EntityFrameworkCore;
using protocol.rabbitmq.shared.Interfaces;

namespace protocol.rabbitmq.data.Data
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IServiceLog log;

        public UnitOfWork(TContext context, IServiceLog log)
        {
            _context = context;
            this.log = log;
        }

        public async Task<bool> IsAvailableAsync(string connectionstring = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(connectionstring))
                {
                    _context.Database.GetDbConnection().ConnectionString = connectionstring;
                }
                return await _context.Database.CanConnectAsync();
            }
            catch
            {
                return false;
            }
        }


        public TContext Context => _context;

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_context, log);
        }

        public IRepository<TEntity> NewRepository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_context, log);
        }



    }

}
