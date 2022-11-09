using System.Collections.Generic;

namespace ds.portal.tasks
{
    public interface ITaskMapper
    {
        /// <summary>
        /// Converts to calendar event.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        TaskEvent ToCalendarEvent(TaskEventViewModel viewModel, int userId);
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        TaskEventViewModel ToViewModel(TaskEvent calendarEvent);
        /// <summary>
        /// Converts to calendar event.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        TaskEvent ToCalendarEvent(TaskEventViewModel viewModel);
        /// <summary>
        /// Converts to viewmodel list.
        /// </summary>
        /// <param name="calendarEvents">The calendar events.</param>
        /// <returns></returns>
        IEnumerable<TaskEventViewModel> ToViewModelList(IEnumerable<TaskEvent> calendarEvents);
    }
}
