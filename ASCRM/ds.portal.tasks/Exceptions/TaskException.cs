using System;

namespace ds.portal.tasks.Exceptions
{
    public class TaskException : Exception
    {
        public TaskException(string message) : base(message)
        {
        }
    }
}
