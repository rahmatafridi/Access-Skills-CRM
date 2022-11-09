using ds.core.configuration;
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

namespace ds.ascrm.ui.Controllers
{
    public class LearnersController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILogger<LearnersController> _logger;
        //private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;

        public LearnersController(
            IRoleAdminRepository _roleAdminRepository,
            IConfigurationRepository configurationRepository,
            ILogger<LearnersController> logger, IMemoryCache memoryCache)
            : base(_roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _configurationRepository = configurationRepository;
            _logger = logger;
            // SetContactUsUrlToViewBag(); // _contact_us_url = _configurationRepository.GetSingleConfigValueByConfigKey("URL_Contactus");
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Manage()
        {
            return View();
        }
        public IActionResult Detail()
        {

            string learner_Id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                learner_Id = HttpContext.Request.Query["id"];
            var id = Encryption.Decrypt(learner_Id);
            ViewBag.Learner_Id = id;
            //ViewBag.CompanyId_ForLead = Encryption.Encrypt(learner_Id.ToString());
            return View();
        }
    }
}
