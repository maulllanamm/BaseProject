using Entities.Base;

namespace Repositories.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        Task<TEntity> Create(TEntity entity);
        Task<int> CreateBulk(List<TEntity> entites);
        Task<int> Delete(int id);
        Task<int> DeleteBulk(List<TEntity> entites);
        Task<int> SoftDelete(int id);
        Task<int> SoftDeleteBulk(List<int> entitesId);
        Task<TEntity> Update(TEntity entity);
        Task<int> UpdateBulk(List<TEntity> entites);
        Task<List<TEntity>> GetAll();
        Task<List<TEntity>> GetAll(int page);
        Task<List<TEntity>> GetByListId(List<int> listId);
        Task<List<TEntity>> GetByListProperty(string field, string[] values);
        Task<TEntity> GetById(int id);
        IEnumerable<TEntity> Filter();
        IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate);
        Task<int> Count();
    }
}
