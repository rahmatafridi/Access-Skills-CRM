using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.logger;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.models.Comman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalContactApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PortalContactApiController : ControllerBase
    {
        IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMailService _emailSender;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<PortalContactApiController> _log;
        LogException LogException;
        public PortalContactApiController(ILogger<PortalContactApiController> log, IConfigurationRepository iConfig, IConfigurationRepository configurationRepository, IEmailTemplateRepository emailTemplateRepository, IMailService emailSender)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _emailSender = emailSender;
            _configurationRepository = configurationRepository;
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
        [HttpPost]
        [Route("SubmitContact")]
        public IActionResult SubmitContact(string subject, string message)
        {
              string sBody = "";
            sBody = "Dear Access Skills Team, <br/><br/>";
            sBody += UserName + " has submitted a contact form from Portal: ";
            //sBody += "<br/>Subject: " + subject;
            sBody += "<br/>Message: " + message;
            sBody += "<br/><br/>Best Regards, <br/> Access Skills Admin";

            //var qemail = new QueueEmails();
            //qemail.qe_is_html = sBody;
            //qemail.
            //_emailTemplateRepository
            _emailSender.SendEmailPortalContact(subject, sBody,UserName);
            return Ok(true);
        }



}
    
}
