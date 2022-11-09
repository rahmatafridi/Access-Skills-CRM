using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
     public class DocCategory
    {
        public int DocCat_Id { get; set; }

        public string DocCat_Title { get; set; }

        public int DocCat_WeightOrder { get; set; }

        public byte? DocCat_Access { get; set; }

        public byte? DocCat_IsActive { get; set; }
       public List<Document> documents { get; set; }
    }
}
