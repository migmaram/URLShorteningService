using System.Linq.Expressions;

namespace URLShorteningService.Tools
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> GetByKeyAsync(string key);
        Task AddAsync(TEntity data);
        Task<bool> AnyByKeyAsync(string key);
        Task<int> FilteredCountAsync(Expression<Func<TEntity, bool>> filter);
        Task<bool> DeleteByKeyAsync(string key);
        void Update(TEntity data);
        Task SaveAsync();
    }
}
