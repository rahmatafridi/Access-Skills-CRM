using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
    public class LearnerDoc
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string cat { get; set; }
        public int learnerId { get; set; }

        public int LearnerDocs_Id { get; set; }
        public string LearnerDocs_Caption { get; set; }
        public string LearnerDocs_File { get; set; }
        public string DocCat_Title { get; set; }
        public string CreatedDate { get; set; }
        public string Users_Username { get; set; }
    }
}
