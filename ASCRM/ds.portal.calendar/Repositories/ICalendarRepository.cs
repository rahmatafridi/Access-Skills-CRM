using ds.portal.calendar.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ds.portal.calendar
{
    public interface ICalendarRepository
    {
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<CalendarEvent>> GetEvents(int userId);
        /// <summary>
        /// Gets the events by user id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<CalendarEvent>> GetEventsByUserId(int userId);
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<CalendarEvent> GetEvent(int eventId);
        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        Task<int> AddEvent(CalendarEvent calendarEvent);
        /// <summary>
        /// Updates the event.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        Task<int> UpdateEvent(CalendarEvent calendarEvent);
        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<int> DeleteEvent(int id);
        /// <summary>
        /// Determines whether [is user busy] [the specified user identifier].
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="event_id">The event identifier.</param>
        /// <returns></returns>
        Task<bool> IsUserBusy(int userId, DateTime start, DateTime end, int? event_id);
        /// <summary>
        /// Gets the calendar users.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<CalendarUserModel>> GetCalendarUsers(int userId);
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
