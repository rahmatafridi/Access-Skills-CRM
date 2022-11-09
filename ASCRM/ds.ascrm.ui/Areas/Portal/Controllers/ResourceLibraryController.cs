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
    [Route("Portal/ResourceLibrary")]
    public class ResourceLibraryController : BaseController
    {

        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<ResourceLibraryController> _log;
        LogException LogException;
        //  private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;


        public ResourceLibraryController(
            IRoleAdminRepository roleAdminRepository,
            IConfigurationRepository configurationRepository,
            ILogger<ResourceLibraryController> log,
            IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
           
            _configurationRepository = configurationRepository;
            _log = log;
            LogException = new LogException(_configurationRepository, _log);
            // _contact_us_url =  _configurationRepository.GetSingleConfigValueByConfigKey("URL_Contactus");
        }
        [Route("Index")]
        public IActionResult Index()
        {
            var SecurityRoleName = "";

            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                SecurityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = SecurityRoleName;
            if(SecurityRoleName == "700")
            {
                //return = Url.Content("~/Home/Index");
                return LocalRedirect("~/Application/Index");
            }
            if (SecurityRoleName == "1000")
            {
                return LocalRedirect("~/Portal/Index");
            }

            return View();
        }
    }
}
