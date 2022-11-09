using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
    public class SurveyLink
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public int SurveyId { get; set; }
        public string SentTo { get; set; }
        public string LearnerEmail { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool isExpired { get; set; }
        public bool isDeleted { get; set; }
        public string  CreatedBy { get; set; }
        public DateTime CompletedDate { get; set; }
        public bool isCompleted { get; set; }
        public string CompletedBy { get; set; }
        public bool isActive { get; set; }
        public string EmployerLearner { get; set; }

    }

    public class SurveyInfo
    {
        public string massage { get; set; }
        public List<SurveyQuestions> surveyQuestions { get; set; }
        public List<SurveyAnswerOptions> options { get; set; }
    }
}
