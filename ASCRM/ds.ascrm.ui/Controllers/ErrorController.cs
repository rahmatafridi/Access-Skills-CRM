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
    public class ErrorController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMemoryCache _cache;

        public ErrorController(IRoleAdminRepository _roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
           : base(_roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _configurationRepository = configurationRepository;
        }
        public IActionResult PermissionDenied()
        {
            ViewBag.Feature = "Error";
            ViewBag.Page = "Permission Denied";
            SetProductDetailToViewBag();

            return View();
        }

        public IActionResult Error()
        {
            ViewBag.Feature = "Error";
            ViewBag.Page = "Error";
            SetProductDetailToViewBag();

            return View();
        }
    }
}