using System.ComponentModel;

namespace Core.Enums
{
    public enum AttendanceOption
    {
        [Description("Registration")]
        Registration,
        [Description("Ticket")]
        Ticket,
        [Description("Free")]
        Free
    }
}
