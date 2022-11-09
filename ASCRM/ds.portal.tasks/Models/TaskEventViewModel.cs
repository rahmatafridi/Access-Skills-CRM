using ds.portal.ui.Models;
using Kendo.Mvc.UI;
using System;

namespace ds.portal.tasks
{
    public class TaskEventViewModel : ISchedulerEvent
    {
        public int EventID { get; set; }
        public int UserId { get; set; }
        public long LeadId { get; set; }
        private string _encodeLeadId { get; set; }
        public string encodeLeadId
        {
            get
            {
                return Encryption.Encrypt(this.LeadId.ToString());
            }
            set
            {
                _encodeLeadId = value;
            }
        }
        public string Reason { get; set; }
        public string Learner { get; set; }
        public string Topic { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
    }
}
