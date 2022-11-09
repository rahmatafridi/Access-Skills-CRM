using ds.portal.calendar.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ds.portal.calendar
{
    public interface ICalendarService
    {
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CalendarEventViewModel>> GetEvents();
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CalendarEventViewModel>> GetEventsByUserId(int userId);
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<CalendarEventViewModel> GetEvent(int eventId);
        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<bool> DeleteEvent(int eventId);
        /// <summary>
        /// Updates the event.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<bool> UpdateEvent(CalendarEventViewModel model);
        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<bool> AddEvent(CalendarEventViewModel model);
        /// <summary>
        /// Gets the calendar users.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CalendarUserModel>> GetCalendarUsers();
        /// <summary>
        /// Gets the reasons.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetReasons();
        /// <summary>
        /// Gets the topics.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<string>> GetTopics();
    }
}
