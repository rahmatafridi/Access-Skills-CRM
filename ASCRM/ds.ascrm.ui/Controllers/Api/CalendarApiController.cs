using ds.portal.calendar;
using ds.portal.calendar.Exceptions;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ds.portal.ui.Controllers.Api
{
    [Authorize]
    [Route("api/CalendarApi")]
    public class CalendarApiController : BaseApiController
    {
        /// <summary>
        /// The calendar service
        /// </summary>
        private readonly ICalendarService _calendarService;
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarApiController"/> class.
        /// </summary>
        /// <param name="calendarService">The calendar service.</param>
        public CalendarApiController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }
        /// <summary>
        /// Reads the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DataSourceResult> Get([DataSourceRequest] DataSourceRequest request)
        {
            var calendarEvents = await _calendarService.GetEvents();
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
            var calendarEvents = await _calendarService.GetEventsByUserId(user_id);
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
            var calendarUsers = await _calendarService.GetCalendarUsers();
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
            var reasons = await _calendarService.GetReasons();
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
            var topics =  await _calendarService.GetTopics();
            return Ok(topics);
        }
        /// <summary>
        /// Creates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="calendarEvent">The calendar event.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CalendarEventViewModel calendarEvent)
        {
            try
            {
                await _calendarService.AddEvent(calendarEvent);
                return Ok();
            }
            catch (CalendarException ex)
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
        public virtual async Task<IActionResult> Put([FromForm] CalendarEventViewModel calendarEvent)
        {
            try
            {
                await _calendarService.UpdateEvent(calendarEvent);
                return Ok();
            }
            catch (CalendarException ex)
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
                await _calendarService.DeleteEvent(id);
            }
            return Ok();
        }
    }
}
