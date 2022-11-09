using ds.core.configuration;
using ds.portal.companies.Models;
using ds.portal.ui.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using portal.data.repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers
{
    public class QuestionValidationController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IMemoryCache _cache;

        public QuestionValidationController(IRoleAdminRepository roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage()
        {

            ViewBag.Page = "Question Validation";
            ViewData["Title"] = "Question Validation";
            return View();
        }
        public IActionResult Create()
        {

            ViewBag.Page = "Question Validation";
            ViewData["Title"] = "Question Validation";
            string validation_Id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                validation_Id = HttpContext.Request.Query["id"];

            ViewBag.Validation_Id = validation_Id;
            return View();
        }

        public IActionResult Edit()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            ViewBag.Feature = "Question Validation";
            ViewBag.Page = "Update";
            ViewData["Title"] = "Update Validation";
           
            string validation_Id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                validation_Id = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(validation_Id);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                validation_Id = id;
            }
            
            ViewBag.Validation_Id = validation_Id;

            return View("Create");
        }
    }
}
