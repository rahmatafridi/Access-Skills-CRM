using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.core.configuration;
using ds.portal.companies.Models;
using Microsoft.AspNetCore.Mvc;
using portal.data.repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IMemoryCache _cache;

        public CompanyController(IRoleAdminRepository roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
        }
        public IActionResult Manage()
        {           
            ViewBag.Feature = "Company Directory";
            ViewBag.Page = "Company Directory";
            ViewData["Title"] = "Company Directory";
            SetProductDetailToViewBag();

            return View();
        }
        public IActionResult Duplicates()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            ViewBag.Feature = "Manage Duplicates";
            ViewBag.Page = "Manage Duplicates";

            return View();
        }
        public IActionResult Create()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            ViewBag.Feature = "Company";
            ViewBag.Page = "Create";
            ViewData["Title"] = "Create Company";
            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");

            string company_Id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                company_Id = HttpContext.Request.Query["id"];

            ViewBag.Company_Id = company_Id;
            ViewBag.CompanyId_ForLead = Encryption.Encrypt(company_Id.ToString());
            return View();
        }

        public IActionResult Edit()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            ViewBag.Feature = "Company";
            ViewBag.Page = "Update";
            ViewData["Title"] = "Update Company";
            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");

            string company_Id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                company_Id = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(company_Id);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                company_Id = id;
            }
            ViewBag.Owner = (isOwnerOfLead(GetSecurityUserId, int.Parse(company_Id)) == true ? "1" : "0");
            ViewBag.Company_Id = company_Id;
            ViewBag.CompanyId_ForLead = Encryption.Encrypt(company_Id.ToString());

            return View("Create");
        }
    }
}