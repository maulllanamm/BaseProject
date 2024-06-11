using EFCore.BulkExtensions;
using Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly DataContext _context;

        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> Count()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public async Task<List<TEntity>> GetAll(int page)
        {
            var pageResult = 5f;
            var items = await _context.Set<TEntity>()
                                      .Skip((page - 1) * (int)pageResult)
                                      .Take((int)pageResult)
                                      .ToListAsync();
            return items;
        }

        public async Task<List<TEntity>> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }

        public async Task<List<TEntity>> GetByListId(List<int> listId)
        {
            return _context.Set<TEntity>().Where(e => listId.Contains(e.id)).ToList();
        }

        public async Task<List<TEntity>> GetByListProperty(string field, string[] values)
        {
            // Membuat parameter ekspresi
            // Pastikan 'TEntity' adalah nama model entitas yang sesuai
            IQueryable<TEntity> query = _context.Set<TEntity>();

            // Filter berdasarkan properti 'field' dan nilai-nilai 'values'
            query = query.Where(e => values.Contains(EF.Property<string>(e, field)));

            // Eksekusi query dan ambil hasilnya
            List<TEntity> result = await query.ToListAsync();

            return result;
        }


        public async Task<TEntity> GetById(int id)
        {
            return _context.Set<TEntity>().FirstOrDefault(e => e.id == id);
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            entity.created_date ??= DateTime.UtcNow;
            entity.created_by ??= "system";
            entity.modified_date ??= DateTime.UtcNow;
            entity.modified_by ??= "system";
            entity.is_deleted = false;

            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync(); // Simpan perubahan ke database
            return entity;
        }

        public async Task<int> CreateBulk(List<TEntity> entities)
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


        public async Task<TEntity> Update(TEntity entity)
        {
            var editedEntity = _context.Set<TEntity>().FirstOrDefault(e => e.id == entity.id);

            if (editedEntity != null)
            {
                // Update properti dari editedEntity dengan nilai dari TEntity yang baru
                _context.Entry(editedEntity).CurrentValues.SetValues(entity);

                _context.SaveChanges();
            }

            return entity;
        }

        public async Task<int> UpdateBulk(List<TEntity> entities)
        {
            await _context.Set<TEntity>().BatchUpdateAsync(entities);

            return entities.Count();
        }

        public async Task<int> Delete(int id)
        {
            var entityToDelete = _context.Set<TEntity>().FirstOrDefault(e => e.id == id);
            if (entityToDelete != null)
            {
                _context.Set<TEntity>().Remove(entityToDelete);
                await _context.SaveChangesAsync(); // Simpan perubahan ke database
            }

            return id;
        }

        public async Task<int> DeleteBulk(List<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);

            return entities.Count();
        }

        public async Task<int> SoftDelete(int id)
        {
            var entityToDelete = _context.Set<TEntity>().FirstOrDefault(e => e.id == id);
            if (entityToDelete != null)
            {
                entityToDelete.is_deleted = true;
                _context.SaveChanges();
            }

            return id;
        }

        public async Task<int> SoftDeleteBulk(List<int> entitiesId)
        {
            var entitiesToDelete = _context.Set<TEntity>().Where(x => entitiesId.Contains(x.id)).ToList();
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

        public IEnumerable<TEntity> Filter()
        {
            return _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate)
        {
            return _context.Set<TEntity>().Where(predicate);
        }
    }
}
