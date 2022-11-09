using ds.core.configuration;
using ds.core.logger;
using ds.portal.companies;
using ds.portal.report;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ds.portal.companies.Models;
using ds.portal.leads.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using portal.data.repository.Repositories;
using CompanyContact = ds.portal.companies.Models.CompanyContact;
using Microsoft.AspNetCore.Cors;
using System.Net;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nancy.Json;
using Nancy.Json.Simple;
using System.Web.Helpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Runtime.Serialization.Json;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/CompanyApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class CompanyApiController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        readonly ILogger<CompanyApiController> _log;
        LogException LogException;
        public CompanyApiController(ICompanyRepository companyRepository, IHostingEnvironment hostingEnvironment
            , ILogger<CompanyApiController> log, IConfigurationRepository iConfig)
        {
            _companyRepository = companyRepository;
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

        #region Company

        [HttpGet]
        [Route("GetCompanyById")]
        public IActionResult GetCompanyById(int id)
        {
            try
            {
                var company = _companyRepository.GetCompanyById(id);
                /// country default set - UK.
                //if (leads.Lead_Contact_Id_Country == 0)
                //{
                //    leads.Lead_Contact_Id_Country = 4;
                //}
                return Ok(company);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("CompanySearch")]
        public IActionResult CompanySearch([DataSourceRequest] DataSourceRequest request, string id, string name, int? sale_id)
        {
            try
            {
                var company = _companyRepository.CompanySearch(id,name,sale_id);

                var model = new  List<CompanyModel>();
                foreach (var item in company)
                {
                    model.Add(new CompanyModel()
                    {
                        id = item.id,
                        name = item.name,
                        sales_advisor = item.sales_advisor,
                        encodedId = Encryption.Encrypt(item.id.ToString())
                });
                }

                /// country default set - UK.
                //if (leads.Lead_Contact_Id_Country == 0)
                //{
                //    leads.Lead_Contact_Id_Country = 4;
                //}
                return Ok(model.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("CompanySearchAtZ")]
        public IActionResult CompanySearchAtZ([DataSourceRequest] DataSourceRequest request, string name,string searchtype)
        {
            try
            {
                var company = _companyRepository.CompanySearchAtZ(name,searchtype);

                var model = new List<CompanySearch>();
                foreach (var item in company)
                {
                    model.Add(new CompanySearch()
                    {
                        id = item.id,
                        name = item.name,
                        sa = item.sa ,
                        let = item.let,
                        encodedId = Encryption.Encrypt(item.id.ToString())
                    });
                }

                /// country default set - UK.
                //if (leads.Lead_Contact_Id_Country == 0)
                //{
                //    leads.Lead_Contact_Id_Country = 4;
                //}
                return Ok(model.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("LoadCompanyByName")]
        public IActionResult LoadCompanyByName([DataSourceRequest] DataSourceRequest request, string name)
        {
            try
            {
                var company = _companyRepository.CompanySearchByWord(name);

                var model = new List<CompanySearch>();
                foreach (var item in company)
                {
                    model.Add(new CompanySearch()
                    {
                        id = item.id,
                        name = item.name,
                        sa = item.sa ,
                        encodedId = Encryption.Encrypt(item.id.ToString())
                    });
                }

                /// country default set - UK.
                //if (leads.Lead_Contact_Id_Country == 0)
                //{
                //    leads.Lead_Contact_Id_Country = 4;
                //}
                return Ok(model.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCompanies")]
        public IActionResult GetCompanies([DataSourceRequest] DataSourceRequest request,int? userid, string startDate, string endDate)
        {
            int? currentUserId = 0;
            if (userid != null)
            {
                currentUserId = userid;
            }
            else
            {
                currentUserId = UserId;
            }

            
            CultureInfo ukCulture = new CultureInfo("en-GB");
            try
            {
                DateTime? dtS = null;
                DateTime? dtE = null;
                if (string.IsNullOrEmpty(startDate))
                {
                    var companies = _companyRepository.GetCompanies(currentUserId, dtS, dtE);
                    //var model = new List<CompanyModel>();
                    //foreach (var item in companies)
                    //{
                    //    model.Add(new CompanyModel()
                    //    {
                    //        id = item.id,
                    //        name = item.name,
                    //        sales_advisor = item.sales_advisor,
                    //        encodedId = Encryption.Encrypt(item.id.ToString()),
                    //        email1 =item.email1,
                    //        tel1 = item.tel1,
                    //        address1 = item.address1,
                    //        postcode = item.postcode
                    //    });
                    //}
                    // return companies.ToDataSourceResult(request);
                    return Ok(companies.ToDataSourceResult(request));
                }
                else
                {
                    var companies = _companyRepository.GetCompanies(currentUserId, DateTime.Parse(startDate, ukCulture.DateTimeFormat), DateTime.Parse(endDate, ukCulture.DateTimeFormat));
                    //var model = new List<CompanyModel>();
                    //foreach (var item in companies)
                    //{
                    //    model.Add(new CompanyModel()
                    //    {
                    //        id = item.id,
                    //        name = item.name,
                    //        sales_advisor = item.sales_advisor,
                    //        encodedId = Encryption.Encrypt(item.id.ToString()),
                    //        email1 = item.email1,
                    //        tel1 = item.tel1,
                    //        address1 = item.address1,
                    //        postcode = item.postcode
                    //    });
                    //}
                    // return companies.ToDataSourceResult(request);
                    return Ok(companies.ToDataSourceResult(request));
                }
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("SearchInCompany")]
        public IActionResult SearchInCompany([DataSourceRequest] DataSourceRequest request,  string name)
        {
            try
            {
                var company = _companyRepository.CompanyInSearch(name);

                var model = new List<CompanyModel>();
                foreach (var item in company)
                {
                    model.Add(new CompanyModel()
                    {
                        id = item.id,
                        name = item.name,
                        sales_advisor = item.sales_advisor,
                        encodedId = Encryption.Encrypt(item.id.ToString()),
                        email1 = item.email1,
                        tel1 = item.tel1,
                        address1 = item.address1,
                        postcode = item.postcode,
                        LeadID = item.LeadID
                    });
                }

                
                return Ok(model.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("CompanyWorkPlaceSearch")]
        public IActionResult CompanyWorkPlaceSearch([DataSourceRequest] DataSourceRequest request, string name,int company_id)
        {
            try
            {
                var company = _companyRepository.CompanyWorkPlaceInSearch(name,company_id);

                //var model = new List<CompanyWorkplaces>();
                //foreach (var item in company)
                //{
                //    model.Add(new CompanyWorkplaces()
                //    {
                //        id = item.id,
                //        name = item.name,
                //        sales_advisor = item.sales_advisor,
                //        encodedId = Encryption.Encrypt(item.id.ToString()),
                //        email1 = item.email1,
                //        tel1 = item.tel1,
                //        address1 = item.address1,
                //        postcode = item.postcode,
                //        LeadID = item.LeadID
                //    });
                //}


                return Ok(company.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("SearchCompanyByName")]
        public IActionResult SearchCompanyByName (string name)
        {
            try
            {
                var company = _companyRepository.CompanySearchByName(name);

                var model = new List<CompanyModel>();
                foreach (var item in company)
                {
                    model.Add(new CompanyModel()
                    {
                        id = item.id,
                        name = item.name,
                        sales_advisor = item.sales_advisor,
                        encodedId = Encryption.Encrypt(item.id.ToString()),
                        email1 = item.email1,
                        tel1 = item.tel1,
                        address1 = item.address1,
                        postcode = item.postcode,
                        LeadID = item.LeadID
                    });
                }


                return Ok(model);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteCompanyById")]
        [HistoryFilter]
        public IActionResult DeleteCompanyById(int id)
        {
            try
            {
                var retVal = _companyRepository.DeleteCompanyById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertCompanyRecord")]
        [HistoryFilter]
        public CompanyModel InsertCompanyRecord(CompanyModel company)
        {
            CultureInfo ukCulture = new CultureInfo("en-GB");//CultureInfo.CreateSpecificCulture("en-GB")
            company.created_by = UserId;
            company.modified_by = UserId;
            try
            {
                return _companyRepository.InsertUpdateCompanyRecord(company);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        #endregion
      

        [HttpGet]
        [Route("GetNoteById")]
        public IActionResult GetNoteById(int id)
        {
            try
            {
                var contact = _companyRepository.GetNoteById(id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        
        [HttpGet]
        [Route("GetAllCompanyNotes")]
        public DataSourceResult GetAllCompanyNotes([DataSourceRequest] DataSourceRequest request, int company_Id)
        {
            try
            {
                return _companyRepository.GetAllCompanyNotes(company_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertCompanyNotes")]
        [HistoryFilter]
        public bool InsertCompanyNotes(CompanyNote note)
        {
            try
            {
                if (note.Note_Id > 0)
                {
                    if (!note.IsOsca)
                    {
                        note.Note_UpdatedByUserId = UserId;
                        note.Note_UpdatedByUserName = UserName;
                    }
                   
                }
                else
                {
                    if (!note.IsOsca)
                    {
                        note.Note_CreatedByUserId = UserId;
                        note.Note_CreatedByUserName = UserName;
                    }
                }
                note.Note_DateCreated = null;
                return _companyRepository.InsertUpdateCompanyNoteRecord(note);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("DeleteNoteById")]
        [HistoryFilter]
        public IActionResult DeleteNoteById(int id)
        {
            try
            {
                var retVal = _companyRepository.DeleteNoteById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #region Compnay Contact
        [HttpGet]
        [Route("DeleteCompanyContactById")]
        [HistoryFilter]
        public IActionResult DeleteCompanyContactById(int id)
        {
            try
            {
                var retVal = _companyRepository.DeleteCompanyContactById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCompanyContactById")]
        public IActionResult GetCompanyContactById(int id)
        {
            try
            {
                var company = _companyRepository.GetCompanyContactById(id);
                /// country default set - UK.
                //if (leads.Lead_Contact_Id_Country == 0)
                //{
                //    leads.Lead_Contact_Id_Country = 4;
                //}
                return Ok(company);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetCompanyWorkPlaceById")]
        public IActionResult GetCompanyWorkPlaceById(int id)
        {
            try
            {
                var company = _companyRepository.GetCompanyWorkplaceById(id);
               
                return Ok(company);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetContactById")]
        public IActionResult GetContactById(int id)
        {
            try
            {
                var contact = _companyRepository.GetNoteById(id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }



        [HttpGet]
        [Route("GetAllCompanyContacts")]
        public DataSourceResult GetAllCompanyContacts([DataSourceRequest] DataSourceRequest request, int company_Id)
        {
            try
            {
                return _companyRepository.GetAllCompanyContacts(company_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllAssignedLearner")]
        public DataSourceResult GetAllAssignedLearner([DataSourceRequest] DataSourceRequest request, int company_Id)
        {
            try
            {
                return _companyRepository.GetAllAssignedLearner(company_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllWorkplaceContacts")]
        public DataSourceResult GetAllWorkplaceContacts([DataSourceRequest] DataSourceRequest request, int company_Id,int wp_idd)
        {
            try
            {
                return _companyRepository.GetAllWorkplaceContacts(company_Id, wp_idd).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertCompanyContact")]
        [HistoryFilter]
        public bool InsertCompanyContact(CompanyContact company)
        {
            try
            {
                if (company.contact_id > 0)
                {
                    company.modified_by = UserId;
                    company.modified_date= DateTime.Now;
                }
                else
                {
                    company.created_by = UserId;
                    company.created_date =DateTime.Now;
                }
                return _companyRepository.InsertUpdateCompanyContactRecord(company);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        #endregion
        [HttpPost]
        [Route("CheckDuplicateCompany")]
        public IActionResult CheckDuplicateCompany(CompanyModel company)
        {
            try
            {
                var duplicateCompanyCount = _companyRepository.CheckDuplicateCompanyPostcode(company.postcode,company.name);

                //if (company.id == 0 && duplicateCompanyCount != null)
                //{
                //    //get lead by lead id
                //    duplicateLead = _leadRepository.GetLeadByLeadId(duplicateLeadsCount.Lead_Id);

                  
                //    var courseLevel = "";
                //    var lastResult = "";
                //    var status = "";
                 
                   
                //}
                return Ok(duplicateCompanyCount);
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

        #region Company WorkPlace

        [HttpGet]
        [Route("GetAllCompanyWorkPlace")]
        public DataSourceResult GetAllCompanyWorkPlace([DataSourceRequest] DataSourceRequest request, int company_Id)
        {
            try
            {
                return _companyRepository.GetAllCompanyWorkplaceses(company_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("InsertCompanyWorkPlace")]
        [HistoryFilter]
        public bool InsertCompanyWorkPlace(CompanyWorkplaces workplaces)
        {
            try
            {
                if (workplaces.wp_id > 0)
                {
                    workplaces.updated_by = UserId;
                   // note.Note_UpdatedByUserName = UserName;
                }
                else
                {
                    workplaces.created_by = UserId;
                   // note.Note_CreatedByUserName = UserName;
                }
                return _companyRepository.InsertUpdateCompanyWorkPlaceRecord(workplaces);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("DeleteCompanyWorkPlaceById")]
        [HistoryFilter]
        public IActionResult DeleteCompanyWorkPlaceById(int id)
        {
            try
            {
                var retVal = _companyRepository.DeleteCompanyWorkPlaceById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion


        [HttpPost]
        [Route("UploadCompany")]
        public List<CompanyFileUpload> UploadCompany(List<CompanyFileUpload> companyFileUploadsFile)
        {
            var result = _companyRepository.BulkInsert(companyFileUploadsFile, UserId);
            return result;
      
        }

        [HttpGet]
        [Route("GetLeadByCompany")]
        public DataSourceResult GetLeadByCompany([DataSourceRequest] DataSourceRequest request, int company_Id)
        {
            try
            {
              //  var permissions = _companyRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
             //bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var coll = _companyRepository.GetLeadByCompany(company_Id, UserId );
                return coll.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("LoadLeaner")]
        public IActionResult LoadLeaner(int id)
        {
            var data = _companyRepository.GetLearnerByCompanyId(id);
            try
            {

                string responseFromServer ="";
                RootObject rootObject = new RootObject();

                var dataList = new List<AssignedLearnerList>();

                foreach (var item in data)
                {
                    string pram = item.learner_id.ToString();

                    
                    WebRequest request = (HttpWebRequest)WebRequest.Create("http://dmss.uk/Company/CrmLearner.asmx/LoadLearrner?id=" + item.learner_id);
                    request.Method = "GET";
                    request.ContentType = "'application/json; charset=utf-8";
                    XmlDocument doc = new XmlDocument();

                    WebResponse response = request.GetResponse();
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);
                       
                         responseFromServer = reader.ReadToEnd();
                
                         rootObject = new JavaScriptSerializer().Deserialize<RootObject>(responseFromServer);

       
                        
                    }
                    //   ReadFrom(response.GetResponseStream());
                }
                return Ok(rootObject);

            }
            catch (WebException e)
            {
                string pageContent = new StreamReader(e.Response.GetResponseStream()).ReadToEnd().ToString();
                return null;
            }
        }
        
        private byte[] CreateHttpRequestData(Dictionary<string, string> dic)
        {
            StringBuilder _sbParameters = new StringBuilder();
            foreach (string param in dic.Keys)
            {
                _sbParameters.Append(param);//key => parameter name
                _sbParameters.Append('=');
                _sbParameters.Append(dic[param]);//key value
                _sbParameters.Append('&');
            }
            _sbParameters.Remove(_sbParameters.Length - 1, 1);

            UTF8Encoding encoding = new UTF8Encoding();

            return encoding.GetBytes(_sbParameters.ToString());

        }
    }
}