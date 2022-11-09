using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.core.configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using portal.data.repository.Interfaces;

namespace ds.portal.ui.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IMemoryCache _cache;


        public ReportController(IRoleAdminRepository roleAdminRepository,
                                IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
        }
        public IActionResult ContactReport()
        {
            SetProductDetailToViewBag();

            return View();
        }

         

    }
}