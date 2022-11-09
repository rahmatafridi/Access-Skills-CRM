using System.Threading.Tasks;

namespace ds.portal.tasks.Validators
{
    public interface ITaskValidator
    {
        /// <summary>
        /// Validates the specified calander event.
        /// </summary>
        /// <param name="calanderEvent">The calander event.</param>
        /// <returns></returns>
        Task Validate(TaskEvent calanderEvent);
    }
}
