using ds.core.configuration;
using ds.core.logger;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using portal.models.RoleAdmin;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class RoleAdminApiController : ControllerBase
    {
        private readonly IRoleAdminRepository _roleAdminRepository;
        readonly ILogger<RoleAdminApiController> _log;
        LogException LogException;
        public RoleAdminApiController(IRoleAdminRepository roleAdminRepository
            , ILogger<RoleAdminApiController> log, IConfigurationRepository iConfig)
        {
            _roleAdminRepository = roleAdminRepository;
            _log = log;
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

        #endregion

        [HttpPost]
        [Route("InsertUpdateRole")]
        [HistoryFilter]
        public bool InsertUpdateRole(Role role)
        {
            try
            {
                return _roleAdminRepository.InsertUpdateRole(role);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            try
            {
                var roles = _roleAdminRepository.GetAllRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllRolesPortal")]
        public IActionResult GetAllRolesPortal()
        {
            try
            {
                var roles = _roleAdminRepository.GetAllRolesPortal();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("Get")]
        public DataSourceResult Get([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return _roleAdminRepository.GetAllRoles().ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetUsersInRole")]
        public DataSourceResult GetUsersInRole([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return _roleAdminRepository.GetListUsersInRoleForClient(UserId).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

      
        [HttpGet]
        [Route("DeletePerms")]
        [HistoryFilter]
        public IActionResult DeletePerms(int roleid, string deletedItems)
        {
            try
            {
                int CurrentRoleId = roleid;

                if (ModelState.IsValid)
                {
                    if (roleid > 0 && (deletedItems != null && deletedItems.Length > 0))
                    {
                        _roleAdminRepository.DeletePermissions(CurrentRoleId, deletedItems);
                    }

                    return Ok(true);

                }
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteRole")]
        [HistoryFilter]
        public IActionResult DeleteRole(int roleid)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (roleid > 0)
                    {
                        _roleAdminRepository.DeleteRole(roleid);
                    }

                    return Ok(true);

                }
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("InsertPerms")]
        [HistoryFilter]
        public IActionResult InsertPerms(int roleid, string insertedPerms)
        {
            try
            {
                int CurrentRoleId = roleid;

                if (ModelState.IsValid)
                {
                    if (roleid > 0 && (insertedPerms != null && insertedPerms.Length > 0))
                    {
                        _roleAdminRepository.InsertPermissions(CurrentRoleId, insertedPerms);
                    }

                    return Ok(true);
                }
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("SavePerms")]
        [HistoryFilter]
        public IActionResult SavePerms(int roleid, IEnumerable<String> checkedFeatures)
        {
            try
            {
                int CurrentRoleId = roleid;

                if (ModelState.IsValid)
                {
                    if (roleid > 0)
                    {
                        _roleAdminRepository.SavePermissions(CurrentRoleId, String.Join(",", checkedFeatures), UserId);
                    }
                    return Ok(true);
                }
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetRoleById")]
        public IActionResult GetRoleById(int id)
        {
            try
            {
                var role = _roleAdminRepository.GetRoleById(id);
                return Ok(role);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        #region "Check Mandatory Role Permissions"
        [HttpGet]
        [Route("CheckMandatoryRolePermissionsByFeatureId")]
        public IActionResult CheckMandatoryRolePermissionsByFeatureId(int Id)
        {
            try
            {
                bool restrictPermissionUncheck = false;
                restrictPermissionUncheck = _roleAdminRepository.CheckMandatoryRolePermissionByFeatureId(Id, UserId);
                return Ok(new { restrictPermissionUncheck = restrictPermissionUncheck });
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("CheckMandatoryRolePermission")]
        public IActionResult CheckMandatoryRolePermission(string Id)
        {
            try
            {
                bool restrictPermissionUncheck = false;
                restrictPermissionUncheck = _roleAdminRepository.CheckMandatoryRolePermission(Id, UserId);
                return Ok(new { restrictPermissionUncheck = restrictPermissionUncheck });
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        #endregion

    }
}