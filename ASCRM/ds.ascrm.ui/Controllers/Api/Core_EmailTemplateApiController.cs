using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.emailtemplates.Models;
using ds.core.logger;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/Core_EmailTemplateApi")]
    [ApiController]
    public class Core_EmailTemplateApiController : ControllerBase
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        readonly ILogger<Core_EmailTemplateApiController> _log;
        LogException LogException;
        public Core_EmailTemplateApiController(IEmailTemplateRepository emailTemplateRepository
            , ILogger<Core_EmailTemplateApiController> log, IConfigurationRepository iConfig)
        {
            _emailTemplateRepository = emailTemplateRepository;
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

        #endregion

        [HttpGet]
        [Route("GetAllEmailTemplates")]
        public DataSourceResult GetAllEmailTemplates([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return _emailTemplateRepository.GetAllEmailTemplates().ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllEmailTemplatesForDropdown")]
        public IActionResult GetAllEmailTemplatesForDropdown()
        {
            try
            {
                return Ok(_emailTemplateRepository.GetAllEmailTemplates());
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetTemplatesForDropdownByType")]
        public IActionResult GetTemplatesForDropdownByType(int id)
        {
            try
            {
                return Ok(_emailTemplateRepository.GetTemplatesForDropdownByType(id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertUpdateEmailTemplate")]
        [HistoryFilter]
        public bool InsertUpdateEmailTemplate(EmailTemplate emailTemplate)
        {
            try
            {
                return _emailTemplateRepository.InsertUpdateEmailTemplate(emailTemplate);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetEmailTemplateById")]
        public IActionResult GetEmailTemplateById(int id)
        {
            try
            {
                var contact = _emailTemplateRepository.GetEmailTemplateById(id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteEmailTemplateById")]
        [HistoryFilter]
        public bool DeleteEmailTemplateById(int id)
        {
            try
            {
                return _emailTemplateRepository.DeleteEmailTemplateById(id, UserId, UserName);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("CheckTitleAndCodeExists")]
        public Tuple<bool, bool> CheckTitleAndCodeExists(EmailTemplate emailTemplate)
        {
            try
            {
                return _emailTemplateRepository.CheckTitleAndCodeExists(emailTemplate);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}