using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
  public  class SurveyCompletedAnswer
    {
        public int Id { get; set; }

        public int? QuestionID { get; set; }

        public int? AnswerOptionScore { get; set; }

        public int SurveyId { get; set; }

        public string CompletedBy { get; set; }

        public DateTime CompletedDate { get; set; }

        public int? SurveyClientId { get; set; }

        public string AnswerType { get; set; }

        public string TextQuestion { get; set; }

        public string TextQuestionAnswer { get; set; }
    }
}
