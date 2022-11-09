using System.Threading.Tasks;

namespace ds.portal.tasks.Services
{
    public interface ITaskEmailService
    {
        /// <summary>
        /// Sends the calendar update.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        Task SendCalendarUpdate(TaskEvent calendarEvent);
    }
}
