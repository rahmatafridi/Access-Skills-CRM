using Dapper;
using ds.core.configuration;
using log4net.Core;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using portal.ui.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Web.Mvc;

namespace ds.portal.ui.Controllers
{
    
    public class HomeController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ILogger<HomeController> _logger;
        //private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;
        
        public HomeController(
            IRoleAdminRepository _roleAdminRepository, 
            IConfigurationRepository configurationRepository,
            ILogger<HomeController> logger, IMemoryCache memoryCache)
            : base(_roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _configurationRepository = configurationRepository;
            _logger = logger;
           // SetContactUsUrlToViewBag(); // _contact_us_url = _configurationRepository.GetSingleConfigValueByConfigKey("URL_Contactus");
        }
        public IActionResult Index()
        {            
            ViewBag.Feature = "Home";
            ViewBag.Page = "Home";
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            return View();
        }
        public IActionResult SearchResult()
        {            
            ViewBag.Feature = "Search Result";
            ViewBag.Page = "Search Result";
            
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            ViewBag.roleName = GetSecurityRoleName;
            return View();
        }

        public IActionResult Privacy()
        {            
            ViewBag.Feature = "Home";
            ViewBag.Page = "Privacy";
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //TODO: MR Error
            //Global errors come here in production. Log here instead on each action.
            var _isLog = GetSingleConfigValueByConfigKey("IsLogger");
            if (_isLog == "1")
            {
                var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionFeature != null)
                {
                    _logger.LogInformation
                        (
                           "Path: " + exceptionFeature.Path + ",  "
                           + "Message: " + exceptionFeature.Error.Message + ",  "
                           + "Error: " + exceptionFeature.Error.ToString()
                        );
                }
            }

            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetSingleConfigValueByConfigKey(string config_key)
        {
            using (SqlConnection conn = new SqlConnection(_configurationRepository.GetConnectionString()))
            {
                string storedProc = "[dbo].[SP_Core_Configuration_GetSingleValueByKey]";
                object dynamicParams = new { config_key = config_key };
                return conn.QuerySingleOrDefault<string>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
            }
        }
        public IActionResult LearnerWork()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            return View();
        }
       
        
        public string GetSecurityRoleName
        {
            get
            {
                var SecurityRoleName = "";
                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleName")))
                {
                    SecurityRoleName = HttpContext.Session.GetString("RoleName");
                }
                return SecurityRoleName;
            }
            set {; }
        }
    }
}
