using crm.portal.OscaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.Survey
{
   public interface ISurveyRepository
    {
        IList<SurveyLink> GetSurveyLinks(int id);
        SurveyInfo GetSurveyLinkOne(int leanerId, int surveyId);
        IList<SurveyQuestions> GetSurveyQuestion(int surveyId);
        IList<SurveyAnswerOptions> GetSurveyAnswerOptions();
        bool SaveSurvey(List<SurveyCompletedAnswer> surveyCompletedAnswers);
    }
}
