using ds.portal.ui.Models;
using System;

namespace ds.portal.diary.Models
{
    public class DiaryModel
    {
        public int Activity_Id { get; set; }
        public string AT_Title { get; set; }
        public DateTime activity_date { get; set; }
        public string activity_Note { get; set; }
        public string Activity_Location { get; set; }
        public string Opt_Title { get; set; }
        public string OPT_Class { get; set; }
        public int Activity_Id_Lead { get; set; }
        public bool is_snoozed { get; set; }
        public int assigned_to { get; set; }
        public bool is_done { get; set; }
        public int task_action_id { get; set; }
        public int module_id { get; set; }
        public bool is_activity { get; set; }
        public bool is_task { get; set; }

        private string _encodeLeadId { get; set; }
        public string encodeLeadId
        {
            get
            {
                return Encryption.Encrypt(this.Activity_Id_Lead.ToString());
            }
            set
            {
                _encodeLeadId = value;
            }
        }

        public string Lead_ContactName { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }
}
