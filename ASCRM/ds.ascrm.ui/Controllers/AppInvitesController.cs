using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.core.configuration;
using Microsoft.AspNetCore.Mvc;
using portal.data.repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class AppInvitesController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMemoryCache _cache;

        public AppInvitesController(IRoleAdminRepository _roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(_roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _configurationRepository = configurationRepository;
        }
        public IActionResult ManageInvites()
        {
            ViewBag.Feature = "Application Invites Management";
            ViewBag.Page = "Manage";
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            return View();
        }
    }
}