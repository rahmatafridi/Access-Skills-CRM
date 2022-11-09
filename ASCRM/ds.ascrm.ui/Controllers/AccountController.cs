using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.ui.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using portal.models.Account;
using portal.ui.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ds.portal.ui.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IMailService _emailSender;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<AccountController> _log;
        LogException LogException;
      //  private string _contact_us_url = string.Empty;
        private readonly IMemoryCache _cache;


        public AccountController(ILoginRepository loginRepository,
            IRoleAdminRepository roleAdminRepository,
            IMailService emailSender,
            IConfigurationRepository configurationRepository,
            IEmailTemplateRepository emailTemplateRepository,
            ILogger<AccountController> log,
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

        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel loginModel, string returnUrl)
        {
            SetContactUsUrlToViewBag();
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.ErrorMessage = null;
            returnUrl = returnUrl ?? Url.Content("~/Home/Index");

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
                    HttpContext.Session.SetString("DisplayName", user.Users_DisplayName);

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
                        if (user.RoleCode == "700")
                        {
                            returnUrl = Url.Content("~/Application/Index");
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

        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            SetContactUsUrlToViewBag();
            SetProductDetailToViewBag();


            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                if (_loginRepository.IsPasswordResetLinkValid(code))
                {
                    ViewBag.Code = code;
                    ViewBag.ErrorMessage = null;
                }
                else
                {
                    ViewBag.Code = "";
                    ViewBag.ErrorMessage = "Your link is expired please apply for new link.";
                }
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel, string returnUrl)
        {
            SetProductDetailToViewBag();

            if (!ModelState.IsValid)
            {
                ViewBag.Code = resetPasswordModel.Code;
                return View();
            }

            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.ErrorMessage = null;
            returnUrl = returnUrl ?? Url.Content("~/Account/Login");

             var _userEmail = _loginRepository.GetUserEmail(resetPasswordModel.Code);
            _loginRepository.ChangePassword(resetPasswordModel.Code, resetPasswordModel.Password);
          
            var _encodeUserEmail = _userEmail.Split('@');
            var _emailId = string.Empty;
            if (_encodeUserEmail != null && _encodeUserEmail.Length > 0)
            {
                _emailId = _encodeUserEmail[0].Substring(0, 2) + "******@" + _encodeUserEmail[1];
            }
            string callbackUrl = Url.Link("Default", new { controller = "Account", action = "ResetPassword", code = HttpUtility.UrlEncode(resetPasswordModel.Code) });
            var emailTemplate = _emailTemplateRepository.GetEmailTemplateByCode("PW_CHANGED");
            if (emailTemplate != null)
            {
                if (!string.IsNullOrEmpty(emailTemplate.ET_Body))
                {
                    string url_template = emailTemplate.ET_Body.Replace("{RESET_URL}", callbackUrl).Replace("{UserEmail}", _emailId);
                    _emailSender.SendEmail(_userEmail, "Your password has been changed", url_template);
                }
            }

            return LocalRedirect(returnUrl);
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            SetContactUsUrlToViewBag();
            SetProductDetailToViewBag();

            ViewBag.ErrorMessage = null;
            HttpContext.Session.Clear();
             
            string returnUrl = "~/Account/Login";
            return LocalRedirect(returnUrl);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            ViewBag.Feature = "Permission";
            ViewBag.Page = "Access Denied";
            SetContactUsUrlToViewBag();
            SetProductDetailToViewBag();

            return View();
        }

        [Route("{*url}", Order = 999)]
        public IActionResult CatchAll()
        {
            Response.StatusCode = 404;
            if (GetSecurityUserId == 0)
            {
                return RedirectToAction("Login");
            }
            else
            {
                LogException.Log("Page not found response code " + Response.StatusCode, this.ControllerContext);
                return View("PageNotFound");
            }
        }
    }
}