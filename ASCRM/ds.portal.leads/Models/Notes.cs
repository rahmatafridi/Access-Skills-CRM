using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.leads.Models
{
    public sealed class Notes
    {
        public int Note_Id { get; set; }
        public int Note_Id_Lead { get; set; }
        public int? Note_Id_Category { get; set; }
        public string Note_Category { get; set; }
        public string Note_Description { get; set; }
        public DateTime? Note_DateCreated { get; set; }
        public int? Note_CreatedByUserId { get; set; }
        public string Note_CreatedByUserName { get; set; }
        public int? Note_UpdatedByUserId { get; set; }
        public string Note_UpdatedByUserName { get; set; }
    }
}
