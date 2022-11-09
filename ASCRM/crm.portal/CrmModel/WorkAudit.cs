using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.CrmModel
{
    public  class WorkAudit
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int learner_id { get; set; }
        public string user_name { get; set; }
        public string learner_name { get; set; }
        public string topic { get; set; }
        public string note { get; set; }
        public int is_rejected { get; set; }
        public DateTime created_on { get; set; }
        public int created_by { get; set; }
        public DateTime updated_on { get; set; }
        public int updated_by { get; set; }
    }
}
