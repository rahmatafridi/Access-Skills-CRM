using ds.portal.ui.Models;

namespace ds.portal.dashboard.Models
{
    public class NotificationModel
    {
        public int activity_id { get; set; }
        public string AT_Title { get; set; }
        public string Lead_ContactName { get; set; }
        public string activity_date { get; set; }
        public int lead_id { get; set; }
        public int notification_id { get; set; }
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
    }
}
