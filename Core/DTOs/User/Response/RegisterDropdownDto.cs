using Core.DTOs.LookUps.Facility.Response;
using Core.DTOs.Role;
using DTOs.Shared;

namespace Core.DTOs.User.Response
{
    public class RegisterDropdownDto
    {
        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();
        public IList<DropdownViewModel> Category { get; set; } = new List<DropdownViewModel>();
        public IList<ListFacilityDto> Facility { get; set; } = new List<ListFacilityDto>();
        public IList<DropdownViewModel> Days { get; set; } = new List<DropdownViewModel>();
    }
}
