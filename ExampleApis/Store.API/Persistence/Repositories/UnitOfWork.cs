using Store.API.Domain.Repositories;
using Store.API.Persistence.Contexts;

namespace Store.API.Persistence.Repositories
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private readonly AppDbContext _context = context;

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}