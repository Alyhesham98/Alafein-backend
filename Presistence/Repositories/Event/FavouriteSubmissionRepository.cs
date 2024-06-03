using Core.Entities.Event;
using Core.Interfaces.Event.Repositories;
using Microsoft.EntityFrameworkCore;
using Presistence.Contexts;

namespace Presistence.Repositories.Event
{
    internal sealed class FavouriteSubmissionRepository : IFavouriteSubmissionRepository
    {
        private readonly ApplicationDbContext _context;
        public FavouriteSubmissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(FavouriteSubmission favouriteSubmission)
        {
            _context.Add(favouriteSubmission);
        }

        public void Remove(FavouriteSubmission favouriteSubmission)
        {
            _context.Remove(favouriteSubmission);
        }


        public void RemoveRange(IList<FavouriteSubmission> favouriteSubmissions)
        {
            _context.FavouriteSubmissions.RemoveRange(favouriteSubmissions);
        }

        public async Task<bool> Exists(string userId, long submissionId)
        {
            return await _context.FavouriteSubmissions
                                 .AnyAsync(f => f.UserId == userId &&
                                                f.SubmissionId == submissionId);
        }

        public async Task<FavouriteSubmission?> Get(string userId, long submissionId)
        {
            return await _context.FavouriteSubmissions
                                 .Where(f => f.UserId == userId &&
                                             f.SubmissionId == submissionId)
                                 .FirstOrDefaultAsync();
        }
    }
}
