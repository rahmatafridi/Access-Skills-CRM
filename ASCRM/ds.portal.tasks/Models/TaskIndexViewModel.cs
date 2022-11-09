using System;
using System.Collections.Generic;

namespace ds.portal.tasks.Models
{
    public class TaskIndexViewModel
    {
        public IEnumerable<TaskUserModel> CalendarUsers { get; set; }
        public DateTime CalendarDate { get; set; }
        public DateTime CalendarStartTime { get; set; }
    }
}
