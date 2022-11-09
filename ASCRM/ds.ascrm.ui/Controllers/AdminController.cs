using ds.core.configuration;
using Microsoft.AspNetCore.Mvc;
using portal.data.repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMemoryCache _cache;

        public AdminController(IRoleAdminRepository _roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(_roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _configurationRepository = configurationRepository;
        }

        public IActionResult Upgrade()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            return View();
        }
    }
}