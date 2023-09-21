﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T: BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }
        
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task AddAsync(T entity) {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity) {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids) {
            throw new NotImplementedException();
        }

        public Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate) {
            throw new NotImplementedException();
        }
    }
}