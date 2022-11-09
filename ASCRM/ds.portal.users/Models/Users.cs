using ds.portal.ui.Models;
using System;

namespace ds.portal.users.Models
{
    public sealed class Users
    {
        public int Users_ID { get; set; }
        public int Lead_Id_SalesAdvisor { get; set; }
        private string _encodeUsersId { get; set; }
        public string encodeUsersId
        {
            get
            {
                return Encryption.Encrypt(this.Users_ID.ToString());
            }
            set
            {
                _encodeUsersId = value;
            }
        }

        public string Users_UserName { get; set; }
        public string Users_Email { get; set; }
        public string Users_Password { get; set; }
        public string Users_DisplayName { get; set; }
        public int Users_IsActive { get; set; }
        public int Users_RoleId { get; set; }
        public string Users_RoleName { get; set; }
        public string Mobile { get; set; }
        public bool Users_IsEmailNotification { get; set; }
        public bool Users_IsSendCopyToPersonalEmail { get; set; }
        public string Users_Image { get; set; }
        public int Users_IsAccepting_Lead { get; set; }
        public bool Users_Share_Calendar { get; set; }
        public int Users_Id_CourseLevel { get; set; }
        public int Users_Id_Care { get; set; }
        public bool Users_SeeLearnerCourceSummaryDocs { get; set; }
        public DateTime? Users_ShowLearnersFrom { get; set; }
        public int Users_IsCare { get; set; }
        public int Users_Id_TrainingProvider { get; set; }
        public int Users_Id_AwardingBody { get; set; }
    }
}
