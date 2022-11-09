using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class ApplicationSteps
    {
        public int as_id { get; set; }
        public string as_title { get; set; }
        public int? as_sortorder { get; set; }
        public string as_description { get; set; }
        public string as_icon { get; set; }
        public IEnumerable<ApplicationSection> applicationsections { get; set; }
    }
}
