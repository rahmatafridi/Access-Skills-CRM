using System;

namespace ds.portal.leads.Models
{
    public class Activity
    {
        public int Activity_Id { get; set; }
        public int Activity_Id_Lead { get; set; }
        public int Activity_Id_Type { get; set; }
        public string Activity_Type { get; set; }
        public int Activity_Id_Action { get; set; }
        public string Activity_Reminder_Date { get; set; }
        public string Activity_Note { get; set; }
        public string Activity_EmailBody { get; set; }
        public string Activity_EmailSubject { get; set; }
        public string Activity_EmailTo { get; set; }
        public string Activity_Date { get; set; }
        public string Activity_Location { get; set; }
        public int Activity_CreatedByUserId { get; set; }
        public string Activity_CreatedByUserName { get; set; }
        public DateTime? Activity_DateCreated { get; set; }
        public int Activity_UpdatedByUserId { get; set; }
        public string Activity_UpdatedByUserName { get; set; }
        public bool Activity_IsDeleted { get; set; }
        public int Activity_DeletedByUserId { get; set; }
        public string Activity_DeletedByUserName { get; set; }
    }

    public class DuplicateLead
    {
        public int Count { get; set; }
        
    }

}
