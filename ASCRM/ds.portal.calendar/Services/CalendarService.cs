namespace ds.portal.calendar
{
    using ds.core.common;
    using ds.portal.calendar.Exceptions;
    using ds.portal.calendar.Models;
    using ds.portal.calendar.Services;
    using ds.portal.calendar.Validators;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CalendarService : ICalendarService
    {
        /// <summary>
        /// The expected rows affected
        /// </summary>
        private const int EXPECTED_ROWS_AFFECTED = 1;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CalendarService> _logger;
        /// <summary>
        /// The current user service
        /// </summary>
        private readonly ICurrentUserService _currentUserService;
        /// <summary>
        /// The calendar repository
        /// </summary>
        private readonly ICalendarRepository _calendarRepository;
        /// <summary>
        /// The calendar mapper
        /// </summary>
        private readonly ICalendarMapper _calendarMapper;
        /// <summary>
        /// The calendar validator
        /// </summary>
        private readonly ICalendarValidator _calendarValidator;
        /// <summary>
        /// The calendar email service
        /// </summary>
        private readonly ICalendarEmailService _calendarEmailService;
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="currentUserService">The current user service.</param>
        /// <param name="calendarRepository">The calendar repository.</param>
        /// <param name="calendarMapper">The calendar mapper.</param>
        /// <param name="calendarValidator">The calendar validator.</param>
        /// <param name="calendarEmailService">The calendar email service.</param>
        public CalendarService(
            ILogger<CalendarService> logger,
            ICurrentUserService currentUserService,
            ICalendarRepository calendarRepository,
            ICalendarMapper calendarMapper,
            ICalendarValidator calendarValidator,
            ICalendarEmailService calendarEmailService)
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
        public async Task<bool> AddEvent(CalendarEventViewModel model)
        {
            var calanderEvent = _calendarMapper.ToCalendarEvent(model, _currentUserService.UserId);
            await _calendarValidator.Validate(calanderEvent);
            var rowsAffected = await _calendarRepository.AddEvent(calanderEvent);
            if (rowsAffected != EXPECTED_ROWS_AFFECTED)
            {
                _logger.LogInformation($"Multiple ADD events for {model}");    
            }
            await _calendarEmailService.SendCalendarUpdate(calanderEvent);
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
        public Task<IEnumerable<CalendarUserModel>> GetCalendarUsers()
        {
            return _calendarRepository.GetCalendarUsers(_currentUserService.UserId);
        }
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        public async Task<CalendarEventViewModel> GetEvent(int eventId)
        {
            var singleEvent = await _calendarRepository.GetEvent(eventId);
            if (singleEvent == null)
                throw new CalendarException("Event not found!");
            return _calendarMapper.ToViewModel(singleEvent);
        }
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CalendarEventViewModel>> GetEvents()
        {
            var events = await _calendarRepository.GetEvents(_currentUserService.UserId);
            return _calendarMapper.ToViewModelList(events);
        }
        /// <summary>
        /// Gets the events by user id.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<CalendarEventViewModel>> GetEventsByUserId(int userId)
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
        public async Task<bool> UpdateEvent(CalendarEventViewModel model)
        {
            var calanderEvent = _calendarMapper.ToCalendarEvent(model, _currentUserService.UserId);
            await _calendarValidator.Validate(calanderEvent);
            var rowsAffected = await _calendarRepository.UpdateEvent(calanderEvent);
            if (rowsAffected != EXPECTED_ROWS_AFFECTED)
            {
                _logger.LogInformation($"Multiple UPDATE events for {model}");
            }
            await _calendarEmailService.SendCalendarUpdate(calanderEvent);
            return rowsAffected == EXPECTED_ROWS_AFFECTED;
        }
    }
}
