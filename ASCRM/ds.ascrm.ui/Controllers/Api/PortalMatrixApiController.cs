using crm.portal.Interfaces.Matrix;
using crm.portal.Interfaces.User;
using ds.core.configuration;
using ds.core.logger;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalMatrixApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PortalMatrixApiController : ControllerBase
    {
        private readonly IMatrixRepository _matrixRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IPortalUserRepository _portalUserRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        readonly ILogger<PortalMatrixApiController> _log;
        LogException LogException;
        public PortalMatrixApiController(ILogger<PortalMatrixApiController> log, IConfigurationRepository iConfig, IHostingEnvironment hostingEnvironment,IPortalUserRepository portalUserRepository, IConfigurationRepository configurationRepository, IMatrixRepository matrixRepository)
        {
            _matrixRepository = matrixRepository;
            _configurationRepository = configurationRepository;
            _portalUserRepository = portalUserRepository;
            _hostingEnvironment = hostingEnvironment;
            _log = log;
            LogException = new LogException(iConfig, _log);
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
        [Route("GetMatrixLable")]
        public IActionResult GetMatrixLable()
        {
            try
            {
                return Ok(_matrixRepository.LoadMatrixLables(Convert.ToInt32(UserName)));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetMatrixLableForAccount")]
        public IActionResult GetMatrixLableForAccount(int id)
        {
            try
            {
                return Ok(_matrixRepository.LoadMatrixLables(id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetMatrix")]
        public IActionResult GetMatrix(int matrixId)
        {
            try
            {
                return Ok(_matrixRepository.LoadMatrix(Convert.ToInt32(UserName),matrixId));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetMatrixForAccount")]
        public IActionResult GetMatrixForAccount(int matrixId,int id)
        {
            try
            {
                return Ok(_matrixRepository.LoadMatrix(id,matrixId));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetMatrixSummary")]
        public IActionResult GetMatrixSummary(int matrixId)
        {
            try
            {
                return Ok(_matrixRepository.LoadMatrixSummary(Convert.ToInt32(UserName), matrixId));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }  
        [HttpGet]
        [Route("GetMatrixSummaryForAccount")]
        public IActionResult GetMatrixSummaryForAccount(int matrixId,int id)
        {
            try
            {
                return Ok(_matrixRepository.LoadMatrixSummary(id, matrixId));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }



        [HttpGet]
        [Route("GetMatrixFullSummary")]
        public IActionResult GetMatrixFullSummary(int matrixId,int topicid)
        {
            try
            {
                return Ok(_matrixRepository.LoadMatrixFullSummary(topicid, Convert.ToInt32(UserName), matrixId));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetMatrixFullSummaryForAccount")]
        public IActionResult GetMatrixFullSummaryForAccount(int matrixId,int topicid,int id)
        {
            try
            {
                return Ok(_matrixRepository.LoadMatrixFullSummary(topicid, id, matrixId));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
       
    }
}
