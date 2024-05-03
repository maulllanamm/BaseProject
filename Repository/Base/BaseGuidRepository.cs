using Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Base
{
    public class BaseGuidRepository<TGuidEntity> : IBaseGuidRepository<TGuidEntity> where TGuidEntity : class, IBaseGuidEntity
    {
        private readonly DataContext _context;

        public BaseGuidRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> Count()
        {
            return await _context.Set<TGuidEntity>().CountAsync();
        }

        public async Task<List<TGuidEntity>> GetAll(int page)
        {
            var pageResult = 5f;
            var items = await _context.Set<TGuidEntity>()
                                      .Skip((page - 1) * (int)pageResult)
                                      .Take((int)pageResult)
                                      .ToListAsync();
            return items;
        }

        public async Task<List<TGuidEntity>> GetAll()
        {
            return _context.Set<TGuidEntity>().ToList();
        }

        public async Task<List<TGuidEntity>> GetByListId(List<Guid> listId)
        {
            return _context.Set<TGuidEntity>().Where(e => listId.Contains(e.id)).ToList();
        }

        public async Task<List<TGuidEntity>> GetByListProperty(string field, string[] values)
        {
            // Membuat parameter ekspresi
            // Pastikan 'TGuidEntity' adalah nama model entitas yang sesuai
            IQueryable<TGuidEntity> query = _context.Set<TGuidEntity>();

            // Filter berdasarkan properti 'field' dan nilai-nilai 'values'
            query = query.Where(e => values.Contains(EF.Property<string>(e, field)));

            // Eksekusi query dan ambil hasilnya
            List<TGuidEntity> result = await query.ToListAsync();

            return result;
        }


        public async Task<TGuidEntity> GetById(Guid id)
        {
            return _context.Set<TGuidEntity>().FirstOrDefault(e => e.id == id);
        }

        public async Task<TGuidEntity> Create(TGuidEntity entity)
        {
            entity.created_date ??= DateTime.UtcNow;
            entity.created_by ??= "system";
            entity.modified_date ??= DateTime.UtcNow;
            entity.modified_by ??= "system";
            entity.is_deleted = false;

            await _context.Set<TGuidEntity>().AddAsync(entity);
            await _context.SaveChangesAsync(); // Simpan perubahan ke database
            return entity;
        }

        public async Task<int> CreateBulk(List<TGuidEntity> entities)
        {

            entities = entities.Select(x =>
            {
                x.is_deleted ??= false;
                x.created_by ??= "system";
                x.created_date = DateTime.UtcNow;
                x.modified_by ??= "system";
                x.modified_date = DateTime.UtcNow;
                return x;
            }).ToList();

            var splitSize = 10000;
            if (entities.Count >= 100000)
            {
                splitSize *= 2;
            }

            var batches = entities
                        .Select((entities, index) => (entities, index))
                        .GroupBy(pair => pair.index / splitSize)
                        .Select(group => group.Select(pair => pair.entities).ToList())
                        .ToList();

            foreach (var batch in batches)
            {
                await _context.BulkInsertAsync(batch);
            }
            return entities.Count();
        }


        public async Task<TGuidEntity> Update(TGuidEntity entity)
        {
            var editedEntity = _context.Set<TGuidEntity>().FirstOrDefault(e => e.id == entity.id);

            if (editedEntity != null)
            {
                // Update properti dari editedEntity dengan nilai dari TGuidEntity yang baru
                _context.Entry(editedEntity).CurrentValues.SetValues(entity);

                _context.SaveChanges();
            }

            return entity;
        }

        public async Task<int> UpdateBulk(List<TGuidEntity> entities)
        {
            _context.Set<TGuidEntity>().BulkUpdate(entities);

            return entities.Count();
        }

        public async Task<Guid> Delete(Guid id)
        {
            var entityToDelete = _context.Set<TGuidEntity>().FirstOrDefault(e => e.id == id);
            if (entityToDelete != null)
            {
                _context.Set<TGuidEntity>().Remove(entityToDelete);
                await _context.SaveChangesAsync(); // Simpan perubahan ke database
            }

            return id;
        }

        public async Task<int> DeleteBulk(List<TGuidEntity> entities)
        {
            _context.Set<TGuidEntity>().BulkDelete(entities);

            return entities.Count();
        }

        public async Task<Guid> SoftDelete(Guid id)
        {
            var entityToDelete = _context.Set<TGuidEntity>().FirstOrDefault(e => e.id == id);
            if (entityToDelete != null)
            {
                entityToDelete.is_deleted = true;
                _context.SaveChanges();
            }

            return id;
        }

        public async Task<int> SoftDeleteBulk(List<Guid> entitiesId)
        {
            var entitiesToDelete = _context.Set<TGuidEntity>().Where(x => entitiesId.Contains(x.id)).ToList();
            if (entitiesToDelete != null)
            {
                entitiesToDelete = entitiesToDelete.Select(x =>
                {
                    x.is_deleted = true;
                    return x;
                }).ToList();
            }
            return entitiesToDelete.Count();
        }

        public IEnumerable<TGuidEntity> Filter()
        {
            return _context.Set<TGuidEntity>();
        }

        public IEnumerable<TGuidEntity> Filter(Func<TGuidEntity, bool> predicate)
        {
            return _context.Set<TGuidEntity>().Where(predicate);
        }
    }
}
