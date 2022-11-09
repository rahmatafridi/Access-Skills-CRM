using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public string FileDate { get; set; }
        public string Docs_Version { get; set; }
        public string UploadedBy { get; set; }
    }
}
