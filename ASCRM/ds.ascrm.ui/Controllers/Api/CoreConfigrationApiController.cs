using ds.core.configuration;
using ds.core.configuration.Models;
using ds.core.logger;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/CoreConfigurationApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CoreConfigurationApiController : ControllerBase
    {
        private readonly IConfigurationRepository _coreConfigurationRepository;
        readonly ILogger<CoreConfigurationApiController> _log;
        LogException LogException;
        public CoreConfigurationApiController(IConfigurationRepository mdCore_ConfigurationRepository
            , ILogger<CoreConfigurationApiController> log)
        {
            _coreConfigurationRepository = mdCore_ConfigurationRepository;
            _log = log;
            LogException = new LogException(_coreConfigurationRepository, _log);
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
        [Route("InsertUpdateConfigration")]
        [HistoryFilter]
        public bool InsertUpdateConfigration(ConfigurationModel configuration)
        {
            try
            {
                return _coreConfigurationRepository.InsertUpdateConfigration(configuration);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllConfigrations")]
        public IActionResult GetAllConfigrations()
        {
            try
            {
                var configrationsList = _coreConfigurationRepository.GetAllConfigration();
                return Ok(configrationsList);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetSingleConfigValueByConfigKey")]
        public IActionResult GetSingleConfigValueByConfigKey(string config_key)
        {
            try
            {
                var config_Value = _coreConfigurationRepository.GetSingleConfigValueByConfigKey(config_key);
                return Ok(config_Value);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("CheckKeyAndValueExists")]
        [HistoryFilter]
        public Tuple<bool, bool> CheckKeyAndValueExists(ConfigurationModel configuration)
        {
            try
            {
                return _coreConfigurationRepository.CheckKeyAndValueExists(configuration);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteConfigrationById")]
        [HistoryFilter]
        public bool DeleteConfigrationById(int id)
        {
            try
            {
                return _coreConfigurationRepository.DeleteConfigrationById(id, UserId, UserName);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetSessionOutTime")]
       // [HistoryFilter]
        public int GetSessionOutTime()
        {
            try
            {
                var _timeOut = _coreConfigurationRepository.GetSingleConfigValueByConfigKey("SessionExpired");
                return (Convert.ToInt32(_timeOut) * 1000);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}