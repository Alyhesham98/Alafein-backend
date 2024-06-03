using Core.Entities.Event;
using Core.Interfaces.Event.Repositories;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;
using Presistence.Repositories.Base;

namespace Presistence.Repositories.Event
{
    internal sealed class SubmissionRepository : GenericRepository<Submission>, ISubmissionRepository
    {
        private readonly ApplicationDbContext _context;
        public SubmissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<decimal> MinFee()
        {
            return await _context.Submissions
                                 .Where(f => !f.IsDeleted)
                                 .Select(s => s.PaymentFee)
                                 .MinAsync();
        }

        public async Task<decimal> MaxFee()
        {
            return await _context.Submissions
                                 .Where(f => !f.IsDeleted)
                                 .Select(s => s.PaymentFee)
                                 .MaxAsync();
        }
    }
}
