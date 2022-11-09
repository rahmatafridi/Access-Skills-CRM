using ds.core.configuration;
using ds.core.logger;
using ds.portal.leads;
using ds.portal.ui.Models;
using ds.portal.users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class LeadController : BaseController
    {
        private readonly ILoginRepository _loginRepository;
        private readonly ILeadRepository _leadRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<LeadController> _log;
        LogException LogException;
        private readonly ILead_UserRepository _Lead_UserRepository;
        //private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;

        public LeadController(ILoginRepository loginRepository, IHostingEnvironment hostingEnvironment, ILeadRepository leadRepository, IRoleAdminRepository _roleAdminRepository, IConfigurationRepository configurationRepository,
            ILogger<LeadController> log, ILead_UserRepository Lead_UserRepository, IMemoryCache memoryCache)
            : base(_roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _loginRepository = loginRepository;
            _hostingEnvironment = hostingEnvironment;
            _leadRepository = leadRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            LogException = new LogException(_configurationRepository, _log);
            _Lead_UserRepository = Lead_UserRepository;
           // _contact_us_url = _configurationRepository.GetSingleConfigValueByConfigKey("URL_Contactus");
        }

        public IActionResult Create()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();   
            ViewBag.Feature = "Leads";
            ViewBag.Page = "Create";
            ViewData["Title"] = "Create Lead";
            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");

            string lead_Id = "0";
            string company_Id = "0";
            string id = "0";

            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                lead_Id = HttpContext.Request.Query["id"];
                var leadid = Encryption.Decrypt(lead_Id);
                if (leadid == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                lead_Id = leadid;
            }
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["CompnayId"]))
            {
                company_Id = HttpContext.Request.Query["CompnayId"];
                id = Encryption.Decrypt(company_Id);

            }
            ViewBag.Company_Id = id;
            ViewBag.Lead_Id = lead_Id;
          
            return View();
        }
        public IActionResult Edit()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();  
            ViewBag.Feature = "Leads";
            ViewBag.Page = "Update";
            ViewData["Title"] = "Update Lead";
             
            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");
            string lead_Id = "0";
            string company_Id = "0";
            string id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                lead_Id = HttpContext.Request.Query["id"];
                var leadid = Encryption.Decrypt(lead_Id);
                if (leadid == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                lead_Id = leadid;
            }
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["CompnayId"]))
            {
                company_Id = HttpContext.Request.Query["CompnayId"];
                 id = Encryption.Decrypt(company_Id);

            }
            ViewBag.Owner = (isOwnerOfLead(GetSecurityUserId, int.Parse(lead_Id)) == true ? "1" :"0");
            
            ViewBag.Lead_Id = lead_Id;
            ViewBag.Company_Id = id;
            ///ViewBag.roleName1 = GetSecurityRoleName;

            return View("Create");
        }
        public IActionResult Manage()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();   
            ViewBag.Feature = "Leads";
            ViewBag.Page = "Manage";
          
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
        public IActionResult ViewLead()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();   
            ViewBag.Feature = "Leads";
            ViewBag.Page = "View";
            ViewData["Title"] = "View Lead";
            
            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");

            string lead_Id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                lead_Id = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(lead_Id);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                lead_Id = id;
            }
            ViewBag.Lead_Id = lead_Id;

            return View();
        }
    }
}


