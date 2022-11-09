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
    [Route("Portal/CourseContent")]
    public class CourseContentController : BaseController
    {
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<CourseContentController> _log;
        LogException LogException;
        //  private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;


        public CourseContentController(
            IRoleAdminRepository roleAdminRepository,
            IConfigurationRepository configurationRepository,
            ILogger<CourseContentController> log,
            IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            LogException = new LogException(_configurationRepository, _log);
        }
        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.Role = GetSecurityRoleCode;
            return View();
        }
        [Route("Manage")]

        public IActionResult Manage()
        {
            ViewBag.Role = GetSecurityRoleCode;
            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");
            return View();
        }
        [Route("Create")]
        public IActionResult Create()
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
