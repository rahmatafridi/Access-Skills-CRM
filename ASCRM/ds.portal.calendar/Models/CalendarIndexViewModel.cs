using System;
using System.Collections.Generic;

namespace ds.portal.calendar.Models
{
    public class CalendarIndexViewModel
    {
        public IEnumerable<CalendarUserModel> CalendarUsers { get; set; }
        public DateTime CalendarDate { get; set; }
        public DateTime CalendarStartTime { get; set; }
    }
}
