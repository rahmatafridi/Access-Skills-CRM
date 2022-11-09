using ds.core.configuration;
using Microsoft.AspNetCore.Mvc;
using portal.data.repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class MyDiaryController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IMemoryCache _cache;

        public MyDiaryController(IRoleAdminRepository roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
        }

        public IActionResult Index()
        {
            SetProductDetailToViewBag();

            return View();
        }
    }
}