using ds.core.common;
using ds.portal.tasks.Constants;
using ds.portal.tasks.Exceptions;
using ds.portal.tasks.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ds.portal.tasks.Validators
{
    public class TaskValidator : ITaskValidator
    {
        /// <summary>
        /// The cache key
        /// </summary>
        private const string CacheKey = "nationl-holidays";
        /// <summary>
        /// The assets service
        /// </summary>
        private readonly IAssetsService _assetsService;
        /// <summary>
        /// The calendar repository
        /// </summary>
        private readonly ITaskRepository _calendarRepository;
        /// <summary>
        /// The memory cache
        /// </summary>
        private readonly IMemoryCache _memoryCache;
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskValidator"/> class.
        /// </summary>
        /// <param name="assetsService">The assets service.</param>
        /// <param name="calendarRepository">The calendar repository.</param>
        public TaskValidator(
            IAssetsService assetsService,
            ITaskRepository calendarRepository)
        {
            _assetsService = assetsService;
            _calendarRepository = calendarRepository;
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }
        /// <summary>
        /// Validates the specified calander event.
        /// </summary>
        /// <param name="calanderEvent">The calander event.</param>
        public async Task Validate(TaskEvent calanderEvent)
        {
            CheckDateBasics(calanderEvent.task_start, calanderEvent.task_end);
            CheckDuration(calanderEvent);
            CheckWeekend(calanderEvent.task_start, calanderEvent.task_end);
            CheckNationalHoliday(calanderEvent.task_start, calanderEvent.task_end);
            CheckIfBooking(calanderEvent);
            await CheckIfUserIsBusy(calanderEvent.task_id_user, calanderEvent.task_start, calanderEvent.task_end, calanderEvent.task_id);
            //CheckBookingDaysAllowed(calanderEvent.task_start, calanderEvent.task_start, calanderEvent.task_end);
        }
        /// <summary>
        /// Checks if booking.
        /// </summary>
        /// <param name="calanderEvent">The calander event.</param>
        /// <exception cref="TaskException">Please enter a learner for this booking</exception>
        private void CheckIfBooking(TaskEvent calanderEvent)
        {
            if (calanderEvent.reason == TaskReason.Reason.BOOKING)
            {
                if (string.IsNullOrEmpty(calanderEvent.learner))
                {
                    throw new TaskException("Please enter a learner for this booking");
                }
            }
        }
        /// <summary>
        /// Checks the date basics.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <exception cref="TaskException">
        /// Start date is greater than end date
        /// or
        /// Sorry, you cannot book in the past
        /// </exception>
        private void CheckDateBasics(DateTime start, DateTime end)
        {
            if (start >= end)
                throw new TaskException("Start date is greater than end date");

            var now = DateTime.UtcNow;
            if (start < now || end < now)
                throw new TaskException("Sorry, you cannot book in the past");
        }
        /// <summary>
        /// Checks if user is busy.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <exception cref="TaskException">User already has time booked during this period</exception>
        private async Task CheckIfUserIsBusy(int userId, DateTime start, DateTime end, int eventId)
        {
            var isUserBusy = await _calendarRepository.IsUserBusy(userId, start, end, eventId);
            if (isUserBusy)
                throw new TaskException("User already has time booked during this period");
        }
        /// <summary>
        /// Checks the duration.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <exception cref="TaskException">Booking events should be in 30 minute blocks</exception>
        private void CheckDuration(TaskEvent calanderEvent)
        {
            if (calanderEvent.reason == TaskReason.Reason.BOOKING)
            {
                var timeDiff = calanderEvent.task_end.Subtract(calanderEvent.task_start);
                if (timeDiff != TimeSpan.FromMinutes(30))
                    throw new TaskException("Booking events should be in 30 minute blocks");
            }
        }
        /// <summary>
        /// Checks the weekend.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <exception cref="TaskException">Booking on weekends is not allowed</exception>
        private void CheckWeekend(DateTime start, DateTime end)
        {
            if ((start.DayOfWeek == DayOfWeek.Saturday) || (start.DayOfWeek == DayOfWeek.Sunday) ||
                (end.DayOfWeek == DayOfWeek.Saturday) || (end.DayOfWeek == DayOfWeek.Sunday))
            {
                throw new TaskException("Booking on weekends is not allowed");
            }
        }
        /// <summary>
        /// Checks the national holiday.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <exception cref="TaskException">Booking on national holiday ({holiday.Title}) is not allowed</exception>
        private void CheckNationalHoliday(DateTime start, DateTime end)
        {
            var nationalHolidays = GetNationalHolidays();
            var holiday = nationalHolidays.EnglandAndWales.Events.FirstOrDefault(x => x.Date == start.Date || x.Date == end.Date);
            if (holiday != null)
            {
                throw new TaskException($"Booking on national holiday ({holiday.Title}) is not allowed");
            }
        }
        /// <summary>
        /// Gets the national holidays.
        /// </summary>
        /// <returns></returns>
        private NationalHolidaysModel GetNationalHolidays()
        {
            var holidays = _memoryCache.Get<NationalHolidaysModel>(CacheKey);
            if (holidays == null)
            {
                holidays = _assetsService.LoadFromJson<NationalHolidaysModel>($"{AppDomain.CurrentDomain.GetData("DataDirectory")}\\bank-holidays.json");
                _memoryCache.Set(CacheKey, holidays);
            }
            return holidays;
        }

        /// <summary>
        /// Checks the weekend and national holidays.
        /// </summary>
        /// <param name="booked">Temporary booking date.</param>
        /// <param name="startDate">The start.</param>
        /// <param name="endDate">The end.</param>
        /// <exception cref="TaskException">Booking allowed within 4 business days.</exception>
        private void CheckBookingDaysAllowed(DateTime booked, DateTime startDate, DateTime endDate)
        {
            int cnt = 0;
            var nationalHolidays = GetNationalHolidays();
            
            for (var current = booked.AddDays(-4); current < endDate; current = current.AddDays(1))
            {
                if (current.DayOfWeek == DayOfWeek.Saturday
                    || current.DayOfWeek == DayOfWeek.Sunday
                    || (nationalHolidays.EnglandAndWales.Events.FirstOrDefault(x => x.Date == startDate.Date || x.Date == endDate.Date) != null)
                    )
                {
                    // skip holiday 
                }
                else cnt++;
            }
            if(cnt > 4)
            {
                throw new TaskException("Booking is only allowed within 4 business days starting from "+ booked.ToString("dd/MM/yyyy") + ".");
            }
        }
    }
}
