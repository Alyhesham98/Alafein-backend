using Core.Enums;

namespace Core.DTOs.Dashboard.Request
{
    public class DashboardListParameters
    {
        public DashboardOption? Count { get; set; }
        public DashboardOption? Categories { get; set; }
        public DashboardOption? Venues { get; set; }
        public DashboardOption? Organizer { get; set; }
        public DashboardOption? TopUsers { get; set; }
        public DashboardOption? SystemOverview { get; set; }
    }
}
