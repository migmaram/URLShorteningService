using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using URLShorteningService.Data;

namespace URLShorteningService.Tools
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private ApiDbContext _context;
        private DbSet<TEntity> _dbSet;

        public Repository(ApiDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity data) => await _dbSet.AddAsync(data);

        public async Task<bool> AnyByKeyAsync(string key) => await _dbSet.AnyAsync(entity =>
            EF.Property<string>(entity, "Key") == key);

        public async Task<int> FilteredCountAsync(Expression<Func<TEntity, bool>> filter) => await _dbSet.Where(filter).CountAsync();

        public async Task<bool> DeleteByKeyAsync(string key)
        {
            var toDelete = await _dbSet.FirstOrDefaultAsync(entity =>
            EF.Property<string>(entity, "Key") == key);

            if (toDelete == null)
                return false;

            _dbSet.Remove(toDelete);
            return true;
        }
        public async Task<TEntity> GetByKeyAsync(string key) => await _dbSet.FirstOrDefaultAsync(entity =>
            EF.Property<string>(entity, "Key") == key);

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public void Update(TEntity data)
        {
            _dbSet.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
        }


    }
}
