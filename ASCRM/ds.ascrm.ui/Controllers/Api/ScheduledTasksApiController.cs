using crm.portal.Interfaces.WorkUpload;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.portal.leads;
using ds.portal.users;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using portal.models.Comman;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/ScheduledTasksApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ScheduledTasksApiController : ControllerBase
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMailService _emailSender;
        private readonly IWorkUploadRepository _workUploadRepository;
        private readonly ILead_UserRepository _userRepository;
        private string toEmailId = string.Empty;
        private string toDisplayName = string.Empty;
        private string fromEmailId = string.Empty;
        private readonly ILeadRepository _leadRepository;

        public ScheduledTasksApiController(ILeadRepository leadRepository,ILead_UserRepository userRepository,IWorkUploadRepository workUploadRepository,IMailService emailSender, IEmailTemplateRepository emailTemplateRepository, IConfigurationRepository configurationRepository)
        {
            _configurationRepository = configurationRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailSender = emailSender;
            _workUploadRepository = workUploadRepository;
            _userRepository = userRepository;
            toEmailId = _configurationRepository.GetSingleConfigValueByConfigKey("ToEmail");
            toDisplayName = _configurationRepository.GetSingleConfigValueByConfigKey("ToDisplayName");
            fromEmailId = _configurationRepository.GetSingleConfigValueByConfigKey("FromEmail");
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
        [Route("CheckAssessorTask")]
        public async Task<string> CheckAssessorTask()
        {
            string ip = HttpContext.Connection.RemoteIpAddress.ToString();
            _leadRepository.InsertLeadImportLog(ip, 0);
            var works = _workUploadRepository.CheckAssessorTask();
            if(works != null)
            {
                foreach (var item in works)
                {
                    var user = _userRepository.GetUserById(item.assessor_assigned_user_id);

                    Hashtable ht = new Hashtable();
                    // ht.Add("[COURSELEVEL]", appInvite.api_courseleveltitle.Trim());
                    var emailTemplate = _emailTemplateRepository.GetEmailTemplateByCode("AssignAssessor");
                    if (emailTemplate != null)
                    {
                        if (!string.IsNullOrEmpty(emailTemplate.ET_Body))
                        {
                            var emailModel = new QueueEmails();
                            emailModel.qe_bcc = toEmailId;
                            emailModel.qe_body = emailTemplate.ET_Body;
                            emailModel.qe_cc = "";
                            emailModel.qe_created_by = UserId.ToString();
                            emailModel.qe_created_date = DateTime.Now;
                            emailModel.qe_from = fromEmailId;
                            emailModel.qe_is_html = true;
                            emailModel.qe_mod_date = DateTime.Now;
                            emailModel.qe_priority = 1;
                            emailModel.qe_sent = false;
                            emailModel.qe_status = "";
                            emailModel.qe_subject = "Work Due Date";
                            emailModel.qe_to = user.Users_Email;
                            emailModel.qe_system_id = 1;
                            _emailTemplateRepository.SaveQueueEmail(emailModel);

                            //_emailSender.SendEmail(_userEmail, "Your password has been changed", url_template);
                        }
                    }

                    //Tuple<string, string> tuple = _emailTemplateRepository.GetSubjectAndEmailTemplateBodyReplacedByCode("API001", ht);
                   // appInvite.api_emailbody = tuple.Item2;
                }
            }
            return "Task Scheduled";
        }
    }
}
