using Dapper;
using ds.core.uow;
using ds.portal.tasks.Constants;
using ds.portal.tasks.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ds.portal.tasks
{  
    public class TaskRepository : ITaskRepository
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUOW _unitOfWork;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TaskRepository> _logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskRepository"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        public TaskRepository(
            IUOW unitOfWork, 
            ILogger<TaskRepository> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<IEnumerable<TaskEvent>> GetEvents(int userId)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @task_user_id = userId };
                    return await conn.QueryAsync<TaskEvent>("SP_Lead_Task_GetAllByUserId", param: @params, commandType: CommandType.StoredProcedure);
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
        public async Task<IEnumerable<TaskEvent>> GetEventsByUserId(int userId)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @task_user_id = userId };
                    return await conn.QueryAsync<TaskEvent>("SP_Lead_Task_GetAllByUserId", param: @params, commandType: CommandType.StoredProcedure);
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
        public async Task<int> UpdateEvent(TaskEvent task)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                { 
                    DateTime dtTaskStart = string.IsNullOrEmpty(task.task_start.ToString()) ? DateTime.Now : (DateTime)Convert.ToDateTime(task.task_start);
                    DateTime dtTaskEnd = dtTaskStart.AddMinutes(15);
 
                    var param = new
                    {
                        @task_id = task.task_id,
                        @task_start = dtTaskStart,
                        @task_end = dtTaskEnd,
                        @task_scheduled_with = task.task_scheduled_with,
                        @task_updated_by_user_id = task.booked_by_user_id,
                        //@task_updated_by_username = ,
                        //@task_date_updated = ,
                        
                        /*    task_id = calendarEvent.task_id,
                        @topic = calendarEvent.topic,
                        @learner = calendarEvent.learner,
                        @booked_by_user_id = calendarEvent.booked_by_user_id,
                        @user_id = calendarEvent.task_id_user,
                        @reason = calendarEvent.reason,
                        @title = calendarEvent.title,                        
                        @is_all_day = false,
                        @start = calendarEvent.task_start,
                        @end = calendarEvent.task_end*/
                    };
                    return await conn.ExecuteAsync("SP_Lead_Task_UpdateEvent", param: param, commandType: CommandType.StoredProcedure);
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
        public async Task<int> AddEvent(TaskEvent calendarEvent)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    var param = new
                    {
                        @booked_by_user_id = calendarEvent.booked_by_user_id,
                        @task_user_id = calendarEvent.task_id_user,
                        @reason = calendarEvent.reason,
                        @title = calendarEvent.title,
                        @task_scheduled_with = calendarEvent.task_scheduled_with,
                        @is_all_day = false,
                        @start = calendarEvent.task_start,
                        @end = calendarEvent.task_end,
                        @topic = calendarEvent.topic,
                        @learner = calendarEvent.learner
                    };
                    return await conn.ExecuteAsync("SP_Task_AddEvent", param: param, commandType: CommandType.StoredProcedure);
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
                    object param = new { @task_id = id,
                     // @task_deleted_by_user_id = user_id,
                     // @task_deleted_by_username = username
                    };
                    return await conn.ExecuteAsync("SP_Lead_Task_Delete", param: param, commandType: CommandType.StoredProcedure);
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
        public async Task<TaskEvent> GetEvent(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @task_id = id };
                    return await conn.QueryFirstOrDefaultAsync<TaskEvent>("SP_Lead_Task_GetById", param: @params, commandType: CommandType.StoredProcedure);
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
                    object @params = new { @task_user_id = userId, @start_date_time = start, @end_date_time = end, @task_id = event_id };
                    return await conn.QueryFirstOrDefaultAsync<bool>("SP_Lead_Task_IsUserBusy", param: @params, commandType: CommandType.StoredProcedure);
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
        public async Task<IEnumerable<TaskUserModel>> GetCalendarUsers(int userId)
        {
            try
            {
                using (var conn = new SqlConnection(_unitOfWork.GetConnectionString()))
                {
                    object @params = new { @task_user_id = userId };
                    return await conn.QueryAsync<TaskUserModel>("SP_Lead_Task_GetCalendarUsers", param: @params, commandType: CommandType.StoredProcedure);
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
                    object @params = new { @opt_class = TaskReason.DropDownListKey.CALENDAR_REASON };
                    return await conn.QueryAsync<string>("SP_Lead_Task_GetDropDownItems", param: @params, commandType: CommandType.StoredProcedure);
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
                    object @params = new { @opt_class = TaskReason.DropDownListKey.CALENDAR_TOPIC };
                    return await conn.QueryAsync<string>("SP_Lead_Task_GetDropDownItems", param: @params, commandType: CommandType.StoredProcedure);
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
