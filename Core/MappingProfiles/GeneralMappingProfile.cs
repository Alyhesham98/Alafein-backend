using AutoMapper;
using Core.DTOs.Alert.Request;
using Core.DTOs.Event.Request;
using Core.DTOs.LookUps.Category.Request;
using Core.DTOs.LookUps.Facility.Request;
using Core.DTOs.User;
using Core.DTOs.User.Request;
using Core.Entities.Alert;
using Core.Entities.Event;
using Core.Entities.Identity;
using Core.Entities.LookUps;
using Core.Enums;

namespace Core.MappingProfiles
{
    public class GeneralMappingProfile : Profile
    {
        public GeneralMappingProfile()
        {
            CreateMap<UserAddModel, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)Status.Normal))
                ;

            CreateMap<AdminAddUserDto, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsBlocked, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)Status.Normal))
                ;

            CreateMap<OrganizerDto, Organizer>();

            CreateMap<EditOrganizerDto, Organizer>();

            CreateMap<PhotoDto, Photo>();

            CreateMap<string, Photo>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src));

            CreateMap<long, VenueFacility>()
                .ForMember(dest => dest.FacilityId, opt => opt.MapFrom(src => src));

            CreateMap<WorkDayDto, WorkDay>();

            CreateMap<BranchDto, Branch>();

            CreateMap<VenueDto, Venue>();

            CreateMap<AddCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            CreateMap<AddFacilityDto, Facility>();
            CreateMap<UpdateFacilityDto, Facility>();

            CreateMap<DateTime, SubmissionDate>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => false))
                ;

            CreateMap<AddSubmissionDto, Submission>()
                .ForMember(dest => dest.EventNameEN, opt => opt.MapFrom(src => src.EventNameEN))
                .ForMember(dest => dest.EventNameAR, opt => opt.MapFrom(src => src.EventNameAR))
                .ForMember(dest => dest.EventDescriptionEN, opt => opt.MapFrom(src => src.EventDescriptionEN))
                .ForMember(dest => dest.EventDescriptionAR, opt => opt.MapFrom(src => src.EventDescriptionAR))
                .ForMember(dest => dest.MainArtestNameEN, opt => opt.MapFrom(src => src.MainArtestNameEN))
                .ForMember(dest => dest.MainArtestNameAR, opt => opt.MapFrom(src => src.MainArtestNameAR))
                .ForMember(dest => dest.KidsAvailability, opt => opt.MapFrom(src => src.KidsAvailability))
                .ForMember(dest => dest.AttendanceOption, opt => opt.MapFrom(src => src.AttendanceOption))
                .ForMember(dest => dest.URL, opt => opt.MapFrom(src => src.URL))
                .ForMember(dest => dest.PaymentFee, opt => opt.MapFrom(src => src.PaymentFee))
                .ForMember(dest => dest.Poster, opt => opt.MapFrom(src => src.Poster))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.AddtionalComment, opt => opt.MapFrom(src => src.AddtionalComment))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.VenueId, opt => opt.MapFrom(src => src.VenueId))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsSpotlight, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)SubmissionStatus.PENDING))
                .ForMember(dest => dest.Dates, opt => opt.MapFrom(src => src.Dates))
                ;
            CreateMap<Submission, AddAdminSubmissionDto>()
                .ForMember(dest => dest.Dates, opt => opt.MapFrom(src => src.Dates));

            CreateMap<AddAdminSubmissionDto, Submission>()
                .ForMember(dest => dest.EventNameEN, opt => opt.MapFrom(src => src.EventNameEN))
                .ForMember(dest => dest.EventNameAR, opt => opt.MapFrom(src => src.EventNameAR))
                .ForMember(dest => dest.EventDescriptionEN, opt => opt.MapFrom(src => src.EventDescriptionEN))
                .ForMember(dest => dest.EventDescriptionAR, opt => opt.MapFrom(src => src.EventDescriptionAR))
                .ForMember(dest => dest.MainArtestNameEN, opt => opt.MapFrom(src => src.MainArtestNameEN))
                .ForMember(dest => dest.MainArtestNameAR, opt => opt.MapFrom(src => src.MainArtestNameAR))
                .ForMember(dest => dest.KidsAvailability, opt => opt.MapFrom(src => src.KidsAvailability))
                .ForMember(dest => dest.AttendanceOption, opt => opt.MapFrom(src => src.AttendanceOption))
                .ForMember(dest => dest.URL, opt => opt.MapFrom(src => src.URL))
                .ForMember(dest => dest.PaymentFee, opt => opt.MapFrom(src => src.PaymentFee))
                .ForMember(dest => dest.Poster, opt => opt.MapFrom(src => src.Poster))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.AddtionalComment, opt => opt.MapFrom(src => src.AddtionalComment))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.VenueId, opt => opt.MapFrom(src => src.VenueId))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.IsSpotlight, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)SubmissionStatus.PENDING))
                .ForMember(dest => dest.Dates, opt => opt.MapFrom(src => src.Dates))
                ;

            CreateMap<UpdateAdminSubmissionDto, Submission>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EventNameEN, opt => opt.MapFrom(src => src.EventNameEN))
                .ForMember(dest => dest.EventNameAR, opt => opt.MapFrom(src => src.EventNameAR))
                .ForMember(dest => dest.EventDescriptionEN, opt => opt.MapFrom(src => src.EventDescriptionEN))
                .ForMember(dest => dest.EventDescriptionAR, opt => opt.MapFrom(src => src.EventDescriptionAR))
                .ForMember(dest => dest.MainArtestNameEN, opt => opt.MapFrom(src => src.MainArtestNameEN))
                .ForMember(dest => dest.MainArtestNameAR, opt => opt.MapFrom(src => src.MainArtestNameAR))
                .ForMember(dest => dest.KidsAvailability, opt => opt.MapFrom(src => src.KidsAvailability))
                .ForMember(dest => dest.AttendanceOption, opt => opt.MapFrom(src => src.AttendanceOption))
                .ForMember(dest => dest.URL, opt => opt.MapFrom(src => src.URL))
                .ForMember(dest => dest.PaymentFee, opt => opt.MapFrom(src => src.PaymentFee))
                .ForMember(dest => dest.Poster, opt => opt.MapFrom(src => src.Poster))
                .ForMember(dest => dest.ContactPerson, opt => opt.MapFrom(src => src.ContactPerson))
                .ForMember(dest => dest.AddtionalComment, opt => opt.MapFrom(src => src.AddtionalComment))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.VenueId, opt => opt.MapFrom(src => src.VenueId))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                //.ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.i))
                //.ForMember(dest => dest.IsSpotlight, opt => opt.MapFrom(src => false))
                //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)SubmissionStatus.ACCEPT))
                .ForMember(dest => dest.Dates, opt => opt.MapFrom(src => src.Dates))
                ;

            CreateMap<SubmissionCommentDto, SubmissionComment>()
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.SubmissionId, opt => opt.MapFrom(src => src.SubmissionId))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => false))
                ;

            CreateMap<AddNotificationDto, Notification>();
            CreateMap<UpdateNotificationDto, Notification>();
        }
    }
}
