using System;

namespace ds.portal.tasks
{
    public class TaskEvent
    {
        public int task_id { get; set; }
        public int task_id_user { get; set; }
        public long task_id_lead { get; set; }
        public string reason { get; set; }
        public string topic { get; set; }
        public string title { get; set; }
        public string task_scheduled_with { get; set; }
        public string learner { get; set; }
        public bool is_done { get; set; }
        public DateTime task_start { get; set; }
        public DateTime task_end { get; set; }
        public string start_time_zone { get; set; }
        public string end_time_zone { get; set; }
        public string recurrence_rule { get; set; }
        public string recurrence_exception { get; set; }
        public int booked_by_user_id { get; set; }
    }
}
