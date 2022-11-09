using System;

namespace ds.portal.documents.Models
{
    public class DefaultDocument
    {
        public int DefaultDoc_Id { get; set; }        
        public int? DefaultDoc_Id_Category { get; set; }
        public string DefaultDoc_Category { get; set; }
        public string DefaultDoc_Name { get; set; }
        public string DefaultDoc_Description { get; set; }
        public string DefaultDoc_FilePath { get; set; }
        public DateTime? DefaultDoc_DateCreated { get; set; }
        public int? DefaultDoc_CreatedByUserId { get; set; }
        public string DefaultDoc_CreatedByUserName { get; set; }
        public int? DefaultDoc_UpdatedByUserId { get; set; }
        public string DefaultDoc_UpdatedByUserName { get; set; }
        public string document_file_extension { get; set; }
        public string document_mime_type { get; set; }
        public byte[] document_object { get; set; }
        public Int64 document_size { get; set; }
    }
}
