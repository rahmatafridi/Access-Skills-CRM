using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class App_OfficeUse1
    {
        public int app_id { get; set; }
        public int? app_officeuse1_courseid { get; set; }
        public string app_officeuse1_coursetitle { get; set; }
        public bool app_officeuse1_isliteracynumeracydone { get; set; }
        public bool app_officeuse1_isalldocumentssigned { get; set; }
        public bool app_officeuse1_isconfirmenrolment { get; set; }
        public string user_name { get; set; }
    }
}
