using Core.DTOs.LookUps.Facility.Response;
using DTOs.Shared;

namespace Core.DTOs.User.Response
{
    public class RegisterVenueDropdownDto
    {
        public IList<DropdownViewModel> Category { get; set; } = new List<DropdownViewModel>();
        public IList<ListFacilityDto> Facility { get; set; } = new List<ListFacilityDto>();
        public IList<DropdownViewModel> Days { get; set; } = new List<DropdownViewModel>();
    }
}
