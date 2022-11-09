using ds.portal.ui.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class ApplicationModel
    {
        public string encoded_app_id
        {
            get
            {
                return Encryption.Encrypt(this.AppUser_App_Id.ToString());
            }
            set
            {
                encoded_app_id = value;
            }
        }
        public int appuser_id { get; set; }
        public int AppUser_App_Id { get; set; }
        public int AppUser_UserId { get; set; }
        public string AppUser_SubmittedByUser { get; set; }
        public string AppUser_RejectedByUser { get; set; }
        public string AppUser_DeletedByUser { get; set; }
        public string AppUser_CreatedByUser { get; set; }
        public string AppUser_RejectedReason { get; set; }
        public string AppUser_DeletedReason { get; set; }
        public string AppUser_ReadyToEnrollByUser { get; set; }
        public string RTE_Comment { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedTime { get; set; }
        public string SumittedDate { get; set; }
        public string RejectedOn { get; set; }
        public string DeletedOn { get; set; }
        public string EnrolledDate { get; set; }
        public string IsSubmitted { get; set; }
        public string IsRejected { get; set; }
        public string IsDeleted { get; set; }
        public string IsAmended { get; set; }
        public string IsReadyToEnroll { get; set; }
        public string App_Email { get; set; }
        public string applicantname { get; set; }
        public string ULR { get; set; }
        public int LearnerId { get; set; }
        public int App_AdvisorUserId { get; set; }
        public string SalesAdvisorName { get; set; }
    }
}
