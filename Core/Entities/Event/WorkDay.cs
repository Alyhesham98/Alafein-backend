using Core.Entities.BaseEntities;

namespace Core.Entities.Event
{
    public class WorkDay : BaseEntity
    {
        public DayOfWeek Day { get; set; }
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public long BranchId { get; set; }
        public Branch Branch { get; set; }
    }
}
