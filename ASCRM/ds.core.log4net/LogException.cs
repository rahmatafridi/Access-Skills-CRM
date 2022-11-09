using Dapper;
using ds.core.configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace ds.core.logger
{
    public class LogException
    {
        private string _isLogger = string.Empty;
        readonly ILogger<dynamic> _log;
        public IConfigurationRepository configuration { get; }
        public LogException(IConfigurationRepository iConfig, ILogger<dynamic> log)
        {
            configuration = iConfig;
            _log = log;
        }
        public void Log(Exception ex, ControllerContext controllerContext )
        {
            var _isLog = GetSingleConfigValueByConfigKey("IsLogger");
            if (_isLog == "1")
            {
                _log.LogInformation
                    (
                       "Controller: " + controllerContext.RouteData.Values["controller"].ToString() + ",  "
                       + "Action Method: " + controllerContext.RouteData.Values["action"].ToString() + ",  "
                       + "Message: " + ex.Message + ",  "
                       + "Error: " + ex.ToString()
                    );
            }
        }

        public void Log(string Message, ControllerContext controllerContext)
        {
            var _isLog = GetSingleConfigValueByConfigKey("IsLogger");
            if (_isLog == "1")
            {
                _log.LogInformation
                    (
                        "Controller: " + controllerContext.RouteData.Values["controller"].ToString() + ",  "
                       + "Action Method: " + controllerContext.RouteData.Values["action"].ToString() + ",  "
                       + "Message: " + Message
                    );
            }
        }

        private string GetSingleConfigValueByConfigKey(string config_key)
        {
            string value = "";
            var _conn = configuration.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(_conn))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Core_Configuration_GetSingleValueByKey]";
                    object dynamicParams = new { config_key = config_key };
                    value = conn.QuerySingleOrDefault<string>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return value;
        }
    }
}
