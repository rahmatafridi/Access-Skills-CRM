using System;

namespace ds.core.document.Models
{
    public class documentModel
    {
        public int document_id { get; set; }
        public int document_module { get; set; }

        public string document_file_name { get; set; }
        public string document_file_extension { get; set; }
        public string document_mime_type { get; set; }
        public byte[] document_object { get; set; }
        public Int64 document_size { get; set; }
        public string file_path { get; set; }
    }
}
