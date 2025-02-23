using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.Event.Request
{
    public class SpotlightOrderDto
    {
        public long Id { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Spotlight order must be greater than 0")]
        public int SpotlightOrder { get; set; }
    }
}
