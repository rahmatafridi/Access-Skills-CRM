namespace ds.portal.tasks
{
    using ds.core.common;
    using ds.portal.tasks.Exceptions;
    using ds.portal.tasks.Models;
    using ds.portal.tasks.Services;
    using ds.portal.tasks.Validators;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class TaskService : ITaskService
    {
        /// <summary>
        /// The expected rows affected
        /// </summary>
        private const int EXPECTED_ROWS_AFFECTED = 1;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TaskService> _logger;
        /// <summary>
        /// The current user service
        /// </summary>
        private readonly ICurrentUserService _currentUserService;
        /// <summary>
        /// The calendar repository
        /// </summary>
        private readonly ITaskRepository _calendarRepository;
        /// <summary>
        /// The calendar mapper
        /// </summary>
        private readonly ITaskMapper _calendarMapper;
        /// <summary>
        /// The calendar validator
        /// </summary>
        private readonly ITaskValidator _calendarValidator;
        /// <summary>
        /// The calendar email service
        /// </summary>
        private readonly ITaskEmailService _calendarEmailService;
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="currentUserService">The current user service.</param>
        /// <param name="calendarRepository">The calendar repository.</param>
        /// <param name="calendarMapper">The calendar mapper.</param>
        /// <param name="calendarValidator">The calendar validator.</param>
        /// <param name="calendarEmailService">The calendar email service.</param>
        public TaskService(
            ILogger<TaskService> logger,
            ICurrentUserService currentUserService,
            ITaskRepository calendarRepository,
            ITaskMapper calendarMapper,
            ITaskValidator calendarValidator,
            ITaskEmailService calendarEmailService)
        {
            _logger = logger;
            _currentUserService = currentUserService;
            _calendarRepository = calendarRepository;
            _calendarMapper = calendarMapper;
            _calendarValidator = calendarValidator;
            _calendarEmailService = calendarEmailService;
        }
        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<bool> AddEvent(TaskEventViewModel model)
        {
            var task = _calendarMapper.ToCalendarEvent(model, _currentUserService.UserId);
            await _calendarValidator.Validate(task);
            var rowsAffected = await _calendarRepository.AddEvent(task);
            if (rowsAffected != EXPECTED_ROWS_AFFECTED)
            {
                _logger.LogInformation($"Multiple ADD events for {model}");    
            }
            await _calendarEmailService.SendCalendarUpdate(task);
            return rowsAffected == EXPECTED_ROWS_AFFECTED;
        }
        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        public async Task<bool> DeleteEvent(int eventId)
        {
            var rowsAffected = await _calendarRepository.DeleteEvent(eventId);
            if (rowsAffected != EXPECTED_ROWS_AFFECTED)
            {
                _logger.LogInformation($"Multiple DELETE events for {eventId}");
            }
            return rowsAffected == EXPECTED_ROWS_AFFECTED;
        }
        /// <summary>
        /// Gets the calendar users.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TaskUserModel>> GetCalendarUsers()
        {
            return _calendarRepository.GetCalendarUsers(_currentUserService.UserId);
        }
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        public async Task<TaskEventViewModel> GetEvent(int eventId)
        {
            var singleEvent = await _calendarRepository.GetEvent(eventId);
            if (singleEvent == null)
                throw new TaskException("Event not found!");
            return _calendarMapper.ToViewModel(singleEvent);
        }
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TaskEventViewModel>> GetEvents()
        {
            var events = await _calendarRepository.GetEvents(_currentUserService.UserId);
            return _calendarMapper.ToViewModelList(events);
        }
        /// <summary>
        /// Gets the events by user id.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TaskEventViewModel>> GetEventsByUserId(int userId)
        {
            var events = await _calendarRepository.GetEventsByUserId(userId);
            return _calendarMapper.ToViewModelList(events);
        }
        /// <summary>
        /// Gets the reasons.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetReasons()
        {
            return await _calendarRepository.GetReasons();
        }
        /// <summary>
        /// Gets the topics.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetTopics()
        {
            return await _calendarRepository.GetTopics();
        }
        /// <summary>
        /// Updates the event.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<bool> UpdateEvent(TaskEventViewModel model)
        {
            var task = _calendarMapper.ToCalendarEvent(model, _currentUserService.UserId);
            //we dont need here at the moment
            //await _calendarValidator.Validate(task);
            var rowsAffected = await _calendarRepository.UpdateEvent(task);
            if (rowsAffected != EXPECTED_ROWS_AFFECTED)
            {
                _logger.LogInformation($"Multiple UPDATE events for {model}");
            }
            await _calendarEmailService.SendCalendarUpdate(task);
            return rowsAffected == EXPECTED_ROWS_AFFECTED;
        }
    }
}
