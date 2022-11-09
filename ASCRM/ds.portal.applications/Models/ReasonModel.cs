using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class ReasonModel
    {
        public int app_id { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string reason { get; set; }
    }
}
