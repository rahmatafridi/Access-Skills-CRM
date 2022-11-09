using crm.portal.Interfaces.WorkUpload;
using ds.core.configuration;
using ds.core.logger;
using ds.portal.dashboard;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using System;
using System.Linq;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/DashboardApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]

    public class DashboardApiController : ControllerBase
    {
        IDashboardRepository _dashboardRepository;
        readonly ILogger<DashboardApiController> _log;
        LogException LogException;
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IWorkUploadRepository _workUploadRepository;
        public DashboardApiController(IWorkUploadRepository workUploadRepository,IDashboardRepository dashboardRep
            , ILogger<DashboardApiController> log, IConfigurationRepository iConfig, IRoleAdminRepository roleAdminRepository)
        {
            _dashboardRepository = dashboardRep;
            _log = log;
            LogException = new LogException(iConfig, _log);
            _roleAdminRepository = roleAdminRepository;
            _workUploadRepository = workUploadRepository;
           
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

        private string GetSecurityRoleName
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
        [Route("GetAllLeadsForDashboard")]
        public IActionResult GetAllLeadsForDashboard()
        {
            try
            {
               var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var leads = _dashboardRepository.GetAllLeadsForDashboard(UserId, _isAdmin);
                return Ok(leads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetMyLeadsForDashboard")]
        public DataSourceResult GetMyLeadsForDashboard([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return _dashboardRepository.GetMyLeadsForDashboard(UserId).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetMyActivitiesForDashboard")]
        public DataSourceResult GetMyActivitiesForDashboard([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return _dashboardRepository.GetMyActivitiesForDashboard(UserId).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetLastSevenDaysLead")]
        public IActionResult GetLastSevenDaysLead()
        {
            try
            {
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var leads = _dashboardRepository.GetLastSevenDaysLead(UserId, _isAdmin);
                return Ok(leads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetLeadSalesAdvisor")]
        public IActionResult GetLeadSalesAdvisor()
        {
            try
            {
                var leads = _dashboardRepository.GetLeadSalesAdvisor();
                return Ok(leads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetSearchResults")]
        public IActionResult GetSearchResults(string searchQuery)
        {
            try
            {
                bool _isAdmin = false;
                if (GetSecurityRoleName.ToLower() == "admin" || GetSecurityRoleName.ToLower() == "administrator")
                { _isAdmin = true; }

                var leads = _dashboardRepository.GetSearchResults(searchQuery, UserId, _isAdmin, UserName);

                return Ok(leads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetLastResults")]
        public DataSourceResult GetLastResults([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                bool _isAdmin = false;
                if (GetSecurityRoleName.ToLower() == "admin" || GetSecurityRoleName.ToLower() == "administrator")
                { _isAdmin = true; }

                return _dashboardRepository.GetLastResults(UserId, _isAdmin).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAgreedPaymentSchemes")]
        public DataSourceResult GetAgreedPaymentSchemes([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                bool _isAdmin = false;
                if (GetSecurityRoleName.ToLower() == "admin" || GetSecurityRoleName.ToLower() == "administrator")
                { _isAdmin = true; }

                return _dashboardRepository.GetAgreedPaymentSchemes(UserId, _isAdmin).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetContactAttempts")]
        public IActionResult GetContactAttempts(string lastResult)
        {
            try
            {
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var leads = _dashboardRepository.GetContactAttempts(UserId, _isAdmin, lastResult);
                return Ok(leads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


    }
}