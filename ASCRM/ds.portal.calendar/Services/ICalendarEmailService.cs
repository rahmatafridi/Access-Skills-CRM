using System.Threading.Tasks;

namespace ds.portal.calendar.Services
{
    public interface ICalendarEmailService
    {
        /// <summary>
        /// Sends the calendar update.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        Task SendCalendarUpdate(CalendarEvent calendarEvent);
    }
}
