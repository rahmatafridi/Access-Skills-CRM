using System;
using System.Collections.Generic;
using System.Text;

namespace ds.core.emailtemplates.Models
{
    public class EmailTemplate
    {        
        public int ET_Id { get; set; }
        public string ET_Title { get; set; }
        public int ET_CreatedByUser { get; set; }
        public int ET_IsDeleted { get; set; }
        public string ET_Body { get; set; }
        public int ET_DeletedByUser { get; set; }
        public int ET_IsActive { get; set; }
        public int ET_UpdatedByUser { get; set; }
        public int ET_IsDefaultConcerns { get; set; }
        public int ET_IsDefaultWorkshops { get; set; }
        public string ET_Code { get; set; }
        public string ET_Subject { get; set; }
        public int? ET_Type { get; set; }
    }
}
