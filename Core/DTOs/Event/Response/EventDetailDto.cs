using Core.DTOs.LookUps.Category.Response;
using Core.DTOs.User.Response;
using DTOs.Shared;

namespace Core.DTOs.Event.Response
{
    public class EventDetailDto
    {
        public long Id { get; set; }
        public string Poster { get; set; } = string.Empty;
        public CategoryDropdownDto Category { get; set; } = new CategoryDropdownDto();
        public bool IsFavourite { get; set; }
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public IdentityDropdownDto Organizer { get; set; } = new IdentityDropdownDto();
        public string Description { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public DropdownViewModel AttendanceOption { get; set; } = new DropdownViewModel();
        public string URL { get; set; } = string.Empty;
        public decimal PaymentFee { get; set; }
        public string Address { get; set; } = string.Empty;
        public string MapLink { get; set; } = string.Empty;
        public UserDetailDto EventOrganizer { get; set; } = new UserDetailDto();
        public UserDetailDto Venue { get; set; } = new UserDetailDto();
    }
}
