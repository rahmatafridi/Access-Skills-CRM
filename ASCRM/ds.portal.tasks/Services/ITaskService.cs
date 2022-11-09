using ds.portal.tasks.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ds.portal.tasks
{
    public interface ITaskService
    {
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TaskEventViewModel>> GetEvents();
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TaskEventViewModel>> GetEventsByUserId(int userId);
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<TaskEventViewModel> GetEvent(int eventId);
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
        Task<bool> UpdateEvent(TaskEventViewModel model);
        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<bool> AddEvent(TaskEventViewModel model);
        /// <summary>
        /// Gets the calendar users.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TaskUserModel>> GetCalendarUsers();
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
