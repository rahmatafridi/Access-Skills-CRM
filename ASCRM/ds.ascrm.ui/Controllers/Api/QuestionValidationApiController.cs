using ds.core.configuration;
using ds.core.logger;
using ds.portal.applications;
using ds.portal.applications.Models;
using ds.portal.ui.Controllers.Api;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/QuestionValidationApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class QuestionValidationApiController : ControllerBase
    {
        private readonly IQuestionValidationRepository _validationRepository;
        readonly ILogger<QuestionValidationApiController> _log;
        LogException LogException;

        public QuestionValidationApiController(IQuestionValidationRepository validationRepository, IHostingEnvironment hostingEnvironment
           , ILogger<QuestionValidationApiController> log, IConfigurationRepository iConfig)
        {
            _validationRepository = validationRepository;
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
        [Route("GetQustionValidation")]
        public IActionResult GetQustionValidation([DataSourceRequest] DataSourceRequest request)
        {
 
            CultureInfo ukCulture = new CultureInfo("en-GB");
            try
            {
              
                    var questions = _validationRepository.GetAllQuestionValidation();
                   
                    return Ok(questions.ToDataSourceResult(request));
                }
                
            
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("InsertUpdateValidationRecord")]
        [HistoryFilter]
        public QuestionValidationModel InsertUpdateValidationRecord(QuestionValidationModel question)
        {
            CultureInfo ukCulture = new CultureInfo("en-GB");//CultureInfo.CreateSpecificCulture("en-GB")
            //company.created_by = UserId;
            //company.modified_by = UserId;
            try
            {
                return _validationRepository.AddUpdateQuestionValidation(question);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetValidatinById")]
        public IActionResult GetValidatinById(int id)
        {
            try
            {
                var company = _validationRepository.GetQuestionValidationById(id);
                /// country default set - UK.
                //if (leads.Lead_Contact_Id_Country == 0)
                //{
                //    leads.Lead_Contact_Id_Country = 4;
                //}
                return Ok(company);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetQustionValidationAll")]
        public IActionResult GetQustionValidationAll()
        {

            CultureInfo ukCulture = new CultureInfo("en-GB");
            try
            {

                var questions = _validationRepository.GetAllQuestionValidation();

                return Ok(questions);
            }


            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteQuestionValidationById")]
        [HistoryFilter]
        public IActionResult DeleteQuestionValidationById(int id)
        {
            try
            {
                var retVal = _validationRepository.DeleteValidationById(id);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}
