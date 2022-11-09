using crm.portal.Interfaces.MyDocument;
using ds.core.configuration;
using ds.core.logger;
using ds.portal.ui.Controllers.Api;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalMyDocumentApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PortalMyDocumentApiController : ControllerBase
    {
        private readonly IMyDocumentRepository _myDocumentRepository;
        readonly ILogger<PortalMyDocumentApiController> _log;
        LogException LogException;
        public PortalMyDocumentApiController(ILogger<PortalMyDocumentApiController> log, IConfigurationRepository iConfig, IMyDocumentRepository myDocumentRepository)
        {
            _myDocumentRepository = myDocumentRepository;
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

        [HttpGet]
        [Route("GetMyDocument")]
        public DataSourceResult GetMyDocument([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                return _myDocumentRepository.LoadLearnerDoc(Convert.ToInt32(UserName)).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("DownloadFile")]
        [HistoryFilter]
        public IActionResult DownloadFile(string path)
        {
            try
            {

                return PhysicalFile(path, MimeTypes.GetMimeType(path), Path.GetFileName(path));
           
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}
