using ds.portal.ui.Models;
using ds.portal.users.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ds.portal.leads.Models
{
    public class Lead
    {
        public int Lead_Id { get; set; }
        private string _encodeLeadId { get; set; }
        public string encodeLeadId
        {
            get
            {
                return Encryption.Encrypt(this.Lead_Id.ToString());
            }
            set
            {
                _encodeLeadId = value;
            }
        }

        public string Lead_DateOfEnquiry { get; set; }
        public string Lead_DealClosedDate { get; set; }
        public int Lead_Id_SourceOfEnquiry { get; set; }
        public string Lead_DateClosed { get; set; }
        public int Lead_LearnersEnrolled { get; set; }
        public string Lead_PreferredTimeToContact { get; set; }
        public int Lead_Id_CourseLevel { get; set; } = 0;
        [UIHint("_salesAdvisor")]
        public int Lead_Id_SalesAdvisor { get; set; }

        public int Users_ID { get; set; }
        public int? Lead_Id_Client { get; set; }
        public int? Lead_Id_TrainingProvider { get; set; }
        public string Lead_Email { get; set; }
        public bool? Lead_IsCallAttempt { get; set; }
        public bool? Lead_IsCallReach { get; set; }
        public bool? Lead_IsMeeting { get; set; }
        public bool? Lead_IsLetterSent { get; set; }
        public bool Lead_IsLevyPayingEmployer { get; set; }
        public bool? Lead_IsEnrolmentCancelled { get; set; }
        public string Lead_DateCancellation { get; set; }
        public int Lead_Id_LastResult { get; set; }
        public bool Lead_IsDealClosed { get; set; }
        public string Lead_ContactName { get; set; }
        public string Lead_Contact_Salutation { get; set; }
        public string Lead_Contact_FirstName { get; set; }
        public string Lead_Contact_LastName { get; set; }
        public int Lead_Contact_Id_JobTitle { get; set; }
        public string Lead_JobTitle { get; set; }
        public int Lead_Contact_Id_Department { get; set; }
        public int Lead_Contact_Id_Pathway { get; set; }
        public int Lead_Contact_Id_Registration { get; set; }
        public int Lead_Contact_Id_Country { get; set; }
        public string Lead_Contact_Phone { get; set; }
        public string Lead_Contact_TownCity { get; set; }
        public string Lead_Contact_Mobile { get; set; }
        public string Lead_Contact_Email { get; set; }
        public string Lead_Contact_Email2 { get; set; }
        public string Lead_Contact_Address1 { get; set; }
        public string Lead_Contact_Address2 { get; set; }
        public string Lead_Contact_Address3 { get; set; }
        public string Lead_Contact_Postcode { get; set; }
        public string Lead_Contact_Website { get; set; }
        public string Lead_Contact_WebSubject { get; set; }
        public string Lead_Contact_WebMessage { get; set; }
        public string Lead_CourseTitle { get; set; }
        public DateTime? Lead_DateCreated { get; set; } = DateTime.Now;
        public bool? Lead_IsDuplicate { get; set; }
        public int Lead_Id_Status { get; set; }
        public string Lead_Contact_County { get; set; }
        public int Lead_CreatedByUserId { get; set; }
        public int Lead_UpdatedByUserId { get; set; }
        public string Lead_Contact_Company_Postcode { get; set; }
        public string Lead_Contact_Company_Name { get; set; }
        public bool Lead_is_validated_duplicate { get; set; }
        public string Users_Username { get; set; }
        public int? Lead_Id_CourseLevel_Enquiry { get; set; }
        public string CourseLevelsEnquiry { get; set; }
        public DateTime Lead_DateUpdated { get; set; } = DateTime.Now;
        public string courseLevelEnquiryList { get; set; }
        public string Lead_Learner_Enrolment_Date { get; set; }
        public string Lead_ERN_Number { get; set; }
        public int Lead_Enrolment_Cancelled { get; set; }
        public string Lead_Enrolment_Cancelled_Date { get; set; }
        public string Lead_CompanyHouseReg { get; set;}
        public string Lead_CompanyLineManagerContactName { get; set; }
        public string Lead_CompanyDecisionMakerName { get; set; }
        public string Lead_CompanyEmail { get; set; }
        public long? lead_web_id { get; set; }
        public int Company_Id { get; set; }
        public int Company_WorkPlace_Id { get; set; }

        public int? oldLastResult { get; set; }
        public int? newLastResult { get; set; }
        public string lastResultSelect { get; set; }
    }

    public class Contact
    {
        public int Contact_Id { get; set; }
        public string Contact_Salutation { get; set; }
        public string Contact_FirstName { get; set; }
        public string Contact_LastName { get; set; }
        public int Contact_Id_Company { get; set; }
        public int Contact_Id_JobTitle { get; set; }
        public int Contact_Id_Department { get; set; }
        public int Contact_Id_Pathway { get; set; }
        public int Contact_Id_Registration { get; set; }
        public int Contact_Id_Country { get; set; }
        public string Contact_Phone { get; set; }
        public string Contact_Phone2 { get; set; }
        public string Contact_Mobile { get; set; }
        public string Contact_Mobile2 { get; set; }
        public string Contact_Email { get; set; }
        public string Contact_Email2 { get; set; }
        public string Contact_Address1 { get; set; }
        public string Contact_Address2 { get; set; }
        public string Contact_Address3 { get; set; }
        public string Contact_EmployerPostcode { get; set; }
        public string Contact_Website { get; set; }
    }

    public class DuplicateLeadModel
    {
        public int Lead_Id { get; set; }
        public int LD_Id_Lead { get; set; }
        public int LD_Id { get; set; }
        public int Duplicate_Lead_Id { get; set; }
        public string ContactName { get; set; }
        public string Duplicate_ContactName { get; set; }
        public string Mobile { get; set; }
        public string Duplicate_Mobile { get; set; }
        public string CourseLevel { get; set; }
        public string Duplicate_CourseLevel { get; set; }
        public string LastResult { get; set; }
        public string Duplicate_LastResult { get; set; }
        public string Status { get; set; }
        public string Duplicate_Status { get; set; }
        public string EnquiryDate { get; set; }
        public string Duplicate_EnquiryDate { get; set; }
        public string Email { get; set; }
        public string Duplicate_Email { get; set; }
        public int Sales_Advisor_Id { get; set; }
        public int Duplicate_Sales_Advisor_Id { get; set; }
        public IEnumerable<Users> AdvisorUsers { get; set; }
        public string Duplicate_Lead_Contact_Company_Postcode { get; set; }
        public string Duplicate_Lead_Contact_Company_Name { get; set; }
        public string Lead_Contact_Company_Postcode { get; set; }
        public string Lead_Contact_Company_Name { get; set; }
        public bool Lead_is_validated_duplicate { get; set; }
        public bool Duplicate_is_validated_duplicate { get; set; }

    }
    public class NewDuplicateLeadModel
    {
        public int ld_id_lead { get; set; }
        public int ld_id { get; set; }
        public int lead1_lead_id { get; set; }
        public int lead2_lead_id { get; set; }
         

        
        public string lead1_contact_name { get; set; }
        public string lead2_contact_name { get; set; }
       
        public string lead1_mobile { get; set; }
        public string lead2_mobile { get; set; }
        public string lead1_email { get; set; }
        public string lead2_email { get; set; }

        
        public string lead1_company { get; set; }
        public string lead2_company { get; set; }
        public string lead1_postcode { get; set; }
        public string lead2_postcode { get; set; }

        public string lead1_course_level { get; set; }
        public string lead2_course_level { get; set; }
        public string lead1_lastresult { get; set; }
        public string lead2_lastresult { get; set; }
        public string lead1_status { get; set; }
        public string lead2_status { get; set; }
 
        public string lead1_enquirydate { get; set; }
        public string lead2_enquirydate { get; set; }
        
        public int lead1_sales_advisor_id { get; set; }
        public int lead2_sales_advisor_id { get; set; }
        public IEnumerable<Users> AdvisorUsers { get; set; }
         public bool lead_is_validated_duplicate { get; set; }
        public bool duplicate_is_validated_duplicate { get; set; }

    }

    public class LeadFileUpload
    {        
        public string Sales_Advisor { get; set; }
        public string Company_Name { get; set; }
        public string Date_of_Enquiry { get; set; }
        public string Source_of_Enquiry { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Job_Title { get; set; }
        public string Pathway { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Registration { get; set; }
        public string Employer_Postcode { get; set; }
        //public string Enquiry_Closed { get; set; } Enrolment Date (enquiry closed)
        public string Enrolment_Date { get; set; }
        public string Learner_Enrolled { get; set; }
        public string Department { get; set; }
        public string CourseLevel_Apply { get; set; }
        public string Last_Results { get; set; }
        public string Agreed_Payment_Scheme { get; set; } //TP
        public string Lead_Cancellation_Date { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TownCity { get; set; }
        public string CourseLevel_Enquiry { get; set; }
        public bool isDuplicate { get; set; }
        public string ClosedDate { get; set; }
        public string Notes { get; set; }
        public int user_id { get; set; }


        public int optTitleSourcesId { get; set; }
        public int optTitlePathwayId { get; set; }
        public int optTitleRegistrationsId { get; set; }
        public int optTitleCourseLevelsId { get; set; }
        public int optTitleCourseLevelInquiryId { get; set; }
        public int Training_Provider_id { get; set; }
        public int optTitleLastResultsId { get; set; }
        public int optTitleJobTitlesId { get; set; }

        public bool contact_is_error { get; set; }
        public string contact_issues { get; set; }

        public long? lead_web_id { get; set; }

    }


    public class LeadUploadRequiredOptions
    { 
        public string OptHeader_Title { get; set; }
        public int Opt_Id { get; set; }
        public int Opt_Value { get; set; }
        public string Opt_Title { get; set; }
        public int Opt_Id_OptHeader { get; set; }


    }

    public class ImportNote
    {
      public string LeadId { get; set; }
      public string Note { get; set; }
      public int CategroyId { get; set; }

     public int SalesAdviser { get; set; }
     public string Message { get; set; }
     public bool isError { get; set; }
    }
    public class ImportStatus
    {
        public string LeadId { get; set; }
        public string Status { get; set; }

        public int SalesAdviser { get; set; }
        public string Message { get; set; }
        public bool isError { get; set; }
        public int StatusId { get; set; }
    }

    public class ImportLastResult
    {
        public string LeadId { get; set; }
        public string LastResult { get; set; }

        public int SalesAdviser { get; set; }
        public string Message { get; set; }
        public bool isError { get; set; }
        public int LastResultId { get; set; }
    }

    //public class QueueEmails
    //{
    //    public int qe_id { get; set; }
    //    public string qe_from { get; set; }
    //    public string qe_to { get; set; }
    //    public string qe_subject { get; set; }
    //    public string qe_body { get; set; }
    //    public bool qe_is_html { get; set; }
    //    public string qe_cc { get; set; }
    //    public string qe_attachments { get; set; }
    //    public int qe_priority { get; set; }
    //    public bool qe_sent { get; set; }
    //    public string qe_status { get; set; }
    //    public string qe_created_by { get; set; }
    //    public DateTime qe_created_date { get; set; }
    //    public DateTime qe_mod_date { get; set; }
    //    public string qe_bcc { get; set; }
    //}

}
