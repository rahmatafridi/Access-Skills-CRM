using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.core.configuration;
using ds.portal.companies.Models;
using ds.portal.ui.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using portal.data.repository.Interfaces;

namespace ds.ascrm.ui.Controllers
{
    public class ApplicationQuestionsController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IMemoryCache _cache;

        public ApplicationQuestionsController(IRoleAdminRepository roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
        }
        public IActionResult Manage()
        {
            return View();
        }
        public IActionResult Create()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            ViewBag.Feature = "Question";
            ViewBag.Page = "Create";
            ViewData["Title"] = "Create Question";

            string questionId = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                questionId = HttpContext.Request.Query["id"];
            ViewBag.questionId = questionId;
            return View();
        }
        public IActionResult Edit()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            ViewBag.Feature = "Company";
            ViewBag.Page = "Update";
            ViewData["Title"] = "Update Question";
            
            string questionId = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                questionId = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(questionId);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                questionId = id;
            }
            ViewBag.Owner = (isOwnerOfLead(GetSecurityUserId, int.Parse(questionId)) == true ? "1" : "0");
            ViewBag.questionId = questionId;

            return View("Create");
        }
    }
}
