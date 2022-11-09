using ds.core.configuration;
using ds.core.logger;
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
    [Route("Portal/Home")]
    public class HomeController : BaseController
    {
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<HomeController> _log;
        LogException LogException;
        //  private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;


        public HomeController(
            IRoleAdminRepository roleAdminRepository,
            IConfigurationRepository configurationRepository,
            ILogger<HomeController> log,
            IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            LogException = new LogException(_configurationRepository, _log);
        }
        [Route("Profile")]
        public IActionResult Profile()
        {
            ViewBag.Role = GetSecurityRoleCode;
            return View();
        }

    }
}
