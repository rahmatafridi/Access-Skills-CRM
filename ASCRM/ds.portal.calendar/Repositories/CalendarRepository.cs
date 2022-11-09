namespace ds.portal.calendar
{
    using Dapper;
    using ds.core.uow;
    using ds.portal.calendar.Constants;
    using ds.portal.calendar.Models;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    public class CalendarRepository : ICalendarRepository
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUOW _unitOfWork;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CalendarRepository> _logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        public CalendarRepository(
            IUOW unitOfWork, 
            ILogger<CalendarRepository> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<CalendarEvent>> GetEvents(int userId)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @user_id = userId };
                    return await conn.QueryAsync<CalendarEvent>("SP_Calendar_GetEvents", param: @params, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Gets the events by user id.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<CalendarEvent>> GetEventsByUserId(int userId)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @user_id = userId };
                    return await conn.QueryAsync<CalendarEvent>("SP_Calendar_GetEvents_ByUserId", param: @params, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Updates the event.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        public async Task<int> UpdateEvent(CalendarEvent calendarEvent)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    var param = new
                    {
                        @event_id = calendarEvent.event_id,
                        @topic = calendarEvent.topic,
                        @learner = calendarEvent.learner,
                        @booked_by_user_id = calendarEvent.booked_by_user_id,
                        @user_id = calendarEvent.user_id,
                        @reason = calendarEvent.reason,
                        @title = calendarEvent.title,
                        @description = calendarEvent.description,
                        @is_all_day = false,
                        @start = calendarEvent.event_start,
                        @end = calendarEvent.event_end
                    };
                    return await conn.ExecuteAsync("SP_Calendar_UpdateEvent", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Adds the event.
        /// </summary>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        public async Task<int> AddEvent(CalendarEvent calendarEvent)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    var param = new
                    {
                        @booked_by_user_id = calendarEvent.booked_by_user_id,
                        @user_id = calendarEvent.user_id,
                        @reason = calendarEvent.reason,
                        @title = calendarEvent.title,
                        @description = calendarEvent.description,
                        @is_all_day = false,
                        @start = calendarEvent.event_start,
                        @end = calendarEvent.event_end,
                        @topic = calendarEvent.topic,
                        @learner = calendarEvent.learner
                    };
                    return await conn.ExecuteAsync("SP_Calendar_AddEvent", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Deletes the event.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<int> DeleteEvent(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object param = new { @event_id = id };
                    return await conn.ExecuteAsync("SP_Calendar_DeleteEvent", param: param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        public async Task<CalendarEvent> GetEvent(int eventId)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @event_id = eventId };
                    return await conn.QueryFirstOrDefaultAsync<CalendarEvent>("SP_Calendar_GetEvent", param: @params, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Determines whether [is user busy] [the specified user identifier].
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="event_id">The event identifier.</param>
        /// <returns></returns>
        public async Task<bool> IsUserBusy(int userId, DateTime start, DateTime end, int? event_id)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @user_id = userId, @start_date_time = start, @end_date_time = end, @event_id = event_id };
                    return await conn.QueryFirstOrDefaultAsync<bool>("SP_Calendar_IsUserBusy", param: @params, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Gets the calendar users.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<CalendarUserModel>> GetCalendarUsers(int userId)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @user_id = userId };
                    return await conn.QueryAsync<CalendarUserModel>("SP_Calendar_GetCalendarUsers", param: @params, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Gets the reasons.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetReasons()
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @opt_class = Calendar.DropDownListKey.CALENDAR_REASON };
                    return await conn.QueryAsync<string>("SP_Calendar_GetDropDownItems", param: @params, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
        /// <summary>
        /// Gets the topics.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetTopics()
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @opt_class = Calendar.DropDownListKey.CALENDAR_TOPIC };
                    return await conn.QueryAsync<string>("SP_Calendar_GetDropDownItems", param: @params, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                throw ex;
            }
        }
    }
}
