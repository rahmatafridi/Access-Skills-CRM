using crm.portal.CrmModel;
using crm.portal.Interfaces.CourseSummary;
using crm.portal.Interfaces.Matrix;
using crm.portal.Interfaces.WorkUpload;
using crm.portal.OscaModel;
using Dapper;
using ds.core.configuration;
using ds.core.document.Models;
using ds.core.uow;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace crm.portal.Repositories.CourseSummary
{
    public class CourseSummaryRepository : ICourseSummaryRepository
    {
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;
        private IMatrixRepository _matrixRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private IWorkUploadRepository _workUploadRepository;
        private IConfigurationRepository _configurationRepository;
        public CourseSummaryRepository(IConfigurationRepository configurationRepository, IWorkUploadRepository workUploadRepository, IHostingEnvironment hostingEnvironment, IMatrixRepository matrixRepository, IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();
            _matrixRepository = matrixRepository;
            _hostingEnvironment = hostingEnvironment;
            _workUploadRepository = workUploadRepository;
            _configurationRepository = configurationRepository;
        }
        public CourseTopic getCourseTopicsById(int iLearnerId)
        {
            CourseTopic courseConent = new CourseTopic();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_SS_GetLastTopicIdForActivity]";
                    object dynamicParams = new
                    {
                        LearnerCourseId = iLearnerId
                    };
                    courseConent = conn.QueryFirstOrDefault<CourseTopic>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);
                    return courseConent;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<CourseTopic> getCourseTopics(int iLearnerId)
        {
            List<CourseTopic> courseConent = new List<CourseTopic>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    var data1 = _matrixRepository.LoadLearnerCourse(iLearnerId);
                    List<CourseTopicDocuments> lstCourseTopicsDocsForLearner = new List<CourseTopicDocuments>();
                    lstCourseTopicsDocsForLearner = getCourseTopicsDocsForLearner(iLearnerId);
                    var workupload = _workUploadRepository.GetLearnerWorkForCheck(iLearnerId);
                    string storedProc = "[dbo].[SP_Portal_GetListJourneyLearnerProgressSummary]";
                    object dynamicParams = new
                    {
                        LearnerCourseId = data1.LearnerCourses_id
                    };
                    bool isSubmit = false;
                    var data = conn.Query<CourseTopic>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);
                    foreach (var item in data)
                    {
                        var reslut = workupload.Where(x => x.topic_id == item.SSJLP_Id);
                        if (reslut.Any())
                        {
                            isSubmit = true;
                        }
                        else
                        {
                            isSubmit = false;
                        }
                        courseConent.Add(new CourseTopic()
                        {
                            TopicId = item.TopicId,
                            SSJLP_Id = item.SSJLP_Id,
                            SSJLP_Id_Learner = item.SSJLP_Id_Learner,
                            SSJLP_Id_LearnerCourse = item.SSJLP_Id_LearnerCourse,
                            TopicName = item.SSJLP_Topic,
                            IsCompleted = item.IsCompleted,
                            CompletedDate = item.CompletedDate == null ? "" : item.CompletedDate,
                            HasDocuments = "1",
                            Checkpointdate = item.Checkpointdate == null ? "" : item.Checkpointdate,
                            AlertColour = item.AlertColour,
                            Documents = lstCourseTopicsDocsForLearner.Where(x => x.DocTopic == item.SSJLP_Topic).ToList(),
                            isWorkSubmit = isSubmit

                        });
                    }
                    return courseConent;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<CourseTopicDocuments> getCourseTopicsDocsForLearner(int iLearnerId)
        {
            List<CourseTopicDocuments> courseDocuments = new List<CourseTopicDocuments>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_COURSE_TOPIC_DOC_FOR_LEARNER]";
                    object dynamicParams = new
                    {
                        LearnerId = iLearnerId
                    };

                    courseDocuments = (List<CourseTopicDocuments>)conn.Query<CourseTopicDocuments>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return courseDocuments;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<documentModel> UploadCourseSummaryContent(int id, dynamic document, int Id, HttpRequest Request, out long uploaded_size, out int iCounter, out string sFiles_uploaded)
        {
            List<documentModel> documentModels = new List<documentModel>();
            uploaded_size = 0;
            var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("LearnerUpload");
            string path_for_Uploaded_Files = dbPath;
            var uploaded_files = Request.Form.Files;
            iCounter = 0;
            sFiles_uploaded = "";
            var data1 = GetWorkId(id, Id);
            var workId = 0;
            if (data1 == null)
            {
                var workUpload = new UploadWork();
                

                workUpload.learner_id = id;
                workUpload.is_assessor_marking_valid = 0;
                workUpload.is_assessor_marking_valid = 0;
                workUpload.is_marking_validated = 0;
                workUpload.is_ready_for_marking = 0;
                workUpload.is_rejected = 0;
                workUpload.is_work_checked = 0;
                workUpload.rejected_reason = "";
                workUpload.topic_id = Convert.ToInt32(Id);
                workUpload.assessor_assigned_user_id = 0;
                workUpload.created_by = id;
                workUpload.created_on = DateTime.Now;
                workUpload.is_admin_approved = 0;
                workUpload.is_submited = 1;
                workUpload.learner_submit_count = 1;
                var result = InsertUploadWorkRecord(workUpload);
                workId = result;
            }
            else
            {
                var workUpload = new UploadWork();

                var sumbitcount = loadLearnerCount(id, Convert.ToInt32(Id));
                if (sumbitcount != null)
                {
                        int count = sumbitcount.FirstOrDefault().learner_submit_count + sumbitcount.Count();
                    workUpload.learner_submit_count = count;

                }
                workId = data1.id;
                workUpload.id = workId;
                UpdateUploadWorkRecord(workUpload);
            }
            var listFiles = new List<UploadedWorkFiles>();

            foreach (var uploaded_file in uploaded_files)
            {
                iCounter++;
                uploaded_size += uploaded_file.Length;
                sFiles_uploaded += "\n" + uploaded_file.FileName;

                //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string uploaded_Filename = uploaded_file.FileName;
                string Pathstr = "\\" + id + "\\" + Id + "\\";
                string dbstr = Pathstr + uploaded_Filename;
                string new_Filename_on_Server = path_for_Uploaded_Files + "\\" + Pathstr + uploaded_Filename;

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files + Pathstr))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files + Pathstr);
                }


                if (System.IO.File.Exists(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server);
                }
                if (!System.IO.File.Exists(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    using (FileStream stream = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                    {
                        uploaded_file.CopyTo(stream);
                        stream.Flush();
                    }
                }
                byte[] fileData = null;
                using (FileStream fs = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    var binaryReader = new BinaryReader(fs);
                    fileData = binaryReader.ReadBytes((int)fs.Length);

                    listFiles.Add(new UploadedWorkFiles
                    {
                        file_name = uploaded_file.FileName,
                        file_ext = Path.GetExtension(uploaded_file.FileName),
                        file_type = uploaded_file.ContentType,
                        file_size = uploaded_file.Length.ToString(),
                        work_upload_id = workId,

                        file_path = dbstr
                    });
                }
                //  System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server);

            }


            var data = InsertUploadWorkFileRecord(listFiles);
            return documentModels;
        }
        public UploadWork GetWorkId(int learnerId, int topic_id)
        {
            UploadWork data = new UploadWork();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    object dynamicParams = new
                    {
                        learnerId = learnerId,
                        topicId = topic_id
                    };


                    string storedProc = "[dbo].[SP_PORTAL_CREATE_GET_WORK_Id]";
                    data = conn.QuerySingleOrDefault<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return data;

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public List<UploadWork> loadLearnerCount(int learnerid,int topicid)
        {
            List<UploadWork> learners = new List<UploadWork>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@learnerId", learnerid);
                    dynamicParams.Add("@topicid", topicid);

                    string storedProc = "[dbo].[SP_Portal_Learner_SubmitCount]";

                    learners = (List<UploadWork>)conn.Query<UploadWork>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            return learners;

        }
        public int InsertUploadWorkRecord(UploadWork work)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                int data = 0;
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@learner_id", work.learner_id);
                    dynamicParams.Add("@topic_id", work.topic_id);
                    dynamicParams.Add("@is_work_checked", work.is_work_checked);
                    dynamicParams.Add("@is_ready_for_marking", work.is_ready_for_marking);
                    dynamicParams.Add("@is_marking_validated", work.is_marking_validated);
                    dynamicParams.Add("@is_rejected", work.is_rejected);
                    dynamicParams.Add("@is_assessor_marking_valid", work.is_assessor_marking_valid);
                    dynamicParams.Add("@assessor_assigned_user_id", work.assessor_assigned_user_id);
                    dynamicParams.Add("@rejected_reason", work.rejected_reason);
                    dynamicParams.Add("@created_on", work.created_on);
                    dynamicParams.Add("@created_by", work.created_by);
                    dynamicParams.Add("@is_submited", 1);
                    dynamicParams.Add("@learner_submit_count", work.learner_submit_count);

                    dynamicParams.Add("@ap_id", 0);

                    if (work.is_assessorUpload == true)
                    {

                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_PORTAL_CREATE_WORK_UPLOAD]";
                        data = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    return data;

                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        public int UpdateUploadWorkRecord(UploadWork work)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                int data = 0;
                try
                {
                    var dynamicParams = new DynamicParameters();


                    dynamicParams.Add("@id", work.id);
                    dynamicParams.Add("@learner_submit_count", work.learner_submit_count);


                    string storedProc = "[dbo].[SP_PORTAL_UPDATE_WORK_UPLOAD]";
                    data = conn.QuerySingleOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    return data;

                }
                catch (Exception)
                {
                    throw;
                }
            }

        }

        public bool InsertUploadWorkFileRecord(List<UploadedWorkFiles> works)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    foreach (var work in works)
                    {
                        var dynamicParams = new DynamicParameters();
                        dynamicParams.Add("@work_upload_id", work.work_upload_id);
                        dynamicParams.Add("@file_name", work.file_name);
                        dynamicParams.Add("@file_type", work.file_type);
                        dynamicParams.Add("@file_size", work.file_size);
                        dynamicParams.Add("@file_ext", work.file_ext);
                        dynamicParams.Add("@created_on", DateTime.Now);
                        dynamicParams.Add("@created_by", 1);
                        dynamicParams.Add("@file_path", work.file_path);



                        string storedProc = "[dbo].[SP_PORTAL_CREATE_WORK_FILES]";
                        var data = conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }

                }
                catch (Exception)
                {
                    throw;
                }
                return true;
            }

        }

        public Dashboard LoadDashboard(int id)
        {
            Dashboard courseDocuments = new Dashboard();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Portal_GetLearnerDashboardInfo]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    courseDocuments = conn.QuerySingleOrDefault<Dashboard>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return courseDocuments;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public List<Learner> GetMyLearners(int id)
        {
            List<Learner> learners = new List<Learner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_MYLEARNER]";
                    object dynamicParams = new
                    {
                        assessorId = id
                    };

                    learners = (List<Learner>)conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return learners;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public void AssessorFile(int learnerId, HttpRequest Request, int id)
        {
            var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("LearnerUpload");
            string path_for_Uploaded_Files = dbPath;
            var uploaded_files = Request.Form.Files;
            var iCounter = 0;
            long uploaded_size = 0;
            var sFiles_uploaded = "";

            var result = GetWorkId(learnerId, id);
            var listFiles = new List<UploadedWorkFiles>();

            foreach (var uploaded_file in uploaded_files)
            {
                iCounter++;
                uploaded_size += uploaded_file.Length;
                sFiles_uploaded += "\n" + uploaded_file.FileName;

                //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string uploaded_Filename = uploaded_file.FileName;
                string Pathstr = "\\" + id + "\\" + learnerId + "\\";
                string dbstr = Pathstr + uploaded_Filename;
                string new_Filename_on_Server = path_for_Uploaded_Files + "\\" + Pathstr + uploaded_Filename;

                if (!Directory.Exists(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files + Pathstr))
                {
                    System.IO.Directory.CreateDirectory(_hostingEnvironment.WebRootPath + path_for_Uploaded_Files + Pathstr);
                }


                if (System.IO.File.Exists(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server);
                }
                if (!System.IO.File.Exists(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    using (FileStream stream = new FileStream(_hostingEnvironment.WebRootPath + new_Filename_on_Server, FileMode.Create))
                    {
                        uploaded_file.CopyTo(stream);
                        stream.Flush();
                    }
                }
                byte[] fileData = null;
                using (FileStream fs = System.IO.File.OpenRead(_hostingEnvironment.WebRootPath + new_Filename_on_Server))
                {
                    var binaryReader = new BinaryReader(fs);
                    fileData = binaryReader.ReadBytes((int)fs.Length);

                    listFiles.Add(new UploadedWorkFiles
                    {
                        file_name = uploaded_file.FileName,
                        file_ext = Path.GetExtension(uploaded_file.FileName),
                        file_type = uploaded_file.ContentType,
                        file_size = uploaded_file.Length.ToString(),
                        work_upload_id = result.id,

                        file_path = dbstr
                    });
                }
                //  System.IO.File.Delete(_hostingEnvironment.WebRootPath + new_Filename_on_Server);

            }


            var data = InsertUploadWorkFileRecord(listFiles);
        }

        public List<LearnerDoc> GetLearnerDocs(int id)
        {
            List<LearnerDoc> learners = new List<LearnerDoc>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Track_GetAllFilesForLearnerSection3]";
                    object dynamicParams = new
                    {
                        LearnerId = id
                    };

                    learners = (List<LearnerDoc>)conn.Query<LearnerDoc>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return learners;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public Employer LoadEmployeeAccount(int id)
        {
            Employer employee = new Employer();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Portal_Get_EmployeeForAccount]";
                    object dynamicParams = new
                    {
                        learnerid = id
                    };

                    employee = conn.QuerySingleOrDefault<Employer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return employee;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public List<CourseSummaryLearnerNotes> GetSummaryNote(int iLearnerId)
        {
            List<CourseSummaryLearnerNotes> notes = new List<CourseSummaryLearnerNotes>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Portal_GetListLearnerNotes]";
                    object dynamicParams = new
                    {
                        LearnerId = iLearnerId
                    };

                    notes = (List<CourseSummaryLearnerNotes>)conn.Query<CourseSummaryLearnerNotes>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return notes;
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
    }
}