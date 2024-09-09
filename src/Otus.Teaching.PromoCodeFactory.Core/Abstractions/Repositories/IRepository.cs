using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IRepository<T>
        where T: BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
        
        Task<T> GetByIdAsync(Guid id, CancellationToken token);
        Task AddAsync(T entity, CancellationToken token);
        Task UpdateAsync(T entity, CancellationToken token);
        Task DeleteAsync(Guid id, CancellationToken token);
    }
}