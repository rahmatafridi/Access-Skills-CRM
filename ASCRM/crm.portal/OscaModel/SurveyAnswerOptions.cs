using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
    public class SurveyAnswerOptions
    {
        public int AnswerId { get; set; }
        public string AnswerOption { get; set; }
        public string AnswerOptionDescription { get; set; }
        public int AnswerScore { get; set; }
    }
}
