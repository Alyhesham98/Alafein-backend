using Core.Entities.Event;
using Core.Interfaces.Event.Repositories;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Event
{
    internal sealed class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        private readonly ApplicationDbContext _context;
        public PhotoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
