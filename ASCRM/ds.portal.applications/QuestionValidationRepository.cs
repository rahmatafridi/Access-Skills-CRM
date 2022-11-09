using Dapper;
using ds.core.uow;
using ds.portal.applications.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace ds.portal.applications
{
    public class QuestionValidationRepository : IQuestionValidationRepository
    {

        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;

        public QuestionValidationRepository(IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
            _unitOfWork_OSCA = unitOfWork_OSCA;
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
        }
        public IEnumerable<QuestionValidationModel> GetAllQuestionValidation()
        {
            IEnumerable<QuestionValidationModel> section = new List<QuestionValidationModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Question_Validation_GetAll]";

                    section = conn.Query<QuestionValidationModel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return section;
        }

        public QuestionValidationModel AddUpdateQuestionValidation(QuestionValidationModel  question)
        {
            CultureInfo ukCulture = new CultureInfo("en-GB");
           
            var dynamicParams = new DynamicParameters();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (question.v_id > 0)
                    {
                        dynamicParams.Add("@id", question.v_id);

                    }
                    dynamicParams.Add("@q_max", question.q_max);
                    dynamicParams.Add("@q_min", question.q_min);
                    dynamicParams.Add("@q_type_regex", question.q_type_regex);
                    dynamicParams.Add("@q_type_name", question.q_type_name);
                    dynamicParams.Add("@q_validation_massage", question.q_validation_massage);


                    if (question.v_id > 0)
                    {
                        string storedProc = "[dbo].[SP_QuestionValidation_Update]";
                        conn.Query<QuestionValidationModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_QuestionValidation_Create]";
                        var id = conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if (id > 0)
                        {
                            question.v_id = id;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return question;
        }

        public QuestionValidationModel GetQuestionValidationById(int id)
        {
            QuestionValidationModel section = new QuestionValidationModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Question_Validation_ById]";

                    object dynamicParams = new { id = id };

                    section = conn.QueryFirstOrDefault<QuestionValidationModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return section;
        }


        public bool DeleteValidationById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Question_Validation_DeleteById]";
                    object dynamicParams = new { id = id };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

    }
}
