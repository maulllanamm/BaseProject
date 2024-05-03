using Entities.Base;

namespace Repositories.Base
{
    public interface IBaseGuidRepository<TGuidEntity> where TGuidEntity : class, IBaseGuidEntity
    {
        Task<TGuidEntity> Create(TGuidEntity entity);
        Task<int> CreateBulk(List<TGuidEntity> entites);
        Task<Guid> Delete(Guid id);
        Task<int> DeleteBulk(List<TGuidEntity> entites);
        Task<Guid> SoftDelete(Guid id);
        Task<int> SoftDeleteBulk(List<Guid> entitesId);
        Task<TGuidEntity> Update(TGuidEntity entity);
        Task<int> UpdateBulk(List<TGuidEntity> entites);
        Task<List<TGuidEntity>> GetAll();
        Task<List<TGuidEntity>> GetAll(int page);
        Task<List<TGuidEntity>> GetByListId(List<Guid> listId);
        Task<List<TGuidEntity>> GetByListProperty(string field, string[] values);
        Task<TGuidEntity> GetById(Guid id);
        IEnumerable<TGuidEntity> Filter();
        IEnumerable<TGuidEntity> Filter(Func<TGuidEntity, bool> predicate);
        Task<int> Count();
    }
}
