using System.Threading.Tasks;

namespace ds.portal.calendar.Validators
{
    public interface ICalendarValidator
    {
        /// <summary>
        /// Validates the specified calander event.
        /// </summary>
        /// <param name="calanderEvent">The calander event.</param>
        /// <returns></returns>
        Task Validate(CalendarEvent calanderEvent);
    }
}
