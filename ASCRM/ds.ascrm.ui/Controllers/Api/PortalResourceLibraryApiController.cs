using crm.portal.CrmModel;
using crm.portal.Interfaces.ResourceLibrary;
using ds.core.configuration;
using ds.core.document;
using ds.core.logger;
using ds.portal.ui.Controllers.Api;
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
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalResourceLibraryApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class PortalResourceLibraryApiController : ControllerBase
    {
        private readonly IResourceLibraryRepository _resourceLibraryRepository;
        private readonly IDocument _document;
        readonly ILogger<PortalResourceLibraryApiController> _log;
        LogException LogException;
        public PortalResourceLibraryApiController(ILogger<PortalResourceLibraryApiController> log, IConfigurationRepository iConfig, IDocument document,IResourceLibraryRepository resourceLibraryRepository)
        {

            _resourceLibraryRepository = resourceLibraryRepository;
            _document = document;
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
        [HttpGet]
        [Route("GetCategory")]
        public IActionResult GetCategory()
        {
            try
            {
                return Ok(_resourceLibraryRepository.LoadCategories(RoleCode));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCategoryForAdmin")]
        public IActionResult GetCategoryForAdmin()
        {
            try
            {
                return Ok(_resourceLibraryRepository.LoadCategoriesForAdmin());
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

        [HttpGet]
        [Route("GetResourseFileById")]
        public IActionResult GetResourseFileById(int id)
        {
            try
            {
                var data = _resourceLibraryRepository.GetRecourseFileById(id);
                return Ok(data);
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
                PortalDoc document = JsonConvert.DeserializeObject<PortalDoc>(Request.Form["documentData"]);
                long uploaded_size;
                int iCounter;
                string sFiles_uploaded;

                /// method call to upload document.
                var documents = _document.UploadResource(document, document.Docs_Id_DocCategories, Request, out uploaded_size, out iCounter, out sFiles_uploaded);

                foreach (var doc in documents)
                {
                    //double ContentLength = doc.document_size;
                    //double result;
                    //var fileSize = "";
                    //var type = "";
                    //if (ContentLength >= 1073741824.00)
                    //{
                    //    result = ContentLength / 1073741824.00;
                    //    type = "GB";
                    //}
                    //else if (ContentLength >= 1048576.00)
                    //{
                    //    result = ContentLength / 1048576.00;
                    //    type = "MB";
                    //}
                    //else if (ContentLength >= 1024.00)
                    //{
                    //    result = ContentLength / 1024.00;
                    //    type = "KB";
                    //}
                    //else
                    //{
                    //    result = ContentLength;
                    //    type = "BYTE";

                    //}
                    //fileSize = result.ToString("0.00") + " " + type;


                    //document.CC_Type = doc.document_file_extension.Replace(".", string.Empty);
                    //document.document_object = doc.document_object;
                    document.Docs_File = doc.file_path;
                    document.Docs_Title = doc.document_file_name;
                    document.Docs_EnteredByUser = UserName;
                    document.Docs_EnteredBy = UserId;
                    //document.Document_Name = doc.document_file_name;
                    _resourceLibraryRepository.AddDocResource(document);
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
        [Route("DeleteCate")]
        public IActionResult DeleteCate(int id)
        {
            try
            {
                return Ok(_resourceLibraryRepository.DeleteCate(id));

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("DeleteCateFile")]
        public IActionResult DeleteCateFile(int id)
        {
            try
            {
                return Ok(_resourceLibraryRepository.DeleteCateFile(id));

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}
