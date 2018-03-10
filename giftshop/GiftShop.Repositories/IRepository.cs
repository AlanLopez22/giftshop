using GiftShop.Entities;
using System.Threading.Tasks;

namespace GiftShop.Repositories
{
    public interface IRepository : IReadOnlyRepository
    {
        void Create<TEntity>(TEntity entity) where TEntity : class, IEntityBase;

        void Update<TEntity>(TEntity entity) where TEntity : class, IEntityBase;

        void Delete<TEntity>(object id) where TEntity : class, IEntityBase;

        void Delete<TEntity>(TEntity entity) where TEntity : class, IEntityBase;

        int Save<TEntity>() where TEntity : class, IEntityBase;

        Task<int> SaveAsync<TEntity>() where TEntity : class, IEntityBase;
    }
}