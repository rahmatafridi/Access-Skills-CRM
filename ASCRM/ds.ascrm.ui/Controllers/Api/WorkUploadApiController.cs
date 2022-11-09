using crm.portal.CrmModel;
using crm.portal.Interfaces.CourseSummary;
using crm.portal.Interfaces.Matrix;
using crm.portal.Interfaces.WorkUpload;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.logger;
using ds.portal.companies.Models;
using ds.portal.dashboard;
using ds.portal.ui.Controllers.Api;
using ds.portal.users;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nancy.Json;
using Novacode;
using portal.data.repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Drawing;
using WordToPDF;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/WorkUploadApi")]
    [ApiController]
    public class WorkUploadApiController : ControllerBase
    {
        IDashboardRepository _dashboardRepository;

        readonly ILogger<WorkUploadApiController> _log;
        LogException LogException;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IWorkUploadRepository _workUploadRepository;
        private ICourseSummaryRepository _courseSummaryRepository;
        private ILead_UserRepository _lead_UserRepository;
        private IMailService _mailService;
        private IMatrixRepository _matrixRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        public Microsoft.Office.Interop.Word.Document wordDocument { get; set; }

        public WorkUploadApiController(IHostingEnvironment hostingEnvironment,IMatrixRepository matrixRepository,IMailService mailService, ILead_UserRepository lead_UserRepository,ICourseSummaryRepository courseSummaryRepository, IWorkUploadRepository workUploadRepository, IDashboardRepository dashboardRep
            , ILogger<WorkUploadApiController> log, IConfigurationRepository iConfig, IRoleAdminRepository roleAdminRepository)
        {
            _dashboardRepository = dashboardRep;
            _log = log;
            LogException = new LogException(iConfig, _log);
            _roleAdminRepository = roleAdminRepository;
            _workUploadRepository = workUploadRepository;
            _courseSummaryRepository = courseSummaryRepository;
            _lead_UserRepository = lead_UserRepository;
            _mailService = mailService;
            _matrixRepository = matrixRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        #region Session Values

        private int UserId
        {
            get
            {
                var securityUserId = 0;

                if (HttpContext.Session.Keys.Any(a => a.Equals("UserId")))
                {
                    securityUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                }

                return securityUserId;
            }
            set {; }
        }

        private string UserName
        {
            get
            {
                var securityUserName = "";

                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("UserName")))
                {
                    securityUserName = HttpContext.Session.GetString("UserName");
                }

                return securityUserName;
            }
            set {; }
        }

        private string GetSecurityRoleName
        {
            get
            {
                var SecurityRoleName = "";
                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleName")))
                {
                    SecurityRoleName = HttpContext.Session.GetString("RoleName");
                }
                return SecurityRoleName;
            }
            set {; }
        }
        private string GetSecurityRoleCode
        {
            get
            {
                var SecurityRoleName = "";
                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
                {
                    SecurityRoleName = HttpContext.Session.GetString("RoleCode");
                }
                return SecurityRoleName;
            }
            set {; }
        }
        private int GetSecurityRoleId
        {
            get
            {
                var securityRoleId = 0;

                if (HttpContext.Session.Keys.Any(a => a.Equals("RoleId")))
                {
                    securityRoleId = HttpContext.Session.GetInt32("RoleId") ?? 0;
                }

                return securityRoleId;
            }
            set {; }
        }
        #endregion



        [HttpGet]
        [Route("GetAuditWork")]
        public DataSourceResult GetAuditWork([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                CultureInfo ukCulture = new CultureInfo("en-GB");//CultureInfo.CreateSpecificCulture("en-GB")


                var UploadWorks = new List<WorkAudit>();
                if (GetSecurityRoleCode == "1000")
                {
                    var result = _workUploadRepository.GetAssessorAuditWork(UserId);
                    foreach (var item in result)
                    {
                        //DateTime? due_date = string.IsNullOrEmpty(item.due_date) ? null : (DateTime?)DateTime.Parse(item.due_date, ukCulture.DateTimeFormat);

                        var topics = _courseSummaryRepository.getCourseTopics(item.learner_id);
                        var learnerName = _lead_UserRepository.GetUserByUserName(item.learner_id.ToString());
                       // var topic = topics.FirstOrDefault(x => x.SSJLP_Id == Convert.ToInt32(item.topic_id));
                        //if (item.assessor_assigned_user_id == 0 || item.assessor_assigned_user_id == 0)
                        //{
                        //    item.Status = "Not Assign";
                        //}
                        //else
                        //{
                        //    item.Status = "Assigned";
                        //}
                        UploadWorks.Add(new WorkAudit()
                        {
                            created_by = item.created_by,
                            created_on = item.created_on,
                            id = item.id,
                      
                            is_rejected = item.is_rejected,
                            learner_id = item.learner_id,
                            note = item.note,
                            topic = item.topic,
                            learner_name = learnerName.Users_DisplayName,
                            user_name = item.user_name,

                        });
                    }
                }
              
                return UploadWorks.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetLearnerWork")]
        public DataSourceResult GetLearnerWork([DataSourceRequest] DataSourceRequest request, string type)
        {
            try
            {
                CultureInfo ukCulture = new CultureInfo("en-GB");//CultureInfo.CreateSpecificCulture("en-GB")


                var UploadWorks = new List<UploadWork>();
                
                if (GetSecurityRoleCode == "1000")
                {
                    var result = _workUploadRepository.GetLearnerWorkForAssessor(UserId);
                    foreach (var item in result)
                    {
                        //DateTime? due_date = string.IsNullOrEmpty(item.due_date) ? null : (DateTime?)DateTime.Parse(item.due_date, ukCulture.DateTimeFormat);

                        var topics = _courseSummaryRepository.getCourseTopics(item.learner_id);
                        var learnerName = _lead_UserRepository.GetUserByUserName(item.learner_id.ToString());
                        var topic = topics.FirstOrDefault(x => x.SSJLP_Id == item.topic_id);
                        if (item.assessor_assigned_user_id == 0 || item.assessor_assigned_user_id == 0)
                        {
                            item.Status = "Not Assign";
                        }
                        else
                        {
                            item.Status = "Assigned";
                        }
                        UploadWorks.Add(new UploadWork()
                        {
                            assessor_assigned_user_id = item.assessor_assigned_user_id,
                            created_by = item.created_by,
                            created_on = item.created_on,
                            id = item.id,
                            is_assessor_marking_valid = item.is_assessor_marking_valid,
                            Status = item.Status,
                            is_marking_validated = item.is_marking_validated,
                            is_ready_for_marking = item.is_ready_for_marking,
                            is_rejected = item.is_rejected,
                            is_work_checked = item.is_work_checked,
                            learner_id = item.learner_id,
                            note = item.note,
                            rejected_reason = item.rejected_reason,
                            topic_id = item.topic_id,
                            updated_by = item.updated_by,
                            updated_on = item.updated_on,
                            Topic = topic.TopicName,
                            due_date = item.due_date,
                            assessor_rejected = item.assessor_rejected,
                            LearnerName= learnerName.Users_DisplayName,
                            user_name= item.user_name,
                            completed_date = item.completed_date,
                            learner_submit_count = item.learner_submit_count

                        });
                    }
                }
                else
                {

                    var result = _workUploadRepository.GetLearnerWorkForAdmin(type);
                    foreach (var item in result)
                    {
                        DateTime? dt = null;
                        //if (item.due_date != null) {

                        //    dt = DateTime.ParseExact(item.due_date, "dd/MM/yyyy HH:mm tt", null);

                        //}
                        var topics = _courseSummaryRepository.getCourseTopics(item.learner_id);
                        var learnerName = _lead_UserRepository.GetUserByUserName(item.learner_id.ToString());

                        var topic = topics.FirstOrDefault(x => x.SSJLP_Id == item.topic_id);
                        if (item.assessor_assigned_user_id == 0 || item.assessor_assigned_user_id == 0)
                        {
                            item.Status = "Not Assign";
                        }
                       
                        if (item.assessor_rejected == 1)
                        {
                            item.Status = "Requires Resubmission";
                        }
                        if(item.is_assessor_marking_valid == 1)
                        {
                            item.Status = "Complete";
                        }
                        UploadWorks.Add(new UploadWork()
                        {
                            assessor_assigned_user_id = item.assessor_assigned_user_id,
                            created_by = item.created_by,
                            created_on = item.created_on,
                            id = item.id,
                            is_assessor_marking_valid = item.is_assessor_marking_valid,
                            Status = item.Status,
                            is_marking_validated = item.is_marking_validated,
                            is_ready_for_marking = item.is_ready_for_marking,
                            is_rejected = item.is_rejected,
                            is_work_checked = item.is_work_checked,
                            learner_id = item.learner_id,
                            note = item.note,
                            rejected_reason = item.rejected_reason,
                            topic_id = item.topic_id,
                            updated_by = item.updated_by,
                            updated_on = item.updated_on,
                            Topic = topic.TopicName,
                            due_date = item.due_date,
                            due_date1= dt,
                            assessor_rejected = item.assessor_rejected,
                            LearnerName = learnerName.Users_DisplayName,
                            user_name = item.user_name,
                            completed_date = item.completed_date,
                            learner_submit_count = item.learner_submit_count



                        });
                    }
                }
                return UploadWorks.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("SearchWork")]
        public IActionResult SearchWork([DataSourceRequest] DataSourceRequest request, string type,string name)
        {
            try
            {
                var UploadWorks = new List<UploadWork>();
                if (GetSecurityRoleName == "Portal Assessor")
                {
                    var result = _workUploadRepository.GetLearnerWorkForAssessor(UserId);
                    foreach (var item in result)
                    {
                        var topics = _courseSummaryRepository.getCourseTopics(item.learner_id);
                        var learnerName = _lead_UserRepository.GetUserByUserName(item.learner_id.ToString());

                        var topic = topics.FirstOrDefault(x => x.SSJLP_Id == item.topic_id);
                        if (item.assessor_assigned_user_id == 0 || item.assessor_assigned_user_id == 0)
                        {
                            item.Status = "Not Assign";
                        }
                        else
                        {
                            item.Status = "Assigned";
                        }
                        UploadWorks.Add(new UploadWork()
                        {
                            assessor_assigned_user_id = item.assessor_assigned_user_id,
                            created_by = item.created_by,
                            created_on = item.created_on,
                            id = item.id,
                            is_assessor_marking_valid = item.is_assessor_marking_valid,
                            Status = item.Status,
                            is_marking_validated = item.is_marking_validated,
                            is_ready_for_marking = item.is_ready_for_marking,
                            is_rejected = item.is_rejected,
                            is_work_checked = item.is_work_checked,
                            learner_id = item.learner_id,
                            note = item.note,
                            rejected_reason = item.rejected_reason,
                            topic_id = item.topic_id,
                            updated_by = item.updated_by,
                            updated_on = item.updated_on,
                            Topic = topic.TopicName,
                            due_date =item.due_date,
                            assessor_rejected = item.assessor_rejected,
                            LearnerName = learnerName.Users_DisplayName,
                            user_name = item.user_name,
                             completed_date = item.completed_date

                        });
                    }
                }
                else
                {
                    var result = _workUploadRepository.GetLearnerWorkForSearch(type, name);
                    foreach (var item in result)
                    {
                        var topics = _courseSummaryRepository.getCourseTopics(item.learner_id);
                        var learnerName = _lead_UserRepository.GetUserByUserName(item.learner_id.ToString());

                        var topic = topics.FirstOrDefault(x => x.SSJLP_Id == item.topic_id);
                        if (item.assessor_assigned_user_id == 0 || item.assessor_assigned_user_id == 0)
                        {
                            item.Status = "Not Assign";
                        }
                        if (item.assessor_rejected == 1)
                        {
                            item.Status = "Requires Resubmission";
                        }
                        if (item.is_assessor_marking_valid == 1)
                        {
                            item.Status = "Complete";
                        }
                        UploadWorks.Add(new UploadWork()
                        {
                            assessor_assigned_user_id = item.assessor_assigned_user_id,
                            created_by = item.created_by,
                            created_on = item.created_on,
                            id = item.id,
                            is_assessor_marking_valid = item.is_assessor_marking_valid,
                            Status = item.Status,
                            is_marking_validated = item.is_marking_validated,
                            is_ready_for_marking = item.is_ready_for_marking,
                            is_rejected = item.is_rejected,
                            is_work_checked = item.is_work_checked,
                            learner_id = item.learner_id,
                            note = item.note,
                            rejected_reason = item.rejected_reason,
                            topic_id = item.topic_id,
                            updated_by = item.updated_by,
                            updated_on = item.updated_on,
                            Topic = topic.TopicName,
                            due_date = item.due_date,
                            assessor_rejected = item.assessor_rejected,
                            LearnerName = learnerName.Users_DisplayName,
                            user_name = item.user_name,
                            completed_date = item.completed_date



                        });
                    }
                }
                return Ok(UploadWorks.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
          

        }

     

        [HttpGet]
        [Route("GetWorkFiles")]
        public IActionResult GetWorkFiles(int work_upload_id)
        {
            try
            {
                var coll = _workUploadRepository.WorkFiles(work_upload_id);
                return Ok(coll);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetHistory")]
        public IActionResult GetHistory(int learner_id,int topic_id)
        {
            try
            {
                var coll = _workUploadRepository.LoadTraningHistoryData(learner_id,topic_id);
                return Ok(coll);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetAccesser")]
        public IActionResult GetAccesser()
        {
            try
            {
                return Ok(_workUploadRepository.LoadAssessor());
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("AssignToAssesser")]
        public IActionResult AssignToAssesser(UploadWork uploadWork)
        {
            uploadWork.user_name = UserName;
            uploadWork.user_id = UserId;

            var data = _workUploadRepository.GetUploadWorkById(uploadWork.id);
            if (data != null)
            {
                var resultL = _lead_UserRepository.GetUserById(uploadWork.assessor_assigned_user_id);
                var result = _lead_UserRepository.GetUserById(UserId);

                _mailService.SaveEmailPortal("Assign", uploadWork.rejected_reason, result.Users_Email, resultL.Users_Email, false);
            }
            var user = _lead_UserRepository.GetUserById(uploadWork.assessor_assigned_user_id);
            var topics = _courseSummaryRepository.getCourseTopics(data.learner_id);
            var courseId = _matrixRepository.LoadLearnerCourse(data.learner_id);
            var oscaCourse = _matrixRepository.LoadLearnerCourseForOsca(data.learner_id);
            var topicId = _courseSummaryRepository.getCourseTopicsById(oscaCourse.LearnerCourses_id);
            var learnerName = _lead_UserRepository.GetUserByUserName(data.learner_id.ToString());
            
            AssessorToOsca(user.Users_UserName, data.learner_id, topicId.AP_TopicId, courseId.LearnerCourses_id, uploadWork.due_date,uploadWork.note,learnerName.Users_DisplayName,topics[0].TopicName,2,false);
            return Ok(_workUploadRepository.AssignAssesser(uploadWork));
        }
        public string AssessorToOsca(string username, int learnerId,int topic, int courseid,string  date,string note,string learnrname,string topoicname,int type,bool status)
        {
            //var data = _companyRepository.GetLearnerByCompanyId(id);
            try
            {
                var newDate = DateTime.Now; 
                CultureInfo ukCulture = new CultureInfo("en-GB");//CultureInfo.CreateSpecificCulture("en-GB")
                if (date == "")
                {

                }
                else
                {
                    DateTime? dob_date = string.IsNullOrEmpty(date) ? null : (DateTime?)DateTime.Parse(date, ukCulture.DateTimeFormat);
                    var date1 = Convert.ToDateTime(date);
                    DateTime dt = new DateTime(2015, 12, 31);
                    TimeSpan ts = new TimeSpan(25, 20, 55);

                    newDate = date1.Add(ts);
                }
                    string responseFromServer = "";
                RootObject rootObject = new RootObject();

                var dataList = new List<AssignedLearnerList>();
                    

                    //    string pram = item.learner_id.ToString();


                    WebRequest request = (HttpWebRequest)WebRequest.Create("http://dmss.uk/AssessorCrm/AssessorCrm.asmx/AssignToAssessor?username=" + username + "&learnerId="+ learnerId + "&topic="+ topic + "&course="+courseid +"&date="+newDate+"&note="+note+"&learnername="+learnrname+"&topicname="+topoicname+"&type="+type+"&status="+status);
                    request.Method = "GET";
                    request.ContentType = "'application/json; charset=utf-8";
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();

                    WebResponse response = request.GetResponse();
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);

                        responseFromServer = reader.ReadToEnd();

                       // rootObject = new JavaScriptSerializer().Deserialize<RootObject>(responseFromServer);



                    }
                    //   ReadFrom(response.GetResponseStream());
               
                return "";

            }
            catch (WebException e)
            {
                string pageContent = new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToString();
                return null;
            }

        }
        [HttpPost]
        [Route("Reject")]
        public IActionResult Reject(UploadWork uploadWork)
        {
            uploadWork.user_name = UserName;
            uploadWork.user_id = UserId;

            if (uploadWork.assessor_rejected ==1)
            {
                var result = _lead_UserRepository.GetUserById(UserId);
                _mailService.SaveEmailPortal("Rejected", uploadWork.rejected_reason, result.Users_Email, "",true);
            }
            if(uploadWork.is_rejected == 1)
            {
                var data = _workUploadRepository.GetUploadWorkById(uploadWork.id);
                if(data != null)
                {
                    var resultL = _lead_UserRepository.GetUserByUserName(data.learner_id.ToString());
                    var result = _lead_UserRepository.GetUserById(UserId);

                    _mailService.SaveEmailPortal("Rejected", uploadWork.rejected_reason, result.Users_Email,resultL.Users_Email , false);
                }
                //var user = _lead_UserRepository.GetUserById(data.assessor_assigned_user_id);
                var topics = _courseSummaryRepository.getCourseTopics(data.learner_id);
                //var courseId = _matrixRepository.LoadLearnerCourse(data.learner_id);
                var learnerName = _lead_UserRepository.GetUserByUserName(data.learner_id.ToString());
                uploadWork.LearnerName = learnerName.Users_DisplayName;
                uploadWork.Topic = topics[0].TopicName;
                //AssessorToOsca(user.Users_UserName, data.learner_id, topics[0].SSJLP_Id, courseId.LearnerCourses_id, uploadWork.due_date, uploadWork.note, learnerName.Users_DisplayName, topics[0].TopicName, 1, false);
            }
            return Ok(_workUploadRepository.RejectWork(uploadWork));
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        [HttpPost]
        [Route("Approve")]
        public IActionResult Approve(UploadWork uploadWork)
        {

            try
            {
                if (uploadWork.is_assessor_marking_valid == 1)
                {
                    var data = _workUploadRepository.GetUploadWorkById(uploadWork.id);

                    var learnerName = _lead_UserRepository.GetUserByUserName(data.learner_id.ToString());
                    var topics = _courseSummaryRepository.getCourseTopics(data.learner_id);
                    var courseId = _matrixRepository.LoadLearnerCourse(data.learner_id);
                    var result = _lead_UserRepository.GetUserById(UserId);
                    uploadWork.traningAssessment.tap_assessor_name = result.Users_DisplayName;
                    uploadWork.traningAssessment.tap_learner_name = learnerName.Users_DisplayName;
                    uploadWork.traningAssessment.tap_title = topics[0].TopicName;
                    uploadWork.traningAssessment.tap_level = courseId.LearnerCourse;
                    uploadWork.learner_id = data.learner_id;
                    uploadWork.topic_id = data.topic_id;
                    uploadWork.Topic = topics[0].TopicName;
                    uploadWork.LearnerName = learnerName.Users_DisplayName;
                    var tabfiel = _hostingEnvironment.WebRootPath + "\\Assets\\Tap\\TAP.docx";
                    byte[] fileBytes = GetFile(tabfiel);
                    string sMainTempPath1 = _hostingEnvironment.WebRootPath + "\\Assets\\Temp\\";
                    string sMainTempPath2 = _hostingEnvironment.WebRootPath + "\\Assets\\Temp\\Temp\\"; ;
                    var pdfPath = sMainTempPath2 + uploadWork.learner_id + ".pdf";
                    if (System.IO.File.Exists(pdfPath)) { System.IO.File.Delete(pdfPath); }
                    if (System.IO.File.Exists(sMainTempPath1 + uploadWork.learner_id + ".docx")) { System.IO.File.Delete(sMainTempPath1 + uploadWork.learner_id + ".docx"); }

                    if (!Directory.Exists(sMainTempPath1))
                    {
                        System.IO.Directory.CreateDirectory(sMainTempPath1);
                    }

                    if (!Directory.Exists(sMainTempPath2))
                    {
                        System.IO.Directory.CreateDirectory(sMainTempPath2);
                    }

                    using (var stream = new FileStream(sMainTempPath1 + data.learner_id + ".docx", FileMode.Create, FileAccess.Write, FileShare.Write, 4096, useAsync: true))
                    {
                        //var bytes = Encoding.UTF8.GetBytes(content);
                        stream.WriteAsync(fileBytes, 0, fileBytes.Length);
                    }


                    using (DocX document = DocX.Load(sMainTempPath1 + data.learner_id + ".docx"))
                    {
                        document.ReplaceText("[LearnerName]", learnerName.Users_DisplayName, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[AssessorName]", result.Users_DisplayName, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[Date]", DateTime.Now.ToString("d"), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[CourseTopic]", courseId.LearnerCourse, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[Content]", uploadWork.traningAssessment.tap_content_activty, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[Skills]", uploadWork.traningAssessment.tap_core_skil, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[Development]", uploadWork.traningAssessment.tap_development, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[Reference]", uploadWork.traningAssessment.tap_referencing, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                        document.ReplaceText("[AssessorDate]", DateTime.Now.ToString("d"), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[AdminDate]", "", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        if (uploadWork.traningAssessment.tap_resubmission != "")
                        {
                            uploadWork.traningAssessment.tap_isresubmission = 1;
                            document.ReplaceText("[NoYes]", "No", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                            document.ReplaceText("[Days]", uploadWork.traningAssessment.tap_resubmission, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                        }
                        else
                        {
                            uploadWork.traningAssessment.tap_isresubmission = 0;

                            document.ReplaceText("[NoYes]", "Yes", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                            document.ReplaceText("[Days]", "", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                        }
                        //

                        byte[] bytes = Convert.FromBase64String(uploadWork.traningAssessment.tap_assessor_signature.Split(',')[1]);

                        string sPath = "";
                        string sMainTempPath = _hostingEnvironment.WebRootPath + "\\Assets\\Signature\\";

                        sPath = sMainTempPath + "/signature_.png";
                        string stempPath = sMainTempPath + "/tempsignature_.png";
                        if (!Directory.Exists(sMainTempPath))
                        {
                            System.IO.Directory.CreateDirectory(sMainTempPath);
                        }
                        if (System.IO.File.Exists(sPath)) { System.IO.File.Delete(sPath); }
                        System.IO.File.Copy(stempPath, sPath);
                        if (bytes != null)
                        {
                            using (var imageFile = new FileStream(sPath, FileMode.Create))
                            {
                                imageFile.Write(bytes, 0, bytes.Length);
                                imageFile.Flush();
                                imageFile.Close();
                            }
                            // ReplacedInWordDoc(sPath);
                        }


                        using (var ms = new MemoryStream())
                        {
                            using (var fileStream = new FileStream(sPath, FileMode.Open))
                            {
                                var logo = System.Drawing.Image.FromStream(fileStream);
                                logo.Save(ms, logo.RawFormat);
                                ms.Seek(0, SeekOrigin.Begin);
                            }
                            var myimg = document.AddImage(ms);
                            var pic1 = myimg.CreatePicture();
                            var p = document.InsertParagraph();

                            Novacode.Table placeholderTable = document.Tables[5];
                            placeholderTable.Rows[2].Cells[1].Paragraphs.First().InsertPicture(pic1);
                            //document.ReplaceText("[AssessorSignature]", pic1, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                        }



                        // Save changes made to this document.
                        document.Save();
                    }


                    //var temppdf = sMainTempPath1 + uploadWork.traningAssessment.tap_learner_id + ".pdf";
                    var tempFile = sMainTempPath1 + data.learner_id + ".docx";
                    byte[] sendFile = GetFile(tempFile);
                  

                    uploadWork.traningAssessment.tap_filedata = sendFile;

          


                

                    _mailService.SaveEmailPortal("Apporve", uploadWork.note, result.Users_Email, "", true);
                    //_mailService.SendEmailWithAttachmentsBytes("rusoft1@gmail.com", "Approve File", "Approve Assessor File", sendFile, data.learner_id + ".docs");

                }
                if (uploadWork.is_admin_approved == 1)
                {
                    var data = _workUploadRepository.GetUploadWorkById(uploadWork.id);
                    if (data != null)
                    {
                        var resultL = _lead_UserRepository.GetUserByUserName(data.learner_id.ToString());
                        var result = _lead_UserRepository.GetUserById(UserId);
                        _mailService.SaveEmailPortal("Completed", uploadWork.note, result.Users_Email, resultL.Users_Email, false);
                    }
                    var user = _lead_UserRepository.GetUserById(data.assessor_assigned_user_id);
                    var topics = _courseSummaryRepository.getCourseTopics(data.learner_id);
                    var courseId = _matrixRepository.LoadLearnerCourse(data.learner_id);
                    var learnerName = _lead_UserRepository.GetUserByUserName(data.learner_id.ToString());
                    var oscaCourse = _matrixRepository.LoadLearnerCourseForOsca(data.learner_id);
                    var topicId = _courseSummaryRepository.getCourseTopicsById(oscaCourse.LearnerCourses_id);
                    AssessorToOsca(user.Users_UserName, data.learner_id, topicId.AP_TopicId, courseId.LearnerCourses_id, uploadWork.due_date, uploadWork.note, learnerName.Users_DisplayName, topics[0].TopicName, 1, true);

                    // AssessorToOsca(user.Users_UserName, data.learner_id, topicId.AP_TopicId, courseId.LearnerCourses_id, data.due_date, uploadWork.note, learnerName.Users_DisplayName, topics[0].TopicName, 1, true);
                }

                uploadWork.user_name = UserName;
                uploadWork.user_id = UserId;
                return Ok(_workUploadRepository.ApproveWork(uploadWork));
            }
            catch (Exception ex)
            {

                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
           
        }      
        
        [HttpPost]
        [Route("ApproveUpdate")]
        public IActionResult ApproveUpdate(UploadWork uploadWork)
        {
            //if (uploadWork.is_assessor_marking_valid == 1)
            //{
               var data = _workUploadRepository.GetUploadWorkById(uploadWork.id);

                var learnerName = _lead_UserRepository.GetUserByUserName(uploadWork.traningAssessment.tap_learner_id.ToString());
                var topics = _courseSummaryRepository.getCourseTopics(uploadWork.traningAssessment.tap_learner_id);
                var courseId = _matrixRepository.LoadLearnerCourse(uploadWork.traningAssessment.tap_learner_id);
                var result = _lead_UserRepository.GetUserById(UserId);
                uploadWork.traningAssessment.tap_assessor_name = result.Users_DisplayName;
                uploadWork.traningAssessment.tap_learner_name = learnerName.Users_DisplayName;
                uploadWork.traningAssessment.tap_title = topics[0].TopicName;
                uploadWork.traningAssessment.tap_level = courseId.Course_Title;
                uploadWork.learner_id = uploadWork.traningAssessment.tap_learner_id;
                uploadWork.topic_id = data.topic_id;
            uploadWork.traningAssessment.tap_learner_id = uploadWork.traningAssessment.tap_learner_id;
            uploadWork.traningAssessment.tap_topic_id = (int)data.topic_id;
            var tabfiel = _hostingEnvironment.WebRootPath + "\\Assets\\Tap\\TAP.docx";
            byte[] fileBytes = GetFile(tabfiel);
            string sMainTempPath1 = _hostingEnvironment.WebRootPath + "\\Assets\\Temp\\";
            string sMainTempPath2 = _hostingEnvironment.WebRootPath + "\\Assets\\Temp\\Temp\\"; ;
            //var pdfPath = sMainTempPath2 + uploadWork.traningAssessment.tap_learner_id + ".pdf";
           //if (System.IO.File.Exists(pdfPath)) { System.IO.File.Delete(pdfPath); }
            if (System.IO.File.Exists(sMainTempPath1 + uploadWork.traningAssessment.tap_learner_id + ".docx")) { System.IO.File.Delete(sMainTempPath1+ uploadWork.traningAssessment.tap_learner_id + ".docx"); }

            if (!Directory.Exists(sMainTempPath1))
                {
                    System.IO.Directory.CreateDirectory(sMainTempPath1);
                }

            //if (!Directory.Exists(sMainTempPath2))
            //{
            //    System.IO.Directory.CreateDirectory(sMainTempPath2);
            //}

            using (var stream = new FileStream(sMainTempPath1 + uploadWork.traningAssessment.tap_learner_id + ".docx", FileMode.Create, FileAccess.Write, FileShare.Write, 4096, useAsync: true))
                {
                    //var bytes = Encoding.UTF8.GetBytes(content);
                    stream.WriteAsync(fileBytes, 0, fileBytes.Length);
                }


                using (DocX document = DocX.Load(sMainTempPath1 + uploadWork.traningAssessment.tap_learner_id + ".docx"))
                {
                    document.ReplaceText("[LearnerName]", learnerName.Users_DisplayName, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[AssessorName]", result.Users_DisplayName, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[Date]", DateTime.Now.ToString("d"), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[CourseTopic]", courseId.LearnerCourse, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[Content]", uploadWork.traningAssessment.tap_content_activty, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[Skills]", uploadWork.traningAssessment.tap_core_skil, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[Development]", uploadWork.traningAssessment.tap_development, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[Reference]", uploadWork.traningAssessment.tap_referencing, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                    document.ReplaceText("[AssessorDate]", uploadWork.traningAssessment.tap_date.ToString("d"), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                document.ReplaceText("[AdminDate]", DateTime.Now.ToString("d"), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                if (uploadWork.traningAssessment.tap_resubmission != "")
                {
                    uploadWork.traningAssessment.tap_isresubmission = 1;
                    document.ReplaceText("[NoYes]", "No", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[Days]", uploadWork.traningAssessment.tap_resubmission, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                }
                else
                {
                    uploadWork.traningAssessment.tap_isresubmission = 0;

                    document.ReplaceText("[NoYes]", "Yes", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[Days]", "", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                }
                //

                byte[] bytes = Convert.FromBase64String(uploadWork.traningAssessment.tap_assessor_signature.Split(',')[1]);
                    string sPath = "";
                string sMainTempPath = _hostingEnvironment.WebRootPath + "\\Assets\\signature\\";

                sPath = sMainTempPath + "/signature_.png";
                    string stempPath = sMainTempPath + "/tempsignature_.png";
                    if (!Directory.Exists(sMainTempPath))
                    {
                        System.IO.Directory.CreateDirectory(sMainTempPath);
                    }
                    if (System.IO.File.Exists(sPath)) { System.IO.File.Delete(sPath); }
                    System.IO.File.Copy(stempPath, sPath);
                    if (bytes != null)
                    {
                        using (var imageFile = new FileStream(sPath, FileMode.Create))
                        {
                            imageFile.Write(bytes, 0, bytes.Length);
                            imageFile.Flush();
                            imageFile.Close();
                        }
                        // ReplacedInWordDoc(sPath);
                    }


                    using (var ms = new MemoryStream())
                    {
                        using (var fileStream = new FileStream(sPath, FileMode.Open))
                        {
                            var logo = System.Drawing.Image.FromStream(fileStream);
                            logo.Save(ms, logo.RawFormat);
                            ms.Seek(0, SeekOrigin.Begin);
                        }
                        var myimg = document.AddImage(ms);
                        var pic1 = myimg.CreatePicture();
                        var p = document.InsertParagraph();

                        Novacode.Table placeholderTable = document.Tables[5];
                        placeholderTable.Rows[3].Cells[1].Paragraphs.First().InsertPicture(pic1);
                        //document.ReplaceText("[AssessorSignature]", pic1, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                    }
                    //assessor Sign
                    var assessorsign= _workUploadRepository.LoadTraningData(data.learner_id, (int)data.topic_id);
                uploadWork.traningAssessment.tap_assessor_signature = assessorsign.tap_assessor_signature;
                byte[] bytes3 = Convert.FromBase64String(assessorsign.tap_assessor_signature.Split(',')[1]);
                string sPath3 = "";
                string sMainTempPath3 =  _hostingEnvironment.WebRootPath + "\\Assets\\signature\\";
                

                sPath3 = sMainTempPath3 + "signature_.png";
                string stempPath3 = sMainTempPath3 + "admin_temp_signature.png";
                if (!Directory.Exists(sMainTempPath3))
                {
                    System.IO.Directory.CreateDirectory(sMainTempPath3);
                }
                if (System.IO.File.Exists(sPath3)) { System.IO.File.Delete(sPath3); }
                System.IO.File.Copy(stempPath3, sPath3);
                if (bytes3 != null)
                {
                    using (var imageFile = new FileStream(sPath3, FileMode.Create))
                    {
                        imageFile.Write(bytes3, 0, bytes3.Length);
                        imageFile.Flush();
                        imageFile.Close();
                    }
                    // ReplacedInWordDoc(sPath);
                }


                using (var ms = new MemoryStream())
                {
                    using (var fileStream = new FileStream(sPath3, FileMode.Open))
                    {
                        var logo = System.Drawing.Image.FromStream(fileStream);
                        logo.Save(ms, logo.RawFormat);
                        ms.Seek(0, SeekOrigin.Begin);
                    }
                    var myimg = document.AddImage(ms);
                    var pic1 = myimg.CreatePicture();
                    var p = document.InsertParagraph();

                    Novacode.Table placeholderTable = document.Tables[5];
                    placeholderTable.Rows[2].Cells[1].Paragraphs.First().InsertPicture(pic1);
                    //document.ReplaceText("[AssessorSignature]", pic1, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                }


                // Save changes made to this document.
                document.Save();
                }

            var tempFile = sMainTempPath1 + uploadWork.traningAssessment.tap_learner_id + ".docx";
            byte[] sendFile = GetFile(tempFile);


            uploadWork.traningAssessment.tap_filedata = sendFile;


            _mailService.SaveEmailPortal("Apporve", uploadWork.note, result.Users_Email, "", true);

            //_mailService.SendEmailWithAttachmentsBytes(learnerName.Users_Email, "Approved File", "File Is Approved", sendFile, data.learner_id + ".pdf");


            uploadWork.user_name = UserName;
            uploadWork.user_id = UserId;
            return Ok(_workUploadRepository.UpdateTraningData(uploadWork.traningAssessment));
        }

        [HttpGet]
        [Route("LoadTainingAssessment")]
        public IActionResult LoadTainingAssessment(int learner_id,int topic_id,int id)
        {
            var data = _workUploadRepository.LoadTraningData(learner_id, topic_id);
            var model = new UploadWork();
            model.id = id;
            model.traningAssessment = data;

            return Ok(model);
        }
        [HttpGet]
        [Route("LoadFileData")]
        public IActionResult LoadFileData(int learner_id,int topic_id)
        {
            var data = _workUploadRepository.LoadTraningData(learner_id, topic_id);
            string sMainTempPath2 = _hostingEnvironment.WebRootPath + "\\Assets\\Temp\\Temp\\";
            string viewfiles = _hostingEnvironment.WebRootPath + "\\Assets\\Temp\\ViewFile\\";

            var tempFile = sMainTempPath2 + data.tap_learner_id + ".docx";
           // byte[] sendFile = GetFile(tempFile);
       

            if (System.IO.File.Exists(tempFile)) {

                System.IO.File.Delete(tempFile); 
            }
            if (!Directory.Exists(sMainTempPath2))
            {
                System.IO.Directory.CreateDirectory(sMainTempPath2);
            }
            var pdfPath = viewfiles + data.tap_learner_id + DateTime.Now.ToFileTime() + ".pdf";
            //if (System.IO.File.Exists(pdfPath))
            //{

            //    System.IO.File.Delete(pdfPath);
            //}
            if (!Directory.Exists(viewfiles))
            {
                System.IO.Directory.CreateDirectory(viewfiles);
            }
            if (data.tap_filedata != null)
            {
                using (var imageFile = new FileStream(tempFile, FileMode.Create))
                {
                    BinaryWriter br = new BinaryWriter(imageFile);
                    br.Write(data.tap_filedata, 0, data.tap_filedata.Length);
                    imageFile.Flush();
                    imageFile.Close();
                    imageFile.Dispose();
                }
                // ReplacedInWordDoc(sPath);
            }
            

            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile(tempFile, Spire.Doc.FileFormat.Auto);
            document.SaveToFile(pdfPath, Spire.Doc.FileFormat.PDF);
            document.Close();
            document.Dispose();

     
            byte[] sendFile = GetFile(pdfPath);
            return Ok(sendFile);
        }

        [HttpGet]
        [Route("SendFileToLearner")]
        public IActionResult SendFileToLearner(int learner_id,int topic_id)
        {
            var data = _workUploadRepository.LoadTraningData(learner_id, topic_id);
            var learnerName = _lead_UserRepository.GetUserByUserName(learner_id.ToString());
            //byte[] bytes = Convert.FromBase64String(data.tap_filedata.Split(',')[1]);

            _mailService.SendEmailWithAttachmentsBytes(learnerName.Users_Email, "Feedback File", "You have recived a feedback", data.tap_filedata, data.tap_learner_id + ".pdf");


            return Ok("Success");
        }


    }
}
