using Core.Entities.Event;
using Core.Interfaces.Event.Repositories;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Event
{
    internal sealed class BranchRepository : GenericRepository<Branch>, IBranchRepository
    {
        private readonly ApplicationDbContext _context;
        public BranchRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
