using SD.Core.Entities;
using System.Linq.Expressions;

namespace SD.Core.Repositories
{
    public interface IBaseRepository
    {
        void Add<T>(T entity, bool saveImmediately = false)
            where T : class, IEntity;

        Task AddAsync<T>(T entity, bool saveImmediately = false, CancellationToken cancellationToken = default)
            where T : class, IEntity;

        IQueryable<T> QueryFrom<T>(Expression<Func<T, bool>> whereFilter = null)
            where T : class, IEntity;

        T Update<T>(T entity, object key, bool saveImmediately = false)
            where T : class, IEntity;

        Task<T> UpdateAsync<T>(T entity, object key, bool saveImmediately = false, CancellationToken cancellationToken = default)
            where T : class, IEntity;

        void Remove<T>(T entity, bool saveImmediately = false)
            where T : class, IEntity;

        void RemoveByKey<T>(object key, bool saveImmediately = false)
            where T : class, IEntity;

        Task RemoveAsync<T>(T entity, bool saveImmediately = false, CancellationToken cancellationToken = default)
            where T : class, IEntity;

        Task RemoveByKeyAsync<T>(object key, bool saveImmediately = false, CancellationToken cancellationToken = default)
            where T : class, IEntity;

        void Save();

        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}