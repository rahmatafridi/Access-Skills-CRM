using ds.core.configuration;
using ds.portal.calendar;
using ds.portal.calendar.Models;
using Microsoft.AspNetCore.Mvc;
using portal.data.repository.Interfaces;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class CalendarController : BaseController
    {
        /// <summary>
        /// The calendar service
        /// </summary>
        private readonly ICalendarService _calendarService;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarController"/> class.
        /// </summary>
        /// <param name="roleAdminRepository">The role admin repository.</param>
        /// <param name="configurationRepository">The configuration repository.</param>
        /// <param name="calendarService">The calendar service.</param>
        public CalendarController(
            IRoleAdminRepository roleAdminRepository, 
            IConfigurationRepository configurationRepository,
            ICalendarService calendarService, 
            IMemoryCache memoryCache) : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _calendarService = calendarService;
        }
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "GMT Standard Time"); 
            var viewModel = new CalendarIndexViewModel
            {
                CalendarDate = now,
                CalendarStartTime = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0),
                CalendarUsers = await _calendarService.GetCalendarUsers()
            };

            SetProductDetailToViewBag();
            return View(viewModel);
        }
        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ActionResult Create(CreateEventModel model)
        {
            var start = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(model.Start, "GMT Standard Time");
            var viewModel = new CalendarEventViewModel { Start = start };
            viewModel.End = start.AddMinutes(30);
            return View("_Create", viewModel);
        }
        /// <summary>
        /// Edits the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(EditEventModel model)
        {
            var viewModel = await _calendarService.GetEvent(model.EventId);
            return View("_Edit", viewModel);
        }
        /// <summary>
        /// For customer to confirm booking.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Confirmation()
        {
            var now = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "GMT Standard Time");
            var viewModel = new CalendarIndexViewModel
            {
                CalendarDate = now,
                CalendarStartTime = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0),
                CalendarUsers = await _calendarService.GetCalendarUsers()
            };
            ViewBag.user_id = GetSecurityUserId;
            SetProductDetailToViewBag();
            return View(viewModel);
        }
    }
}
