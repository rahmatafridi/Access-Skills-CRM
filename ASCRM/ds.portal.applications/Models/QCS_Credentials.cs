using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class QCS_Credentials
    {
        public const string Position = "QCS_Credentials";
        public string Url { get; set; }
        public string LocationId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
