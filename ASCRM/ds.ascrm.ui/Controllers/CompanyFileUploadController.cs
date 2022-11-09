using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CsvHelper;
using ds.core.configuration;
using ds.portal.companies;
using ds.portal.companies.Models;
using ds.portal.leads.Models;
using ds.portal.ui.Controllers;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
namespace ds.ascrm.ui.Controllers
{
    public class CompanyFileUploadController : BaseController
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly IRoleAdminRepository _roleAdminRepository;
        readonly ILogger<CompanyFileUploadController> _log;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMemoryCache _cache;
        private readonly ICompanyRepository _companyRepository;
        public CompanyFileUploadController(ICompanyRepository companyRepository,IHostingEnvironment hostingEnvironment,ILogger<CompanyFileUploadController> log,IRoleAdminRepository roleAdminRepository, IConfigurationRepository configurationRepository, IMemoryCache memoryCache) : base(roleAdminRepository, configurationRepository, memoryCache)
        {
            _cache = memoryCache;
            _roleAdminRepository = roleAdminRepository;
            _configurationRepository = configurationRepository;
            _log = log;
            _hostingEnvironment = hostingEnvironment;
            _companyRepository = companyRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Feature = "Company";
            ViewBag.Page = "File Upload";
            SetProductDetailToViewBag();

            return View();
        }


        [HttpPost]
        public IActionResult Templates_Save(List<IFormFile> files)
        {

            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                    Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    var physicalPath = Path.Combine(_hostingEnvironment.WebRootPath, "Assets", "TempCompanyFiles", unixTimestamp + fileName);
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

        [HttpGet]
        public FileResult Download()
        {
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Assets", "CompanyFileUpload", "CompanyUpload.csv");
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "ImportCompany.csv";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        [HttpPost]
        public DataSourceResult GetCompanyFileUpload([DataSourceRequest] DataSourceRequest request)
        {
            

            var path = TempData["PhysicalPath"] as string;
            List<CompanyFileUpload> company = new List<CompanyFileUpload>();

            string filePath = System.IO.Path.GetFullPath(path);

            var reader = new StreamReader(filePath);
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvReader.Configuration.RegisterClassMap<ImportCompanyMap>();
            var fileData = csvReader.GetRecords<CompanyFileUpload>().ToList();
            foreach (var item in fileData)
            {

                CompanyFileUpload companyFileUpload = new CompanyFileUpload
                {
                    name = item.name,
                    reference=item.reference,
                    company_number = item.company_number,
                    postcode = item.postcode,
                    address2 = item.address2,
                    email1 = item.email1,
                    website = item.website,
                    address3 = item.address3,
                    mobile1 = item.mobile1,
                    address1 = item.address1,
                    town = item.town,
                    county = item.county,
                    tel1 = item.tel1,
                    fax1 = item.fax1,
                    rating = item.rating,
                    number_of_beds = item.number_of_beds,
                    number_of_employees = item.number_of_employees,
                    edrs_number = item.edrs_number,
                    company_group_id = item.company_group_id,
                    contact_is_error = item.contact_is_error,
                    contact_issues = item.contact_issues,
                    contact_name1 = item.contact_name1,
                    sales_advisor_id = item.sales_advisor_id,
                    company_group = item.company_group,
                    sale_advisor = item.sale_advisor,
                    business_type =item.business_type,
                    company_house_registration_number = item.company_house_registration_number,
                    cqc_registration_number = item.cqc_registration_number,
                    employer_email_address = item.employer_email_address,
                    job_title = item.job_title,
                    levy_employer = item.levy_employer,
                    name_desision_maker = item.name_desision_maker,
                    no_of_employees= item.no_of_employees

                };
                company.Add(companyFileUpload);

            }




            //check here
            company = _companyRepository.ValidateCompanyForImport(company);

            return company.ToDataSourceResult(request);
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
