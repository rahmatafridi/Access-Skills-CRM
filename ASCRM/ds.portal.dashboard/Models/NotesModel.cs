using System;
using ds.portal.ui.Models;

namespace ds.portal.dashboard.Models
{
    public class NotesModel
    {
        public int Note_Id { get; set; }
        public int Note_Id_Lead { get; set; }
        private string _encodeLeadId { get; set; }
        public string encodeNoteIdLead
        {
            get
            {
                return Encryption.Encrypt(this.Note_Id_Lead.ToString());
            }
            set
            {
                _encodeLeadId = value;
            }
        }
        public int? Note_Id_Category { get; set; }
        public string Note_Category { get; set; }
        public int SalesAdvisorId { get; set; }
        public string ContactName { get; set; }
        public string Note_Description { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? Note_DateCreated { get; set; }
        public int? Note_CreatedByUserId { get; set; }
        public string Note_CreatedByUserName { get; set; }
        public int? Note_UpdatedByUserId { get; set; }
        public string Note_UpdatedByUserName { get; set; }
        public bool CanEdit { get; set; }
    }
}
