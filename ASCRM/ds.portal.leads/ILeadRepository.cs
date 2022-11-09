using ds.portal.leads.Models;
using portal.models.Comman;
using System.Collections.Generic;
namespace ds.portal.leads
{
    public interface ILeadRepository
    {
        void InsertLeadImportLog(string ip, int total);

        #region "Lead "

        Lead InsertUpdateLeadRecord(Lead lead);
        IEnumerable<LeadViewModel> GetAllLeads(int userId, bool isAdmin);
        Lead GetLeadById(int id);      
        bool DeleteLeadById(int id, int userId, string userName);
        
        // added by Suhail
        bool Insert(Lead lead);
         Documents DownloadFile(int documentId);

        bool UpdateSalesAdvisor(int lead_id, int sales_advisor_id);
        LeadViewModel GetLeadByLeadId(int leadId);

        #endregion

        #region "not used>"
        Contact GetContactById(int id);
        #endregion

        #region "dashboard "

        IEnumerable<Contact> GetAllContactsForDropdown();
        IEnumerable<LeadViewModel> GetAllLeadsByAgreedPaymentScheme(int userId, bool isAdmin, int agreed_pay_val);
        IEnumerable<LeadViewModel> GetAllLeadsByLastResults(int userId, bool isAdmin, int last_result_val);


        #endregion

        #region " QueueEmailModel"
        bool InsertQueueEmail(QueueEmailModel queueEmailModel);
        bool UpdateSentEmail(QueueEmailModel queueEmailModel);


        #endregion

        #region "Course levels rota"
        //---------------------------------------------------------------
        IEnumerable<Course> GetAllCourses();
        IEnumerable<CourseLevel> GetAllCourseLevels();
        IEnumerable<CourseLevel> GetAllCourseLevelEnquiries();
        int GetAvailableSalesAdvisorIdRota();
        //---------------------------------------------------------------


        #endregion

        #region "Tab Documents"

        IEnumerable<Documents> GetAllLeadDocuments(int lead_Id);
        bool InsertUpdateLeadDocumentRecord(Documents document);
        Documents GetDocumentById(int id);
        bool DeleteDocumentById(int id, int userId, string userName);
        //---------------------------------------------------------------

        #endregion

        #region "Tab Notes"
        bool InsertUpdateLeadNoteRecord(Notes note);
        IEnumerable<Notes> GetAllLeadNotes(int lead_Id);
        Notes GetNoteById(int id);
        Company GetCompantContactDeatil(int company_id);
        IEnumerable<CompanyWorkplace> GetCompanyWorkPlace(int company_id);

        bool DeleteNoteById(int id, int userId, string userName);
        //---------------------------------------------------------------


        #endregion

        #region "TAB-CSL"
        IEnumerable<CommunicationSummaryLogViewModel> GetAllLeadCSL(int lead_Id);

        bool AddUpdateCSL(CommunicationSummaryLogViewModel csl);

        bool DeleteCSLById(int id, int userId, string userName);

        IEnumerable<GenericOptionType> GetAllOutcomesTypes();

        #endregion

        #region "TAB activity"
        IEnumerable<ActivityType> GetAllActivityTypes();
        //---------------------------------------------------------------
        IEnumerable<ActivityViewModel> GetAllLeadActivities(int lead_Id);

        bool InsertUpdateLeadActivityRecord(Activity activity);
        Activity GetActivityById(int id);
        bool DeleteActivityById(int id, int userId, string userName);
        #endregion
        
        #region " CheckDuplicateLead "

        Lead CheckDuplicateLead(string contactName, string email, string mobile, int userId, bool isAdmin);
        DuplicateLeadModel GetDuplicateLeadLeadById(int id);
        bool DeleteDuplicateLeadById(int ld_id, int lead_id, int lead_duplicate_id);



        DuplicateLeadModel GetDuplicateLead(int id);
        bool RemoveLeadDuplicate(int lead_id, int ld_id);
         
        #endregion

        #region " HistoryModel "
        IEnumerable<HistoryModel> GetAllHistory(int user_id, int lead_id);

        #endregion

        #region "BulkInsert "
        List<LeadFileUpload> BulkInsert(List<LeadFileUpload> leadFileUploads, int userId);
        List<ImportNote> BulkImportNote(List<ImportNote> importNotes, int userId,string role, string UserName);
        List<ImportStatus> BulkImportStatus(List<ImportStatus> importStatus, int userId, string role);
        List<ImportLastResult> BulkImportLastResult(List<ImportLastResult> importLastResult, int userId, string role);

        List<LeadFileUpload> ValidateLeadsForImport(List<LeadFileUpload> leadFileUploads);

        List<ImportNote> ValidateLeadNoteImport(List<ImportNote> leadFileUploads, int userId,string role);

        List<ImportStatus> ValidateLeadStatusImport(List<ImportStatus> leadFileUploads, int userId, string role);
        List<ImportLastResult> ValidateLeadLastResultImport(List<ImportLastResult> leadFileUploads, int userId, string role);


        #endregion

        #region ""

        #endregion

        #region "TAB-Task"
        IEnumerable<TaskViewModel> GetAllLeadTask(int lead_Id);

        bool AddUpdateTask(TaskViewModel csl);

        bool DeleteTaskById(int id, int userId, string userName);
        bool TaskDone(long id, long lead_id, int userId, string userName);

        bool CheckExistingTask(TaskViewModel csl);

        IEnumerable<GenericOptionType> GetAllActivityAppointmentMeetingActions();

        #endregion

        #region "Manage Duplicates"
        
        NewDuplicateLeadModel GetDuplicateLeadsById(int id);
        IEnumerable<DuplicateLeadViewModel> GetAllDuplicateLeads(int userId, bool isAdmin);

        bool RemoveFromDuplicates(int ld_id);

        bool DeleteSingleDuplicateLead(int ld_id, int lead_id);

        #endregion


        bool SaveEamil(QueueEmails queueEmails);

    }
}
