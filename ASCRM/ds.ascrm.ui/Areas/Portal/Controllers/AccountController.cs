using ds.core.configuration;
using ds.core.logger;
using ds.portal.companies.Models;
using ds.portal.ui.Controllers;
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
    [Route("Portal/Account")]
    public class AccountController : BaseController
    {
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<AccountController> _log;
        LogException LogException;
        //  private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;


        public AccountController(
            IRoleAdminRepository roleAdminRepository,
            IConfigurationRepository configurationRepository,
            ILogger<AccountController> log,
            IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            LogException = new LogException(_configurationRepository, _log);
        }
        [Route("Learner")]
        public IActionResult Learner()
        {
            ViewBag.Role = GetSecurityRoleCode;
            return View();
        }
        
        [Route("LearnerDetail")]
        public IActionResult LearnerDetail()
        {
            ViewBag.Role = GetSecurityRoleCode;
            string learnerId = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                learnerId = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(learnerId);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                learnerId = id;
            }
            ViewBag.LearnerId = learnerId;
            return View();
        }

    }
}
