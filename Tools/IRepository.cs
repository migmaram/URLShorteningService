using System.Linq.Expressions;

namespace URLShorteningService.Tools
{
    public interface IRepository<TEntity>
    {
        TEntity GetByKey(string key);
        void Add(TEntity data);
        bool AnyByKey(string key);
        int FilteredCount(Expression<Func<TEntity, bool>> filter);
        bool DeleteByKey(string key);
        void Update(TEntity data);
        void Save();
    }
}
