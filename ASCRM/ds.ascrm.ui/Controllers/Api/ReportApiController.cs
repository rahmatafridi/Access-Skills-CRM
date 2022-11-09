using ds.core.configuration;
using ds.core.logger;
using ds.portal.report;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/ReportApi")]
    [ApiController]
    public class ReportApiController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        readonly ILogger<ReportApiController> _log;
        LogException LogException;
        public ReportApiController(IReportRepository reportRepository, IHostingEnvironment hostingEnvironment
            , ILogger<ReportApiController> log, IConfigurationRepository iConfig)
        {
            _reportRepository = reportRepository;
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
        [Route("GetContacts")]
        public IActionResult GetContacts(string startDate, string endDate)
        {
            int currentUserId = UserId;
            CultureInfo ukCulture = new CultureInfo("en-GB");
            try
            {
                DateTime? dtS = null;
                DateTime? dtE = null;
                if (string.IsNullOrEmpty(startDate))
                {
                    var contactreport = _reportRepository.GetContacts(currentUserId, dtS, dtE);
                    return Ok(contactreport);
                }
                else
                {
                    var contactreport = _reportRepository.GetContacts(currentUserId, DateTime.Parse(startDate, ukCulture.DateTimeFormat), DateTime.Parse(endDate, ukCulture.DateTimeFormat));
                    return Ok(contactreport);
                }
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}