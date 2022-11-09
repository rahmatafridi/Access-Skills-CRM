using System.ComponentModel;

namespace ds.portal.tasks
{
    public enum EventReason
    {
        [Description("Sick")]
        Sick = 0,
        [Description("Booking")]
        Booking = 1
    }
}
