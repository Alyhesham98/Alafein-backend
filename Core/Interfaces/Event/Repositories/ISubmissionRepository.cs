using Core.Entities.Event;
using Core.Interfaces.Base;

namespace Core.Interfaces.Event.Repositories
{
    public interface ISubmissionRepository : IGenericRepository<Submission>
    {
        Task<decimal> MinFee();
        Task<decimal> MaxFee();
    }
}
