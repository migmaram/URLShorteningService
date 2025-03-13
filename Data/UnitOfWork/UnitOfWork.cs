using URLShorteningService.Data;
using URLShorteningService.Models;

namespace URLShorteningService.Tools
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;
        public IRepository<Url>? _urls;
        public IRepository<Visit>? _visits;
        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
        }

        public IRepository<Url> Urls
        {
            get 
            {
                return _urls ?? new Repository<Url>(_context);
            }
        }
        public IRepository<Visit> Visits
        {
            get
            {
                return _visits ?? new Repository<Visit>(_context);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
