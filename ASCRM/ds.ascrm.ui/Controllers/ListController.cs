using ds.core.configuration;
using Microsoft.AspNetCore.Mvc;
using portal.data.repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class ListController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMemoryCache _cache;

        public ListController(IRoleAdminRepository _roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(_roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _configurationRepository = configurationRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Feature = "List Management";
            ViewBag.Page = "Manage";
            SetProductDetailToViewBag();

            return View();
        }
    }
}