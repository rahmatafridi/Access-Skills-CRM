using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
     public  class Tab
    {
        public int TAP_Id { get; set; }
        public int Type { get; set; }
        public byte[] TAP_FinalFile { get; set; }
        public string TAP_FinalFileStr { get; set; }
        public string TAP_LearnerSignatureText { get; set; }
        public string TAP_LearnerComments { get; set; }
        public bool TAP_IsCompletedByLearner { get; set; }
    }
}
