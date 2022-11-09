using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using portal.models.Account;
using portal.ui.Models;
using Microsoft.Extensions.Caching.Memory;
using Rotativa;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ds.portal.ui.Controllers
{
    public class ApplicationController : BaseController
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IMailService _emailSender;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<AccountController> _log;
        LogException LogException;
       // private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;

        public ApplicationController(ILoginRepository loginRepository, IRoleAdminRepository roleAdminRepository
            , IMailService emailSender, IConfigurationRepository configurationRepository
            , ILogger<AccountController> log
            , IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _loginRepository = loginRepository;
            _roleAdminRepository = roleAdminRepository;
            _emailSender = emailSender;
            _configurationRepository = configurationRepository;
            _log = log;
            LogException = new LogException(_configurationRepository, _log);
           // _contact_us_url = _configurationRepository.GetSingleConfigValueByConfigKey("URL_Contactus");
        }
        public IActionResult Login()
        {
            ViewBag.UserName = GetSecurityUserName;
            ViewBag.ErrorMessage = null;
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel loginModel, string returnUrl)
        {
            SetContactUsUrlToViewBag();
            ViewBag.ErrorMessage = null;
            returnUrl = returnUrl ?? Url.Content("~/Application/Index");
            ViewData["ReturnUrl"] = returnUrl;
           
            SetProductDetailToViewBag(); 

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


                        if (user.RoleCode != "700" && user.RoleCode != "900")
                        {
                            returnUrl = Url.Content("~/Home/Index");
                        }
                       
                        if (user.RoleCode == "900" || user.RoleCode == "1000" || user.RoleCode == "1100" || user.RoleCode == "1200" || user.RoleCode == "1300")
                        {
                            returnUrl = Url.Content("~/Portal/Index");
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
        public IActionResult Index()
        {
            ViewBag.Feature = "Dashboard";
            ViewBag.Page = "Dashboard";
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            return View();
        }

        public IActionResult Progress()
        {
            ViewBag.Feature = "Progress";
            ViewBag.Page = "Progress";
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            string app_id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                app_id = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(app_id);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                app_id = id;
            }

            ViewBag.App_Id = app_id;
            return View();
        }
        public IActionResult ManageApplication()
        {
            ViewBag.Feature = "Manage Application";
            ViewBag.Page = "Manage Application";
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            return View();
        }
        public IActionResult ProgressAdmin()
        {
            ViewBag.Feature = "Admin Progress";
            ViewBag.Page = "Admin Progress";
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            string app_id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                app_id = HttpContext.Request.Query["id"]; 
                var id = Encryption.Decrypt(app_id);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                app_id = id;
            }

            ViewBag.App_Id = app_id;
            return View();
        }
        public IActionResult AppPreview()
        {
            string app_id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                app_id = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(app_id);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                app_id = id;
            }

            ViewBag.App_Id = app_id;
            return View();
        }
        public IActionResult Print()
        {
            ViewBag.Feature = "Print";
            ViewBag.Page = "Print";
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            string app_id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                app_id = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(app_id);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                app_id = id;
            }

            ViewBag.App_Id = app_id;
            return View();
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            ViewBag.ErrorMessage = null;
            HttpContext.Session.Clear();
            string returnUrl = "~/Application/Login";
            return LocalRedirect(returnUrl);
        }

        public IActionResult Options()
        {
            return View();
        }
        public IActionResult CourseLevel()
        {
            ViewBag.Feature = "Course Level";
            ViewBag.Page = "Course Level";
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();

            string app_id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                app_id = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(app_id);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                app_id = id;
            }

            ViewBag.App_Id = app_id;
            return View();
        }

        public IActionResult AddCourseLevel()
        {
            return View();
        }
        public IActionResult EditCourse()
        {
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
            ViewBag.Feature = "Course Level";
            ViewBag.Page = "Update";
            ViewData["Title"] = "Update Course Level";


            string level_Id = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                level_Id = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(level_Id);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                level_Id = id;
            }
            ViewBag.Owner = (isOwnerOfLead(GetSecurityUserId, int.Parse(level_Id)) == true ? "1" : "0");
            ViewBag.Level_Id = level_Id;

            return View("AddCourseLevel");
        }
        public IActionResult Profile()
        {
            return View();
        }
        //public ActionResult DownlaodPdf(string id)
        //{
        //    return new ActionAsPdf()
        //}
    }
}