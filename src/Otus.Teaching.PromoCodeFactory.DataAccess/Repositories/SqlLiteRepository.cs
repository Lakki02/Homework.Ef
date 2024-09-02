using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dataDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _dataDbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            T? enity = await GetByIdAsync(id);
            if (enity != null)
            {
                _dbSet.Remove(enity);
                await _dataDbContext.SaveChangesAsync();
            }
        }
    }
}
