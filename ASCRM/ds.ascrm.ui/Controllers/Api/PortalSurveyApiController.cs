using crm.portal.Interfaces.Survey;
using crm.portal.OscaModel;
using ds.core.configuration;
using ds.core.logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalSurveyApi")]
    [ApiController]
    public class PortalSurveyApiController : ControllerBase
    {

        private readonly ISurveyRepository _surveyRepository;
        readonly ILogger<PortalSurveyApiController> _log;
        LogException LogException;
        public PortalSurveyApiController(ILogger<PortalSurveyApiController> log, IConfigurationRepository iConfig, ISurveyRepository surveyRepository)
        {
            _log = log;
            LogException = new LogException(iConfig, _log);
            _surveyRepository = surveyRepository;
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


        [HttpPost]
        [Route("SubmitSurveyLearner")]
        public ActionResult SubmitSurveyLearner(List<SurveyCompletedAnswer> answer)
        {
            try
            {
                return Ok(_surveyRepository.SaveSurvey(answer));

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
         [HttpGet]
        [Route("GetSurveyLinks")]
        public IActionResult GetSurveyLinks()
        {
            try
            {
                return Ok(_surveyRepository.GetSurveyLinks(8763));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetSurveyLinkOne")]
        public IActionResult GetSurveyLinkOne(int surveyId)
        {
            try
            {
                return Ok(_surveyRepository.GetSurveyLinkOne(8763, surveyId));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("SubmitSurvey")]
        public IActionResult SubmitSurvey()
        {

            try
            {

            
            var surveyName = _surveyRepository.GetSurveyLinks(8763);
            string answerText = "";
            string sBody = "<body style='font-family:arial;font-size:15px;'>";
            string strRow = "";
            strRow += " <table border=0 style='font-family:arial;font-size:14px;' cellspacing=0 cellpadding=2 width=\"100%\" align=left>";
            strRow += " <tbody>";
            strRow += "<table width='820' cellspacing=0 cellpadding=0 border='0' style='border:1px solid #c3c3c3; padding:10px;' align='left'><tbody>";
            strRow += "<tr>";
            strRow += "<td width='33%' style='text-align:left;'><img src='http://osca.ukqcs.com/images/intouch.jpg' border='0' width='160' /></td>";
            strRow += "<td width='33%' style='text-align:center;'><img src='http://osca.ukqcs.com/images/access-skills-logo.png' style='margin-bottom: -20px;' border='0' /></td>";
            strRow += "<td width='33%' style='text-align:right;'><img src='http://osca.ukqcs.com/images/bournevile.png' style='width: 156px;margin: 0 10px -9px 0;' border='0' /></td>";
            strRow += "</tr><tr>";
            strRow += "<td colspan='3' style='height:15px;'></td>";
            strRow += " </tr><tr>";
            strRow += "<td colspan='3' style='background:#4d5bf3; color:#fff;font-weight:bold; padding:10px 0;text-align:center;'>";
            //strRow += "Learner Voice Mid Course Evaluation";
            // servury Name
            strRow += "testing";
            strRow += "</td></tr><tr><td colspan='3' style='height:15px;'></td></tr><tr>";
            strRow += "<td style='border:1px solid #000; padding:10px ;'>";
            strRow += "Learner Name ";
            strRow += " </td><td colspan='2' style='border:1px solid #000; padding:10px ;'>";
            //CompletedbyName
            strRow += "testing";
            strRow += "</td></tr><tr><td style='border:1px solid #000; padding:10px ;'>";
            strRow += "Course Title </td><td colspan='2' style='border:1px solid #000; padding:10px ;'>";
            //courseName
            strRow += "test";
            strRow += "</td></tr><tr><td colspan='3'>";
            strRow += "<table width='800' border=0 style='font-family:verdana,arial;padding:10px; font-size:14px;border:1px solid #c3c3c3;' cellspacing=0 cellpadding=2 align=left><tbody>";


            return null;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}
