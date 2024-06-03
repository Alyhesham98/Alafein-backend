using Core.Entities.BaseEntities;
using Core.Entities.Event;

namespace Core.Entities.LookUps
{
    public class Facility : BaseEntity
    {
        public string ImagePath { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;

        public ICollection<VenueFacility> VenueFacilities { get; set; }
    }
}
