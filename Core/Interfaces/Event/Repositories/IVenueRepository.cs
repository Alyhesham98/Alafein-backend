using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Entities.Event;
using Core.Interfaces.Base;

namespace Core.Interfaces.Event.Repositories
{
    public interface IVenueRepository : IGenericRepository<Venue>
    {
        Task<(int Count, IList<ListVenueDto>? Data)> GetPaginaton(VenueParameters parameters);
        Task<int> GetLastTaskOrderId();
    }
}
