using ds.core.configuration;
using ds.portal.ui.Models;
using Microsoft.AspNetCore.Mvc;
using portal.data.repository.Interfaces;
using portal.models.RoleAdmin;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class RoleAdminController : BaseController
    {
        readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMemoryCache _cache;

        public RoleAdminController(IRoleAdminRepository roleAdminRepository, IRoleAdminRepository roleAdminRep,
            IConfigurationRepository configurationRepository, IMemoryCache memoryCache)
            : base(roleAdminRep, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _configurationRepository = configurationRepository;
            _roleAdminRepository = roleAdminRepository;
        }
        public IActionResult Permissions(string id)
        {
            ViewBag.Feature = "Role Management";
            ViewBag.Page = "Manage Role Permissions";
            SetProductDetailToViewBag();

            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Error", "Error");
            }
            var roleId = 0;

            var decodeRoleId = Encryption.Decrypt(id);
            if (decodeRoleId == "")
            {
                return RedirectToAction("Error", "Error");
            }
            roleId = Convert.ToInt32(decodeRoleId);

            if (roleId > 0)
            {
                var featurepermissions = _roleAdminRepository.GetListFeaturesForRole(roleId, GetSecurityUserId);
                ViewBag.CurrentRoleName = _roleAdminRepository.GetRoleById(roleId).name;
                ViewBag.Featurepermissions = featurepermissions;
                ViewBag.Id = id;
                return View(new RoleFPModel { Id = roleId, features = featurepermissions.features });
            }
            return new StatusCodeResult(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
        }

        public IActionResult CreateRole()
        {
            ViewData["Title"] = "Create Role";
            ViewBag.Feature = "Role Management";
            ViewBag.Page = "Create Role";
            ViewBag.RoleId = 0;

            SetProductDetailToViewBag();

            return View();
        }
        public IActionResult EditRole()
        {
            ViewData["Title"] = "Edit Role";
            ViewBag.Feature = "Role Management";
            ViewBag.Page = "Edit Role";
            SetProductDetailToViewBag();

            string roleId = "0";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
            {
                roleId = HttpContext.Request.Query["id"];
                var id = Encryption.Decrypt(roleId);
                if (id == "")
                {
                    return RedirectToAction("Error", "Error");
                }
                roleId = id;
            }

            ViewBag.RoleId = roleId;

            return View("CreateRole");
        }
        public IActionResult Index()
        {
            ViewBag.Feature = "Role Management";
            ViewBag.Page = "Manage Roles";
            SetProductDetailToViewBag();

            return View();
        }
		public IActionResult TestLead()
        {
            return View();
        }
        
    }
}