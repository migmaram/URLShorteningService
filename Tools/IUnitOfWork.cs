using URLShorteningService.Models;

namespace URLShorteningService.Tools
{
    public interface IUnitOfWork
    {
        public IRepository<Url> Urls { get; }
        public IRepository<Visit> Visits { get; }
        public Task SaveAsync();
    }
}
