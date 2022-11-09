using crm.portal.CrmModel;
using crm.portal.Interfaces.CourseContent;
using ds.core.configuration;
using ds.core.document;
using ds.core.logger;
using ds.portal.applications;
using ds.portal.ui.Controllers.Api;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nancy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalCourseContentApi")]
    [ApiController]
 
    [EnableCors("AllowOrigin")]
    public class PortalCourseContentApiController : ControllerBase
    {

        private readonly IApplicationRepository _applicationRepository;
        private readonly ICourseContentRepository _courseContentRepository;
        private readonly IDocument _document;
        private readonly IConfigurationRepository _configurationRepository;
        readonly ILogger<PortalCourseContentApiController> _log;
        LogException LogException;
        public PortalCourseContentApiController(ILogger<PortalCourseContentApiController> log, IConfigurationRepository iConfig, IConfigurationRepository configurationRepository, IDocument document, IApplicationRepository applicationRepo, ICourseContentRepository courseContentRepository)
        {
            _applicationRepository = applicationRepo;
            _courseContentRepository = courseContentRepository;
            _document = document;
            _configurationRepository = configurationRepository;
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
        private string RoleName
        {
            get
            {
                var securityUserName = "";

                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleName")))
                {
                    securityUserName = HttpContext.Session.GetString("RoleName");
                }

                return securityUserName;
            }
            set {; }
        }
        private string RoleCode
        {
            get
            {
                var securityUserName = "";

                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
                {
                    securityUserName = HttpContext.Session.GetString("RoleCode");
                }

                return securityUserName;
            }
            set {; }
        }

        private int LearnerContactsId
        {
            get
            {
                var securityUserId = 0;

                if (HttpContext.Session.Keys.Any(a => a.Equals("Users_Id_LearnerContacts")))
                {
                    securityUserId = HttpContext.Session.GetInt32("Users_Id_LearnerContacts") ?? 0;
                }

               
                return securityUserId;
            }
            set {; }
        }


        #endregion

        [HttpGet]
        [Route("GetCourseContentLearner")]
        public DataSourceResult GetCourseContentLearner([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                 return _courseContentRepository.GetCourseContents(Convert.ToInt32(UserName), 1, false).ToDataSourceResult(request);

               
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCourseContentGrid")]
        public DataSourceResult GetCourseContentGrid([DataSourceRequest] DataSourceRequest request,int id)
        {
            try
            {
                if (RoleCode == "900")
                {
                    return _courseContentRepository.GetCourseContents(Convert.ToInt32(UserName), id, false).ToDataSourceResult(request);

                }
                else
                {
                    return _courseContentRepository.GetCourseContents(1, id, true).ToDataSourceResult(request);
                }
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCourseContents")]

        public ActionResult GetCourseContents( int id)
        {
            try
            {

                if (RoleCode == "900")
                {
                    return Ok(_courseContentRepository.GetCourseContents(Convert.ToInt32(UserName), id, false));

                }
                else
                {
                    return Ok(_courseContentRepository.GetCourseContents(1, id, true));
                }
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetCourses")]
        public IActionResult GetCourses()
        {
            try
            {
                string type = "1";
                return Ok(_courseContentRepository.LoadCourses());
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("DownloadFile")]
        [HistoryFilter]
        public IActionResult DownloadFile(int Id)
        {
            try
            {
                byte[] bytes;

                var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("CourseContent");
            
             

                var data = _courseContentRepository.GetCourseContentById(Id);
                var newpath = dbPath + data.CC_FilePath;
                string path = _document.DownlaodFileFromPath(newpath);
                //var path = _document.DownlaodFileFromPath(data.CC_FilePath);


                return PhysicalFile(path, MimeTypes.GetMimeType(path), Path.GetFileName(path));
          
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

      
        [HttpGet]
        [Route("GetCourseContentById")]
        public IActionResult GetCourseContentById(int id)
        {
            try
            {
            
                return Ok(_courseContentRepository.GetCourseContentById(id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("UploadDocumentsAjax")]
        [HistoryFilter]
        public bool UploadDocumentsAjax()
        {
            try
            {
                CourseContent_Portal document = JsonConvert.DeserializeObject<CourseContent_Portal>(Request.Form["documentData"]);
                long uploaded_size;
                int iCounter;
                string sFiles_uploaded;
             
                /// method call to upload document.
                var documents = _document.UploadCourseContent(document, document.CC_Id_Course_Level, Request, out uploaded_size, out iCounter, out sFiles_uploaded);
               
                foreach (var doc in documents)
                {
                    double ContentLength = doc.document_size;
                    double result;
                    var fileSize = "";
                    var type = "";
                    if (ContentLength >= 1073741824.00)
                    {
                        result = ContentLength / 1073741824.00;
                        type = "GB";
                    }
                    else if (ContentLength >= 1048576.00)
                    {
                        result = ContentLength / 1048576.00;
                        type = "MB";
                    }
                    else if (ContentLength >= 1024.00)
                    {
                        result = ContentLength / 1024.00;
                        type = "KB";
                    }
                    else
                    {
                        result = ContentLength;
                        type = "BYTE";

                    }
                    fileSize = result.ToString("0.00") + " " + type;

         
                    document.CC_Type = doc.document_file_extension.Replace(".", string.Empty);
                    //document.document_object = doc.document_object;
                    document.CC_File_Size = fileSize;
                    document.CC_FilePath = doc.file_path;
                    document.CC_CreatedByUserId = UserId;
                    document.CC_CreatedByUserName = UserName;
                    //document.Document_Name = doc.document_file_name;
                     _courseContentRepository.InsertUpdateDocumentRecord(document);
                }

                return true;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        
            [HttpGet]
        [Route("DeleteCourseContentById")]
        public IActionResult DeleteCourseContentById(int id)
        {
            try
            {
                bool result = false;
                var data = _courseContentRepository.GetCourseContentById(id);
                var dbPath = _configurationRepository.GetSingleConfigValueByConfigKey("CourseContent");

                if (data != null)
                {
                    var isfileDelete = _document.DeleteFileCourseContent(dbPath + data.CC_FilePath);
                    if (isfileDelete)
                    {
                        result = _courseContentRepository.DeleteCourseContentById(id);
                    }
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}
