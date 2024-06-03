using Core.Entities.Event;

namespace Core.Interfaces.Event.Repositories
{
    public interface IFavouriteSubmissionRepository
    {
        void Add(FavouriteSubmission favouriteSubmission);
        void Remove(FavouriteSubmission favouriteSubmission);
        void RemoveRange(IList<FavouriteSubmission> favouriteSubmissions);
        Task<bool> Exists(string userId, long submissionId);
        Task<FavouriteSubmission?> Get(string userId, long submissionId);
    }
}
