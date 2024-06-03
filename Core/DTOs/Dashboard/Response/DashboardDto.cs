namespace Core.DTOs.Dashboard.Response
{
    public class DashboardDto
    {
        public int Event { get; set; }
        public int Users { get; set; }
        public int EventOragnizer { get; set; }
        public int Venue { get; set; }
        public IList<DashboardCategoryDto> TopCategories { get; set; } = new List<DashboardCategoryDto>();
        public IReadOnlyList<TopUserDto> TopUsers { get; set; } = new List<TopUserDto>();
        public IList<DashboardVenueDto> TopVenues { get; set; } = new List<DashboardVenueDto>();
        public IList<DashboardOrganizerDto> TopOrganizers { get; set; } = new List<DashboardOrganizerDto>();
        public IList<DashboardEventDto> SystemOverview { get; set; } = new List<DashboardEventDto>();
    }
}
