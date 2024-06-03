using System.ComponentModel;

namespace Core.Enums
{
    public enum DashboardOption
    {
        [Description("This Week")]
        Week,
        [Description("This Month")]
        Month,
        [Description("This Year")]
        Year
    }
}
