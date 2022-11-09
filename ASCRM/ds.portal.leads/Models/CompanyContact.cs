using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.leads.Models
{ 
    public class CompanyContact
    {
        public int id { get; set; }

        public int company_id { get; set; }

        public string contact_name { get; set; }

        public string title { get; set; }

        public string job_title { get; set; }

        public string email1 { get; set; }

        public string email2 { get; set; }

        public string mobile1 { get; set; }

        public string address1 { get; set; }

        public string address2 { get; set; }

        public string address3 { get; set; }

        public string town { get; set; }

        public string county { get; set; }

        public string postcode { get; set; }

        public int? country_id { get; set; }

        public string website { get; set; }

        public bool is_deleted { get; set; }

        public int? created_by { get; set; }

        public DateTime created_date { get; set; }

        public DateTime? modified_date { get; set; }

        public int? modified_by { get; set; }

        public DateTime? deleted_date { get; set; }

        public int? deleted_by { get; set; }

    } 
}
