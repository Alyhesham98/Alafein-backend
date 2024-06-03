using Core.Entities.LookUps;
using Core.Interfaces.LookUps.Repositories;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.LookUps
{
    internal sealed class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
