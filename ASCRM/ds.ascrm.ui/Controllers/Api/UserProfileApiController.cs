using ds.core.configuration;
using ds.core.logger;
using ds.portal.users;
using ds.portal.users.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/UserProfileApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserProfileApiController : ControllerBase
    {
        private readonly ILead_UserRepository _userRepository;
        private IConfigurationRepository configuration;
        private string _fromEmailId = string.Empty;
        readonly ILogger<UserProfileApiController> _log;
        LogException LogException;
        public UserProfileApiController(ILead_UserRepository userRepository,IConfigurationRepository iConfig, ILogger<UserProfileApiController> log)
        {
            _userRepository = userRepository;
            configuration = iConfig;
            _log = log;
            _fromEmailId = configuration.GetSingleConfigValueByConfigKey("FromEmail");
            LogException = new LogException(iConfig, _log);
        }
        #region Session Values

        private int UserId
        {
            get
            {
                var securityUserId = 0;

                if (HttpContext.Session.Keys.Any(a => a.Equals("UserId")))
                {
                    securityUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                }

                return securityUserId;
            }
            set {; }
        }
        private string UserName
        {
            get
            {
                var securityUserName = "";

                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("UserName")))
                {
                    securityUserName = HttpContext.Session.GetString("UserName");
                }

                return securityUserName;
            }
            set {; }
        }

        protected string GetSecurityRoleName
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

        #endregion

        [HttpPost]
        [Route("InsertUpdateUserRecord")]
        [HistoryFilter]
        public bool InsertUpdateUserRecord(Users user)
        {
            try
            {
                return _userRepository.InsertUpdateUserRecord(user);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetUserById")]
        public IActionResult GetUserById()
        {
            try
            {
                Users users = new Users();
                if (UserId > 0)
                {
                    users = _userRepository.GetUserById(UserId);
                    users.Users_RoleName = GetSecurityRoleName;
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("CheckEmailAlreadyExists")]
        public IActionResult CheckEmailAlreadyExists(Users user)
        {
            try
            {
                var isExists = _userRepository.CheckEmailAlreadyExists(user.Users_ID, user.Users_UserName);
                return Ok(isExists);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}