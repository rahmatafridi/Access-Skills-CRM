using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.CrmModel
{
    public class PortalDocCategory
    {
        public int DocCat_Id { get; set; }
        public string DocCat_Title { get; set; }
        public int DocCat_WeightOrder { get; set; }
        public int DocCat_Access { get; set; }
        public string Type { get; set; }
        public int DocCat_IsActive { get; set; }

    }
}
