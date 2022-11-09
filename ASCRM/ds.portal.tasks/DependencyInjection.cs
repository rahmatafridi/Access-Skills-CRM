namespace ds.portal.tasks
{
    using ds.portal.tasks.Services;
    using ds.portal.tasks.Validators;
    using Microsoft.Extensions.DependencyInjection;
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds the calendar.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection AddMyTask(this IServiceCollection services)
        {
            services.AddSingleton<ITaskMapper, TaskMapper>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITaskEmailService, TaskEmailService>();
            services.AddScoped<ITaskValidator, TaskValidator>();
            return services;
        }
    }
}
