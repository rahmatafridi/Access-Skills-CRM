using ds.core.configuration;
using ds.core.logger;
using ds.portal.companies.Models;
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
    [Route("Portal/Messages")]
    public class MessagesController : BaseController
    {
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<MessagesController> _log;
        LogException LogException;
        //  private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;


        public MessagesController(
            IRoleAdminRepository roleAdminRepository,
            IConfigurationRepository configurationRepository,
            ILogger<MessagesController> log,
            IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            LogException = new LogException(_configurationRepository, _log);
        }


        [Route("Manage")]
        public IActionResult Manage()
        {
            var SecurityRoleName = "";

            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                SecurityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = SecurityRoleName;
            return View();
        }
        [Route("Create")]
        public IActionResult Create()
        {
            var SecurityRoleName = "";

            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                SecurityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = SecurityRoleName;
            return View();
        }
        [Route("ViewMessage")]
        public IActionResult ViewMessage(string id)
        {
            //string message_Id = "0";
            //if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            //{
            //    message_Id = HttpContext.Request.Query["id"];
            //    var ids = Encryption.Decrypt(message_Id);
            //    if (ids == "")
            //    {
            //        return RedirectToAction("Error", "Error");
            //    }
            //    message_Id = ids;
            //}
            var SecurityRoleName = "";

            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                SecurityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = SecurityRoleName;
            var ids = Encryption.Decrypt(id);
            if (ids == "")
            {
                return RedirectToAction("Error", "Error");
            }
            ViewBag.Message_Id = ids;

            return View();
        }
    }
}
