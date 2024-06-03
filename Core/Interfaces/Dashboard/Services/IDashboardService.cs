using Core.DTOs.Dashboard.Request;
using Core.DTOs.Dashboard.Response;
using DTOs.Shared;
using DTOs.Shared.Responses;

namespace Core.Interfaces.Dashboard.Services
{
    public interface IDashboardService
    {
        Task<Response<DashboardDto>> Dashboard(DashboardListParameters filter);
        Task<Response<List<DropdownViewModel>>> Dropdown();
    }
}
