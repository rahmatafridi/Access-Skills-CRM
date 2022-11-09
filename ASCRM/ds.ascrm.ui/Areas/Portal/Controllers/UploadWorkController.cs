using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.ui.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Areas.Portal.Controllers
{
    [Area("Portal")]
    [Route("Portal/UploadWork")]
    public class UploadWorkController : BaseController
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IMailService _emailSender;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<UploadWorkController> _log;
        LogException LogException;
        //  private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;


        public UploadWorkController(ILoginRepository loginRepository,
            IRoleAdminRepository roleAdminRepository,
            IMailService emailSender,
            IConfigurationRepository configurationRepository,
            IEmailTemplateRepository emailTemplateRepository,
            ILogger<UploadWorkController> log,
            IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _loginRepository = loginRepository;
            _roleAdminRepository = roleAdminRepository;
            _emailSender = emailSender;
            _emailTemplateRepository = emailTemplateRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            LogException = new LogException(_configurationRepository, _log);
            // _contact_us_url =  _configurationRepository.GetSingleConfigValueByConfigKey("URL_Contactus");
        }

        [Route("Manage")]
        public IActionResult Manage()
        {
            var SecurityRoleName = "";

            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                SecurityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = SecurityRoleName;
            return View();
        }
    }
}
