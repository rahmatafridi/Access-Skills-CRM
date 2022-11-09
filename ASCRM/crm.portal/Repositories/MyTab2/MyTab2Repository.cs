using crm.portal.Interfaces.MyTab2;
using crm.portal.OscaModel;
using Dapper;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace crm.portal.Repositories.MyTab2
{
  public  class MyTab2Repository: IMyTab2Repository
    {
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;

        public MyTab2Repository(IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();

        }

        public int SaveSignatur(Tab tab)
        {
            
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@TAP_Id", tab.TAP_Id);
                    dynamicParams.Add("@Type", 2);
                    dynamicParams.Add("@TAP_FinalFile", tab.TAP_FinalFile);
                    dynamicParams.Add("@TAP_LearnerSignatureText", tab.TAP_LearnerSignatureText);
                    dynamicParams.Add("@TAP_LearnerComments", tab.TAP_LearnerComments);
                    dynamicParams.Add("@TAP_IsCompletedByLearner", true);
      
                    string storedProc = "[dbo].[SP_SS_TAP2UpdateReview]";
                   conn.Query<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return 1;

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public List<TrainingAssessmentPlan> TrainingAssessmentPlanGet(int Id)
        {
            var data = new List<TrainingAssessmentPlan>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_TAB2]";
                    object dynamicParams = new
                    {
                        learenerId = Id
                    };

                    data = (List<TrainingAssessmentPlan>)conn.Query<TrainingAssessmentPlan>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public TrainingAssessmentPlan TrainingAssessmentPlanGetBy_Id(int Id)
        {
            var data = new TrainingAssessmentPlan();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_TAB2_ById]";
                    object dynamicParams = new
                    {
                        id = Id
                    };

                    data = conn.QueryFirstOrDefault<TrainingAssessmentPlan>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
    }
}
