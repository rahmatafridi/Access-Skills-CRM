using ds.portal.tasks;
using ds.portal.tasks.Exceptions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ds.portal.ui.Controllers.Api
{
    [Authorize]
    [Route("api/MyTaskApi")]
    public class MyTaskApiController : BaseApiController
    {
        /// <summary>
        /// The calendar service
        /// </summary>
        private readonly ITaskService _taskService;
        /// <summary>
        /// Initializes a new instance of the <see cref="MyTaskApiController"/> class.
        /// </summary>
        /// <param name="calendarService">The calendar service.</param>
        public MyTaskApiController(ITaskService calendarService)
        {
            _taskService = calendarService;
        }
        /// <summary>
        /// Reads the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest] DataSourceRequest request)
        {
            var calendarEvents = await _taskService.GetEvents();
            return calendarEvents.ToDataSourceResult(request);
        }
        /// <summary>
        /// Reads the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetByUserId")]
        public async Task<DataSourceResult> GetByUserId([DataSourceRequest] DataSourceRequest request, int user_id)
        {
            var calendarEvents = await _taskService.GetEventsByUserId(user_id);
            return calendarEvents.ToDataSourceResult(request);
        }
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var calendarUsers = await _taskService.GetCalendarUsers();
            return Ok(calendarUsers);
        }
        /// <summary>
        /// Gets the reasons.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetReasons")]
        public async Task<IActionResult> GetReasons()
        {
            var reasons = await _taskService.GetReasons();
            return Ok(reasons);
        }
        /// <summary>
        /// Gets the reasons.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTopics")]
        public async Task<IActionResult> GetTopics()
        {
            var topics =  await _taskService.GetTopics();
            return Ok(topics);
        }
        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] TaskEventViewModel calendarEvent)
        {
            try
            {
                await _taskService.AddEvent(calendarEvent);
                return Ok();
            }
            catch (TaskException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Updates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        [HttpPut]
        public virtual async Task<IActionResult> Put([FromForm] TaskEventViewModel calendarEvent)
        {
            try
            {
                await _taskService.UpdateEvent(calendarEvent);
                return Ok();
            }
            catch (TaskException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                await _taskService.DeleteEvent(id);
            }
            return Ok();
        }
    }
}
