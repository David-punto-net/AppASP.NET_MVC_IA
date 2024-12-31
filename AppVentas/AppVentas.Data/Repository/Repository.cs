using Microsoft.EntityFrameworkCore;

namespace AppVentas.Data.Repository
{
    public class Repository<TId, TEntity> : IRepository<TId, TEntity> where TEntity : class
    {
        protected readonly DataContext _context;
        protected DbSet<TEntity> Entities => _context.Set<TEntity>();

        public Repository(DataContext context)
        {
            _context = context;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _context.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> GetByIdAsync(TId id)
        {
            try
            {
                return await _context.FindAsync<TEntity>(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve the entity with id {id}: {ex.Message}");
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity must not be null");
            }

            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error saving entity: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity must not be null");
            }

            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error updating entity: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        public virtual async Task DeleteAsync(TId id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}