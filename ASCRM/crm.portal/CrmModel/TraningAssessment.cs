using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.CrmModel
{
    public class TraningAssessment
    {
        public int id { get; set; }

        public string tap_learner_name { get; set; }

        public string tap_assessor_name { get; set; }

        public string tap_title { get; set; }

        public string tap_level { get; set; }

        public DateTime tap_date { get; set; }

        public string tap_content_activty { get; set; }

        public string tap_core_skil { get; set; }

        public string tap_referencing { get; set; }

        public string tap_development { get; set; }

        public int? tap_assessment_criteria { get; set; }

        public string tap_resubmission { get; set; }

        public string tap_assessor_signature { get; set; }
        public string tap_assessor_sign { get; set; }
        public DateTime? tap_signdate { get; set; }
        public int tap_learner_id { get; set; }
        public int tap_topic_id { get; set; }
        public byte[] tap_filedata { get; set; }

        public string tap_admin_signature { get; set; }
        public string tap_admin_sign { get; set; }
        public int tap_isresubmission { get; set; }

    }
}
