using System;

namespace ds.portal.leads.Models
{
    public sealed class Documents
    {
        public int Document_Id { get; set; }
        public int Document_Id_Lead { get; set; }
        public int? Document_Id_Category { get; set; }
        public string Document_Category { get; set; }
        public string Document_Name { get; set; }
        public string Document_Description { get; set; }
        public string Document_FilePath { get; set; }
        public DateTime? Document_DateCreated { get; set; }
        public int? Document_CreatedByUserId { get; set; }
        public string Document_CreatedByUserName { get; set; }
        public int? Document_UpdatedByUserId { get; set; }
        public string Document_UpdatedByUserName { get; set; }
        public string document_file_extension { get; set; }
        public string document_mime_type { get; set; }
        public byte[] document_object { get; set; }
        public Int64 document_size { get; set; }
    }
}
