
using ds.portal.ui.Models;
using System;

namespace ds.core.task.Models
{
    public class TaskModel
    {
        public int task_id { get; set; }
        public string title { get; set; }
        public string note { get; set; }
        public string task_date { get; set; }
        public string date_time { get; set; }
        public string date { get; set; }
        public bool is_snoozed { get; set; }
        public int assigned_to { get; set; }
        public bool is_done { get; set; }
        public int task_action_id { get; set; }
        public int module_id { get; set; }
        public bool is_synced { get; set; }
        public int sync_type_id { get; set; }
        public int created_by { get; set; }
        public int updated_by { get; set; }
        public DateTime created_date_time { get; set; }
        public DateTime updated_date_time { get; set; }
        public string Lead_ContactName { get; set; }
        public string Class_Name { get; set; } 

        private string _encodeLeadId { get; set; }
        public string encodeLeadId
        {
            get
            {
                return Encryption.Encrypt(this.lead_id.ToString());
            }
            set
            {
                _encodeLeadId = value;
            }
        }

        public int lead_id { get; set; }
    }

    public class TaskLeadModel
    {
        public int Lead_Id { get; set; }
        public string Lead_ContactName { get; set; }
    }
}
