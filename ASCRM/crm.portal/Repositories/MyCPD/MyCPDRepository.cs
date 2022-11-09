using crm.portal.Interfaces.MyCPD;
using crm.portal.OscaModel;
using Dapper;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace crm.portal.Repositories.MyCPD
{
    public class MyCPDRepository : IMyCPDRepository
    {

        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;

        public MyCPDRepository(IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();

        }
        public PrimeReviewStats CPDGetPrimeReviewStats(int Id)
        {
            PrimeReviewStats stats = new PrimeReviewStats();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_SS_CPDGetPrimeReviewStats]";
                    object dynamicParams = new
                    {
                        cpdprime_learner_id = Id
                    };

                    stats = conn.QueryFirstOrDefault<PrimeReviewStats>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return stats;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<AdditinalActivities> GetAdditinalActivities(int Id)
        {
            List<AdditinalActivities> learnerDoc = new List<AdditinalActivities>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_ADDITINOAL_ACTIVITY]";
                    object dynamicParams = new
                    {
                        learnerId = Id
                    };

                    learnerDoc = (List<AdditinalActivities>)conn.Query<AdditinalActivities>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return learnerDoc;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<CompletedActivities> GetCompletedActivities(int Id)
        {
            IEnumerable<CompletedActivities> learnerDoc = new List<CompletedActivities>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_CPD_Load_Completed_Activities]";
                    object dynamicParams = new
                    {
                        learner_id = Id
                    };

                    learnerDoc = conn.Query<CompletedActivities>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return learnerDoc;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public bool UpdateAdditinalActivity(AdditinalActivities act,int userId)
        {
            var dynamicParams = new DynamicParameters();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    dynamicParams.Add("@learner_Id", userId);
                    dynamicParams.Add("@activity_Title", act.Activity_Title);
                    dynamicParams.Add("@completed_Date", Convert.ToDateTime(act.Completed_Date));
                    dynamicParams.Add("@description", act.Description);
                    dynamicParams.Add("@actual_Otj", act.Actual_OTJ);
                    if (act.PId > 0)
                    {
                        dynamicParams.Add("@id", act.PId);

                    }
                   // dynamicParams.Add("@Actual_OTJ", act.Actual_OTJ);


                    if (act.PId > 0)
                    {
                        string storedProc = "[dbo].[SP_CPD_Additional_Update]";

                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_CPD_Additional_Insert]";

                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool UpdateCompletedActivity(CompletedActivities act)
        {
            var dynamicParams = new DynamicParameters();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    if (act.PId > 0)
                    {
                        dynamicParams.Add("@id", act.PId);

                    }
                    dynamicParams.Add("@Actual_OTJ", act.Actual_OTJ);
                   

                    if (act.PId > 0)
                    {
                        string storedProc = "[dbo].[SP_PORTAL_UPDAE_COMPLETE_ACTIVITY]";

                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    
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
