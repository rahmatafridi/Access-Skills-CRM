using Dapper;
using ds.core.configuration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using portal;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ds.portal.ui.Controllers.Api
{
    public class HistoryFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ipAddress = filterContext.HttpContext.Connection.RemoteIpAddress.ToString();
            var url = filterContext.HttpContext.Request.GetDisplayUrl();
            var userId = filterContext.HttpContext.Session.GetInt32("UserId") ?? 0;
            var roleId = filterContext.HttpContext.Session.GetInt32("RoleId") ?? 0;
            StringBuilder stringBuilder = new StringBuilder();
            var audit = string.Empty;
            if (filterContext.ActionArguments.Count > 1)
            {
                foreach (var item in filterContext.ActionArguments)
                {
                    stringBuilder.Append(item.Value);
                }
                 audit = JsonConvert.SerializeObject(stringBuilder.ToString());
            }
            else
            {
                var model = filterContext.ActionArguments.Select(m => m.Value).FirstOrDefault();
                audit = JsonConvert.SerializeObject(model);
            }

            var _connString = filterContext.HttpContext.Connection;

            //// Audit history.
            AuditHistory(ipAddress, url, userId, audit, roleId);
        }

        private void AuditHistory(string ipAddress, string url, int userId, string audit, int roleId)
        {
            HistoryModel historyModel  = new HistoryModel()
            {
                role_Id = roleId,
                date_time = DateTime.Now,
                ip_address = ipAddress.ToString(),
                user_Id = userId,
                Audit = audit,
                url = url
            };
           InsertHistoryConfiguration(historyModel);
        }

        private bool InsertHistoryConfiguration(HistoryModel auditModel)
        {
            //MYASP var _connString = "Data Source=SQL5031.site4now.net;Initial Catalog=DB_A12518_mmp247;Integrated Security=False;Persist Security Info=False;User ID=DB_A12518_mmp247_admin;Password=mmp247!##";
            //var _connString = "Data Source=crm.osca.hostedapps.mmsuk.net;Initial Catalog=crm;Integrated Security=False;Persist Security Info=False;User ID=mastercrm;Password=slcq8njyzawgkdoirefv";
            //var _connString = "Data Source=DEV3\\SQLSERVER;Initial Catalog=portalOne;Integrated Security=True;";
            //crm.osca.hostedapps.mmsuk.net

            return true;
            var _connString = "Data Source=127.0.0.1;Initial Catalog=crm;Integrated Security=False;Persist Security Info=False;User ID=mastercrm;Password=slcq8njyzawgkdoirefv";

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_History_Add]";
                    object dynamicParams = auditModel;
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
    }
}
