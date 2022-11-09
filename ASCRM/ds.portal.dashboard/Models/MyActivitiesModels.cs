using ds.portal.ui.Models;
using System;

namespace ds.portal.dashboard.Models
{
    public class MyActivitiesModels
    {
        public int Activity_Id { get; set; }
        public int Activity_Id_Lead { get; set; }
        private string _encodeLeadId { get; set; }
        public string encodeActivityLeadId
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
        public int Activity_Id_Type { get; set; }
        public string Activity_Type { get; set; }
        public string Activity_Note { get; set; }
        public string Activity_CreatedByUserName { get; set; }
        public DateTime? Activity_DateCreated { get; set; }
        private string _activityDateCreated;
        public string ActivityDateCreated_S
        {
            get => (Activity_DateCreated == null) ? string.Empty : Activity_DateCreated.Value.ToString("dd/MM/yyyy");
            set
            {
                _activityDateCreated = value;
            }
        }
        public string Activity_PhoneActions { get; set; }
        public string Activity_EmailActions { get; set; }
        public string Activity_MeetingActions { get; set; }
        public string Lead_ContactName { get; set; }

        public int SalesAdvisorId { get; set; }
        public string ContactName { get; set; }
        public string AssignedTo { get; set; }
        public bool CanEdit { get; set; }
    }
}
