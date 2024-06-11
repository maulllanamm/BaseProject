using Entities.Base;
using Repositories.Base;

namespace Repositories.ConfigUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Entity> GetBaseRepository<Entity>() where Entity : class, IBaseEntity;
        IBaseGuidRepository<GuidEntity> GetBaseGuidRepository<GuidEntity>() where GuidEntity : class, IBaseGuidEntity;
        void BeginTransaction();
        void Commit();
        void Rollback();
        int SaveChanges();
        void Dispose();
    }
}
