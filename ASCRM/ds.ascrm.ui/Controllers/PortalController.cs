using crm.portal.CrmModel;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.ui.Controllers;
using ds.portal.ui.Models;
using ds.portal.users.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using portal.models.Account;
using portal.ui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers
{
    public class PortalController : BaseController
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IMailService _emailSender;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<PortalController> _log;
        LogException LogException;
        //  private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;


        public PortalController(ILoginRepository loginRepository,
            IRoleAdminRepository roleAdminRepository,
            IMailService emailSender,
            IConfigurationRepository configurationRepository,
            IEmailTemplateRepository emailTemplateRepository,
            ILogger<PortalController> log,
            IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _loginRepository = loginRepository;
            _roleAdminRepository = roleAdminRepository;
            _emailSender = emailSender;
            _emailTemplateRepository = emailTemplateRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            LogException = new LogException(_configurationRepository, _log);
            // _contact_us_url =  _configurationRepository.GetSingleConfigValueByConfigKey("URL_Contactus");
        }

        public IActionResult Index()
        {
            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");

            var securityRoleName = "";

            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                securityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = securityRoleName;
            return View();
        }
        [AllowAnonymous]

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.ErrorMessage = null;
            returnUrl = returnUrl ?? Url.Content("~/Portal/Index");
           User user = _loginRepository.ValidateUser(loginModel.Email, loginModel.Password, loginModel.RememberMe);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                ViewBag.ErrorMessage = "Invalid login attempt.";
                return View();
            }
            else
            {
                if (user.Users_IsActive == 1) //&& user.IsAutenticated
                {
                    HttpContext.Session.SetInt32("UserId", user.Users_ID);
                    HttpContext.Session.SetString("UserName", user.Users_UserName);
                    HttpContext.Session.SetString("DisplayName", user.Users_DisplayName);
                    HttpContext.Session.SetInt32("Users_Id_LearnerContacts", (int)user.Users_Id_LearnerContacts);

                    //Check role id for user                    

                    if (user.Users_RoleId > 0)
                    {
                        HttpContext.Session.SetInt32("RoleId", user.Users_RoleId);
                        HttpContext.Session.SetString("RoleName", user.Users_RoleName);
                        HttpContext.Session.SetString("RoleCode", user.RoleCode);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Users_UserName),
                            new Claim(ClaimTypes.Role, user.Users_RoleName),
                            new Claim("id", $"{user.Users_ID}"),
                        };

                        var userIdentity = new ClaimsIdentity(claims, "Auth");

                        ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                        await HttpContext.SignInAsync(principal);
                        if (user.RoleCode != "700" && user.RoleCode != "900" && user.RoleCode != "1000" && user.RoleCode != "1100" && user.RoleCode != "1200")
                        {
                            returnUrl = Url.Content("~/Home/Index");
                        }
                        if (user.RoleCode == "700")
                        {
                            returnUrl = Url.Content("~/Application/Index");

                        }
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        ViewBag.ErrorMessage = "Invalid login attempt.";
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    ViewBag.ErrorMessage = "Invalid login attempt.";
                    return View();
                }
            }
           
        }
        [AllowAnonymous]
        public IActionResult Logout()
        {
            //SetContactUsUrlToViewBag();
            //SetProductDetailToViewBag();

            ViewBag.ErrorMessage = null;
            HttpContext.Session.Clear();

            string returnUrl = "~/Portal/Login";
            return LocalRedirect(returnUrl);
        }
        [AllowAnonymous]
        public IActionResult CourseContents()
        {
            ViewBag.Role = GetSecurityRoleCode;
            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");
            return View();
        }
        public IActionResult ManageUser()
        {
            return View();
        }
        public IActionResult CreateUser()
        {
            ViewBag.Role = GetSecurityRoleCode;
            ViewBag.UserId = 0;
            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");
            return View();
        }
        public IActionResult EditUser()
        {
            ViewData["Title"] = "Edit User";
            ViewBag.Feature = "Users Edit";
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
            return View("CreateUser");
        }

        public IActionResult MyLearners()
        {
            ViewBag.Role = GetSecurityRoleCode;
            return View();
        }
        public IActionResult AssignLearner()
        {
            ViewBag.Role = GetSecurityRoleCode;
            return View();
        }

        public IActionResult ManageMessages()
        {
            var SecurityRoleName = "";

            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                SecurityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = SecurityRoleName;
            return View();
        }
        public IActionResult CreateMessage()
        {
            var SecurityRoleName = "";

            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                SecurityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = SecurityRoleName;
            return View();
        }
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
        public IActionResult ManageResources()
        {
            var SecurityRoleName = "";

            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                SecurityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = SecurityRoleName;

            ViewBag.acceptDocumentTypes = _configurationRepository.GetSingleConfigValueByConfigKey("DocumentTypes");
            ViewBag.isMultipleDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MultipleDocumentUpload");
            ViewBag.maxDocumentUpload = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentUpload");
            ViewBag.maxDocumentFileSize = _configurationRepository.GetSingleConfigValueByConfigKey("MaxDocumentFileSize");
            return View();
        }
        public IActionResult UpdateEmployer()
        {
            var SecurityRoleName = "";
            if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
            {
                SecurityRoleName = HttpContext.Session.GetString("RoleCode");
            }
            ViewBag.Role = SecurityRoleName;
            return View();
        }
    }
}
