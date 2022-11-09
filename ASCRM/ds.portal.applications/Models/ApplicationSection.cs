using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class ApplicationSection
    {
        public int ase_id { get; set; }
        public int ase_id_appstep { get; set; }
        public string ase_title { get; set; }
        public int? ase_sortorder { get; set; }
        public string ase_description { get; set; }
        public IEnumerable<ApplicationQuestions> applicationquestions { get; set; }
    }
}
