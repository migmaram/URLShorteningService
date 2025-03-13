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

        public void Add(TEntity data) => _dbSet.Add(data);

        public bool AnyByKey(string key) => _dbSet.Any(entity =>
            EF.Property<string>(entity, "Key") == key);

        public int FilteredCount(Expression<Func<TEntity, bool>> filter) => _dbSet.Where(filter).Count();

        public bool DeleteByKey(string key)
        {
            var toDelete = _dbSet.FirstOrDefault(entity =>
            EF.Property<string>(entity, "Key") == key);

            if (toDelete == null)
                return false;

            _dbSet.Remove(toDelete);
            return true;
        }
        public TEntity GetByKey(string key) => _dbSet.FirstOrDefault(entity =>
            EF.Property<string>(entity, "Key") == key) ??
            throw new NullReferenceException($"Not record found with the key: {key} :(");

        public void Save() => _context.SaveChanges();

        public void Update(TEntity data)
        {
            _dbSet.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
        }


    }
}
