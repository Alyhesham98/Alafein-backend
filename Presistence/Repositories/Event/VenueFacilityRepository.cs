using Core.Entities.Event;
using Core.Interfaces.Event.Repositories;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Event
{
    internal sealed class VenueFacilityRepository : GenericRepository<VenueFacility>, IVenueFacilityRepository
    {
        private readonly ApplicationDbContext _context;
        public VenueFacilityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
