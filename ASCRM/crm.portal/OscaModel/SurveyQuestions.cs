using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
  public  class SurveyQuestions
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DeletedDate { get; set; }
        public int SurveyId { get; set; }
        public string QuestionType { get; set; }
    }
}
