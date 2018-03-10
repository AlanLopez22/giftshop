using GiftShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GiftShop.Repositories
{
    public interface IReadOnlyRepository
    {
        IEnumerable<TEntity> List<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where TEntity : class, IEntityBase, new();

        Task<IEnumerable<TEntity>> ListAsync<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where TEntity : class, IEntityBase, new();

        IEnumerable<TEntity> ListBy<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where TEntity : class, IEntityBase, new();

        Task<IEnumerable<TEntity>> ListByAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where TEntity : class, IEntityBase, new();

        TEntity FindBy<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null) where TEntity : class, IEntityBase, new();

        Task<TEntity> FindByAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null) where TEntity : class, IEntityBase, new();

        TEntity FindByID<TEntity>(object id) where TEntity : class, IEntityBase, new();

        Task<TEntity> FindByIDAsync<TEntity>(object id) where TEntity : class, IEntityBase, new();

        int Count<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntityBase, new();

        Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntityBase, new();

        bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntityBase, new();

        Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntityBase, new();
    }
}
