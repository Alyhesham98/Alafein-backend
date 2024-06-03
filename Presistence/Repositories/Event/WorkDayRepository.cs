using Core.Entities.Event;
using Core.Interfaces.Event.Repositories;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Event
{
    internal sealed class WorkDayRepository : GenericRepository<WorkDay>, IWorkDayRepository
    {
        private readonly ApplicationDbContext _context;
        public WorkDayRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
