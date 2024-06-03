using Core.DTOs.Event.Request;
using Core.DTOs.Event.Response;
using Core.Entities.Event;
using Core.Interfaces.Base;
using System.Linq.Expressions;

namespace Core.Interfaces.Event.Repositories
{
    public interface ISubmissionDateRepository : IGenericRepository<SubmissionDate>
    {
        Task<(int Count, IList<ListEventDto>? Data)> GetEvents(EventListParameters parameters, Expression<Func<SubmissionDate, bool>>? filter);
        Task<(int Count, IList<ListEventMobileDto>? Data)> GetEventsMobile(EventMobileListParameters parameters, Expression<Func<SubmissionDate, bool>>? filter, string userId);
        Task<IList<HomeEventDto>> Home(HomeEventListParameters parameters, Expression<Func<SubmissionDate, bool>>? filter);
        Task<List<SubmissionDate>> GetBySubmissionId(long? submissionId);
        Task<bool> SpotlightOrder(long id, int order);
        Task<bool> DeleteSubmissionDates(long submissionId);
    }
}
