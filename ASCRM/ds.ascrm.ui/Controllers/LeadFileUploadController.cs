using ds.core.configuration;
using ds.portal.leads;
using ds.portal.leads.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Extensions.Caching.Memory;

namespace ds.portal.ui.Controllers
{
    public class LeadFileUploadController : BaseController
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        readonly ILogger<LeadFileUploadController> _log;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        public LeadFileUploadController(IRoleAdminRepository roleAdminRepository,
            IConfigurationRepository configurationRepository, 
            ILogger<LeadFileUploadController> log,
            IHostingEnvironment hostingEnvironment, 
            ILeadRepository leadRepository, 
            IMemoryCache memoryCache)
            : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _leadRepository = leadRepository;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.Feature = "Lead";
            ViewBag.Page = "File Upload";
            SetProductDetailToViewBag();


            return View();
        }


        public ActionResult Templates()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Templates_Save(IEnumerable<IFormFile> files)
        {

            if (files != null)
            {
                var uploaded_files = Request.Form.Files;

                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var physicalPath = Path.Combine(_hostingEnvironment.WebRootPath, "Assets", "TempLeadFiles", unixTimestamp + fileName);
                    if (file.Length > 0)
                    {
                        using (var stream = new FileStream(physicalPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            stream.Flush();
                            TempData["PhysicalPath"] = physicalPath;
                        }
                    }
                }
            }
            return Content("");
        }

        [HttpPost]
        public DataSourceResult GetLeadFileUpload([DataSourceRequest]DataSourceRequest request)
        {
            var path = TempData["PhysicalPath"] as string;
            List<LeadFileUpload> leads = new List<LeadFileUpload>();
            var dt = LoadFromExcelFile(path);

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var contactName = Convert.ToString(row["Contact"]);
                    var email = Convert.ToString(row["E-mail"]);
                    var mobile = Convert.ToString(row["Phone"]);

                    if (Convert.ToString(row["Contact"]) != "")
                    {

                        LeadFileUpload leadFileUpload = new LeadFileUpload
                        {
                            /* 
                            public bool isDuplicate { get; set; }
                            public string ClosedDate { get; set; }

                            public int user_id { get; set; } 

                            public int optTitleSourcesId { get; set; }
                            public int optTitlePathwayId { get; set; }
                            public int optTitleRegistrationsId { get; set; }
                            public int optTitleCourseLevelsId { get; set; }
                            public int optTitleCourseLevelInquiryId { get; set; }
                            public int Training_Provider_id { get; set; }
                            public int optTitleLastResultsId { get; set; }
                            public int optTitleJobTitlesId { get; set; }                          
                             */

                            // columns.Bound(p => p.Company_Name).Title("Enquiry Closed Date").Width(150);
                            //isDuplicate = (_isDuplicate.Count > 0) ? true : false,
                            Sales_Advisor = Convert.ToString(row["Sales advisor"]),
                            Date_of_Enquiry = Convert.ToString(row["Date of enquiry"]),
                            Source_of_Enquiry = Convert.ToString(row["Source of enquiry"]).Trim(),
                            Company_Name = Convert.ToString(row["Company name"]).Trim(),
                            Contact = Convert.ToString(row["Contact"]),
                            Phone = Convert.ToString(row["Phone"]).Trim(),
                            Mobile = Convert.ToString(row["Mobile"]).Trim(),
                            Email = Convert.ToString(row["E-mail"]).Trim(),
                            Job_Title = Convert.ToString(row["Job title"]).Trim(),
                            Department = Convert.ToString(row["Department"]).Trim(),
                            Pathway = Convert.ToString(row["Pathway"]).Trim(),
                            Registration = Convert.ToString(row["Registration"]).Trim(),
                            Agreed_Payment_Scheme = Convert.ToString(row["Agreed payment scheme"]).Trim(),
                            Enrolment_Date = Convert.ToString(row["Enrolment date"]),
                            Learner_Enrolled = Convert.ToString(row["Learner enrolled"]),
                            Lead_Cancellation_Date = Convert.ToString(row["Enrolment cancellation date"]),
                            CourseLevel_Apply = Convert.ToString(row["Course level to apply"]).Trim(), //DO NOT USE LEVEL TO ENQUIRY first item anymore...  GetAtLeastALevelToApply(Convert.ToString(row["Course Level"]), Convert.ToString(row["Course Level Enquiry"])),
                            CourseLevel_Enquiry = Convert.ToString(row["Course level enquiry"]).Trim(),
                            Last_Results = Convert.ToString(row["Last results"]).Trim(),
                            Address1 = Convert.ToString(row["Address 1"]).Trim(),
                            Address2 = Convert.ToString(row["Address 2"]).Trim(),
                            TownCity = Convert.ToString(row["Town/City"]).Trim(),
                            Employer_Postcode = Convert.ToString(row["Employer postcode"]).Trim(),
                            Notes = Convert.ToString(row["Notes"]), 
                            contact_issues = "",
                            contact_is_error = false
                        };
                        leads.Add(leadFileUpload);

                    }
                }
            }

            System.IO.File.Delete(path);

            //check here
            leads = _leadRepository.ValidateLeadsForImport(leads);

            return leads.ToDataSourceResult(request);
        }
        private string GetAtLeastALevelToApply(string sCourseLevel_Apply, string sCourseLevel_Enquiry)
        {
            if (sCourseLevel_Apply != "")
            {
                return sCourseLevel_Apply;
            }
            else
            {
                // we got empty level now look for 1st level in enquiry
                if (sCourseLevel_Enquiry != "")
                {
                    string[] lvl = sCourseLevel_Enquiry.Split(";");
                    if (lvl.Length > 0)
                    {
                        return lvl[0];
                    }
                    else
                    {
                        return sCourseLevel_Enquiry;
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        [HttpGet]
        public FileResult Download()
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Assets", "FileUpload", "UploadLead.xls");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "ImportLead.xls";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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
                        excelConnection.Close();
                        excelConnection.Open();
                        using (OleDbDataReader dReader = cmd.ExecuteReader())
                        {
                            DataTable myData = new DataTable();
                            myData.Load(dReader);
                            return myData;
                        }

                    }
                }
            }
            else 
                return null;

           
        }

        [HttpPost]
        public ActionResult Templates_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(_hostingEnvironment.WebRootPath, "App_Data", fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
    }
}

