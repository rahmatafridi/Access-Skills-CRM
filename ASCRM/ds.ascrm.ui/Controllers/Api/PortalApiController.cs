using crm.portal.Interfaces.CourseContent;
using crm.portal.Interfaces.WorkUpload;
using Microsoft.AspNetCore.Cors;

using ds.core.logger;
using ds.portal.dashboard;
using ds.portal.ui.Controllers.Api;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using crm.portal.CrmModel;
using crm.portal.Interfaces.CourseSummary;
using ds.portal.users;
using crm.portal.Interfaces.User;
using crm.portal.Interfaces.ResourceLibrary;
using ds.core.configuration;
using crm.portal.Interfaces.MyCPD;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]

    public class PortalApiController : ControllerBase
    {
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly IWorkUploadRepository _workUploadRepository;
        private ICourseSummaryRepository _courseSummaryRepository;
        private ILead_UserRepository _lead_UserRepository;
        private IPortalUserRepository _portalUserRepository;
        private IResourceLibraryRepository _resourceLibraryRepository;
        private IMyCPDRepository _myCPDRepository;
        readonly ILogger<PortalApiController> _log;
        LogException LogException;
        public PortalApiController(IMyCPDRepository myCPDRepository,ILogger<PortalApiController> log, IConfigurationRepository iConfig,IResourceLibraryRepository resourceLibraryRepository,IPortalUserRepository portalUserRepository, ILead_UserRepository lead_UserRepository, ICourseSummaryRepository courseSummaryRepository, IWorkUploadRepository workUploadRepository, ICourseContentRepository courseContentRepository)
        {
            _courseContentRepository = courseContentRepository;
            _myCPDRepository = myCPDRepository;
            _workUploadRepository = workUploadRepository;
            _courseSummaryRepository = courseSummaryRepository;
            _lead_UserRepository = lead_UserRepository;
            _portalUserRepository = portalUserRepository;
            _resourceLibraryRepository = resourceLibraryRepository;
            _log = log;
            LogException = new LogException(iConfig, _log);
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
        [Route("GetAssosserSummary")]
        public IActionResult GetAssosserSummary()
        {
            try
            {
                var UploadWorks = new AccessorSummary();
                if (GetSecurityRoleCode == "1000")
                {
                    var data = _workUploadRepository.AssessorSummary(UserId);
                    int totalRejected = 0;
                    int totalApprove = 0;
                    foreach (var item in data)
                    {
                        totalRejected += item.assessor_reject_count;
                        totalApprove += item.assessor_approve_count;
                    }
                    UploadWorks.assessor_approve_count = totalApprove;
                    UploadWorks.assessor_reject_count = totalRejected;
                   

                }
                return Ok(UploadWorks);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetNotification")]
        public IActionResult GetNotification()
        {
            try
            {
                var UploadWorks = new List<UploadWork>();
                if (GetSecurityRoleCode == "900")
                {
                    var data = _workUploadRepository.Notifications(Convert.ToInt32(UserName));
                    foreach (var item in data)
                    {
                        var topics = _courseSummaryRepository.getCourseTopics(item.learner_id);
                        var topic = topics.FirstOrDefault(x => x.SSJLP_Id == item.topic_id);
                        if (item.assessor_rejected == 1 || item.is_rejected == 1)
                        {
                            item.Status = "Rejected";
                        }
                        else if(item.is_submited==1 && item.is_ready_for_marking ==0)
                        {
                            item.Status = "Awaiting Submission";
                        }
                        else if (item.is_ready_for_marking == 1)
                        {
                            item.Status = "Awaiting Feedback";

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
                            

                        });
                    }
                }
                return Ok(UploadWorks);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("DeleteWorkUploadById")]
        public IActionResult DeleteWorkUploadById(int id)
        {

            return Ok(_workUploadRepository.DeleteWorkFile(id));
        }

        [HttpGet]
        [Route("GetPortalAllUsers")]
        public DataSourceResult GetPortalAllUsers([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                return _lead_UserRepository.GetPortalAllUsers().ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetDashboardData")]
        public IActionResult GetDashboardData()
        {

            try
            {
                if (GetSecurityRoleCode == "900")
                {
                    var data = _courseSummaryRepository.LoadDashboard(Convert.ToInt32(UserName));
                    return Ok(data);
                }
                else
                {
                    return Ok(null);
                }
            }
            catch (Exception ex)
            {

                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
           
        
        } 
        [HttpGet]
        [Route("GetDashboardDataForAccount")]
        public IActionResult GetDashboardDataForAccount(int id)
        {

            try
            {
                var data = _courseSummaryRepository.LoadDashboard(id);
                return Ok(data);

            }
            catch (Exception ex)
            {

                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }

           
        
        }
        [HttpGet]
        [Route("GetEmployeeDataForAccount")]
        public IActionResult GetEmployeeDataForAccount(int id)
        {
            try
            {
                var data = _courseSummaryRepository.LoadEmployeeAccount(id);
                return Ok(data);

            }
            catch (Exception ex)
            {

                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
          
        }
        [HttpGet]
        [Route("GetMyLearners")]
        public DataSourceResult  GetMyLearners([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var data = _courseSummaryRepository.GetMyLearners(UserId);
                return data.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {

                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
           
        }
        [HttpGet]
        [Route("GetListUsersForPortal")]
        public DataSourceResult GetListUsersForPortal([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                return _portalUserRepository.GetListUsersForPortal(UserId).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        

        //[HttpPost]
        //[Route("InsertUpdateUserRecord")]
        //[HistoryFilter]
        //public bool InsertUpdateUserRecord(Users user)
        //{
        //    try
        //    {
        //        return _userRepository.InsertUpdateUserRecord(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogException.Log(ex, this.ControllerContext);
        //        throw new Exception(ex.ToString());
        //    }
        //}


        [HttpPost]
        [Route("CheckEmailAlreadyExists")]
        public IActionResult CheckEmailAlreadyExists(Portal_User user)
        {
            try
            {
                var isExists = _portalUserRepository.CheckEmailAlreadyExists(user.Users_Id, user.Users_Username);
                return Ok(isExists);
            }
            catch (Exception ex)
            {
            LogException.Log(ex, this.ControllerContext);
            throw new Exception(ex.ToString());
             }
    
          }
        [HttpGet]
        [Route("GetDocFiles")]
        public DataSourceResult GetDocFiles([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                return _resourceLibraryRepository.LoadDocFiles().ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetDocFilesById")]
        public ActionResult GetDocFilesById(int id)
        {
            try
            {
                return Ok(_resourceLibraryRepository.LoadDocFilesById(id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetDocCategory")]
        public DataSourceResult GetDocCategory([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                return _resourceLibraryRepository.LoadDocCategory().ToDataSourceResult(request);

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("AddUpdateCategory")]
        public IActionResult AddUpdateCategory(PortalDocCategory cat)
        {
            try
            {
                var data = _resourceLibraryRepository.AddDocCategory(cat);
                return Ok(data);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCategoryList")]
        public IActionResult GetCategoryList()
        {
            try
            {
                var data = _resourceLibraryRepository.LoadDocCategory();
                return Ok(data);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetQuickStats")]
        public IActionResult GetQuickStats(int id)
        {
            try
            {
                var data = _resourceLibraryRepository.LoadQuickStat(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAdditinalActivities")]
        public DataSourceResult GetAdditinalActivities([DataSourceRequest] DataSourceRequest request,int id)
        {
            try
            {
                return _myCPDRepository.GetAdditinalActivities(id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCompletedActivities")]
        public DataSourceResult GetCompletedActivities([DataSourceRequest] DataSourceRequest request,int id)
        {
            try
            {
                var data = _myCPDRepository.GetCompletedActivities(id);
                return data.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCategoryById")]
        public IActionResult GetCategoryById(int id)
        {
            try
            {
                var data = _resourceLibraryRepository.GetDocCategoryById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}
