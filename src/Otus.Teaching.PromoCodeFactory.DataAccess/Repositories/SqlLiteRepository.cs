using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class SqlLiteRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataDbContext _dataDbContext;
        private readonly DbSet<T> _dbSet;

        public SqlLiteRepository(DataDbContext context)
        {
            _dataDbContext = context;
            _dbSet = _dataDbContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken token)
        {
            return await _dbSet.AsNoTracking().ToListAsync(token);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken token)
        {
            return await _dbSet.FindAsync(id, token);
        }

        public async Task AddAsync(T entity, CancellationToken token)
        {
            await _dbSet.AddAsync(entity, token);
            await _dataDbContext.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(T entity, CancellationToken token)
        {
            _dbSet.Update(entity);
            await _dataDbContext.SaveChangesAsync(token);
        }
        public async Task DeleteAsync(Guid id, CancellationToken token)
        {
            T? enity = await GetByIdAsync(id, token);
            if (enity != null)
            {
                _dbSet.Remove(enity);
                await _dataDbContext.SaveChangesAsync(token);
            }
        }
    }
}
