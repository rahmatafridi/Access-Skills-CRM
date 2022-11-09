using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class SuccessModel
    {
        public bool update_success { get; set; }
        public bool portal_user_success { get; set; }
        public bool osca_learner_success { get; set; }
        public bool is_qcs_success { get; set; }
        public string error_message { get; set; }
    }
}
