using ds.portal.ui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.CrmModel
{
  public  class Portal_User
    {
        public int Users_Id { get; set; }

        public string Users_Username { get; set; }

        public string Users_Email { get; set; }

        public string Users_Password { get; set; }

        public string Users_DisplayName { get; set; }
        public string DisplayName { get; set; }

        public byte Users_IsActive { get; set; }

        public int? Users_Id_AccessLevel { get; set; }

        public int? Users_Id_UserInfo { get; set; }

        public int? Users_Id_AssessorContacts { get; set; }

        public int? Users_Id_LearnerContacts { get; set; }

        public byte Users_IsAssessor { get; set; }

        public byte Users_IsFreelance { get; set; }

        public byte Users_IsLearner { get; set; }

        public byte Users_IsGuest { get; set; }

        public byte Users_IsCare { get; set; }

        public byte Users_IsCareGroup { get; set; }

        public byte? Users_IsAdvisor { get; set; }

        public byte? Users_IsExcluded { get; set; }

        public DateTime? Users_CreatedDate { get; set; }

        public DateTime? Users_PasswordChangedDate { get; set; }

        public int? Users_Id_OSCA { get; set; }

        public int? Users_Id_FreelanceContacts { get; set; }

        public int? Users_Id_TrainingProvider { get; set; }

        public int? Users_Id_AwardingBody { get; set; }

        public DateTime? Users_ShowLearnersFrom { get; set; }

        public int Users_Id_Care { get; set; }

        public bool? Users_SeeLearnerCourceSummaryDocs { get; set; }

    }

  public class PortalUserModel
    {
        public int id { get; set; }

        private string _encodeUsersId { get; set; }
        public string encodeUsersId
        {
            get
            {
                return Encryption.Encrypt(this.id.ToString());
            }
            set
            {
                _encodeUsersId = value;
            }
        }

        public int security_user_id { get; set; }
        public string username { get; set; }
        public int security_role_id { get; set; }
        public string role_name { get; set; }
        public string displayname { get; set; }
        public  string useremail { get; set; }
    }
 
}
