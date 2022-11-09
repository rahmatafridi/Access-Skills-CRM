using crm.portal.CrmModel;
using crm.portal.Interfaces.Message;
using crm.portal.Interfaces.Matrix;
using crm.portal.OscaModel;
using Dapper;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace crm.portal.Repositories.Message
{
    public class MessageRepository : IMassageRepository
    {
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;
        private IMatrixRepository _matrixRepository;
        public MessageRepository(IMatrixRepository matrixRepository, IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();
            _matrixRepository = matrixRepository;

        }

        public IList<CourseTopic> courseTopics(int learnerId)
        {
            List<CourseTopic> courseConent = new List<CourseTopic>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    var data = _matrixRepository.LoadLearnerCourse(learnerId);
                    string storedProc = "[dbo].[SP_SS_GetListJourneyLearnerProgressAssessors]";
                    object dynamicParams = new
                    {
                        LearnerCourseId = data.LearnerCourses_id
                    };

                    courseConent = (List<CourseTopic>)conn.Query<CourseTopic>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);
                   
                    return courseConent;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IList<CourseTopic> courseTopicsByAdmin(int? learnerId)
        {
            List<CourseTopic> courseConent = new List<CourseTopic>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    var data = _matrixRepository.LoadLearnerCourse((int)learnerId);

                    string storedProc = "[dbo].[SP_SS_GetListJourneyLearnerProgressAssessors]";
                    object dynamicParams = new
                    {
                        LearnerCourseId = data.LearnerCourses_id
                    };

                    courseConent = (List<CourseTopic>)conn.Query<CourseTopic>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);

                    return courseConent;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
        public IList<Messages> GetMessages(int learnerId)
        {
            List<Messages> messages = new List<Messages>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var data = _matrixRepository.LoadLearnerCourse(learnerId);
                    string storedProc = "[dbo].[SP_PORTAL_GET_MESSAGES_BYLEARNER]";
                    object dynamicParams = new
                    {
                        LearnerId = learnerId
                    };

                    messages = (List<Messages>)conn.Query<Messages>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);

                    return messages;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IList<Messages> GetMessagesAdmin()
        {
            List<Messages> messages = new List<Messages>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_MESSAGES_BYADMIN]";
                    object dynamicParams = new
                    {
                       
                    };

                    messages = (List<Messages>)conn.Query<Messages>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);

                    return messages;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public Messages GetMessagesById(int id)
        {
            Messages messages = new Messages();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_MESSAGE_BYId]";
                    object dynamicParams = new
                    {
                        id = id
                    };

                    messages = conn.QueryFirstOrDefault<Messages>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);

                    return messages;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public bool InsertMassage(Messages massages)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    //dynamicParams.Add("@id", );
                    dynamicParams.Add("@topic_id", massages.topic_id);
                    dynamicParams.Add("@topic", massages.topic);
                    dynamicParams.Add("@message", massages.message);
                    dynamicParams.Add("@subject", massages.subject);
                    dynamicParams.Add("@created_date", DateTime.Now);
                    dynamicParams.Add("@user_id", massages.user_id);
                    dynamicParams.Add("@user_name", massages.user_name);
                    dynamicParams.Add("@learner_id", massages.learner_id);

                    if (massages.id > 0)
                    {
                        dynamicParams.Add("@is_question", 0);
                        dynamicParams.Add("@id", massages.id);
                        string storedProc1 = "[dbo].[SP_PORTAL_INSERT_MESSAGEDETAIL]";
                        conn.Query(storedProc1, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        dynamicParams.Add("@is_question", 1);
                        string storedProc = "[dbo].[SP_PORTAL_INSERT_MASSAGE]";
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }



                }
                catch (Exception  ex)
                {
                    throw;
                }
            }
            return true;
        }

        public IList<MessageDetail> messageDetails(int id)
        {
            List<MessageDetail> messages = new List<MessageDetail>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //var data = _matrixRepository.LoadLearnerCourse(learnerId);
                    string storedProc = "[dbo].[SP_PORTAL_GET_MESSAGESDETAIL_BYId]";
                    object dynamicParams = new
                    {
                        id = id
                    };

                    messages = (List<MessageDetail>)conn.Query<MessageDetail>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);

                    return messages;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
    }
}
