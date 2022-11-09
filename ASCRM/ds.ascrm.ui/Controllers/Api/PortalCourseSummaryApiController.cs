using crm.portal.CrmModel;
using crm.portal.Interfaces.CourseSummary;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.document;
using ds.core.logger;
using ds.portal.leads.Models;
using ds.portal.ui.Controllers.Api;
using ds.portal.users;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalCourseSummaryApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    
    public class PortalCourseSummaryApiController : ControllerBase
    {
        private readonly ICourseSummaryRepository _courseSummaryRepository;
        IDocument _document;
        private ILead_UserRepository _lead_UserRepository;
        private IMailService _mailService;
        readonly ILogger<PortalCourseSummaryApiController> _log;
        LogException LogException;
        public PortalCourseSummaryApiController(ILogger<PortalCourseSummaryApiController> log, IConfigurationRepository iConfig, ILead_UserRepository lead_UserRepository, IMailService mailService, IDocument document, ICourseSummaryRepository courseSummaryRepository)
        {
            _lead_UserRepository = lead_UserRepository;
            _mailService = mailService;
            _courseSummaryRepository = courseSummaryRepository;
            _document = document;
            _log = log;
            LogException = new LogException(iConfig, _log);
        }

        

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
        [HttpGet]
        [Route("GetCourseTopics")]
        public IActionResult GetCourseTopics()
        {
            try
            {
                return Ok(_courseSummaryRepository.getCourseTopics(Convert.ToInt32(UserName)));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        } 
        [HttpGet]
        [Route("GetCourseTopicsForAccount")]
        public IActionResult GetCourseTopicsForAccount(int id)
        {
            try
            {
                return Ok(_courseSummaryRepository.getCourseTopics(id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        } 
        [HttpGet]
        [Route("GetLearnerNotes")]
        public IActionResult GetLearnerNotes(int id)
        {
            try
            {
                return Ok(_courseSummaryRepository.GetSummaryNote(id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetCourseTopicsById")]
        public IActionResult GetCourseTopicsById(int id)
        {
            try
            {
                return Ok(_courseSummaryRepository.getCourseTopics(id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetLearnerDocForAccount")]
        public IActionResult GetLearnerDocForAccount(int id)
        {
            try
            {
                return Ok(_courseSummaryRepository.GetLearnerDocs(id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("UploadDocumentsAjax")]
        [HistoryFilter]
        public bool UploadDocumentsAjax()
        {
            try
            {
                UploadedWorkFiles document = JsonConvert.DeserializeObject<UploadedWorkFiles>(Request.Form["documentData"]);
                long uploaded_size;
                int iCounter;
                string sFiles_uploaded;

                if (document.isAssessor == true)
                {
                    var files = Request.Form.Files;
                    _courseSummaryRepository.AssessorFile(document.learner_id,Request,Convert.ToInt32(document.topic_id));

                }
                else
                {
                    /// method call to upload document.
                    var documents = _courseSummaryRepository.UploadCourseSummaryContent(int.Parse(UserName), document, Convert.ToInt32(document.topic_id), Request, out uploaded_size, out iCounter, out sFiles_uploaded);
                    var result = _lead_UserRepository.GetUserById(UserId);
                    _mailService.SaveEmailPortal("Submitted", "Submitted", result.Users_Email, "", true);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}
