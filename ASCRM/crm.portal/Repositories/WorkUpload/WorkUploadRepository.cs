using crm.portal.CrmModel;
using crm.portal.Interfaces.WorkUpload;
using Dapper;
using ds.core.comms.Mail;
using ds.core.emailtemplates;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace crm.portal.Repositories.WorkUpload
{
    public class WorkUploadRepository : IWorkUploadRepository
    {


        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMailService _emailSender;

        public WorkUploadRepository(IMailService emailSender, IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal, IEmailTemplateRepository emailTemplateRepository)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
            _unitOfWork_OSCA = unitOfWork_OSCA;
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _emailTemplateRepository = emailTemplateRepository;
            _emailSender = emailSender;
        }

        public IEnumerable<UploadWork> GetLearnerWorkForAdmin(string type)
        {
            IEnumerable<UploadWork> work = new List<UploadWork>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (type == "Submitted")
                    {
                        string storedProc = "[dbo].[SP_PORTAL_GET_WORK_FOR_ADMIN_Sumbitted]";
                        object dynamicParams = new {  };

                        work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    if (type == "Approved")
                    {
                        string storedProc = "[dbo].[SP_PORTAL_GET_WORK_FOR_ADMIN_Approved]";
                        object dynamicParams = new {  };

                        work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    if (type == "Validated")
                    {
                        string storedProc = "[dbo].[SP_PORTAL_GET_WORK_FOR_ADMIN_Validated]";
                        object dynamicParams = new {  };

                        work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }  
                    if (type == "ReadyToChecked")
                    {
                        string storedProc = "[dbo].[SP_PORTAL_GET_WORK_FOR_ADMIN_ReadyToChecked]";
                        object dynamicParams = new {  };

                        work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return work;
        }

        public IEnumerable<Portal_User> LoadAssessor()
        {
            IEnumerable<Portal_User> work = new List<Portal_User>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_ASSESSER]";
                    object dynamicParams = new { };

                    work = conn.Query<Portal_User>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return work;
        }

        public int AssignAssesser(UploadWork uploadWork)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                CultureInfo ukCulture = new CultureInfo("en-GB");//CultureInfo.CreateSpecificCulture("en-GB")

                DateTime? dob_date = string.IsNullOrEmpty(uploadWork.due_date) ? null : (DateTime?)DateTime.Parse(uploadWork.due_date, ukCulture.DateTimeFormat);
                var date = Convert.ToDateTime(uploadWork.due_date);
                DateTime dt = new DateTime(2015, 12, 31);
                TimeSpan ts = new TimeSpan(25, 20, 55);

                var newDate = date.Add(ts);

                //  var date = DateTime.Parse(uploadWork.due_date);

                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@id", uploadWork.id);
                    dynamicParams.Add("@assessor_assigned_user_id", uploadWork.assessor_assigned_user_id);
                    dynamicParams.Add("@is_ready_for_marking", 1);

                    dynamicParams.Add("@due_date", newDate);



                    string storedProc = "[dbo].[SP_PORTAL_UPDATE_WORK_UPLOAD_ASSIGN]";
                    int data = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    var dynamicParams1 = new DynamicParameters();
                    dynamicParams1.Add("@work_id", uploadWork.id);
                    dynamicParams1.Add("@user_id", uploadWork.user_id);
                    dynamicParams1.Add("@user_name", uploadWork.user_name);
                    dynamicParams1.Add("@note", uploadWork.note);
                    dynamicParams1.Add("@is_rejected", 0);

                    string storedProc1 = "[dbo].[SP_PORTAL_CREATE_AUDIT]";
                    int data1 = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);


                    return data;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public int RejectWork(UploadWork uploadWork)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@id", uploadWork.id);
                    if(uploadWork.assessor_rejected == 1)
                    {
                        var load = loadAssessorRejectCount(uploadWork.learner_id, uploadWork.topic_id, uploadWork.assessor_assigned_user_id);

                        dynamicParams.Add("@assessor_rejected", 1);
                        dynamicParams.Add("@is_rejected", 0);
                        dynamicParams.Add("@assessor_reject_count", load.Count +1);

                        //dynamicParams.Add("@is_work_checked", 0);
                        //dynamicParams.Add("@is_ready_for_marking", 0);

                        //dynamicParams.Add("@is_marking_validated", 0);
                        //dynamicParams.Add("@is_assessor_marking_valid", 0);
                        //dynamicParams.Add("@assessor_assigned_user_id", 0);


                    }
                    else
                    {
                        dynamicParams.Add("@assessor_rejected", 0);
                        dynamicParams.Add("@is_rejected", 1);
                        dynamicParams.Add("@is_submited", 0);

                        //dynamicParams.Add("@is_work_checked", 0);
                        //dynamicParams.Add("@is_ready_for_marking", 0);

                        //dynamicParams.Add("@is_marking_validated", 0);
                        //dynamicParams.Add("@is_assessor_marking_valid", 0);
                        //dynamicParams.Add("@assessor_assigned_user_id", 0);
                    }
                    dynamicParams.Add("@rejected_reason", uploadWork.rejected_reason);
                    dynamicParams.Add("@rejectd_user", uploadWork.user_name);



                    string storedProc = "[dbo].[SP_PORTAL_UPDATE_WORK_UPLOAD_REJECT]";
                    int data = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    var dynamicParams1 = new DynamicParameters();
                    dynamicParams1.Add("@work_id", uploadWork.id);
                    dynamicParams1.Add("@user_id", uploadWork.user_id);
                    dynamicParams1.Add("@user_name", uploadWork.user_name);
                    dynamicParams1.Add("@note", uploadWork.rejected_reason);
                    dynamicParams1.Add("@learner_id", uploadWork.learner_id);
                    dynamicParams1.Add("@topic", uploadWork.Topic);
                    dynamicParams1.Add("@learner_name", uploadWork.LearnerName);

                    string storedProc1 = "[dbo].[SP_PORTAL_CREATE_AUDIT]";
                    int data1 = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);


                    return data;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IEnumerable<UploadWork> GetLearnerWorkForCheck(int leanerId)
        {
            IEnumerable<UploadWork> work = new List<UploadWork>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_WORK_FOR_CHECK]";
                    object dynamicParams = new { learnerId = leanerId };

                    work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return work;
        }

        public IEnumerable<UploadedWorkFiles> WorkFiles(int id)
        {
            IEnumerable<UploadedWorkFiles> work = new List<UploadedWorkFiles>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_WORK_FILES]";
                    object dynamicParams = new { id = id };

                    work = conn.Query<UploadedWorkFiles>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return work;
        }

        public IEnumerable<UploadWork> Notifications(int id)
        {
            IEnumerable<UploadWork> work = new List<UploadWork>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_WORK_NOTIFICATION]";
                    object dynamicParams = new { learnerId = id };

                    work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return work;
        }

        public IEnumerable<UploadWork> GetLearnerWorkForAssessor(int id)
        {
            IEnumerable<UploadWork> work = new List<UploadWork>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_WORK_FOR_ASSESSOR]";
                    object dynamicParams = new { id = id };

                    work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return work;
        }
        public List<UploadWork> loadAssessorApproveCount(int learnerid, int? topicid,int assessorid)
        {
            List<UploadWork> learners = new List<UploadWork>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@learnerId", learnerid);
                    dynamicParams.Add("@topicid", topicid);
                    dynamicParams.Add("@assessorid", assessorid);

                    string storedProc = "[dbo].[SP_Portal_AssessorApprove_count]";

                    learners = (List<UploadWork>)conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            return learners;

        }
        public List<UploadWork> loadAssessorRejectCount(int learnerid, int? topicid, int assessorid)
        {
            List<UploadWork> learners = new List<UploadWork>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@learnerId", learnerid);
                    dynamicParams.Add("@topicid", topicid);
                    dynamicParams.Add("@assessorid", assessorid);

                    string storedProc = "[dbo].[SP_Portal_AssessorReject_count]";

                    learners = (List<UploadWork>)conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            return learners;

        }

        public int ApproveWork(UploadWork uploadWork)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@id", uploadWork.id);
                    if (uploadWork.is_assessor_marking_valid == 1)
                    {
                        DateTime? dt = null;
                        var load = loadAssessorApproveCount(uploadWork.learner_id, uploadWork.topic_id, uploadWork.user_id);
                        dynamicParams.Add("@is_admin_approved", 0);
                        dynamicParams.Add("@is_ready_for_marking", 0);

                        if (uploadWork.traningAssessment.tap_resubmission != "")
                        {
                            var load1 = loadAssessorRejectCount(uploadWork.learner_id, uploadWork.topic_id, uploadWork.assessor_assigned_user_id);

                            dynamicParams.Add("@assessor_rejected", 1);
                            dynamicParams.Add("@assessor_reject_count", load1.Count + 1);
                            dynamicParams.Add("@is_assessor_marking_valid", 0);

                        }
                        else
                        {
                            dynamicParams.Add("@is_assessor_marking_valid", 1);
                            dynamicParams.Add("@assessor_rejected", 0);

                            dynamicParams.Add("@completed_date", dt);
                            dynamicParams.Add("@assessor_approve_count", load.Count + 1);
                        }

                        var dynamicParam2 = new DynamicParameters();
                        dynamicParam2.Add("@tap_learner_name", uploadWork.traningAssessment.tap_learner_name);
                        dynamicParam2.Add("@tap_assessor_name", uploadWork.traningAssessment.tap_assessor_name);
                        dynamicParam2.Add("@tap_title", uploadWork.traningAssessment.tap_title);
                        dynamicParam2.Add("@tap_level", uploadWork.traningAssessment.tap_level);
                        dynamicParam2.Add("@tap_date", DateTime.Now);
                        dynamicParam2.Add("@tap_content_activty", uploadWork.traningAssessment.tap_content_activty);
                        dynamicParam2.Add("@tap_core_skil", uploadWork.traningAssessment.tap_core_skil);
                        dynamicParam2.Add("@tap_referencing", uploadWork.traningAssessment.tap_referencing);
                        dynamicParam2.Add("@tap_development", uploadWork.traningAssessment.tap_development);
                        dynamicParam2.Add("@tap_assessment_criteria", uploadWork.traningAssessment.tap_assessment_criteria);
                        dynamicParam2.Add("@tap_resubmission", uploadWork.traningAssessment.tap_resubmission);
                        dynamicParam2.Add("@tap_assessor_signature", uploadWork.traningAssessment.tap_assessor_signature);
                        dynamicParam2.Add("@tap_signdate", DateTime.Now);
                        dynamicParam2.Add("@tap_learner_id", uploadWork.learner_id);
                        dynamicParam2.Add("@tap_topic_id", uploadWork.topic_id);
                        dynamicParam2.Add("@tap_fileData", uploadWork.traningAssessment.tap_filedata);

                        string storedProc2 = "[dbo].[Sp_Portal_Insert_Traning_Assessment]";
                        int data2 = conn.QuerySingleOrDefault<int>(storedProc2, param: dynamicParam2, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        dynamicParams.Add("@is_admin_approved", 1);
                        dynamicParams.Add("@completed_date", DateTime.Now);
                        dynamicParams.Add("@is_ready_for_marking", 0);
                        dynamicParams.Add("@is_assessor_marking_valid", 0);
                    }

                    //dynamicParams.Add("@rejected_reason", uploadWork.rejected_reason);
                    string storedProc = "[dbo].[SP_PORTAL_UPDATE_WORK_UPLOAD_APPROVE]";
                    int data = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                   
                    var dynamicParams1 = new DynamicParameters();
                    dynamicParams1.Add("@work_id", uploadWork.id);
                    dynamicParams1.Add("@user_id", uploadWork.user_id);
                    dynamicParams1.Add("@user_name", uploadWork.user_name);
                    dynamicParams1.Add("@note", uploadWork.note);
                    dynamicParams1.Add("@learner_id", uploadWork.learner_id);
                    dynamicParams1.Add("@topic", uploadWork.Topic);
                    dynamicParams1.Add("@learner_name", uploadWork.LearnerName);
                    dynamicParams1.Add("@is_rejected", 0);

                    string storedProc1 = "[dbo].[SP_PORTAL_CREATE_AUDIT]";
                    int data1 = conn.QuerySingleOrDefault<int>(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public IEnumerable<UploadWork> GetLearnerWorkForSearch(string type, string name)
        {
            IEnumerable<UploadWork> work = new List<UploadWork>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_WORK_Search]";
                    object dynamicParams = new { Type = type,name =name };

                    work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return work;
        }

        public IEnumerable<UploadWork> GetLearnerWorkForSearchSubmitted(string name)
        {
            IEnumerable<UploadWork> work = new List<UploadWork>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_WORK_Search]";
                    object dynamicParams = new { name = name };

                    work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return work;
        }

        public UploadWork GetUploadWorkById(int id)
        {
            UploadWork work = new UploadWork();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_WORK_ByID]";
                    object dynamicParams = new { id = id };

                    work = conn.QueryFirstOrDefault<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return work;
        }

        public bool DeleteWorkFile(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_DELETE_UploadFile]";
                    object dynamicParams = new { Id = id };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return true;
        }

        public IEnumerable<UploadWork> CheckAssessorTask()
        {
            IEnumerable<UploadWork> work = new List<UploadWork>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_ASSESSOR_WORKCHECK]";
                    object dynamicParams = new {  };

                    work = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return work;
        }

        public IEnumerable<WorkAudit> GetAssessorAuditWork(int id)
        {
            IEnumerable<WorkAudit> work = new List<WorkAudit>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_WORK_Audit]";
                    object dynamicParams = new { id = id };

                    work = conn.Query<WorkAudit>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return work;
        }

        public IEnumerable<UploadWork> AssessorSummary(int id)
        {
            IEnumerable<UploadWork> submitted = new List<UploadWork>();
            //IEnumerable<UploadWork> rejected = new List<UploadWork>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[Sp_Portal_Get_AssessorSummary]";
                    object dynamicParams = new { id = id };

                    submitted = conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            return submitted;
        }

        public List<TraningAssessment> LoadTraningHistoryData(int learner_id, int topic_id)
        {
            List<TraningAssessment> work = new List<TraningAssessment>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[Sp_Portal_GetHistoryPortalAssessment]";
                    object dynamicParams = new { learner_id = learner_id,topic_id = topic_id };

                    work = (List<TraningAssessment>)conn.Query<TraningAssessment>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return work;
        }
        public TraningAssessment LoadTraningData(int learner_id, int topic_id)
        {
            TraningAssessment work = new TraningAssessment();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[Sp_Portal_GetPortalAssessment]";
                    object dynamicParams = new { learner_id = learner_id,topic_id = topic_id };

                    work = conn.QueryFirstOrDefault<TraningAssessment>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return work;
        }

        public int UpdateTraningData(TraningAssessment traningAssessment)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
               
    

                     var dynamicParam2 = new DynamicParameters();
                    dynamicParam2.Add("@id", traningAssessment.id);

                    dynamicParam2.Add("@tap_learner_name", traningAssessment.tap_learner_name);
                        dynamicParam2.Add("@tap_assessor_name", traningAssessment.tap_assessor_name);
                        dynamicParam2.Add("@tap_title", traningAssessment.tap_title);
                        dynamicParam2.Add("@tap_level", traningAssessment.tap_level);
                        dynamicParam2.Add("@tap_date", DateTime.Now);
                        dynamicParam2.Add("@tap_content_activty", traningAssessment.tap_content_activty);
                        dynamicParam2.Add("@tap_core_skil", traningAssessment.tap_core_skil);
                        dynamicParam2.Add("@tap_referencing", traningAssessment.tap_referencing);
                        dynamicParam2.Add("@tap_development", traningAssessment.tap_development);
                        dynamicParam2.Add("@tap_assessment_criteria", traningAssessment.tap_assessment_criteria);
                        dynamicParam2.Add("@tap_resubmission", traningAssessment.tap_resubmission);
                        dynamicParam2.Add("@tap_assessor_signature", traningAssessment.tap_assessor_signature);
                        dynamicParam2.Add("@tap_signdate", DateTime.Now);
                        dynamicParam2.Add("@tap_learner_id", traningAssessment.tap_learner_id);
                        dynamicParam2.Add("@tap_topic_id", traningAssessment.tap_topic_id);
                    dynamicParam2.Add("@tap_fileData", traningAssessment.tap_filedata);

                    string storedProc2 = "[dbo].[Sp_Portal_Update_Traning_Assessment]";
                        int data2 = conn.QuerySingleOrDefault<int>(storedProc2, param: dynamicParam2, commandType: CommandType.StoredProcedure);



                   
                }
                catch (Exception ex)
                {
                    throw;
                }
                return 1;
            }
        }
    }
}