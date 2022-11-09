using ds.portal.tasks.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ds.portal.tasks
{
    public interface ITaskRepository
    {
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<TaskEvent>> GetEvents(int userId);
        /// <summary>
        /// Gets the events by user id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<TaskEvent>> GetEventsByUserId(int userId);
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<TaskEvent> GetEvent(int eventId);
        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        Task<int> AddEvent(TaskEvent calendarEvent);
        /// <summary>
        /// Updates the event.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        Task<int> UpdateEvent(TaskEvent calendarEvent);
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
        Task<IEnumerable<TaskUserModel>> GetCalendarUsers(int userId);
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
