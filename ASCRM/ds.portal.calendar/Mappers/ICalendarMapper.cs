using System.Collections.Generic;

namespace ds.portal.calendar
{
    public interface ICalendarMapper
    {
        /// <summary>
        /// Converts to calendar event.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        CalendarEvent ToCalendarEvent(CalendarEventViewModel viewModel, int userId);
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        CalendarEventViewModel ToViewModel(CalendarEvent calendarEvent);
        /// <summary>
        /// Converts to calendar event.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        CalendarEvent ToCalendarEvent(CalendarEventViewModel viewModel);
        /// <summary>
        /// Converts to viewmodel list.
        /// </summary>
        /// <param name="calendarEvents">The calendar events.</param>
        /// <returns></returns>
        IEnumerable<CalendarEventViewModel> ToViewModelList(IEnumerable<CalendarEvent> calendarEvents);
    }
}
