using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.companies.Models
{
  public  class CompanyAssignedLearnerModel
    {
        public int id { get; set; }
        public int? learner_id { get; set; }
        public string name { get; set; }
        public bool is_active { get; set; }
        public string address1 { get; set; }
        public  string address2 { get; set; }
        public string post_code { get; set; }
        public int is_current { get; set; }
        public string purposes { get; set; }
        public int? company_contact_id { get; set; }
        public int? company_wp_contact_id { get; set; }
        public int? company_id { get; set; }
        public int? wp_id { get; set; }
    }
}
