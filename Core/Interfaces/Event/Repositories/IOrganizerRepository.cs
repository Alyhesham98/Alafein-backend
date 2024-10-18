using Core.DTOs.User.Request;
using Core.DTOs.User.Response;
using Core.Entities.Event;
using Core.Interfaces.Base;

namespace Core.Interfaces.Event.Repositories
{
    public interface IOrganizerRepository : IGenericRepository<Organizer>
    {
        Task<(int Count, IList<ListEventOrganizerDto>? Data)> GetPaginaton(EventOrganizerParameters parameters);
        Task<int> GetLastTaskOrderId();
    }
}
