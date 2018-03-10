using GiftShop.Entities;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace GiftShop.Repositories
{
    public class EntityFrameworkRepository<TContext> : EntityFrameworkReadOnlyRepository<TContext>, IRepository where TContext : DbContext
    {
        public EntityFrameworkRepository(TContext context) : base(context)
        {

        }

        public void Create<TEntity>(TEntity entity) where TEntity : class, IEntityBase
        {
            context.Set<TEntity>().Add(entity);
        }

        public void Delete<TEntity>(object id) where TEntity : class, IEntityBase
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityBase
        {
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public int Save<TEntity>() where TEntity : class, IEntityBase
        {
            try
            {
                return context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
                return -1;
            }
        }

        public async Task<int> SaveAsync<TEntity>() where TEntity : class, IEntityBase
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                ThrowEnhancedValidationException(e);
                return -1;
            }
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class, IEntityBase
        {
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        protected virtual void ThrowEnhancedValidationException(DbEntityValidationException e)
        {
            var errorMessages = e.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
            var fullErrorMessage = string.Join("; ", errorMessages);
            var exceptionMessage = string.Concat(e.Message, " The validation errors are: ", fullErrorMessage);
            throw new DbEntityValidationException(exceptionMessage, e.EntityValidationErrors);
        }
    }
}
