using GiftShop.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GiftShop.Repositories
{
    public class EntityFrameworkReadOnlyRepository<TContext> : IReadOnlyRepository where TContext : DbContext
    {
        protected readonly TContext context;

        public EntityFrameworkReadOnlyRepository(TContext context)
        {
            this.context = context;
        }

        public virtual IQueryable<TEntity> GetQueryable<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where TEntity : class, IEntityBase
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return query;
        }

        public IEnumerable<TEntity> ListBy<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where TEntity : class, IEntityBase, new()
        {
            return GetQueryable(filter, orderBy, includeProperties, skip, take).ToList();
        }

        public IEnumerable<TEntity> List<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where TEntity : class, IEntityBase, new()
        {
            return GetQueryable(null, orderBy, includeProperties, skip, take).ToList();
        }

        public async Task<IEnumerable<TEntity>> ListAsync<TEntity>(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where TEntity : class, IEntityBase, new()
        {
            return await GetQueryable(null, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> ListByAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, int? skip = null, int? take = null) where TEntity : class, IEntityBase, new()
        {
            return await GetQueryable(filter, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public TEntity FindByID<TEntity>(object id) where TEntity : class, IEntityBase, new()
        {
            return context.Set<TEntity>().Find(id);
        }

        public async Task<TEntity> FindByIDAsync<TEntity>(object id) where TEntity : class, IEntityBase, new()
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public int Count<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntityBase, new()
        {
            return GetQueryable(filter).Count();
        }

        public async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntityBase, new()
        {
            return await GetQueryable(filter).CountAsync();
        }

        public bool Exists<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntityBase, new()
        {
            return GetQueryable(filter).Any();
        }

        public async Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class, IEntityBase, new()
        {
            return await GetQueryable(filter).AnyAsync();
        }

        public TEntity FindBy<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null) where TEntity : class, IEntityBase, new()
        {
            return GetQueryable(filter, null, includeProperties).SingleOrDefault();
        }

        public Task<TEntity> FindByAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = null) where TEntity : class, IEntityBase, new()
        {
            return GetQueryable(filter, null, includeProperties).SingleOrDefaultAsync();
        }
    }
}