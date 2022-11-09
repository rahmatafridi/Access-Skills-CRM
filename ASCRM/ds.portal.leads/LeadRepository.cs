using Dapper;
using ds.core.configuration;
using ds.core.uow;
using ds.portal.leads.Models;
using ds.portal.leads.Models.Shared;
using ds.portal.users.Models;
using portal.models.Comman;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace ds.portal.leads
{
    public class LeadRepository : ILeadRepository
    {



        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public LeadRepository(IUOW unitOfWork, IConfigurationRepository iConfig)
        {


            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }


        #region "Lead stuff "


        public bool Insert(Lead lead)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //SP_Lead_Insert old
                    string storedProc = "[dbo].[SP_Lead_AddByAPI_new]";
                    var p = new DynamicParameters();

                    p.Add("@lead_contact_name", lead.Lead_ContactName);
                    p.Add("@lead_email", lead.Lead_Email);
                    p.Add("@lead_contact_mobile", lead.Lead_Contact_Mobile);
                    p.Add("@job_title", lead.Lead_JobTitle);   //  Lead_Contact_Id_JobTitle>> find the id
                    p.Add("@course_level_enquiry", lead.Lead_CourseTitle); // find the id?
                    p.Add("@web_subject", lead.Lead_Contact_WebSubject);
                    p.Add("@web_message", lead.Lead_Contact_WebMessage);
                    p.Add("@lead_web_id", lead.lead_web_id);

                    //p.Add("@LeadName", lead.Lead_ContactName);
                    //p.Add("@LeadEmail", lead.Lead_Email);
                    //p.Add("@Phone", lead.Lead_Contact_Mobile);
                    //p.Add("@JobTitle", lead.Lead_JobTitle);   //  Lead_Contact_Id_JobTitle               
                    //p.Add("@CourseName", lead.Lead_CourseTitle);
                    //p.Add("@Subject", lead.Lead_Contact_WebSubject);
                    //p.Add("@Message", lead.Lead_Contact_WebMessage);
                    //p.Add("@Lead_Id_LastResult", lead.Lead_Id_LastResult);
                    //p.Add("@DepartmentId", lead.Lead_Contact_Id_Department);
                    //p.Add("@Salutation", lead.Lead_Contact_Salutation);

                    int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return true;
        }
        public Lead InsertUpdateLeadRecord(Lead lead)
        {
            CultureInfo ukCulture = new CultureInfo("en-GB");//CultureInfo.CreateSpecificCulture("en-GB")

            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    DateTime? dtDateOfEnquiry = string.IsNullOrEmpty(lead.Lead_DateOfEnquiry) ? null : (DateTime?)DateTime.Parse(lead.Lead_DateOfEnquiry, ukCulture.DateTimeFormat);
                    DateTime? dtDateClosed = string.IsNullOrEmpty(lead.Lead_DateClosed) ? null : (DateTime?)DateTime.Parse(lead.Lead_DateClosed, ukCulture.DateTimeFormat);
                    DateTime? dtDateCancellation = string.IsNullOrEmpty(lead.Lead_DateCancellation) ? null : (DateTime?)DateTime.Parse(lead.Lead_DateCancellation, ukCulture.DateTimeFormat);
                    DateTime? dtDealClosedDate = string.IsNullOrEmpty(lead.Lead_DealClosedDate) ? null : (DateTime?)DateTime.Parse(lead.Lead_DealClosedDate, ukCulture.DateTimeFormat);
                    DateTime? dtLearnerEnrolmentDate = string.IsNullOrEmpty(lead.Lead_Learner_Enrolment_Date) ? null : (DateTime?)DateTime.Parse(lead.Lead_Learner_Enrolment_Date, ukCulture.DateTimeFormat);
                    DateTime? dtEnrolmentCancelledDate = string.IsNullOrEmpty(lead.Lead_Enrolment_Cancelled_Date) ? null : (DateTime?)DateTime.Parse(lead.Lead_Enrolment_Cancelled_Date, ukCulture.DateTimeFormat);

                    //DateTime? dtDateOfEnquiry = string.IsNullOrEmpty(lead.Lead_DateOfEnquiry) ? null : (DateTime?)Convert.ToDateTime(lead.Lead_DateOfEnquiry);
                    //DateTime? dtDateClosed = string.IsNullOrEmpty(lead.Lead_DateClosed) ? null : (DateTime?)Convert.ToDateTime(lead.Lead_DateClosed);
                    //DateTime? dtDateCancellation = string.IsNullOrEmpty(lead.Lead_DateCancellation) ? null : (DateTime?)Convert.ToDateTime(lead.Lead_DateCancellation);
                    //DateTime? dtDealClosedDate = string.IsNullOrEmpty(lead.Lead_DealClosedDate) ? null : (DateTime?)Convert.ToDateTime(lead.Lead_DealClosedDate);
                    //DateTime? dtLearnerEnrolmentDate = string.IsNullOrEmpty(lead.Lead_Learner_Enrolment_Date) ? null : (DateTime?)Convert.ToDateTime(lead.Lead_Learner_Enrolment_Date);
                    //DateTime? dtEnrolmentCancelledDate = string.IsNullOrEmpty(lead.Lead_Enrolment_Cancelled_Date) ? null : (DateTime?)Convert.ToDateTime(lead.Lead_Enrolment_Cancelled_Date);

                    dynamicParams.Add("@Lead_Id", lead.Lead_Id);
                    dynamicParams.Add("@Lead_DateOfEnquiry", dtDateOfEnquiry);
                    dynamicParams.Add("@Lead_Id_SourceOfEnquiry", lead.Lead_Id_SourceOfEnquiry);
                    dynamicParams.Add("@Lead_DateClosed", dtDateClosed);
                    dynamicParams.Add("@Lead_LearnersEnrolled", lead.Lead_LearnersEnrolled);
                    dynamicParams.Add("@Lead_PreferredTimeToContact", lead.Lead_PreferredTimeToContact);
                    dynamicParams.Add("@Lead_Id_CourseLevel", lead.Lead_Id_CourseLevel);
                    dynamicParams.Add("@Lead_Id_SalesAdvisor", lead.Lead_Id_SalesAdvisor);
                    dynamicParams.Add("@Lead_Id_Client", lead.Lead_Id_Client);
                    dynamicParams.Add("@Lead_Id_TrainingProvider", lead.Lead_Id_TrainingProvider);
                    dynamicParams.Add("@Lead_Email", lead.Lead_Email);
                    dynamicParams.Add("@Lead_IsCallAttempt", lead.Lead_IsCallAttempt);
                    dynamicParams.Add("@Lead_IsCallReach", lead.Lead_IsCallReach);
                    dynamicParams.Add("@Lead_IsMeeting", lead.Lead_IsMeeting);
                    dynamicParams.Add("@Lead_IsLetterSent", lead.Lead_IsLetterSent);
                    dynamicParams.Add("@Lead_IsLevyPayingEmployer", lead.Lead_IsLevyPayingEmployer);
                    dynamicParams.Add("@Lead_IsEnrolmentCancelled", lead.Lead_IsEnrolmentCancelled);
                    dynamicParams.Add("@Lead_DateCancellation", dtDateCancellation);
                    dynamicParams.Add("@Lead_Id_LastResult", lead.Lead_Id_LastResult);
                    dynamicParams.Add("@Lead_UpdatedByUserId", lead.Lead_UpdatedByUserId);
                    dynamicParams.Add("@Lead_UpdatedByUserName", "");
                    dynamicParams.Add("@Lead_DateUpdated", lead.Lead_DateUpdated);
                    dynamicParams.Add("@Lead_IsDealClosed", lead.Lead_IsDealClosed);
                    dynamicParams.Add("@Lead_ContactName", lead.Lead_ContactName);
                    dynamicParams.Add("@Lead_Contact_Salutation", lead.Lead_Contact_Salutation);
                    dynamicParams.Add("@Lead_Contact_FirstName", lead.Lead_Contact_FirstName);
                    dynamicParams.Add("@Lead_Contact_LastName", lead.Lead_Contact_LastName);
                    dynamicParams.Add("@Lead_Contact_Company_Name", lead.Lead_Contact_Company_Name);
                    dynamicParams.Add("@Lead_Contact_Id_JobTitle", lead.Lead_Contact_Id_JobTitle);
                    dynamicParams.Add("@Lead_Contact_Id_Department", lead.Lead_Contact_Id_Department);
                    dynamicParams.Add("@Lead_Contact_Id_Pathway", lead.Lead_Contact_Id_Pathway);
                    dynamicParams.Add("@Lead_Contact_Id_Registration", lead.Lead_Contact_Id_Registration);
                    dynamicParams.Add("@Lead_Contact_Id_Country", lead.Lead_Contact_Id_Country);
                    dynamicParams.Add("@Lead_Contact_Phone", lead.Lead_Contact_Phone);
                    dynamicParams.Add("@Lead_Contact_TownCity", lead.Lead_Contact_TownCity);
                    dynamicParams.Add("@Lead_Contact_Mobile", lead.Lead_Contact_Mobile);
                    dynamicParams.Add("@Lead_Contact_Email", lead.Lead_Contact_Email);
                    dynamicParams.Add("@Lead_Contact_Email2", lead.Lead_Contact_Email2);
                    dynamicParams.Add("@Lead_Contact_Address1", lead.Lead_Contact_Address1);
                    dynamicParams.Add("@Lead_Contact_Address2", lead.Lead_Contact_Address2);
                    dynamicParams.Add("@Lead_Contact_Address3", lead.Lead_Contact_Address3);
                    dynamicParams.Add("@Lead_Contact_Postcode", lead.Lead_Contact_Postcode);
                    dynamicParams.Add("@Lead_Contact_Website", lead.Lead_Contact_Website);
                    dynamicParams.Add("@Lead_Contact_WebSubject", lead.Lead_Contact_WebSubject);
                    dynamicParams.Add("@Lead_Contact_WebMessage", lead.Lead_Contact_WebMessage);
                    dynamicParams.Add("@Lead_Id_Status", lead.Lead_Id_Status);
                    dynamicParams.Add("@Lead_Contact_County", lead.Lead_Contact_County);
                    dynamicParams.Add("@Lead_Contact_Company_Postcode", lead.Lead_Contact_Company_Postcode);
                    dynamicParams.Add("@Lead_is_validated_duplicate", lead.Lead_is_validated_duplicate);
                    dynamicParams.Add("@Lead_DealClosedDate", dtDealClosedDate);
                    dynamicParams.Add("@Lead_Id_CourseLevel_Enquiry", lead.Lead_Id_CourseLevel_Enquiry);
                    dynamicParams.Add("@Lead_Learner_Enrolment_Date", dtLearnerEnrolmentDate);
                    dynamicParams.Add("@Lead_ERN_Number", lead.Lead_ERN_Number);
                    dynamicParams.Add("@Lead_Enrolment_Cancelled_Date", dtEnrolmentCancelledDate);
                    dynamicParams.Add("@Lead_Enrolment_Cancelled", lead.Lead_Enrolment_Cancelled);
                    dynamicParams.Add("@Lead_CompanyHouseReg", lead.Lead_CompanyHouseReg);

                    dynamicParams.Add("@Lead_CompanyLineManagerContactName", lead.Lead_CompanyLineManagerContactName);
                    dynamicParams.Add("@Lead_CompanyDecisionMakerName", lead.Lead_CompanyDecisionMakerName);
                    dynamicParams.Add("@Lead_CompanyEmail", lead.Lead_CompanyEmail);
                    dynamicParams.Add("@Company_Id", lead.Company_Id);
                    dynamicParams.Add("@Company_WorkPlace_Id", lead.Company_WorkPlace_Id);

                    if (lead.Lead_Id > 0)
                    {
                        string storedProc = "[dbo].[SP_Lead_Update]";
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_Lead_Add]";
                        var id = conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (id > 0)
                        {
                            lead.Lead_Id = id;
                        }
                    }
                    if (DeleteCourseLevelEnquiryMappingByLeadId(lead.Lead_Id))
                    {
                        if (!string.IsNullOrEmpty(lead.courseLevelEnquiryList))
                        {
                            string storedProc = "[dbo].[SP_Lead_CourseLevelsMapping_Add]";

                            if (lead.courseLevelEnquiryList.Contains(','))
                            {
                                var courseLevelEnquiryIds = lead.courseLevelEnquiryList.Split(',');
                                foreach (var courseId in courseLevelEnquiryIds)
                                {
                                    if (!string.IsNullOrEmpty(courseId))
                                    {
                                        object dynamicParams1 = new
                                        {
                                            lead_id = lead.Lead_Id,
                                            course_level_id = Convert.ToInt32(courseId.Trim())
                                        };
                                        conn.Query<DropdownOption>(storedProc, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                                    }
                                }
                            }
                            else
                            {
                                object dynamicParams2 = new
                                {
                                    lead_id = lead.Lead_Id,
                                    course_level_id = lead.courseLevelEnquiryList
                                };
                                conn.Query<DropdownOption>(storedProc, param: dynamicParams2, commandType: CommandType.StoredProcedure);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lead;
        }
        public IEnumerable<Contact> GetAllContactsForDropdown()
        {
            IEnumerable<Contact> contacts = new List<Contact>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Contacts_GetForDropdown]";

                    contacts = conn.Query<Contact>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return contacts;
        }
        public IEnumerable<LeadViewModel> GetAllLeads(int userId, bool isAdmin)
        {
            IEnumerable<LeadViewModel> leads = new List<LeadViewModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetAll]";
                    object dynamicParams = new { @UserId = userId, @isAdmin = isAdmin };
                    leads = conn.Query<LeadViewModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return leads;
        }
        public Lead GetLeadById(int id)
        {
            Lead lead = new Lead();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_Get]";
                    object dynamicParams = new { Lead_Id = id };

                    lead = conn.QueryFirstOrDefault<Lead>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return lead;
        }
        public bool DeleteLeadById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdLead_DeleteById]";
                    object dynamicParams = new { Lead_Id = id, Lead_DeletedByUserId = userId, Lead_DeletedByUserName = userName };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return true;
        }
        public Contact GetContactById(int id)
        {
            Contact contact = new Contact();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Contacts_GetById]";
                    object dynamicParams = new { Contact_Id = id };

                    contact = conn.QuerySingleOrDefault<Contact>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return contact;
        }
        public void InsertLeadImportLog(string ip, int total)
        {
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {

                    dynamicParams.Add("@ip", ip);

                    string storedProc = "[dbo].[SP_Lead_LeadImportLog]";
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                }
                catch (Exception)
                {
                    throw;
                }
            }

        }


        private bool DeleteCourseLevelEnquiryMappingByLeadId(int leadId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_CourseLevelEnquiry_Delete]";
                    object dynamicParams = new { lead_id = leadId };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        #endregion

        #region Modal at dashboard, grid  results

        public IEnumerable<LeadViewModel> GetAllLeadsByAgreedPaymentScheme(int userId, bool isAdmin, int agreed_pay_val)
        {
            IEnumerable<LeadViewModel> leads = new List<LeadViewModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetAllByAgreedPaymentScheme]";
                    object dynamicParams = new { @UserId = userId, @isAdmin = isAdmin, @agreed_pay_val = agreed_pay_val };
                    leads = conn.Query<LeadViewModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return leads;
        }
        public IEnumerable<LeadViewModel> GetAllLeadsByLastResults(int userId, bool isAdmin, int last_result_val)
        {
            IEnumerable<LeadViewModel> leads = new List<LeadViewModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetAllByLastResults]";
                    object dynamicParams = new { @UserId = userId, @isAdmin = isAdmin, @last_result_val = last_result_val };
                    leads = conn.Query<LeadViewModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return leads;
        }



        #endregion

        #region Courses
        public IEnumerable<Course> GetAllCourses()
        {
            IEnumerable<Course> courses = new List<Course>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdLead_CourseLevel_GetAll]";

                    courses = conn.Query<Course>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return courses;
        }
        public IEnumerable<CourseLevel> GetAllCourseLevels()
        {
            IEnumerable<CourseLevel> courseLevels = new List<CourseLevel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdLead_CourseLevel_GetAll]";

                    courseLevels = conn.Query<CourseLevel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return courseLevels;
        }
        public IEnumerable<CourseLevel> GetAllCourseLevelEnquiries()
        {
            IEnumerable<CourseLevel> courseLevels = new List<CourseLevel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdLead_CourseLevelEnquiry_GetAll]";

                    courseLevels = conn.Query<CourseLevel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return courseLevels;
        }
        #endregion

        #region Lead Notes
        public bool InsertUpdateLeadNoteRecord(Notes note)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    note.Note_DateCreated = DateTime.Now;
                    if (note.Note_Id > 0)
                    {
                        string storedProc = "[dbo].[SP_mdlead_Notes_Update]";
                        object dynamicParams = note;
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_mdlead_Notes_Insert]";
                        object dynamicParams = note;
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public IEnumerable<Notes> GetAllLeadNotes(int lead_Id)
        {
            IEnumerable<Notes> notes = new List<Notes>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_Notes_GetAllByLeadId]";
                    object dynamicParams = new { Note_Id_Lead = lead_Id };

                    notes = conn.Query<Notes>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return notes;
        }
        public Notes GetNoteById(int id)
        {
            Notes note = new Notes();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Notes_GetById]";
                    object dynamicParams = new { Note_Id = id };

                    note = conn.QuerySingleOrDefault<Notes>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return note;
        }
        public bool DeleteNoteById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Notes_DeleteById]";
                    object dynamicParams = new { Note_Id = id, Note_DeletedByUserId = userId, Note_DeletedByUserName = userName };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        #endregion

        #region Lead Documents
        public IEnumerable<Documents> GetAllLeadDocuments(int lead_Id)
        {
            IEnumerable<Documents> documents = new List<Documents>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Documents_GetAllByLeadId]";
                    object dynamicParams = new { Document_Id_Lead = lead_Id };

                    documents = conn.Query<Documents>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return documents;
        }
        public bool InsertUpdateLeadDocumentRecord(Documents document)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    document.Document_DateCreated = DateTime.Now;
                    if (document.Document_Id > 0)
                    {
                        string storedProc = "[dbo].[SP_mdlead_Documents_Update]";
                        object dynamicParams = document;
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_mdlead_Documents_Insert]";
                        object dynamicParams = document;
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public Documents GetDocumentById(int id)
        {
            Documents document = new Documents();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Documents_GetById]";
                    object dynamicParams = new { Document_Id = id };

                    document = conn.QuerySingleOrDefault<Documents>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return document;
        }
        public bool DeleteDocumentById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Documents_DeleteById]";
                    object dynamicParams = new { Document_Id = id, Document_DeletedByUserId = userId, Document_DeletedByUserName = userName };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public Documents DownloadFile(int documentId)
        {
            Documents document = new Documents();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Document_GetDocumentByID]";
                    /// document module 1 for lead document.
                    object dynamicParams = new { @document_Id = documentId, @document_module = 1 };
                    document = conn.QuerySingleOrDefault<Documents>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return document;
        }
        #endregion

        #region Lead Activities
        public IEnumerable<ActivityType> GetAllActivityTypes()
        {
            IEnumerable<ActivityType> activityTypes = new List<ActivityType>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_GetAllActivityTypes]";

                    activityTypes = conn.Query<ActivityType>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return activityTypes;
        }
        public Lead CheckDuplicateLead(string contactName, string email, string mobile, int userId, bool isAdmin)
        {
            Lead lead = new Lead();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_CheckDuplicate]";
                    object dynamicParams = new
                    {
                        @ContactName = string.IsNullOrEmpty(contactName) ? null : contactName,
                        @ContactEmail = string.IsNullOrEmpty(email) ? null : email,
                        @ContactMobile = string.IsNullOrEmpty(mobile) ? null : mobile,
                        @userId = userId,
                        @isAdmin = isAdmin
                    };
                    lead = conn.QuerySingleOrDefault<Lead>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lead;
        }
        public bool InsertUpdateLeadActivityRecord(Activity activity)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (activity.Activity_Id_Type == 1)
                    {
                        if (!string.IsNullOrEmpty(activity.Activity_Reminder_Date))
                        {
                            activity.Activity_Date = activity.Activity_Reminder_Date;
                        }
                    }

                    activity.Activity_DateCreated = DateTime.Now;

                    DateTime? dtActivityDate = string.IsNullOrEmpty(activity.Activity_Date) ? null : (DateTime?)Convert.ToDateTime(activity.Activity_Date);

                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@Activity_Id_Lead", activity.Activity_Id_Lead);
                    dynamicParams.Add("@Activity_Id_Type", activity.Activity_Id_Type);
                    dynamicParams.Add("@Activity_Id_Action", activity.Activity_Id_Action);
                    dynamicParams.Add("@Activity_Date", dtActivityDate);
                    dynamicParams.Add("@Activity_Note", activity.Activity_Note);
                    dynamicParams.Add("@Activity_EmailBody", activity.Activity_EmailBody);
                    dynamicParams.Add("@Activity_EmailSubject", activity.Activity_EmailSubject);
                    dynamicParams.Add("@Activity_Location", activity.Activity_Location);
                    dynamicParams.Add("@@Activity_CreatedByUserId", activity.Activity_CreatedByUserId);
                    dynamicParams.Add("@Activity_CreatedByUserName", activity.Activity_CreatedByUserName);
                    dynamicParams.Add("@Activity_DateCreated", activity.Activity_DateCreated);
                    dynamicParams.Add("@Activity_Id", activity.Activity_Id);

                    if (activity.Activity_Id > 0)
                    {
                        string storedProc = "[dbo].[SP_mdlead_Activity_Update]";
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_mdlead_Activity_Insert]";
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public IEnumerable<ActivityViewModel> GetAllLeadActivities(int lead_Id)
        {
            IEnumerable<ActivityViewModel> activities = new List<ActivityViewModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Activity_GetAllByLeadId]";
                    object dynamicParams = new { Activity_Id_Lead = lead_Id };

                    activities = conn.Query<ActivityViewModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return activities;
        }
        public Activity GetActivityById(int id)
        {
            Activity activity = new Activity();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Activity_GetById]";
                    object dynamicParams = new { Activity_Id = id };

                    activity = conn.QuerySingleOrDefault<Activity>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return activity;
        }
        public bool DeleteActivityById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Activity_DeleteById]";
                    object dynamicParams = new { Activity_Id = id, Activity_DeletedByUserId = userId, Activity_DeletedByUserName = userName };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        #endregion

        #region " QueueEmailModel "
        public bool InsertQueueEmail(QueueEmailModel queueEmailModel)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //// inserting into queue table.
                    string _storedProc = "[dbo].[UP_Web_LeadActivity_QueueEmail_Insert]";
                    object _dynamicParams = queueEmailModel;
                    conn.Query<QueueEmailModel>(_storedProc, param: _dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool UpdateSentEmail(QueueEmailModel queueEmailModel)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //// inserting into queue table.
                    string _storedProc = "[dbo].[UP_Web_LeadActivity_QueueEmail_update]";
                    object dynamicParams = new { @qe_id = queueEmailModel.qe_id };
                    conn.Query<QueueEmailModel>(_storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        #endregion



        #region Sales Advisor
        public int GetAvailableSalesAdvisorIdRota()
        {
            int salesAdvisorId = 0;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdLead_SalesAdvisors_GetAvailableIdRota]";

                    salesAdvisorId = conn.QuerySingleOrDefault<int>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return salesAdvisorId;
        }

        #endregion

        #region Duplicate Lead

        public DuplicateLeadModel GetDuplicateLeadLeadById(int id)
        {
            DuplicateLeadModel _duplicateLead = new DuplicateLeadModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetDuplicateByDuplicateID]";
                    object dynamicParams = new { @duplicate_Lead_Id = id };
                    var _lead = conn.QueryFirstOrDefault<DuplicateLeadModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    if (_lead != null)
                    {
                        int firstLead = _lead.Duplicate_Lead_Id;
                        int secondLead = _lead.LD_Id_Lead;

                        var duplicateLead = GetLeadByLeadId(firstLead);
                        var lead = GetLeadByLeadId(secondLead);
                        if (duplicateLead != null)
                        {
                            _duplicateLead = new DuplicateLeadModel
                            {
                                ContactName = lead.Lead_ContactName,
                                Duplicate_ContactName = duplicateLead.Lead_ContactName,
                                Mobile = lead.Lead_Contact_Mobile,
                                Duplicate_Mobile = duplicateLead.Lead_Contact_Mobile,
                                CourseLevel = lead.Lead_CourseTitle,
                                Duplicate_CourseLevel = duplicateLead.Lead_CourseTitle,
                                LastResult = lead.Lead_LastResult,
                                Duplicate_LastResult = duplicateLead.Lead_LastResult,
                                Status = lead.Lead_Status,
                                Duplicate_Status = duplicateLead.Lead_Status,
                                EnquiryDate = lead.Lead_DateOfEnquiry,
                                Duplicate_EnquiryDate = duplicateLead.Lead_DateOfEnquiry,
                                Lead_Id = _lead.LD_Id_Lead,
                                Duplicate_Lead_Id = id,
                                LD_Id = _lead.LD_Id,
                                Email = lead.Lead_Contact_Email,
                                Duplicate_Email = duplicateLead.Lead_Contact_Email,
                                Sales_Advisor_Id = lead.Lead_Id_SalesAdvisor,
                                Duplicate_Sales_Advisor_Id = duplicateLead.Lead_Id_SalesAdvisor,
                                Duplicate_Lead_Contact_Company_Postcode = duplicateLead.Lead_Contact_Company_Postcode,
                                Duplicate_Lead_Contact_Company_Name = duplicateLead.Lead_Contact_Company_Name,
                                Lead_Contact_Company_Postcode = lead.Lead_Contact_Company_Postcode,
                                Lead_Contact_Company_Name = lead.Lead_Contact_Company_Name,
                                Duplicate_is_validated_duplicate = duplicateLead.Lead_is_validated_duplicate,
                                Lead_is_validated_duplicate = lead.Lead_is_validated_duplicate
                            };
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return _duplicateLead;
        }
        public bool DeleteDuplicateLeadById(int ld_id, int lead_id, int lead_duplicate_id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_DeleteDuplicateLead]";
                    object dynamicParams = new { @ld_id = ld_id, @lead_id = lead_id, @lead_duplicate_id = lead_duplicate_id };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool UpdateSalesAdvisor(int lead_id, int sales_advisor_id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_UpdateSalesAdvisor]";
                    object dynamicParams = new { @lead_id = lead_id, @sales_advisor_id = sales_advisor_id };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool RemoveLeadDuplicate(int lead_id, int ld_id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_RemovedDuplicateLead]";
                    object dynamicParams = new { @lead_id = lead_id, @ld_id = ld_id };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public LeadViewModel GetLeadByLeadId(int leadId)
        {
            LeadViewModel leads = new LeadViewModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetById]";
                    object dynamicParams = new { @LeadId = leadId };
                    leads = conn.QueryFirstOrDefault<LeadViewModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return leads;
        }
        private IEnumerable<Users> GetAllSalesAdvisorUsers()
        {
            IEnumerable<Users> optionsHeaders = new List<Users>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdLead_SalesAdvisors_GetAll]";

                    optionsHeaders = conn.Query<Users>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return optionsHeaders;
        }



        private List<LeadFileUpload> CheckExistAndInsert(List<LeadFileUpload> leadFileUploads)
        {
            IEnumerable<LeadFileUpload> contacts = new List<LeadFileUpload>();
            foreach (var item in leadFileUploads)
            {
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    try
                    {
                        string storedProc = "[dbo].[SP_Lead_BulkUpload_InsertIfExists]";
                        object dynamicParams = new
                        {
                            @sales_advisor = item.Sales_Advisor,
                            @optTitleSources = item.Source_of_Enquiry,
                            @optTitlePathway = item.Pathway,
                            @optTitleRegistrations = item.Registration,
                            @optTitleCourseLevels = item.CourseLevel_Enquiry,
                            @optTitleLastResults = item.Last_Results,
                            @optTitleJobTitles = item.Job_Title
                        };
                        contacts = conn.Query<LeadFileUpload>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (contacts != null)
                        {
                            foreach (var itm in contacts)
                            {
                                item.user_id = itm.user_id;
                                item.optTitleSourcesId = itm.optTitleSourcesId;
                                item.optTitlePathwayId = itm.optTitlePathwayId;
                                item.optTitleRegistrationsId = itm.optTitleRegistrationsId;
                                item.optTitleCourseLevelsId = itm.optTitleCourseLevelsId;
                                item.optTitleLastResultsId = itm.optTitleLastResultsId;
                                item.optTitleJobTitlesId = itm.optTitleJobTitlesId;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return leadFileUploads;
        }
        #endregion

        #region History
        public IEnumerable<HistoryModel> GetAllHistory(int user_id, int lead_id)
        {
            IEnumerable<HistoryModel> history = new List<HistoryModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetAllHistory]";
                    object dynamicParams = new { @lead_id = lead_id };
                    history = conn.Query<HistoryModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return history;
        }
        public DuplicateLeadModel GetDuplicateLead(int id)
        {
            DuplicateLeadModel _duplicateLead = new DuplicateLeadModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetDuplicateByLeadID]";
                    object dynamicParams = new { @Lead_Id = id };
                    var _lead = conn.QueryFirstOrDefault<DuplicateLeadModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    if (_lead != null)
                    {
                        if (_lead.Duplicate_Lead_Id > 0)
                        {
                            var duplicateLead = GetLeadByLeadId(_lead.Duplicate_Lead_Id);
                            var lead = GetLeadByLeadId(id);

                            _duplicateLead = new DuplicateLeadModel
                            {
                                ContactName = lead.Lead_ContactName,
                                Duplicate_ContactName = duplicateLead.Lead_ContactName,
                                Mobile = lead.Lead_Contact_Mobile,
                                Duplicate_Mobile = duplicateLead.Lead_Contact_Mobile,
                                CourseLevel = lead.Lead_CourseTitle,
                                Duplicate_CourseLevel = duplicateLead.Lead_CourseTitle,
                                LastResult = lead.Lead_LastResult,
                                Duplicate_LastResult = duplicateLead.Lead_LastResult,
                                Status = lead.Lead_Status,
                                Duplicate_Status = duplicateLead.Lead_Status,
                                EnquiryDate = lead.Lead_DateOfEnquiry,
                                Duplicate_EnquiryDate = duplicateLead.Lead_DateOfEnquiry,
                                Lead_Id = id,
                                Duplicate_Lead_Id = _lead.Duplicate_Lead_Id,
                                LD_Id = _lead.LD_Id,
                                Email = lead.Lead_Contact_Email,
                                Duplicate_Email = duplicateLead.Lead_Contact_Email,
                                Sales_Advisor_Id = lead.Lead_Id_SalesAdvisor,
                                Duplicate_Sales_Advisor_Id = duplicateLead.Lead_Id_SalesAdvisor,
                            };
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return _duplicateLead;
        }
        #endregion

        #region Bulk Lead Insert
        public List<LeadFileUpload> BulkInsert(List<LeadFileUpload> leadFileUploads, int userId)
        {
            List<LeadFileUpload> leadFileUploadsChecked = new List<LeadFileUpload>();
            leadFileUploadsChecked = ValidateLeadsForImport(leadFileUploads);

            List<LeadFileUpload> failedLeads = new List<LeadFileUpload>();
            //bulk import uploads todo yas
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                foreach (var item in leadFileUploadsChecked)
                {
                    if (item.Contact != "" && item.contact_is_error == false)
                    {
                        string storedProc = "[dbo].[SP_Lead_Import]";
                        var p = new DynamicParameters();

                        p.Add("@Lead_SalesAdvisor", item.Sales_Advisor);
                        p.Add("@Lead_Contact_Company_Name", item.Company_Name);
                        p.Add("@Lead_DateOfEnquiry", ds.core.common.DateUtilities.ConvertDateToSQL(item.Date_of_Enquiry));
                        p.Add("@Lead_SourceOfEnquiry", item.Source_of_Enquiry);
                        p.Add("@Lead_ContactName", item.Contact);
                        p.Add("@Lead_Contact_Email", item.Email);
                        p.Add("@Lead_JobTitle", item.Job_Title);
                        p.Add("@Lead_Pathway", item.Pathway);
                        p.Add("@Lead_Contact_Phone", item.Phone);
                        p.Add("@Lead_Contact_Mobile", item.Mobile);
                        p.Add("@Lead_Registration", item.Registration);


                        var _postCode = string.Empty;
                        var _webSubject = string.Empty;
                        if (!string.IsNullOrEmpty(item.Employer_Postcode))
                        {
                            if (item.Employer_Postcode.Contains(','))
                            {
                                var items = item.Employer_Postcode.Split(',');
                                _postCode = items[items.Length - 1];
                                foreach (var itm in items)
                                {
                                    if (_postCode != itm)
                                    {
                                        _webSubject += itm;
                                    }
                                }
                            }
                            else
                            {
                                string last4Char = (item.Employer_Postcode.Length > 3) ? item.Employer_Postcode.Substring(item.Employer_Postcode.Length - 4, 4) : item.Employer_Postcode;
                                var val = last4Char.Split(' ');
                                if (val.Length > 1)
                                {
                                    _postCode = item.Employer_Postcode;
                                }
                                else
                                {
                                    _webSubject = item.Employer_Postcode;
                                }
                            }
                        }

                        p.Add("@Lead_Contact_Company_Postcode", _postCode);
                        p.Add("@Lead_Enrolment_Date", ds.core.common.DateUtilities.ConvertDateToSQL(item.Enrolment_Date));//Lead_DealClosedDate param Lead_Enrolment_Date
                        p.Add("@Lead_Learner_Enrolled", (item.Learner_Enrolled.ToLower() == "yes" ? "1" : "0"));//Learner_Enrolled param Lead_Learner_Enrolled
                        p.Add("@Lead_Department", item.Department); //param Lead_Department
                        p.Add("@Lead_CourseLevelApply", item.CourseLevel_Apply);
                        p.Add("@Lead_LastResult", item.Last_Results);
                        p.Add("@Lead_Agreed_Payment_Scheme", item.Agreed_Payment_Scheme); //old Lead_Id_TrainingProvider param Lead_Agreed_Payment_Scheme
                        p.Add("@Lead_Enrolment_Cancellation_Date", ds.core.common.DateUtilities.ConvertDateToSQL(item.Lead_Cancellation_Date));//Lead_Cancellation_Date  param Lead_Enrolment_Cancellation_Date
                        p.Add("@Lead_Contact_Address1", item.Address1);
                        p.Add("@Lead_Contact_Address2", item.Address2);
                        p.Add("@Lead_Contact_TownCity", item.TownCity);
                        p.Add("@Lead_CourseLevelEnquiry", item.CourseLevel_Enquiry);
                        p.Add("@Notes", item.Notes);

                        p.Add("@Lead_Contact_WebSubject", _webSubject);

                        p.Add("@Lead_CreatedByUserId", userId);

                        int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);

                    }
                    else
                    {

                        failedLeads.Add(item);
                    }
                }
            }

            if (failedLeads.Count > 0)
            {

            }
            return failedLeads;
        }


        public List<ImportNote> BulkImportNote(List<ImportNote> importNotes, int userId, string role, string UserName)
        {
            List<ImportNote> leadFileUploadsChecked = new List<ImportNote>();
            leadFileUploadsChecked = ValidateLeadNoteImport(importNotes, userId, role);

            List<ImportNote> failedLeads = new List<ImportNote>();
            //bulk import uploads todo yas
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                foreach (var item in leadFileUploadsChecked)
                {
                    if (item.LeadId != "" && item.isError == false)
                    {
                        string storedProc = "[dbo].[SP_mdlead_Notes_Insert]";
                        var p = new DynamicParameters();

                        p.Add("@Note_Id", null);
                        p.Add("@Note_Id_Lead", item.LeadId);
                        p.Add("@Note_Description ", item.Note);
                        p.Add("@Note_Id_Category", 1);
                        p.Add("@Note_Category", "");
                        p.Add("@Note_CreatedByUserId", userId);
                        p.Add("@Note_CreatedByUserName", UserName);
                        p.Add("@Note_UpdatedByUserId", userId);
                        p.Add("@Note_DateCreated", DateTime.Now);
                        p.Add("@Note_UpdatedByUserName", role);
                        p.Add("@Note_IsDeleted", true);
                        p.Add("@Note_DeletedByUserId", userId);
                        p.Add("@Note_DeletedByUserName", UserName);

                        int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);

                    }
                    else
                    {

                        failedLeads.Add(item);
                    }
                }
            }

            if (failedLeads.Count > 0)
            {

            }
            return failedLeads;
        }

        public List<ImportStatus> BulkImportStatus(List<ImportStatus> importStatus, int userId, string role)
        {
            List<ImportStatus> leadFileUploadsChecked = new List<ImportStatus>();
            leadFileUploadsChecked = ValidateLeadStatusImport(importStatus, userId, role);

            List<ImportStatus> failedLeads = new List<ImportStatus>();
            //bulk import uploads todo yas
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                foreach (var item in leadFileUploadsChecked)
                {
                    if (item.LeadId != "" && item.isError == false)
                    {
                        string storedProc = "[dbo].[SP_Lead_Update_Status]";
                        var p = new DynamicParameters();

                        p.Add("@Lead_Id", item.LeadId);
                        p.Add("@Lead_UpdatedByUserId ", userId);
                        p.Add("@Lead_UpdatedByUserName", role);
                        p.Add("@Lead_DateUpdated", DateTime.Now);
                        p.Add("@Lead_Id_Status", item.StatusId);
                        p.Add("@Lead_Status", "");


                        int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);

                    }
                    else
                    {

                        failedLeads.Add(item);
                    }
                }
            }

            if (failedLeads.Count > 0)
            {

            }
            return failedLeads;
        }
        public List<ImportLastResult> BulkImportLastResult(List<ImportLastResult> importStatus, int userId, string role)
        {
            List<ImportLastResult> leadFileUploadsChecked = new List<ImportLastResult>();
            leadFileUploadsChecked = ValidateLeadLastResultImport(importStatus, userId, role);

            List<ImportLastResult> failedLeads = new List<ImportLastResult>();
            //bulk import uploads todo yas
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                foreach (var item in leadFileUploadsChecked)
                {
                    if (item.LeadId != "" && item.isError == false)
                    {
                        string storedProc = "[dbo].[SP_Lead_Update_LastResult]";
                        var p = new DynamicParameters();

                        p.Add("@Lead_Id", item.LeadId);
                        p.Add("@Lead_UpdatedByUserId ", userId);
                        p.Add("@Lead_UpdatedByUserName", role);
                        p.Add("@Lead_DateUpdated", DateTime.Now);
                        p.Add("@Lead_Id_LastResults", item.LastResultId);


                        int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);

                    }
                    else
                    {

                        failedLeads.Add(item);
                    }
                }
            }

            if (failedLeads.Count > 0)
            {

            }
            return failedLeads;
        }


        private IEnumerable<LeadUploadRequiredOptions> getAllOptions()
        {
            IEnumerable<LeadUploadRequiredOptions> _options = new List<LeadUploadRequiredOptions>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetRequiredOptions]";
                    //object dynamicParams = new { @lead_id = lead_id, @sales_advisor_id = sales_advisor_id };
                    _options = conn.Query<LeadUploadRequiredOptions>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return _options;
        }
        private bool IsOptionContains(IEnumerable<LeadUploadRequiredOptions> options, int opt_header, string find_val)
        {
            if (find_val == "") return true;

            foreach (LeadUploadRequiredOptions opt in options)
            {
                if (opt.Opt_Id_OptHeader == opt_header)
                {
                    if (opt.Opt_Title.ToLower().Trim() == find_val.ToLower().Trim())
                    {
                        return true;

                    }

                }



            }
            return false;

        }



        public List<LeadFileUpload> ValidateLeadsForImport(List<LeadFileUpload> leadFileUploads)
        {
            IEnumerable<LeadUploadRequiredOptions> _options = getAllOptions();
            string current_option = "";
            if (_options != null)
            {
                IEnumerable<LeadFileUpload> contacts_to_check = new List<LeadFileUpload>();
                foreach (var item in leadFileUploads)
                {
                    //check 
                    /*1-Job Title	
                    2	Departments
                    3	LastResults
                    4	Pathways
                    5	Registrations
                    7	Sources
                    17	CourseLevels
                    18	AgreedPaymentScheme
                    20	CourseLevelsEnquiry
                    */
                    //1-Job Title
                    current_option = "Job Title";
                    if (!IsOptionContains(_options, 1, item.Job_Title))
                    {
                        item.contact_is_error = true;
                        item.contact_issues += current_option + ",";
                    }
                    // 2	Departments
                    current_option = "Departments";
                    if (!IsOptionContains(_options, 2, item.Department))
                    {
                        item.contact_is_error = true;
                        item.contact_issues += current_option + ",";
                    }
                    //3 LastResults
                    current_option = "LastResults";
                    if (!IsOptionContains(_options, 3, item.Last_Results))
                    {
                        item.contact_is_error = true;
                        item.contact_issues += current_option + ",";
                    }


                    //4  Pathways
                    current_option = "Pathways";
                    if (!IsOptionContains(_options, 4, item.Pathway))
                    {
                        item.contact_is_error = true;
                        item.contact_issues += current_option + ",";
                    }
                    //5   Registrations
                    current_option = "Registrations";
                    if (!IsOptionContains(_options, 5, item.Registration))
                    {
                        item.contact_is_error = true;
                        item.contact_issues += current_option + ",";
                    }
                    //7   Sources
                    current_option = "Sources";
                    if (!IsOptionContains(_options, 7, item.Source_of_Enquiry))
                    {
                        item.contact_is_error = true;
                        item.contact_issues += current_option + ",";
                    }
                    //17  CourseLevels
                    current_option = "CourseLevels";
                    if (!IsOptionContains(_options, 17, item.CourseLevel_Apply))
                    {
                        item.contact_is_error = true;
                        item.contact_issues += current_option + ",";
                    }
                    //18  AgreedPaymentScheme
                    current_option = "AgreedPaymentScheme";
                    if (!IsOptionContains(_options, 18, item.Agreed_Payment_Scheme))
                    {
                        item.contact_is_error = true;
                        item.contact_issues += current_option + ",";
                    }
                    //20  CourseLevelsEnquiry
                    current_option = "CourseLevelsEnquiry";
                    if (item.CourseLevel_Enquiry.Contains(";"))
                    {
                        string[] levels = item.CourseLevel_Enquiry.Split(";");
                        if (levels.Length > 0)
                        {
                            foreach (string s in levels)
                            {
                                if (!IsOptionContains(_options, 20, s.Trim()))
                                {
                                    item.contact_is_error = true;
                                    item.contact_issues += s + ",";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!IsOptionContains(_options, 20, item.CourseLevel_Enquiry))
                        {
                            item.contact_is_error = true;
                            item.contact_issues += current_option + ",";
                        }
                    }

                }

            }



            return leadFileUploads;

        }

        public List<ImportNote> ValidateLeadNoteImport(List<ImportNote> leadFileUploads, int userId, string role)
        {
            Lead lead = new Lead();



            IEnumerable<ImportNote> Note_to_check = new List<ImportNote>();
            foreach (var item in leadFileUploads)
            {

                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    try
                    {
                        string storedProc = "[dbo].[SP_Lead_Get]";
                        object dynamicParams = new { Lead_Id = item.LeadId };

                        lead = conn.QueryFirstOrDefault<Lead>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (lead != null)
                        {
                            if (role == "Sales Advisor")

                            {
                                if (lead.Lead_Id_SalesAdvisor != userId)
                                {
                                    item.isError = true;
                                    item.Message = "this lead is not assigned to you";


                                }
                                else
                                {
                                    if (lead.Lead_Id != Convert.ToInt32(item.LeadId))
                                    {
                                        item.Message = "will show an error and not to be imported";
                                        item.isError = true;
                                    }
                                    else
                                    {
                                        item.isError = false;
                                    }

                                }
                            }
                        }
                        else
                        {
                            item.isError = true;
                            item.Message = "this lead is not available";

                        }


                    }
                    catch (Exception e)
                    {

                    }
                }






            }
            return leadFileUploads;
        }
        public List<ImportStatus> ValidateLeadStatusImport(List<ImportStatus> leadFileUploads, int userId, string role)
        {
            Lead lead = new Lead();

            IEnumerable<DropdownOption> options = new List<DropdownOption>();


            IEnumerable<ImportStatus> Note_to_check = new List<ImportStatus>();
            foreach (var item in leadFileUploads)
            {

                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    try
                    {
                        string storedProc = "[dbo].[SP_Lead_Get]";
                        object dynamicParams = new { Lead_Id = item.LeadId };

                        lead = conn.QueryFirstOrDefault<Lead>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (lead != null)
                        {

                            string storedProc1 = "[dbo].[SP_DDLOptions_GetByByHeaderId]";
                            object dynamicParams1 = new { Opt_Id_OptHeader = 16 };

                            options = conn.Query<DropdownOption>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                            if (options.Any(x => x.Opt_Title == item.Status))
                            {
                                item.StatusId = Convert.ToInt32(options.FirstOrDefault(x => x.Opt_Title == item.Status).Opt_Value);
                                if (role == "Sales Advisor")
                                {
                                    if (lead.Lead_Id_SalesAdvisor != userId)
                                    {
                                        item.isError = true;
                                        item.Message = "this lead is not assigned to you";


                                    }
                                    else
                                    {
                                        if (lead.Lead_Id != Convert.ToInt32(item.LeadId))
                                        {
                                            item.Message = "will show an error and not to be imported";
                                            item.isError = true;
                                        }
                                        else
                                        {
                                            item.isError = false;
                                        }

                                    }
                                }
                            }
                            else
                            {
                                item.isError = true;
                                item.Message = "Invalid Status";

                            }
                        }
                        else
                        {
                            item.isError = true;
                            item.Message = "this lead is not available";

                        }


                    }
                    catch (Exception e)
                    {

                    }
                }






            }
            return leadFileUploads;
        }
        public List<ImportLastResult> ValidateLeadLastResultImport(List<ImportLastResult> leadFileUploads, int userId, string role)
        {
            Lead lead = new Lead();

            IEnumerable<DropdownOption> options = new List<DropdownOption>();


            IEnumerable<ImportLastResult> Note_to_check = new List<ImportLastResult>();
            foreach (var item in leadFileUploads)
            {

                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    try
                    {
                        string storedProc = "[dbo].[SP_Lead_Get]";
                        object dynamicParams = new { Lead_Id = item.LeadId };

                        lead = conn.QueryFirstOrDefault<Lead>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (lead != null)
                        {

                            string storedProc1 = "[dbo].[SP_DDLOptions_GetByByHeaderId]";
                            object dynamicParams1 = new { Opt_Id_OptHeader = 3 };

                            options = conn.Query<DropdownOption>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                            if (options.Any(x => x.Opt_Title == item.LastResult))
                            {
                                // if()

                                var result = item.LastResult = options.FirstOrDefault(x => x.Opt_Title == item.LastResult).Opt_Title;

                                if (result == "Enrolled on course" || result == "Registered to CPD/ Short courses")
                                {
                                    item.isError = true;
                                    item.Message = "you cannot update to this status";

                                }
                                else
                                {
                                    item.LastResultId = Convert.ToInt32(options.FirstOrDefault(x => x.Opt_Title == item.LastResult).Opt_Value);
                                    if (role == "Sales Advisor")
                                    {
                                        if (lead.Lead_Id_SalesAdvisor != userId)
                                        {
                                            item.isError = true;
                                            item.Message = "this lead is not assigned to you";


                                        }
                                        else
                                        {
                                            if (lead.Lead_Id != Convert.ToInt32(item.LeadId))
                                            {
                                                item.Message = "will show an error and not to be imported";
                                                item.isError = true;
                                            }
                                            else
                                            {
                                                item.isError = false;
                                            }

                                        }
                                    }
                                }
                            }
                            else
                            {
                                item.isError = true;
                                item.Message = "Invalid Status";

                            }
                        }
                        else
                        {
                            item.isError = true;
                            item.Message = "this lead is not available";

                        }


                    }
                    catch (Exception e)
                    {

                    }
                }






            }
            return leadFileUploads;
        }

        #endregion

        #region TAB-CSL 

        public IEnumerable<GenericOptionType> GetAllOutcomesTypes()
        {
            IEnumerable<GenericOptionType> outcomesTypes = new List<GenericOptionType>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_DDLOptions_GetByByHeaderName]";
                    //
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@Header", "CommunicationSummaryLogs");

                    outcomesTypes = conn.Query<GenericOptionType>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return outcomesTypes;
        }


        public IEnumerable<CommunicationSummaryLogViewModel> GetAllLeadCSL(int lead_id)
        {
            IEnumerable<CommunicationSummaryLogViewModel> activities = new List<CommunicationSummaryLogViewModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_CSL_GetAll]";
                    object dynamicParams = new { CSL_Id_Lead = lead_id };

                    activities = conn.Query<CommunicationSummaryLogViewModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return activities;
        }

        public bool AddUpdateCSL(CommunicationSummaryLogViewModel csl)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //if (csl.csl_Id_Type == 1)
                    //{
                    //    if (!string.IsNullOrEmpty(activity.Activity_Reminder_Date))
                    //    {
                    //        activity.Activity_Date = activity.Activity_Reminder_Date;
                    //    }
                    //}

                    //csl.Activity_DateCreated = DateTime.Now;

                    DateTime? dtCSLDate = string.IsNullOrEmpty(csl.CSL_Date.ToString()) ? null : (DateTime?)Convert.ToDateTime(csl.CSL_Date);

                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@CSL_Id_Lead", csl.CSL_Id_Lead);

                    dynamicParams.Add("@CSL_Id_OptHeader", csl.CSL_Id_OptHeader);
                    dynamicParams.Add("@CSL_Id_Outcome", csl.CSL_Id_Outcome);
                    dynamicParams.Add("@CSL_Date", dtCSLDate);
                    dynamicParams.Add("@CSL_Note", csl.CSL_Note);

                    // dynamicParams.Add("@CSL_IsDeleted", csl.xxxxxxxxxxxx);
                    //  dynamicParams.Add("@CSL_DeletedByUserId", csl.xxxxxxxxxxxx);
                    // dynamicParams.Add("@CSL_DeletedByUserName", csl.xxxxxxxxxxxx);
                    //dynamicParams.Add("@CSL_DateDeleted", csl.xxxxxxxxxxxx);



                    if (csl.CSL_Id > 0)
                    {
                        dynamicParams.Add("@CSL_UpdatedByUserId", csl.CSL_CreatedByUserId);
                        dynamicParams.Add("@CSL_UpdatedByUserName", csl.CSL_CreatedByUserName);
                        dynamicParams.Add("@CSL_DateUpdated", DateTime.Now);

                        string storedProc = "[dbo].[SP_Lead_CSL_Update]";
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        dynamicParams.Add("@CSL_CreatedByUserId", csl.CSL_CreatedByUserId);
                        dynamicParams.Add("@CSL_CreatedByUserName", csl.CSL_CreatedByUserName);
                        dynamicParams.Add("@CSL_DateCreated", DateTime.Now);

                        string storedProc = "[dbo].[SP_Lead_CSL_Add]";
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool DeleteCSLById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_CSL_Delete]";
                    object dynamicParams = new
                    {
                        CSL_Id = id
                        ,
                        CSL_IsDeleted = 1
                        ,
                        CSL_DeletedByUserId = userId
                        ,
                        CSL_DeletedByUserName = userName
                        ,
                        CSL_DateDeleted = DateTime.Now
                    };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }


        #endregion


        #region TAB-Task 

        public IEnumerable<GenericOptionType> GetAllActivityAppointmentMeetingActions()
        {
            IEnumerable<GenericOptionType> outcomesTypes = new List<GenericOptionType>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_DDLOptions_GetByByHeaderName]";
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@Header", "ActivityAppointmentMeetingActions");

                    outcomesTypes = conn.Query<GenericOptionType>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return outcomesTypes;
        }

        public IEnumerable<TaskViewModel> GetAllLeadTask(int lead_id)
        {
            IEnumerable<TaskViewModel> tasks = new List<TaskViewModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_Task_GetAll]";
                    object dynamicParams = new { task_id_lead = lead_id };

                    tasks = conn.Query<TaskViewModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return tasks;
        }

        public bool AddUpdateTask(TaskViewModel task)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //if (task.task_Id_Type == 1)
                    //{
                    //    if (!string.IsNullOrEmpty(activity.Activity_Reminder_Date))
                    //    {
                    //        activity.Activity_Date = activity.Activity_Reminder_Date;
                    //    }
                    //}

                    //task.Activity_DateCreated = DateTime.Now;
                    task.task_id_optheader = 14;//

                    DateTime dtTaskStart = string.IsNullOrEmpty(task.task_start.ToString()) ? DateTime.Now : (DateTime)Convert.ToDateTime(task.task_start);
                    DateTime dtTaskEnd = dtTaskStart.AddMinutes(15);

                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@task_id_lead", task.task_id_lead);
                    dynamicParams.Add("@task_id_user", task.task_id_user);
                    dynamicParams.Add("@task_id_optheader", task.task_id_optheader);
                    dynamicParams.Add("@task_id_activity", task.task_id_activity);
                    dynamicParams.Add("@task_start", dtTaskStart);
                    dynamicParams.Add("@task_end", dtTaskEnd);
                    dynamicParams.Add("@task_note", task.task_note);
                    dynamicParams.Add("@task_scheduled_with", task.task_scheduled_with);
                    dynamicParams.Add("@task_id_optheader", task.task_id_optheader);


                    if (task.task_id > 0)
                    {
                        dynamicParams.Add("@task_id", task.task_id);
                        dynamicParams.Add("@task_updated_by_user_id", task.task_updated_by_user_id);
                        dynamicParams.Add("@task_updated_by_username", task.task_created_by_username);
                        dynamicParams.Add("@task_date_updated", DateTime.Now);

                        string storedProc = "[dbo].[SP_Lead_Task_Update]";
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        dynamicParams.Add("@task_created_by_user_id", task.task_created_by_user_id);
                        dynamicParams.Add("@task_created_by_username", task.task_created_by_username);
                        dynamicParams.Add("@task_date_created", DateTime.Now);

                        string storedProc = "[dbo].[SP_Lead_Task_Add]";
                        conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool DeleteTaskById(int id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_Task_Delete]";
                    object dynamicParams = new
                    {
                        task_id = id,
                        task_is_deleted = 1,
                        task_deleted_by_user_id = userId,
                        task_deleted_by_username = userName,
                        task_date_deleted = DateTime.Now
                    };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool TaskDone(long id, long lead, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_Task_Done]";
                    object dynamicParams = new
                    {
                        task_id = id,
                        task_id_lead = lead,
                        task_id_user = userId,
                        task_done_by_user_id = userId,
                        task_done_by_username = userName
                        //task_date_done = DateTime.Now
                    };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool CheckExistingTask(TaskViewModel task)
        {
            bool bExisting = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //task.Activity_DateCreated = DateTime.Now;
                    task.task_id_optheader = 14;//

                    DateTime dtTaskStart = string.IsNullOrEmpty(task.task_start.ToString()) ? DateTime.Now : (DateTime)Convert.ToDateTime(task.task_start);
                    DateTime dtTaskEnd = dtTaskStart.AddMinutes(15);

                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@task_id_lead", task.task_id_lead);
                    dynamicParams.Add("@task_id_user", task.task_id_user);
                    dynamicParams.Add("@task_id_optheader", task.task_id_optheader);
                    dynamicParams.Add("@task_id_activity", task.task_id_activity);
                    dynamicParams.Add("@task_start", dtTaskStart);
                    dynamicParams.Add("@task_end", dtTaskEnd);
                    dynamicParams.Add("@task_note", task.task_note);
                    dynamicParams.Add("@task_scheduled_with", task.task_scheduled_with);
                    dynamicParams.Add("@task_id_optheader", task.task_id_optheader);

                    string storedProc = "[dbo].[SP_Lead_Task_CheckExisting]";
                    var res = conn.Query<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure).AsList();

                    bExisting = (res.Count > 0) ? true : false;

                    //if (task.task_id > 0)
                    //{
                    //    dynamicParams.Add("@task_id", task.task_id);
                    //    dynamicParams.Add("@task_updated_by_user_id", task.task_updated_by_user_id);
                    //    dynamicParams.Add("@task_updated_by_username", task.task_created_by_username);
                    //    dynamicParams.Add("@task_date_updated", DateTime.Now);

                    //    string storedProc = "[dbo].[SP_Lead_Task_Update]";
                    //    conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    //}
                    //else
                    //{
                    //    dynamicParams.Add("@task_created_by_user_id", task.task_created_by_user_id);
                    //    dynamicParams.Add("@task_created_by_username", task.task_created_by_username);
                    //    dynamicParams.Add("@task_date_created", DateTime.Now);

                    //    string storedProc = "[dbo].[SP_Lead_Task_Add]";
                    //    conn.Query<DropdownOption>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    //}
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return bExisting;
        }


        #endregion


        #region "manage duplicate leads"

        public NewDuplicateLeadModel GetDuplicateLeadsById(int id)
        {
            NewDuplicateLeadModel duplicates = new NewDuplicateLeadModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetAnyDuplicateOfLeadId]";
                    object dynamicParams = new { @lead_id = id };
                    var _lead = conn.QueryFirstOrDefault<NewDuplicateLeadModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    if (_lead != null)
                    {
                        var duplicateLead = GetLeadByLeadId(_lead.lead1_lead_id);
                        var lead = GetLeadByLeadId(_lead.lead2_lead_id);
                        if (lead != null && duplicateLead != null)
                        {
                            duplicates = new NewDuplicateLeadModel
                            {
                                lead1_lead_id = _lead.lead1_lead_id,
                                lead2_lead_id = _lead.lead2_lead_id,
                                ld_id = _lead.ld_id,

                                lead1_contact_name = lead.Lead_ContactName,
                                lead1_mobile = lead.Lead_Contact_Mobile,
                                lead1_course_level = lead.Lead_CourseTitle,
                                lead1_lastresult = lead.Lead_LastResult,
                                lead1_status = lead.Lead_Status,
                                lead1_enquirydate = lead.Lead_DateOfEnquiry,
                                lead1_email = lead.Lead_Contact_Email,
                                lead1_sales_advisor_id = lead.Lead_Id_SalesAdvisor,
                                lead1_company = lead.Lead_Contact_Company_Name,
                                lead1_postcode = lead.Lead_Contact_Company_Postcode,

                                lead2_contact_name = duplicateLead.Lead_ContactName,
                                lead2_mobile = duplicateLead.Lead_Contact_Mobile,
                                lead2_course_level = duplicateLead.Lead_CourseTitle,
                                lead2_lastresult = duplicateLead.Lead_LastResult,
                                lead2_status = duplicateLead.Lead_Status,
                                lead2_enquirydate = duplicateLead.Lead_DateOfEnquiry,
                                lead2_email = duplicateLead.Lead_Contact_Email,
                                lead2_sales_advisor_id = duplicateLead.Lead_Id_SalesAdvisor,
                                lead2_company = duplicateLead.Lead_Contact_Company_Name,
                                lead2_postcode = duplicateLead.Lead_Contact_Company_Postcode,


                                duplicate_is_validated_duplicate = duplicateLead.Lead_is_validated_duplicate,
                                lead_is_validated_duplicate = lead.Lead_is_validated_duplicate
                            };
                        }
                        else if (lead == null && duplicateLead != null)
                        {

                            RemoveFromDuplicates(_lead.ld_id);

                        }
                        else if (lead != null && duplicateLead == null)
                        {

                            RemoveFromDuplicates(_lead.ld_id);

                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return duplicates;
        }

        public IEnumerable<DuplicateLeadViewModel> GetAllDuplicateLeads(int userId, bool isAdmin)
        {
            IEnumerable<DuplicateLeadViewModel> duplicateLeads = new List<DuplicateLeadViewModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetAllDuplicates]";
                    object dynamicParams = new { @UserId = userId, @isAdmin = isAdmin };
                    duplicateLeads = conn.Query<DuplicateLeadViewModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return duplicateLeads;
        }


        public bool RemoveFromDuplicates(int ld_id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_RemovedFromDuplicates]";
                    object dynamicParams = new { @ld_id = ld_id };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool DeleteSingleDuplicateLead(int ld_id, int lead_id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_DeleteDuplicateLead]";
                    object dynamicParams = new { @ld_id = ld_id, @lead_id = lead_id };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public Company GetCompantContactDeatil(int company_id)
        {
            Company company = new Company();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_GetById]";
                    object dynamicParams = new { id = company_id };

                    company = conn.QuerySingleOrDefault<Company>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return company;
        }

        public IEnumerable<CompanyWorkplace> GetCompanyWorkPlace(int company_id)
        {
            IEnumerable<CompanyWorkplace> companyWorkPlace = new List<CompanyWorkplace>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Company_WorkPlace_GetById]";
                    object dynamicParams = new { id = company_id };

                    companyWorkPlace = conn.Query<CompanyWorkplace>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return companyWorkPlace;
        }


        #endregion
        public bool SaveEamil(QueueEmails queueEmails)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {

                if (queueEmails.qe_to != "")
                {
                    string storedProc = "[dbo].[SP_Email_Insert]";
                    var p = new DynamicParameters();

                    p.Add("@qe_from", queueEmails.qe_from);
                    p.Add("@qe_to", queueEmails.qe_to);
                    p.Add("@qe_subject", queueEmails.qe_subject);
                    p.Add("@qe_body", queueEmails.qe_body);
                    p.Add("@qe_is_html", queueEmails.qe_is_html);
                    p.Add("@qe_cc", queueEmails.qe_cc);
                    p.Add("@qe_bcc", queueEmails.qe_bcc);
                    p.Add("@qe_priority", queueEmails.qe_priority);
                    p.Add("@qe_sent", queueEmails.qe_sent);
                    p.Add("@qe_status", queueEmails.qe_status);
                    p.Add("@qe_created_by", queueEmails.qe_created_by);
                    p.Add("@qe_created_date", queueEmails.qe_created_date);
                    p.Add("@qe_mod_date", queueEmails.qe_mod_date);
                    p.Add("@qe_system_id", queueEmails.qe_system_id);
                    int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);



                }

                return true;
            }


        }
    }
}
