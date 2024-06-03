using Core.Entities.LookUps;
using Core.Interfaces.LookUps.Repositories;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.LookUps
{
    internal sealed class FacilityRepository : GenericRepository<Facility>, IFacilityRepository
    {
        private readonly ApplicationDbContext _context;
        public FacilityRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
