using System;
using System.Collections.Generic;
using System.Text;

namespace portal.models.Account
{
    public class User
    {
        public int Users_ID { get; set; }
        public string Users_UserName { get; set; }
        public string Users_DisplayName { get; set; }
        public int Users_IsActive { get; set; }
        public int? Users_Id_AssessorContacts { get; set; }
        public int? Users_Id_FreelanceContacts { get; set; }
        public int? Users_Id_TrainingProvider { get; set; }
        public int? Users_Id_LearnerContacts { get; set; }
        public int? Users_Id_AwardingBody { get; set; }
        public int Users_Id_Care { get; set; }
        public int? Users_Id_OSCA { get; set; }
        public bool IsAutenticated { get; set; }
        public int Users_RoleId { get; set; }
        public string Users_RoleName { get; set; }
        public string RoleCode { get; set; }
    }
}
