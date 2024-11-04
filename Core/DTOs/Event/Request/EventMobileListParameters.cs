using Core.DTOs.Shared;

namespace Core.DTOs.Event.Request
{
    public class EventMobileListParameters : PaginationParameter
    {
        public string? NameAr { get; set; }
        public string? NameEN { get; set; }
        public bool? IsFavourite { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public long? CategoryId { get; set; }
        public decimal? MinFee { get; set; }
        public decimal? MaxFee { get; set; }
    }
}
