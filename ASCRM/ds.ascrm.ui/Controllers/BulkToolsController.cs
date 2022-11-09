using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ds.core.configuration;
using ds.portal.leads;
using ds.portal.leads.Models;
using ds.portal.ui.Controllers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;

namespace ds.ascrm.ui.Controllers
{
    public class BulkToolsController : BaseController
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        readonly ILogger<BulkToolsController> _log;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;

        public BulkToolsController(IRoleAdminRepository roleAdminRepository,
            IConfigurationRepository configurationRepository,
            ILogger<BulkToolsController> log,
            IHostingEnvironment hostingEnvironment,
            ILeadRepository leadRepository,
            IMemoryCache memoryCache
          
            )
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _leadRepository = leadRepository;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult ImportNote()
        {
            ViewBag.Feature = "Bulk Note";
            ViewBag.Page = "File Upload";
            SetProductDetailToViewBag();
            return View();
        }
        [HttpPost]
        public DataSourceResult GetLeadFileUpload([DataSourceRequest] DataSourceRequest request)
        {
            var path = TempData["PhysicalPath"] as string;
            List<ImportNote> leads = new List<ImportNote>();
            var dt = LoadFromExcelFile(path);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    
     

                    if (Convert.ToString(row["Lead Id"]) != "")
                    {

                        ImportNote leadFileUpload = new ImportNote
                        {
    
                            LeadId = Convert.ToString(row["Lead Id"]),
                            Note = Convert.ToString(row["Note"]),

                   
                        };
                        leads.Add(leadFileUpload);

                    }
                }
            }

            System.IO.File.Delete(path);

            //check here
            leads = _leadRepository.ValidateLeadNoteImport(leads, GetSecurityUserId, GetSecurityRoleName);

            return leads.ToDataSourceResult(request);
        }
        private DataTable LoadFromExcelFile(string filePath)
        {
            if (filePath != null)
            {
                String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=YES\"", filePath);
                using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
                {
                    using (OleDbCommand cmd = new OleDbCommand("SELECT * from [Sheet1$]", excelConnection))
                    {
                        try
                        {

                      
                        excelConnection.Close();
                        excelConnection.Open();
                        using (OleDbDataReader dReader = cmd.ExecuteReader())
                        {
                            DataTable myData = new DataTable();
                            myData.Load(dReader);
                            return myData;
                        }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }
            }
            else
                return null;


        }

        public IActionResult ImportStatus()
        {

            ViewBag.Feature = "Bulk Status";
            ViewBag.Page = "File Upload";
            SetProductDetailToViewBag();
            return View();
        }
        [HttpPost]
        public DataSourceResult GetLeadStatusFileUpload([DataSourceRequest] DataSourceRequest request)
        {
            var path = TempData["PhysicalPath"] as string;
            List<ImportStatus> leadStatus = new List<ImportStatus>();
            var dt = LoadFromExcelFile(path);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {



                    if (Convert.ToString(row["Lead Id"]) != "")
                    {

                        ImportStatus leadFileUpload = new ImportStatus
                        {

                            LeadId = Convert.ToString(row["Lead Id"]),
                            Status = Convert.ToString(row["Status"]),


                        };
                        leadStatus.Add(leadFileUpload);

                    }
                }
            }

            System.IO.File.Delete(path);

            //check here
            leadStatus = _leadRepository.ValidateLeadStatusImport(leadStatus, GetSecurityUserId, GetSecurityRoleName);

            return leadStatus.ToDataSourceResult(request);
        }
        public IActionResult ImportLastResult()
        {

            ViewBag.Feature = "Bulk LastResult";
            ViewBag.Page = "File Upload";
            SetProductDetailToViewBag();
            return View();
        }
        [HttpPost]
        public DataSourceResult GetLeadLastResultFileUpload([DataSourceRequest] DataSourceRequest request)
        {
            var path = TempData["PhysicalPath"] as string;
            List<ImportLastResult> leadResult = new List<ImportLastResult>();
            var dt = LoadFromExcelFile(path);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {



                    if (Convert.ToString(row["Lead Id"]) != "")
                    {

                        ImportLastResult leadFileUpload = new ImportLastResult
                        {

                            LeadId = Convert.ToString(row["Lead Id"]),
                            LastResult = Convert.ToString(row["LastResult"]),


                        };
                        leadResult.Add(leadFileUpload);

                    }
                }
            }

            System.IO.File.Delete(path);

            //check here
            leadResult = _leadRepository.ValidateLeadLastResultImport(leadResult, GetSecurityUserId, GetSecurityRoleName);

            return leadResult.ToDataSourceResult(request);
        }

        [HttpGet]
        public FileResult DownloadNote()
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Assets", "FileUpload", "ImportNote.xls");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "ImportLeadNote.xls";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpGet]
        public FileResult DownloadStatus()
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Assets", "FileUpload", "ImportStatus.xls");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "ImportLeadStatus.xls";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpGet]
        public FileResult DownloadLastResult()
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Assets", "FileUpload", "ImportLastResult.xls");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "ImportLeadLastResult.xls";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
