using System;

namespace ds.portal.calendar
{
    public class CalendarEvent
    {
        public int event_id { get; set; }
        public int user_id { get; set; }
        public string reason { get; set; }
        public string topic { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string learner { get; set; }
        public bool is_all_day { get; set; }
        public DateTime event_start { get; set; }
        public DateTime event_end { get; set; }
        public string start_time_zone { get; set; }
        public string end_time_zone { get; set; }
        public string recurrence_rule { get; set; }
        public string recurrence_exception { get; set; }
        public int booked_by_user_id { get; set; }
    }
}
