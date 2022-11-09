using Dapper;
using ds.core.configuration;
using ds.core.configuration.Models;
using ds.portal.ui.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using portal.data.repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ds.portal.ui.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        private readonly string _connString = "";
        private readonly IRoleAdminRepository _roleAdminRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IMemoryCache _cache;

        public BaseController(
            IRoleAdminRepository roleAdminRepository,
            IConfigurationRepository configurationRepository,
            IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            _connString = configurationRepository.GetConnectionString();
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
          //  ViewBag.ContactUsURL = _configurationRepository.GetSingleConfigValueByConfigKey("URL_Contactus");
            SetProductDetailToViewBag();
            SetContactUsUrlToViewBag();
        }

        protected void SetProductDetailToViewBag()
        {  
            string product_detail = null;
            if (_cache.TryGetValue("ProductDetail", out product_detail))
            { 
                ViewBag.ProductDetail = product_detail;
            }
            else
            { 
                var product = _configurationRepository.GetProduct();
                var expiry = _configurationRepository.GetSingleConfigValueByConfigKey("cache_timeout_min");
                int expiry_min = (expiry != null ? int.Parse(expiry) : 1);

                if (product != null)
                {
                    string productDetail = string.Format("{0} ({1}.{2}.{3})", product.ProductName, product.Release, product.Build, product.Patch);

                    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                    options.AbsoluteExpiration = DateTime.Now.AddMinutes(expiry_min);
                    options.SlidingExpiration = TimeSpan.FromMinutes(expiry_min);
                    _cache.Set<string>("ProductDetail", productDetail, options);

                   // _cache.Set("ProductDetail", productDetail);
                    ViewBag.ProductDetail = productDetail;
                }                
            }
        }
        protected void SetContactUsUrlToViewBag()
        {
            string contact_url = null;
            if (_cache.TryGetValue("ContactUsURL", out contact_url))
            {
                ViewBag.ContactUsURL = contact_url;
            }
            else
            {
                var contact_us_url = _configurationRepository.GetSingleConfigValueByConfigKey("URL_Contactus");
                var expiry = _configurationRepository.GetSingleConfigValueByConfigKey("cache_timeout_min");
                int expiry_min = (expiry != null ? int.Parse(expiry) : 1);

                if (contact_us_url != null)
                {
                    MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                    options.AbsoluteExpiration = DateTime.Now.AddMinutes(expiry_min);
                    options.SlidingExpiration = TimeSpan.FromMinutes(expiry_min);
                    _cache.Set<string>("ContactUsURL", contact_us_url, options);

                   // _cache.Set("ContactUsURL", contact_us_url);
                    ViewBag.ContactUsURL = contact_us_url;
                }
            }
        }

        protected bool isOwnerOfLead(int user_id, int lead_id)
        {

            return _roleAdminRepository.IsOwnerLead(user_id, lead_id);
        }

        protected int GetSecurityUserId
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
        protected int GetSecurityRoleId
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
        protected string GetSecurityUserName
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

        protected string GetSecurityDisplayName
        {
            get
            {
                var securityDisplayName = "";

                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("UserName")))
                {
                    securityDisplayName = HttpContext.Session.GetString("DisplayName");
                }

                return securityDisplayName;
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
        protected string GetSecurityRoleCode
        {
            get
            {
                var SecurityRoleName = "";

                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
                {
                    SecurityRoleName = HttpContext.Session.GetString("RoleCode");
                }

                return SecurityRoleName;
            }
            set {; }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext != null)
            {
                int paraValue = 0;
                var actionMethod = filterContext.RouteData.Values["action"].ToString();
                var controllerName = filterContext.RouteData.Values["controller"].ToString();

                //// Audit history.
                if (GetSecurityUserId > 0)
                {
                    AuditHistory(actionMethod, controllerName);
                }

                List<AccessPermissionModel> accessPermission = new List<AccessPermissionModel>();
                using (SqlConnection conn = new SqlConnection(_connString))
                {
                    try
                    {
                        if (HttpContext.Session.GetString("Controls") == null)
                        {
                            accessPermission = conn.Query<AccessPermissionModel>("UP_Web_Permissions_GetControllerActions",
                                new
                                {
                                }).ToList();

                            HttpContext.Session.SetObjectAsJson("Controls", accessPermission);
                        }
                        else
                        {
                            accessPermission = HttpContext.Session.GetObjectFromJson<List<AccessPermissionModel>>("Controls");
                        }

                        var isExist = accessPermission.Where(x => x.controller_name.ToLower() == controllerName.ToLower() && x.action_name.ToLower() == actionMethod.ToLower()).Count();

                        if (!String.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                        {
                            var lead_Id = HttpContext.Request.Query["id"];
                            var id = Encryption.Decrypt(lead_Id);
                            if (id == "")
                            {
                                filterContext.Result = new RedirectToRouteResult
                                  (
                          new RouteValueDictionary
                          {
{"controller", "Error"},
{"action", "Error"},
{"area",""}
                          }
                                 );
                            }
                            else
                            {
                                paraValue = Convert.ToInt32(id);
                            }
                        }

                        if (isExist > 0)
                        {
                            bool hasAccess = false;

                            if (paraValue > -1)
                            {
                                using (SqlConnection con = new SqlConnection(_connString))
                                {
                                    object dynamicParams = new
                                    {
                                        @action_method = actionMethod,
                                        @controller_name = controllerName,
                                        @Id = paraValue,
                                        @security_user_id = GetSecurityUserId
                                    };

                                    hasAccess = con.ExecuteScalar<bool>("UP_Web_Permission_CheckAccessPermission", param: dynamicParams, commandType: CommandType.StoredProcedure);
                                }

                            }
                            if (!hasAccess)
                            {
                                filterContext.Result = new RedirectToRouteResult
                                (
                        new RouteValueDictionary
                        {
{"controller", "Error"},
{"action", "PermissionDenied"},
{"area",""}
                        }
                               );
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

                if (string.IsNullOrEmpty(GetSecurityUserName) && (controllerName != "Account" && (actionMethod != "Login" && actionMethod != "Logout")) && (controllerName != "Home" && actionMethod != "Index"))
                {
                    filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary { { "controller", "Account" }, { "action", "Login" } });
                    return;
                }

                if (filterContext.Controller is Controller controller)
                {
                    controller.ViewBag.UserName = GetSecurityUserName;
                    controller.ViewBag.DisplayName = GetSecurityDisplayName;
                    controller.ViewBag.UserId = GetSecurityUserId;
                    controller.ViewBag.RoleName = GetSecurityRoleName;
                    var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                    controller.ViewBag.Permissions = permissions;
                }
            }
        }
        private void AuditHistory(string actionMethod, string controllerName)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress;
            var url = Request.GetDisplayUrl();
            AuditModel auditModel = new AuditModel()
            {
                action_method = actionMethod,
                action_url = url,
                date_time = DateTime.Now,
                ip_address = ipAddress.ToString(),
                user_Id = GetSecurityUserId
            };
            _configurationRepository.InsertAuditConfiguration(auditModel);
        }
    }
}

public static class SessionExtensions
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}