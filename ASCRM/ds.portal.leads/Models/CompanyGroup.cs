using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.leads.Models
{ 
    public class CompanyGroup
    {
        public int id { get; set; }

        public string name { get; set; }

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
