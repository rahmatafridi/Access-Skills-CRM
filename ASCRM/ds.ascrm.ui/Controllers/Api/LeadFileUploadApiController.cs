using ds.core.configuration;
using ds.core.document;
using ds.core.logger;
using ds.core.task;
using ds.core.task.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/LeadFileUploadApi")]
    [ApiController]
    public class LeadFileUploadApiController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        readonly ILogger<DocumentApiController> _log;
        LogException LogException;
        public LeadFileUploadApiController(ITaskRepository taskRepository, IHostingEnvironment hostingEnvironment
            , ILogger<DocumentApiController> log, IConfigurationRepository iConfig
            , IDocument document)
        {
            _taskRepository = taskRepository;
            _hostingEnvironment = hostingEnvironment;
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

        public ActionResult Submit(IEnumerable<IFormFile> files)
        {
            IEnumerable<string> fileInfo = new List<string>();

            if (files != null)
            {
                fileInfo = GetFileInfo(files);
            }

            return Ok(true);
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<IFormFile> files)
        {
            List<string> fileInfo = new List<string>();

            foreach (var file in files)
            {
                var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));

                fileInfo.Add(string.Format("{0} ({1} bytes)", fileName, file.Length));
            }

            return fileInfo;
        }
    }
}