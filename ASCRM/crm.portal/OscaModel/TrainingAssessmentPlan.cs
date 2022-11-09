using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
  public class TrainingAssessmentPlan
    {
        public int TAP_Id { get; set; }
        public int TAP_Id_Learner { get; set; }
        public int TAP_Id_LearnerCourse { get; set; }
        public int TAP_Id_Status { get; set; }
        public string TAP_CreatedDate { get; set; }
        public string TAP_CreatedByUser { get; set; }
        public string TAP_CreatedByUsername { get; set; }

        public string TAP_ReviewNumber { get; set; }
        public string TAP_ReviewDate { get; set; }
        public string TAP_IsApprenticeship { get; set; }
        public string TAP_IsSentToLearner { get; set; }
        public string TAP_LearnerSentDate { get; set; }
        public string TAP_FileName { get; set; }
        public string TAP_IsCompletedByLearner { get; set; }
        public string TAP_LearnerSignatureDate { get; set; }
        public string TAP_AdminComments { get; set; }
        public Byte[] TAP_FinalFile { get; set; }
        public string TAP_FileContentType { get; set; }

    }
}
