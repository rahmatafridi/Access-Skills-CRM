using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.companies.Models
{
  public  class CompanyWorkplaces
    {
        public int wp_id { get; set; }

        public int? company_id { get; set; }

        public string wp_name { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string post_code { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_date { get; set; }

        public int? updated_by { get; set; }

        public DateTime? updated_date { get; set; }

        public int? deleted_by { get; set; }
        
        public DateTime? deleted_date { get; set; }
        public bool? is_deleted { get; set; }
        public string TotalContact { get; set; }

        public string town { get; set; }
        public string county { get; set; }
        public string employee_email { get; set; }
        public string phone_number { get; set; }
        public int? job_title { get; set; }
        public string job_Title { get; set; }
        public string business_type { get; set; }
        public string company_name { get; set; }
    }
}
