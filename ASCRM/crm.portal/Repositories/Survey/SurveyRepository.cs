using crm.portal.Interfaces.Survey;
using crm.portal.OscaModel;
using Dapper;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace crm.portal.Repositories.Survey
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;

        public SurveyRepository(IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();

        }

        public IList<SurveyAnswerOptions> GetSurveyAnswerOptions()
        {
            var data = new List<SurveyAnswerOptions>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_GET_PORTAL_SurveyAnswerOptions]";
                    object dynamicParams = new
                    {
                       
                    };

                    data = (List<SurveyAnswerOptions>)conn.Query<SurveyAnswerOptions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public SurveyInfo GetSurveyLinkOne(int leanerId, int surveyId)
        {
            var data = new SurveyLink();
            var result = new SurveyInfo();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Survey_GetSurveyClientLnkForLearnerOnlyOne]";
                    object dynamicParams = new
                    {
                        learnerId = leanerId,
                        surveyId= surveyId
                    };

                    data = conn.QueryFirstOrDefault<SurveyLink>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    if (DateTime.Now <= data.ExpiryDate || !data.isExpired)
                    {
                        result.surveyQuestions = (List<SurveyQuestions>)GetSurveyQuestion(surveyId);
                        result.options = (List<SurveyAnswerOptions>)GetSurveyAnswerOptions();
                    }
                    else
                    {
                        UpdateClinetLink(data.Id);
                        result.massage = "Your survey is expired";
                    }
                    }
                catch (Exception ex)
                {

                    throw;
                }
                return result;

            }
        }

        public IList<SurveyLink> GetSurveyLinks(int id)
        {
            var data = new List<SurveyLink>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Survey_GetSurveyClientLnkForLearner]";
                    object dynamicParams = new
                    {
                        learnerId = id
                    };

                    data = (List<SurveyLink>)conn.Query<SurveyLink>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IList<SurveyQuestions> GetSurveyQuestion(int surveyId)
        {
            var data = new List<SurveyQuestions>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_Survey_Questions]";
                    object dynamicParams = new
                    {
                        surveyId = surveyId
                    };

                    data = (List<SurveyQuestions>)conn.Query<SurveyQuestions>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public bool SaveSurvey(List<SurveyCompletedAnswer> surveyCompletedAnswers)
        {

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    var SurveryId = surveyCompletedAnswers.Where(x => x.SurveyId != 0).FirstOrDefault().SurveyId;

                    foreach (var item in surveyCompletedAnswers)
                    {
                        if (item.QuestionID != null)
                        {
                            var dynamicParams = new DynamicParameters();
                            dynamicParams.Add("@QuestionID", item.QuestionID);
                            dynamicParams.Add("@AnswerOptionScore", item.AnswerOptionScore);
                            dynamicParams.Add("@SurveyId", SurveryId);
                            dynamicParams.Add("@CompletedBy", 1);
                            dynamicParams.Add("@SurveyClientId", 1);
                            dynamicParams.Add("@AnswerType", item.AnswerType);
                            dynamicParams.Add("@TextQuestion", item.TextQuestion);
                            dynamicParams.Add("@TextQuestionAnswer", item.TextQuestionAnswer);

                            dynamicParams.Add("@CompletedDate", DateTime.Now);

                            string storedProc = "[dbo].[SP_PORTAL_SAVE_SURVEY]";
                            conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public void UpdateClinetLink(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_UPdate_SurveryClientLnk]";
                    object dynamicParams = new
                    {
                        surveyClientId = id,
                    };

                    var ads = conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
    }
}
