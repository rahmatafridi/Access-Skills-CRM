using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.companies.Models
{
  public  class CompanyNote
    {
        public int Note_Id { get; set; }
        public int Note_Id_Company { get; set; }
        public int? Note_Id_Category { get; set; }
        public string Note_Category { get; set; }
        public string Note_Description { get; set; }
        public DateTime? Note_DateCreated { get; set; }
        public int? Note_CreatedByUserId { get; set; }
        public string Note_CreatedByUserName { get; set; }
        public int? Note_UpdatedByUserId { get; set; }
        public string Note_UpdatedByUserName { get; set; }
        public bool IsOsca { get; set; }
    }
}
