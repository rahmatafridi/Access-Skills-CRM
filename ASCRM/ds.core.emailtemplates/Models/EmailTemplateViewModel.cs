using System;
using System.Collections.Generic;
using System.Text;

namespace ds.core.emailtemplates.Models
{
    public class EmailTemplateViewModel : EmailTemplate
    {
        public DateTime? ET_CreatedDate { get; set; }
        public DateTime? ET_UpdatedDate { get; set; }
        public DateTime? ET_DeletedDate { get; set; }
        public string ET_TemplateType { get; set; }
    }
}
