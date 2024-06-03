using Core.Entities.Event;
using Core.Interfaces.Event.Repositories;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Event
{
    internal sealed class BlockedCommentRepository : GenericRepository<BlockedComment>, IBlockedCommentRepository
    {
        private readonly ApplicationDbContext _context;
        public BlockedCommentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
