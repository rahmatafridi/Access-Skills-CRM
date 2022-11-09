using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.CrmModel
{
   public class PortalDoc
    {
        public int Docs_Id { get; set; }

        public int Docs_Id_DocCategories { get; set; }

        public string Docs_Title { get; set; }

        public string Docs_File { get; set; }

        public byte? Docs_IsActive { get; set; }

        public DateTime? Docs_CreatedDate { get; set; }

        public int? Docs_EnteredBy { get; set; }

        public string Docs_EnteredByUser { get; set; }

        public DateTime? Docs_FileDate { get; set; }

        public string Docs_Version { get; set; }
    }
}
