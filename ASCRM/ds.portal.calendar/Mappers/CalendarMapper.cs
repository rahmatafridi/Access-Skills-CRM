using ds.portal.calendar.Constants;
using System.Collections.Generic;

namespace ds.portal.calendar
{
    public class CalendarMapper : ICalendarMapper
    {
        /// <summary>
        /// Converts to calendar event.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public CalendarEvent ToCalendarEvent(CalendarEventViewModel viewModel, int userId) 
        {
            var calendarEvent = ToCalendarEvent(viewModel);
            if (viewModel.Reason != Calendar.Reason.BOOKING)
            {
                calendarEvent.user_id = userId;
            }
            calendarEvent.booked_by_user_id = userId;
            return calendarEvent;
        }
        /// <summary>
        /// Converts to calendar event.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public CalendarEvent ToCalendarEvent(CalendarEventViewModel viewModel)
        {
            return new CalendarEvent
            {
                event_id = viewModel.EventID,
                user_id = viewModel.UserId,
                title = viewModel.Title,
                description = viewModel.Description,
                is_all_day = viewModel.IsAllDay,
                event_start = viewModel.Start,
                event_end = viewModel.End,
                reason = viewModel.Reason,
                topic = viewModel.Topic,
                learner = viewModel.Learner
            };
        }
        /// <summary>
        /// Converts to viewmodel.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        public CalendarEventViewModel ToViewModel(CalendarEvent calendarEvent)
        {
            return new CalendarEventViewModel
            {
                EventID = calendarEvent.event_id,
                UserId = calendarEvent.user_id,
                Start = calendarEvent.event_start,
                End = calendarEvent.event_end,
                Title = string.IsNullOrEmpty(calendarEvent.title) ? string.Empty : calendarEvent.title,
                Description = string.IsNullOrEmpty(calendarEvent.description) ? string.Empty : calendarEvent.description,
                Reason = calendarEvent.reason,
                Topic = calendarEvent.topic,
                Learner = calendarEvent.learner
            };
        }
        /// <summary>
        /// Converts to viewmodel list.
        /// </summary>
        /// <param name="calendarEvents">The calendar events.</param>
        /// <returns></returns>
        public IEnumerable<CalendarEventViewModel> ToViewModelList(IEnumerable<CalendarEvent> calendarEvents)
        {
            var models = new List<CalendarEventViewModel>();
            foreach (var calendarEvent in calendarEvents)
            {
                models.Add(ToViewModel(calendarEvent));
            }
            return models;
        }
    }
}
