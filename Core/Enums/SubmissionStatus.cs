using System.ComponentModel;

namespace Core.Enums
{
    public enum SubmissionStatus
    {
        [Description("PENDING")]
        PENDING,
        [Description("ACCEPT")]
        ACCEPT,
        [Description("REJECT")]
        REJECT
    }
}
