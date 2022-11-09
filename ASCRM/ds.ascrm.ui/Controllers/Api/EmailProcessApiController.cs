using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.portal.leads;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using portal.models.Comman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/EmailProcessApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class EmailProcessApiController : ControllerBase
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMailService _emailSender;
        private readonly ILeadRepository _leadRepository;
        public EmailProcessApiController(ILeadRepository leadRepository,IMailService emailSender,IEmailTemplateRepository emailTemplateRepository, IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailSender = emailSender;
            _leadRepository = leadRepository;
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
        [Route("EmailProcess")]

        public async Task<string> EmailProcess()
        {
            string ip = HttpContext.Connection.RemoteIpAddress.ToString();
            _leadRepository.InsertLeadImportLog(ip,0);
            var number = _configurationRepository.GetSingleConfigValueByConfigKey("EmailNumber");
            var emails = _emailTemplateRepository.GetQueueEmailByNumber(Convert.ToInt32(number));
            foreach (var item in emails)
            {
                _emailSender.SendEmail(item.qe_to, item.qe_subject, item.qe_body);

                var qemail = new QueueEmails();
                qemail.qe_id = item.qe_id;
                _emailTemplateRepository.InsertUpdateQueueEmail(qemail);
            }
            return "Email Processed";
        }
    }
}
