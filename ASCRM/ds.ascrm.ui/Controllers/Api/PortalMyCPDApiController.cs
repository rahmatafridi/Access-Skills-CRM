using crm.portal.Interfaces.MyCPD;
using crm.portal.OscaModel;
using ds.core.configuration;
using ds.core.logger;
using ds.portal.ui.Controllers.Api;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalMyCPDApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PortalMyCPDApiController : ControllerBase
    {

        private readonly IMyCPDRepository _myCPDRepository;
        readonly ILogger<PortalMyCPDApiController> _log;
        LogException LogException;
        public PortalMyCPDApiController(ILogger<PortalMyCPDApiController> log, IConfigurationRepository iConfig, IMyCPDRepository myCPDRepository)
        {
            _log = log;
            LogException = new LogException(iConfig, _log);
            _myCPDRepository = myCPDRepository;
        }

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

        [HttpGet]
        [Route("GetReviewStats")]
        public IActionResult GetReviewStats()
        {
            try
            {
                return Ok(_myCPDRepository.CPDGetPrimeReviewStats(Convert.ToInt32(UserName)));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAdditinalActivities")]
        public DataSourceResult GetAdditinalActivities([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                return _myCPDRepository.GetAdditinalActivities(Convert.ToInt32(UserName)).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCompletedActivities")]
        public DataSourceResult GetCompletedActivities([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var data = _myCPDRepository.GetCompletedActivities(Convert.ToInt32(UserName));
                return data.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("CompletedActivitiesList")]
        public IActionResult CompletedActivitiesList()
        {
            try
            {
                var data = _myCPDRepository.GetCompletedActivities(Convert.ToInt32(UserName));
                return Ok(data);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("AdditionalActivitiesList")]
        public IActionResult AdditionalActivitiesList()
        {
            try
            {
                var data = _myCPDRepository.GetAdditinalActivities(Convert.ToInt32(UserName));
                return Ok(data);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpPost]
        [Route("UpdateCompletedActivity")]
        [HistoryFilter]
        public bool UpdateCompletedActivity(CompletedActivities act)
        {
            try
            {

                return _myCPDRepository.UpdateCompletedActivity(act);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }



        [HttpPost]
        [Route("UpdateAdditinalActivity")]
        [HistoryFilter]
        public bool UpdateAdditinalActivity(AdditinalActivities act)
        {
            try
            {

                return _myCPDRepository.UpdateAdditinalActivity(act,Convert.ToInt32(UserName));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}
