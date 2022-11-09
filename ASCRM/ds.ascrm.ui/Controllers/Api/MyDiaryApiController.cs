using ds.core.configuration;
using ds.core.logger;
using ds.portal.diary;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using System;
using System.Linq;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/DiaryApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class MyDiaryApiController : ControllerBase
    {
        private readonly IDiaryRepository _diaryRepository;
        readonly ILogger<MyDiaryApiController> _log;
        LogException LogException;
        private readonly IRoleAdminRepository _roleAdminRepository;
        public MyDiaryApiController(IDiaryRepository diaryRepository
            , ILogger<MyDiaryApiController> log, IConfigurationRepository iConfig, IRoleAdminRepository roleAdminRepository)
        {
            _diaryRepository = diaryRepository;
            _log = log;
            LogException = new LogException(iConfig, _log);
            _roleAdminRepository = roleAdminRepository;
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

        private int GetSecurityRoleId
        {
            get
            {
                var securityRoleId = 0;

                if (HttpContext.Session.Keys.Any(a => a.Equals("RoleId")))
                {
                    securityRoleId = HttpContext.Session.GetInt32("RoleId") ?? 0;
                }

                return securityRoleId;
            }
            set {; }
        }
        #endregion

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var diaryModels = _diaryRepository.GetAll(UserId, _isAdmin);
                return Ok(diaryModels);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}