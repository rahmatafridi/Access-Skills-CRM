using ds.portal.tasks.Constants;
using System.Collections.Generic;

namespace ds.portal.tasks
{
    public class TaskMapper : ITaskMapper
    {
        /// <summary>
        /// Converts to calendar event.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public TaskEvent ToCalendarEvent(TaskEventViewModel viewModel, int userId) 
        {
            var calendarEvent = ToCalendarEvent(viewModel);
            if (viewModel.Reason != TaskReason.Reason.BOOKING)
            {
                calendarEvent.task_id_user = userId;
            }
            calendarEvent.booked_by_user_id = userId;
            return calendarEvent;
        }
        /// <summary>
        /// Converts to calendar event.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        public TaskEvent ToCalendarEvent(TaskEventViewModel viewModel)
        {
            return new TaskEvent
            {
                task_id = viewModel.EventID,
                task_id_user = viewModel.UserId,
                title = viewModel.Title,
                task_scheduled_with = viewModel.Description,
               // is_all_day = viewModel.IsAllDay,
                task_start = viewModel.Start,
                task_end = viewModel.End,
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
        public TaskEventViewModel ToViewModel(TaskEvent calendarEvent)
        {
            return new TaskEventViewModel
            {
                EventID = calendarEvent.task_id,
                UserId = calendarEvent.task_id_user,
                Start = calendarEvent.task_start,
                End = calendarEvent.task_end,
                Title = string.IsNullOrEmpty(calendarEvent.title) ? string.Empty : calendarEvent.title,
                Description = string.IsNullOrEmpty(calendarEvent.task_scheduled_with) ? string.Empty : calendarEvent.task_scheduled_with,
                Reason = calendarEvent.reason,
                Topic = calendarEvent.topic,
                Learner = calendarEvent.learner,
                LeadId = calendarEvent.task_id_lead
            };
        }
        /// <summary>
        /// Converts to viewmodel list.
        /// </summary>
        /// <param name="calendarEvents">The calendar events.</param>
        /// <returns></returns>
        public IEnumerable<TaskEventViewModel> ToViewModelList(IEnumerable<TaskEvent> calendarEvents)
        {
            var models = new List<TaskEventViewModel>();
            foreach (var calendarEvent in calendarEvents)
            {
                models.Add(ToViewModel(calendarEvent));
            }
            return models;
        }
    }
}
