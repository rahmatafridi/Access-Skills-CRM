namespace ds.portal.calendar
{
    using ds.portal.calendar.Services;
    using ds.portal.calendar.Validators;
    using Microsoft.Extensions.DependencyInjection;
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the calendar.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddCalendar(this IServiceCollection services)
        {
            services.AddSingleton<ICalendarMapper, CalendarMapper>();
            services.AddScoped<ICalendarRepository, CalendarRepository>();
            services.AddScoped<ICalendarService, CalendarService>();
            services.AddScoped<ICalendarEmailService, CalendarEmailService>();
            services.AddScoped<ICalendarValidator, CalendarValidator>();
            return services;
        }
    }
}
