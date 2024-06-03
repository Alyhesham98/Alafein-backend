using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Event.Request
{
    public class AddAdminSubmissionDto
    {
        public string EventNameEN { get; set; } = string.Empty;
        public string EventNameAR { get; set; } = string.Empty;
        public string EventDescriptionEN { get; set; } = string.Empty;
        public string EventDescriptionAR { get; set; } = string.Empty;
        public string? MainArtestNameEN { get; set; }
        public string? MainArtestNameAR { get; set; }
        public bool KidsAvailability { get; set; }
        public AttendanceOption AttendanceOption { get; set; }
        public string? URL { get; set; }
        public decimal PaymentFee { get; set; } 
        public string? Poster { get; set; } 
        public string? ContactPerson { get; set; }
        public string? AddtionalComment { get; set; }
        public long CategoryId { get; set; }
        public long VenueId { get; set; }
        public long OrganizerId { get; set; }
        public long BranchId { get; set; }
        [Range(0, 3)]
        public int Repeat { get; set; } 
        public List<DateTime> Dates { get; set; } = new List<DateTime>();

        public void ExpandDates()
        {
            List<DateTime> expandedDates = new List<DateTime>();
            foreach (var date in Dates)
            {
                expandedDates.Add(date);
                for (int i = 1; i <= Repeat; i++)
                {
                    expandedDates.Add(date.AddDays(7 * i));
                }
            }
            Dates = expandedDates;
        }
    }
}
