using ds.core.configuration;
using ds.portal.ui.Models;
using Microsoft.AspNetCore.Mvc;
using portal.data.repository.Interfaces;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class UserAdminController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMemoryCache _cache;

        public UserAdminController(IRoleAdminRepository _roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(_roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _configurationRepository = configurationRepository;
        }
        public IActionResult Manage()
        {
            ViewBag.Feature = "Users Management";
            ViewBag.Page = "Manage";
            SetProductDetailToViewBag();

            return View();
        }
        public IActionResult Create()
        {
            ViewData["Title"] = "Create User";
            ViewBag.Feature = "Users Management";
            ViewBag.Page = "Create";
            SetProductDetailToViewBag();

            ViewBag.UserId = 0;
            return View();
        }

        public IActionResult Edit()
        {
            ViewData["Title"] = "Edit User";
            ViewBag.Feature = "Users Management";
            ViewBag.Page = "Edit";
            string userId = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                userId = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(userId);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                userId = id;
            }

            ViewBag.UserId = userId;
            return View("Create");
        }
    }
}