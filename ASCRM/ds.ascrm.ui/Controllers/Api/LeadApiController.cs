using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.document;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.leads;
using ds.portal.leads.Models;
using ds.portal.users;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using portal.data.repository.Interfaces;
using portal.models.Comman;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/LeadApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    // [EnableCors("MyPolicy")]
    public class LeadApiController : ControllerBase
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ILead_ListRepository _Lead_ListRepository;
        private readonly ILead_UserRepository _Lead_UserRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMailService _emailSender;
        private IConfigurationRepository configuration;
        private string _fromEmailId = string.Empty;
        private readonly ILoginRepository _loginRepository;
        readonly ILogger<LeadApiController> _log;
        LogException LogException;
        IDocument _document;
        private readonly IEmailTemplateRepository _emailTemplateRepository;

        private readonly IRoleAdminRepository _roleAdminRepository;

        private string toEmailId = string.Empty;
        private string toDisplayName = string.Empty;
        private string fromEmailId = string.Empty;

        public LeadApiController(ILeadRepository leadRepository, ILead_ListRepository Lead_ListRepository, IHostingEnvironment hostingEnvironment,
           ILogger<LeadApiController> log, IMailService emailSender
            , ILead_UserRepository Lead_UserRepository, IConfigurationRepository iConfig
            , ILoginRepository loginRepository, IDocument document,
           IRoleAdminRepository roleAdminRepository,
           IEmailTemplateRepository emailTemplateRepository
           )
        {
            _emailTemplateRepository = emailTemplateRepository;
            _leadRepository = leadRepository;
            _Lead_ListRepository = Lead_ListRepository;
            _Lead_UserRepository = Lead_UserRepository;
            _hostingEnvironment = hostingEnvironment;
            _emailSender = emailSender;
            configuration = iConfig;
            _fromEmailId = configuration.GetSingleConfigValueByConfigKey("FromEmail");
            _loginRepository = loginRepository;
            _log = log;
            LogException = new LogException(iConfig, _log);
            _document = document;
            _roleAdminRepository = roleAdminRepository;

            toEmailId = configuration.GetSingleConfigValueByConfigKey("ToEmail");
            toDisplayName = configuration.GetSingleConfigValueByConfigKey("ToDisplayName");
            fromEmailId = configuration.GetSingleConfigValueByConfigKey("FromEmail");

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
        private string GetSecurityRoleName
        {
            get
            {
                var SecurityRoleName = "";
                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleName")))
                {
                    SecurityRoleName = HttpContext.Session.GetString("RoleName");
                }
                return SecurityRoleName;
            }
            set {; }
        }
        private int GetSecurityRoleId
        {
            get
            {
                var securityRoleId = 0;

                if (HttpContext.Session.Keys.Any(a => a.Equals("RoleId")))
                {
                    securityRoleId = HttpContext.Session.GetInt32("RoleId") ?? 0;
                }

                return securityRoleId;
            }
            set {; }
        }
        #endregion

        #region "--- Dashboard "
        [HttpGet]
        [Route("GetModalLeadsAgreedPaymentScheme")]
        public DataSourceResult GetModalLeadsAgreedPaymentScheme([DataSourceRequest]DataSourceRequest request, int agreed_pay_val)
        {
            try
            {
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var coll = _leadRepository.GetAllLeadsByAgreedPaymentScheme(UserId, _isAdmin, agreed_pay_val);
                return coll.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }



        [HttpGet]
        [Route("GetModalLeadsLastResults")]
        public DataSourceResult GetModalLeadsLastResults([DataSourceRequest]DataSourceRequest request, int last_result_val)
        {
            try
            {
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var coll = _leadRepository.GetAllLeadsByLastResults(UserId, _isAdmin, last_result_val);
                return coll.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        #endregion

        #region "--- get all "

        [HttpGet]
        [Route("GetAllLeads")]
        public DataSourceResult GetAllLeads([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var coll = _leadRepository.GetAllLeads(UserId, _isAdmin);
                return coll.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        #endregion

        #region "--- Leads Get "

        [HttpGet]
        [Route("GetLeadById")]
        public IActionResult GetLeadById(int id)
        {
            try
            {
                var leads = _leadRepository.GetLeadById(id);
                /// country default set - UK.
                if (leads.Lead_Contact_Id_Country == 0)
                {
                    leads.Lead_Contact_Id_Country = 4;
                }
                return Ok(leads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region "--- Leads delete "

        [HttpGet]
        [Route("DeleteLeadById")]
        [HistoryFilter]
        public IActionResult DeleteLeadById(int id)
        {
            try
            {
                var retVal = _leadRepository.DeleteLeadById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region "--- insert "

        #endregion


        #region "--- "
        #endregion

        #region "--- "
        #endregion

        #region "--- "
        #endregion                      

        #region "--- sort them out"

        [HttpPost]
        [Route("InsertLeadRecord")]
        [HistoryFilter]
        public Lead InsertLeadRecord(Lead lead)
        {
            CultureInfo ukCulture = new CultureInfo("en-GB");//CultureInfo.CreateSpecificCulture("en-GB")

            if (string.IsNullOrEmpty(lead.Lead_DateOfEnquiry))
            {
                lead.Lead_DateOfEnquiry = DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("en-GB"));
            }
            if (string.IsNullOrEmpty(lead.Lead_DateClosed))
            {
                lead.Lead_DateClosed = null;
            }
            if (string.IsNullOrEmpty(lead.Lead_DateCancellation))
            {
                lead.Lead_DateCancellation = null;
            }
            if (string.IsNullOrEmpty(lead.Lead_PreferredTimeToContact))
            {
                lead.Lead_PreferredTimeToContact = null;
            }
            //lead.Lead_DateCreated = null;
            lead.Lead_CreatedByUserId = UserId;
            lead.Lead_UpdatedByUserId = UserId;
            //lead.Lead_DateCreated = DateTime.Parse(DateTime.Now.ToShortDateString(), ukCulture.DateTimeFormat) ;

            try
            {
                var leads= _leadRepository.InsertUpdateLeadRecord(lead);

                if (lead.oldLastResult != lead.newLastResult)
                {
                    if (lead.lastResultSelect == "Enrolled on course" || lead.lastResultSelect == "Registered to CPD/ Short courses")
                    {
                        SendLastResultEamil(lead);
                    }
                }

                return leads;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllDropdowns")]
        public IActionResult GetAllDropdowns()
        {
            try
            {
                var jobTitles = _Lead_ListRepository.GetDropdownOptionsByHeaderName("JobTitles");
                var departments = _Lead_ListRepository.GetDropdownOptionsByHeaderName("Departments");
                var lastResults = _Lead_ListRepository.GetDropdownOptionsByHeaderName("LastResults");
                var pathways = _Lead_ListRepository.GetDropdownOptionsByHeaderName("Pathways");
                var registrations = _Lead_ListRepository.GetDropdownOptionsByHeaderName("Registrations");
                var countries = _Lead_ListRepository.GetDropdownOptionsByHeaderName("Countries");
                var sources = _Lead_ListRepository.GetDropdownOptionsByHeaderName("Sources");
                var dealClosedOption = _Lead_ListRepository.GetDropdownOptionsByHeaderName("YesNo");
                var notesCategories = _Lead_ListRepository.GetDropdownOptionsByHeaderName("NotesCategories");
                var documentCategories = _Lead_ListRepository.GetDropdownOptionsByHeaderName("DocumentCategories");
                var leadStatuses = _Lead_ListRepository.GetDropdownOptionsByHeaderName("LeadStatus");
                var courseLevels = _Lead_ListRepository.GetDropdownOptionsByHeaderName("CourseLevels");
                var courseLevelEnquiries = _Lead_ListRepository.GetDropdownOptionsByHeaderName("CourseLevelsEnquiry");
                var trainingProviders = _Lead_ListRepository.GetDropdownOptionsByHeaderName("AgreedPaymentScheme"); // AgreedPaymentScheme
                var salutations = _Lead_ListRepository.GetDropdownOptionsByHeaderName("Salutations");
                var learnerEnrolled = _Lead_ListRepository.GetDropdownOptionsByHeaderName("LearnerEnrolled");
                var companyNoteCategory = _Lead_ListRepository.GetDropdownOptionsByHeaderName("CompanyNotes");
                var title = _Lead_ListRepository.GetDropdownOptionsByHeaderName("Title");
                var companyGroup = _Lead_ListRepository.GetAllCompanyGroup();
                var company = _Lead_ListRepository.GetAllCompany();
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var salesAdvisors = _Lead_UserRepository.GetAllSalesAdvisorUsers(_isAdmin);
                var newSalesAdvisors = _Lead_UserRepository.GetAllSalesAdvisor();
                if (departments != null)
                {
                    departments = departments.OrderBy(x => x.description).ToList();
                }

                return new OkObjectResult(new
                {
                    JobTitles = jobTitles,
                    Departments = departments,
                    LastResults = lastResults,
                    Pathways = pathways,
                    Registrations = registrations,
                    Countries = countries,
                    Sources = sources,
                    DealClosedOption = dealClosedOption,
                    NotesCategories = notesCategories,
                    DocumentCategories = documentCategories,
                    LeadStatuses = leadStatuses,
                    CourseLevels = courseLevels,
                    TrainingProviders = trainingProviders,
                    Salutations = salutations,
                    CourseLevelEnquiries = courseLevelEnquiries,
                    LearnerEnrolleds = learnerEnrolled,
                    SalesAdvisors = salesAdvisors,
                    CompanyNoteCategory= companyNoteCategory,
                    CompanyGroup = companyGroup,
                    Title=title,
                    Company=company,
                    NewSalesAdvisors = newSalesAdvisors
                });
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllCourses")]
        public IActionResult GetAllCourses()
        {
            try
            {
                var courses = _leadRepository.GetAllCourses();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllCourseLevels")]
        public IActionResult GetAllCourseLevels()
        {
            try
            {
                var courseLevels = _leadRepository.GetAllCourseLevels();
                return Ok(courseLevels);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllCourseLevelEnquiries")]
        public IActionResult GetAllCourseLevelEnquiries()
        {
            try
            {
                var courseLevelEnquiries = _leadRepository.GetAllCourseLevelEnquiries();
                return Ok(courseLevelEnquiries);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllContactsForDropdown")]
        public IActionResult GetAllContactsForDropdown()
        {
            try
            {
                var contacts = _leadRepository.GetAllContactsForDropdown();
                return Ok(contacts);
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
                var contact = _leadRepository.GetContactById(id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllActivityTypes")]
        public IActionResult GetAllActivityTypes()
        {
            try
            {
                var activityTypes = _leadRepository.GetAllActivityTypes();
                return Ok(activityTypes);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        #endregion
               
        #region Lead Notes

        [HttpPost]
        [Route("InsertLeadNotes")]
        [HistoryFilter]
        public bool InsertLeadNotes(Notes note)
        {
            try
            {
                if (note.Note_Id > 0)
                {
                    note.Note_UpdatedByUserId = UserId;
                    note.Note_UpdatedByUserName = UserName;
                }
                else
                {
                    note.Note_CreatedByUserId = UserId;
                    note.Note_CreatedByUserName = UserName;
                }
                note.Note_DateCreated = null;
                return _leadRepository.InsertUpdateLeadNoteRecord(note);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllLeadNotes")]
        public DataSourceResult GetAllLeadNotes([DataSourceRequest]DataSourceRequest request, int lead_Id)
        {
            try
            {
                return _leadRepository.GetAllLeadNotes(lead_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetNoteById")]
        public IActionResult GetNoteById(int id)
        {
            try
            {
                var contact = _leadRepository.GetNoteById(id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetCompanyContact")]
        public IActionResult GetCompanyContact(int id)
        {
            try
            {
                var contact = _leadRepository.GetCompantContactDeatil(id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetCompanyWorkPlace")]
        public IActionResult GetCompanyWorkPlace(int id)
        {
            try
            {
                var contact = _leadRepository.GetCompanyWorkPlace(id);
                return Ok(contact);
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
                var retVal = _leadRepository.DeleteNoteById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("DeleteCompanyContactById")]
        [HistoryFilter]
        public IActionResult DeleteCompanyContactById(int id)
        {
            try
            {
                var retVal = _leadRepository.DeleteNoteById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region Lead Documents

        [HttpGet]
        [Route("GetAllLeadDocuments")]
        public DataSourceResult GetAllLeadDocuments([DataSourceRequest]DataSourceRequest request, int lead_Id)
        {
            try
            {
                return _leadRepository.GetAllLeadDocuments(lead_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteDocumentById")]
        [HistoryFilter]
        public IActionResult DeleteDocumentById(int id)
        {
            try
            {
                var retVal = _leadRepository.DeleteDocumentById(id, UserId, UserName);
                return Ok(retVal);
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
                Documents document = JsonConvert.DeserializeObject<Documents>(Request.Form["documentData"]);
                long uploaded_size;
                int iCounter;
                string sFiles_uploaded;

                /// method call to upload document.
                var documents = _document.Upload(document, Convert.ToInt32(document.Document_Id_Lead), Request, out uploaded_size, out iCounter, out sFiles_uploaded);

                foreach (var doc in documents)
                {
                    document.Document_CreatedByUserId = UserId;
                    document.Document_CreatedByUserName = UserName;
                    document.Document_UpdatedByUserId = UserId;
                    document.Document_UpdatedByUserName = UserName;
                    document.document_file_extension = doc.document_file_extension;
                    document.document_mime_type = doc.document_mime_type;
                    document.document_object = doc.document_object;
                    document.document_size = doc.document_size;
                    document.Document_Name = doc.document_file_name;
                    _leadRepository.InsertUpdateLeadDocumentRecord(document);
                }

                string message = "Upload successful!\n files uploaded:" + iCounter + "\nsize:" + uploaded_size + "\n" + sFiles_uploaded;
                return true;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("DownloadFile")]
        [HistoryFilter]
        public IActionResult DownloadFile(int document_Id)
        {
            try
            {
                byte[] bytes;
                string fileName, contentType;
                var result = _document.DownloadFile(document_Id, 1);
                bytes = result.document_object;
                contentType = result.document_mime_type;
                fileName = result.document_file_name;
                return File(bytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        #endregion

        #region Lead Activity

        [HttpPost]
        [Route("InsertUpdateLeadActivityRecord")]
        [HistoryFilter]
        public bool InsertUpdateLeadActivityRecord(Activity activity)
        {
            try
            {
                if (activity.Activity_Id > 0)
                {
                    activity.Activity_UpdatedByUserId = UserId;
                    activity.Activity_UpdatedByUserName = UserName;
                }
                else
                {
                    activity.Activity_CreatedByUserId = UserId;
                    activity.Activity_CreatedByUserName = UserName;
                }
                activity.Activity_DateCreated = null;

                bool retVal = _leadRepository.InsertUpdateLeadActivityRecord(activity);
                return retVal;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InserLeadActivityAndSendEmail")]
        [HistoryFilter]
        public async Task<bool> InserLeadActivityAndSendEmail()
        {
            try
            {
                Activity activity = JsonConvert.DeserializeObject<Activity>(Request.Form["activityData"]);
                List<string> defaultAttachments = JsonConvert.DeserializeObject<List<string>>(Request.Form["defaultAttachments"]);

                for (int i = 0; i < defaultAttachments.Count; ++i)
                {
                    defaultAttachments[i] = _hostingEnvironment.WebRootPath + defaultAttachments[i];
                }
                var uploaded_files = Request.Form.Files;

                activity.Activity_CreatedByUserId = UserId;
                activity.Activity_CreatedByUserName = UserName;
                activity.Activity_DateCreated = null;

                byte[] bytes = new byte[uploaded_files.Count + defaultAttachments.Count];
                var _count = 0;
                foreach (var file in uploaded_files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            bytes[_count] = fileBytes.ToArray()[_count];
                            _count++;
                        }
                    }
                }

                if (defaultAttachments != null && defaultAttachments.Count > 0)
                {
                    foreach (var file in defaultAttachments)
                    {
                        using (var ms = new MemoryStream())
                        {
                            var fileBytes = System.IO.File.ReadAllBytes(file);
                            bytes[_count] = fileBytes.ToArray()[_count];
                            _count++;
                        }
                    }
                }

                QueueEmailModel queueEmailModel = new QueueEmailModel()
                {
                    qe_attachments = bytes,
                    qe_bcc = "",
                    qe_body = activity.Activity_EmailBody,
                    qe_cc = "",
                    qe_created_by = activity.Activity_CreatedByUserName,
                    qe_created_date = DateTime.Now,
                    qe_from = _fromEmailId, // getting from config file.
                    qe_id = 0,
                    qe_is_html = true,
                    qe_mod_date = DateTime.Now,
                    qe_priority = 2, // default set
                    qe_sent = false, // default is 0 and when the email will be gone this will set to 1
                    qe_status = "Sent",
                    qe_subject = activity.Activity_EmailSubject,
                    qe_to = activity.Activity_EmailTo
                };

                bool retVal = _leadRepository.InsertUpdateLeadActivityRecord(activity);

                if (activity.Activity_Id_Type == 2 && retVal)
                {
                    //send email to user
                    await _emailSender.SendEmailWithAttachmentsAsync(activity.Activity_EmailTo, activity.Activity_EmailSubject, activity.Activity_EmailBody, uploaded_files.ToList(), defaultAttachments);
                    _leadRepository.InsertQueueEmail(queueEmailModel);
                    _leadRepository.UpdateSentEmail(queueEmailModel);
                }

                return retVal;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllLeadActivities")]
        public DataSourceResult GetAllLeadActivities([DataSourceRequest]DataSourceRequest request, int lead_Id)
        {
            try
            {
                return _leadRepository.GetAllLeadActivities(lead_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetActivityById")]
        public IActionResult GetActivityById(int id)
        {
            try
            {
                var activity = _leadRepository.GetActivityById(id);
                return Ok(activity);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteActivityById")]
        [HistoryFilter]
        public IActionResult DeleteActivityById(int id)
        {
            try
            {
                var retVal = _leadRepository.DeleteActivityById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region "tab CSL"


        [HttpGet]
        [Route("GetAllOutcomesTypes")]
        public IActionResult GetAllOutcomesTypes()
        {
            try
            {
                var activityTypes = _leadRepository.GetAllOutcomesTypes();
                return Ok(activityTypes);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllLeadCSL")]
        public DataSourceResult GetAllLeadCSL([DataSourceRequest]DataSourceRequest request, int lead_Id)
        {
            try
            {
                return _leadRepository.GetAllLeadCSL(lead_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
         

        [HttpPost]
        [Route("AddUpdateCSL")]
        [HistoryFilter]
        public bool AddUpdateCSL(CommunicationSummaryLogViewModel csl)
        {
            try
            {
                if (csl.CSL_Id > 0)
                {
                    csl.CSL_UpdatedByUserId = UserId;
                    csl.CSL_UpdatedByUserName = UserName;
                }
                else
                {
                    csl.CSL_CreatedByUserId = UserId;
                    csl.CSL_CreatedByUserName = UserName;
                    csl.CSL_DateCreated = DateTime.Now;

                }

                bool retVal = _leadRepository.AddUpdateCSL(csl);
                return retVal;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteCSLById")]
        [HistoryFilter]
        public IActionResult DeleteCSLById(int id)
        {
            try
            {
                var retVal = _leadRepository.DeleteCSLById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion 
        
        #region Sales Advisor

        [HttpGet]
        [Route("GetAllSalesAdvisorUsers")]
        public IActionResult GetAllSalesAdvisorUsers()
        {
            try
            {
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var salesAdvisors = _Lead_UserRepository.GetAllSalesAdvisorUsers(_isAdmin);
                return Ok(salesAdvisors);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAvailableSalesAdvisorIdRota")]
        public IActionResult GetAvailableSalesAdvisorIdRota()
        {
            try
            {
                var salesAdvisorId = _leadRepository.GetAvailableSalesAdvisorIdRota();
                return Ok(salesAdvisorId);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion
        
        #region access skills api create lead
        [HttpPost]
        [Route("Create")]

        public string Create(string uid, string pwd, Lead lead)
        {
            if (!_loginRepository.ValidateUser(uid, pwd))
            {
                return "Invalid User";
            }
            lead.Lead_Id_LastResult = 2;
            lead.Lead_Contact_Id_Department = 0;

            /*    {
    "Lead_ContactName": 'test_joedoe_from_website_1',
    "Lead_Email": 'testing@fdesigns.co.uk',
    "Lead_Contact_Mobile": '070707070707', 
    //"Lead_Contact_Id_JobTitle": 1,
    "Lead_JobTitle": 'Owner',
    "Lead_CourseTitle": 'E-Learning CPD',
    //"Lead_Id_Course": lead.CourseId,
    "Lead_Contact_WebSubject": 'ignore',
    "Lead_Contact_WebMessage": 'ignore'
 }*/ 
            //int res = lead.Lead_Id_LastResult;
            //int dep = lead.Lead_Contact_Id_Department;
            //NOT USED ANYMORE header =19 but no data!!!
            // string sal = lead.Lead_Contact_Salutation;
            //if (lead.Lead_Id_LastResult == 0)
            //{
            //    lead.Lead_Id_LastResult = 2; // this is set to NA which is deleted ddl options: (header=3) id=7	 value=2	title=[NA]	deleted=yes
            //}
            //if (lead.Lead_Contact_Id_Department == 0)
            //{
            //    lead.Lead_Contact_Id_Department = 0;// this is NOT set ddl options: (header=2) - no value assigned
            //}



            //if (string.IsNullOrWhiteSpace(lead.Lead_Contact_Salutation))
            //{
            //    lead.Lead_Contact_Salutation = "Mr.";
            //}
            bool issuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    issuccess = _leadRepository.Insert(lead);
                }
                catch (Exception ex)
                {
                    LogException.Log(ex, this.ControllerContext);
                    throw new Exception(ex.ToString());
                }
                if (issuccess)
                {
                    return "Lead created successfully";
                }
                else
                {
                    return "Something went wrong while saving lead";
                }
            }
            else
            {
                return "Some mandatory fields are missing";
            }
        }


        #endregion

        #region access skills api import leads via cronjob

        [HttpPost]
        [Route("Import")]
        public async Task<string> Import(string uid, string pwd, DateTime dt, int total, string guid)
        {

            if (total == -1) total = 10;
            if (guid == "") guid = "Yasar";

            string leads_json = "";
            List<Lead> webLeads = new List<Lead>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://www.accessskills.co.uk/api/api.php?datefrom=2021-01-25&dateto=2025-08-05&proc=0&guid="+ guid + "&total="+ total.ToString() + "&dt=" + dt.ToString()))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        leads_json = apiResponse;
                        webLeads = JsonConvert.DeserializeObject<List<Lead>>(apiResponse);

                        if (webLeads!= null && webLeads.Count > 0)
                        {
                            foreach (Lead lead in webLeads)
                            {
                                bool is_success = _leadRepository.Insert(lead);

                                if (is_success)
                                {
                                    int iStatus = 1;
                                    //update back the web api
                                    using (var response2 = await httpClient.GetAsync("https://www.accessskills.co.uk/api/apiupdate.php?lead_created=" + iStatus.ToString() + "&formid=" + lead.lead_web_id.ToString() + "&guid=Yasar"))
                                    {
                                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                                        {
                                            string error = "all good";
                                        }
                                    }
                                }
                                //try
                                //{
                                //    issuccess = _leadRepository.Insert(lead);
                                //}
                                //catch (Exception ex)
                                //{
                                //    //LogException.Log(ex, this.ControllerContext);
                                //    //throw new Exception(ex.ToString());
                                //}
                            }
                        }
                        
                       
                        //Console.Write(apiResponse);
                    }
                    else
                        return response.StatusCode.ToString();
                }
            }
            return leads_json ;
        }

        [HttpPost]
        [Route("Importsss")]

        public string Import1(string uid, string pwd, DateTime dt)
        {
            //if (!_loginRepository.ValidateUser(uid, pwd))
            //{
            //    return "Invalid User";
            //}

             List<Lead> leads = new List<Lead>();
            //using (var httpClient = new HttpClient())
            //{
            //    using (var response = httpClient.Get("https://www.accessskills.co.uk/api/api.php?datefrom=2021-01-25&dateto=2025-08-05&proc=1&guid=Yasar"))
            //    {
            //        string apiResponse = await response.Content.ReadAsStringAsync();
            //        leads = JsonConvert.DeserializeObject<List<Lead>>(apiResponse);
            //    }
            //}
            //return View(reservationList);

            return leads.ToString();

            Lead lead = new Lead();
            lead.Lead_Id_LastResult = 2;
            lead.Lead_Contact_Id_Department = 0;

            /*    {
    "Lead_ContactName": 'test_joedoe_from_website_1',
    "Lead_Email": 'testing@fdesigns.co.uk',
    "Lead_Contact_Mobile": '070707070707', 
    //"Lead_Contact_Id_JobTitle": 1,
    "Lead_JobTitle": 'Owner',
    "Lead_CourseTitle": 'E-Learning CPD',
    //"Lead_Id_Course": lead.CourseId,
    "Lead_Contact_WebSubject": 'ignore',
    "Lead_Contact_WebMessage": 'ignore'
 }*/
            //int res = lead.Lead_Id_LastResult;
            //int dep = lead.Lead_Contact_Id_Department;
            //NOT USED ANYMORE header =19 but no data!!!
            // string sal = lead.Lead_Contact_Salutation;
            //if (lead.Lead_Id_LastResult == 0)
            //{
            //    lead.Lead_Id_LastResult = 2; // this is set to NA which is deleted ddl options: (header=3) id=7	 value=2	title=[NA]	deleted=yes
            //}
            //if (lead.Lead_Contact_Id_Department == 0)
            //{
            //    lead.Lead_Contact_Id_Department = 0;// this is NOT set ddl options: (header=2) - no value assigned
            //}



            //if (string.IsNullOrWhiteSpace(lead.Lead_Contact_Salutation))
            //{
            //    lead.Lead_Contact_Salutation = "Mr.";
            //}
            bool issuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    issuccess = _leadRepository.Insert(lead);
                }
                catch (Exception ex)
                {
                    LogException.Log(ex, this.ControllerContext);
                    throw new Exception(ex.ToString());
                }
                if (issuccess)
                {
                    return "Lead created successfully";
                }
                else
                {
                    return "Something went wrong while saving lead";
                }
            }
            else
            {
                return "Some mandatory fields are missing";
            }
        }


        #endregion

        #region Duplicate Lead

        [HttpPost]
        [Route("CheckDuplicateLead")]
        public IActionResult CheckDuplicateLead(Lead lead)
        {
            try
            {
                LeadViewModel duplicateLead = new LeadViewModel();
                DuplicateLeadModel _duplicateLead = new DuplicateLeadModel();
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var duplicateLeadsCount = _leadRepository.CheckDuplicateLead(lead.Lead_ContactName, lead.Lead_Contact_Email, lead.Lead_Contact_Mobile, UserId, _isAdmin);

                if (lead.Lead_Id == 0 && duplicateLeadsCount != null)
                {
                    //get lead by lead id
                    duplicateLead = _leadRepository.GetLeadByLeadId(duplicateLeadsCount.Lead_Id);

                    var lastResults = _Lead_ListRepository.GetDropdownOptionsByHeaderName("LastResults");
                    var leadStatuses = _Lead_ListRepository.GetDropdownOptionsByHeaderName("LeadStatus");
                    var courseLevels = _Lead_ListRepository.GetDropdownOptionsByHeaderName("CourseLevelsEnquiry");
                    var courseLevel = "";
                    var lastResult = "";
                    var status = "";
                    if (lead.Lead_Id_CourseLevel > 0)
                    {
                        courseLevel = courseLevels.Where(x => x.id == lead.Lead_Id_CourseLevel).FirstOrDefault().description;
                    }
                    if (lead.Lead_Id_LastResult > 0)
                    {
                        lastResult = lastResults.Where(x => x.id == lead.Lead_Id_LastResult).FirstOrDefault().description;
                    }
                    if (lead.Lead_Id_Status > 0)
                    {
                        status = leadStatuses.Where(x => x.id == lead.Lead_Id_Status).FirstOrDefault().description;
                    }

                    if (duplicateLead != null)
                    {
                        _duplicateLead = new DuplicateLeadModel
                        {
                            ContactName = lead.Lead_ContactName,
                            Duplicate_ContactName = duplicateLead.Lead_ContactName,
                            Mobile = lead.Lead_Contact_Mobile,
                            Duplicate_Mobile = duplicateLead.Lead_Contact_Mobile,
                            CourseLevel = courseLevel,
                            Duplicate_CourseLevel = duplicateLead.Lead_CourseTitle,
                            LastResult = lastResult,
                            Duplicate_LastResult = duplicateLead.Lead_LastResult,
                            Status = status,
                            Duplicate_Status = duplicateLead.Lead_Status,
                            EnquiryDate = lead.Lead_DateOfEnquiry,
                            Duplicate_EnquiryDate = duplicateLead.Lead_DateOfEnquiry,
                            //Lead_Id = _lead.LD_Id_Lead,
                            Duplicate_Lead_Id = duplicateLead.Lead_Id,
                            //LD_Id = _lead.LD_Id,
                            Email = lead.Lead_Contact_Email,
                            Duplicate_Email = duplicateLead.Lead_Contact_Email,
                            Sales_Advisor_Id = lead.Lead_Id_SalesAdvisor,
                            Duplicate_Sales_Advisor_Id = duplicateLead.Lead_Id_SalesAdvisor,
                            Duplicate_Lead_Contact_Company_Postcode = duplicateLead.Lead_Contact_Company_Postcode,
                            Duplicate_Lead_Contact_Company_Name = duplicateLead.Lead_Contact_Company_Name,
                            Lead_Contact_Company_Postcode = lead.Lead_Contact_Company_Postcode,
                            Lead_Contact_Company_Name = lead.Lead_Contact_Company_Name,
                            Duplicate_is_validated_duplicate = duplicateLead.Lead_is_validated_duplicate,
                            Lead_is_validated_duplicate = lead.Lead_is_validated_duplicate
                        };
                    }
                }
                return Ok(_duplicateLead);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetDuplicateLeadLeadById")]
        public IActionResult GetDuplicateLeadLeadById(int id)
        {
            try
            {
                var leads = _leadRepository.GetDuplicateLeadLeadById(id);
                return Ok(leads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetDuplicateLeadsById")]
        public IActionResult GetDuplicateLeadsById(int id)
        {
            try
            {
                var leads = _leadRepository.GetDuplicateLeadsById(id);
                return Ok(leads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("DeleteDuplicateLeadById")]
        [HistoryFilter]
        public IActionResult DeleteDuplicateLeadById(int ld_id, int lead_id)
        {
            try
            {
                //var retVal = _leadRepository.DeleteDuplicateLeadById(ld_id, lead_id, lead_duplicate_id);
                var retVal = _leadRepository.DeleteSingleDuplicateLead(ld_id, lead_id);
                

                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("UpdateSalesAdvisor")]
        [HistoryFilter]
        public IActionResult UpdateSalesAdvisor(int lead_id, int sales_advisor_id)
        {
            try
            {
                var retVal = _leadRepository.UpdateSalesAdvisor(lead_id, sales_advisor_id);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("RemoveLeadDuplicate")]
        [HistoryFilter]
        public IActionResult RemoveLeadDuplicate(int lead_id, int ld_id)
        {
            try
            {
                var retVal = _leadRepository.RemoveLeadDuplicate(lead_id, ld_id);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region History

        [HttpGet]
        [Route("GetAllHistory")]
        public DataSourceResult GetAllHistory([DataSourceRequest]DataSourceRequest request, int lead_id)
        {
            try
            {
                return _leadRepository.GetAllHistory(UserId, lead_id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetDuplicateLead")]
        public IActionResult GetDuplicateLead(int id)
        {
            try
            {
                var leads = _leadRepository.GetDuplicateLead(id);
                return Ok(leads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion

        #region File upload

        [HttpPost]
        [Route("UploadLead")]
        public List<LeadFileUpload> UploadLead(List<LeadFileUpload> leadFileUploads)
        {
            var result = _leadRepository.BulkInsert(leadFileUploads, UserId);
            return result;
            //try
            //{
            //     var result = _leadRepository.BulkInsert(leadFileUploads,UserId);
            //     return 0;
            //}
            //catch (Exception ex)
            //{
            //    LogException.Log(ex, this.ControllerContext);
            //    throw new Exception(ex.ToString());
            //}
        }


        [HttpPost]
        [Route("NoteImport")]
        public List<ImportNote> NoteImport(List<ImportNote> ImportNotes)
        {
            var result = _leadRepository.BulkImportNote(ImportNotes, UserId, GetSecurityRoleName, UserName);
            return result;
            //try
            //{
            //     var result = _leadRepository.BulkInsert(leadFileUploads,UserId);
            //     return 0;
            //}
            //catch (Exception ex)
            //{
            //    LogException.Log(ex, this.ControllerContext);
            //    throw new Exception(ex.ToString());
            //}
        }


        [HttpPost]
        [Route("StatusImport")]
        public List<ImportStatus> StatusImport(List<ImportStatus> ImportNotes)
        {
            var result = _leadRepository.BulkImportStatus(ImportNotes, UserId, GetSecurityRoleName);
            return result;
            //try
            //{
            //     var result = _leadRepository.BulkInsert(leadFileUploads,UserId);
            //     return 0;
            //}
            //catch (Exception ex)
            //{
            //    LogException.Log(ex, this.ControllerContext);
            //    throw new Exception(ex.ToString());
            //}
        }

        [HttpPost]
        [Route("LastResultImport")]
        public List<ImportLastResult> LastResultImport(List<ImportLastResult> ImportNotes)
        {
            var result = _leadRepository.BulkImportLastResult(ImportNotes, UserId, GetSecurityRoleName);
            return result;
            //try
            //{
            //     var result = _leadRepository.BulkInsert(leadFileUploads,UserId);
            //     return 0;
            //}
            //catch (Exception ex)
            //{
            //    LogException.Log(ex, this.ControllerContext);
            //    throw new Exception(ex.ToString());
            //}
        }
        #endregion

        #region "tab Task"


        [HttpGet]
        [Route("GetAllActivityAppointmentMeetingActions")]
        public IActionResult GetAllActivityAppointmentMeetingActions()
        {
            try
            {
                var activityTypes = _leadRepository.GetAllActivityAppointmentMeetingActions();
                return Ok(activityTypes);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllLeadTask")]
        public DataSourceResult GetAllLeadTask([DataSourceRequest]DataSourceRequest request, int lead_Id)
        {
            try
            {
                return _leadRepository.GetAllLeadTask(lead_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpPost]
        [Route("AddUpdateTask")]
        [HistoryFilter]
        public bool AddUpdateTask(TaskViewModel task)
        {
            try
            {
                task.task_id_user = UserId;

                if (task.task_id > 0)
                { 
                    task.task_updated_by_user_id = UserId;
                    task.task_updated_by_username = UserName;
                }
                else
                {
                    task.task_created_by_user_id = UserId;
                    task.task_created_by_username = UserName;
                    task.task_date_created = DateTime.Now;

                }

                bool retVal = _leadRepository.CheckExistingTask(task);
                if (retVal == true)
                {
                    return false;
                }
                
                retVal = _leadRepository.AddUpdateTask(task);
                 
                return  retVal;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }



        
        [HttpPost]
        [Route("CheckExistingTask")]
        [HistoryFilter]
        public bool CheckExistingTask(TaskViewModel task)
        {
            try
            {
                task.task_id_user = UserId;

                if (task.task_id > 0)
                {
                    task.task_updated_by_user_id = UserId;
                    task.task_updated_by_username = UserName;
                }
                else
                {
                    task.task_created_by_user_id = UserId;
                    task.task_created_by_username = UserName;
                    task.task_date_created = DateTime.Now;

                }

                bool retVal = _leadRepository.CheckExistingTask(task);
                return retVal;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("DeleteTaskById")]
        [HistoryFilter]
        public IActionResult DeleteTaskById(int id)
        {
            try
            {
                var retVal = _leadRepository.DeleteTaskById(id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
         
        [HttpGet]
        [Route("TaskDone")]
        [HistoryFilter]
        public IActionResult TaskDone(long id, long user_id, long lead_id)
        {
            try
            {
                var retVal = _leadRepository.TaskDone(id, lead_id,  UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        #endregion 

        #region "Manage Duplicate Leads Page"

        [HttpGet]
        [Route("GetAllDuplicateLeads")]
        public DataSourceResult GetAllDuplicateLeads([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var permissions = _roleAdminRepository.FetchRolePermissionsByRoleId(GetSecurityRoleId);
                bool _isAdmin = (permissions == null) ? false : (bool)permissions["MD_LEAD_MSTR"];
                var coll = _leadRepository.GetAllDuplicateLeads(UserId, _isAdmin);
                return coll.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("RemoveFromDuplicates")]
        [HistoryFilter]
        public IActionResult RemoveFromDuplicates(int ld_id)
        {
            try
            {
                var retVal = _leadRepository.RemoveFromDuplicates(ld_id);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("DeleteSingleDuplicateLead")]
        [HistoryFilter]
        public IActionResult DeleteSingleDuplicateLead(int ld_id, int lead_id)
        {
            try
            {
                var retVal = _leadRepository.DeleteSingleDuplicateLead(ld_id, lead_id);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        #endregion

        
        public string SendLastResultEamil(Lead lead)
        {
            string status = "success";
            try
            {
                var db_opt_value = _Lead_ListRepository.GetDropdownOptionsByHeaderNameAndValue("LastResults", lead.Lead_Id_LastResult.ToString());

                var user = _Lead_UserRepository.GetUserById(lead.Lead_Id_SalesAdvisor);
                var emailTemplate = _emailTemplateRepository.GetEmailTemplateByCode("LAST_RST");
                var emailModel = new QueueEmails();
                if (emailTemplate != null)
                {
                    if (!string.IsNullOrEmpty(emailTemplate.ET_Body))
                    {

                        emailTemplate.ET_Body = emailTemplate.ET_Body.Replace("{SaleAdviser_Name}", user.Users_DisplayName);
                        emailTemplate.ET_Body = emailTemplate.ET_Body.Replace("{LeadId}", lead.Lead_Id.ToString());
                        emailTemplate.ET_Body = emailTemplate.ET_Body.Replace("{ContactName}", lead.Lead_ContactName);
                        emailTemplate.ET_Body = emailTemplate.ET_Body.Replace("{LastResultStatus}", db_opt_value.description);
                        emailTemplate.ET_Body = emailTemplate.ET_Body.Replace("{LastDateTime}",DateTime.Now.Date.ToString());
                        
                        emailModel.qe_bcc = toEmailId;
                        emailModel.qe_body = emailTemplate.ET_Body;
                        emailModel.qe_cc = "";
                        emailModel.qe_created_by = UserId.ToString();
                        emailModel.qe_created_date = DateTime.Now;
                        emailModel.qe_from = fromEmailId;
                        emailModel.qe_is_html = true;
                        emailModel.qe_mod_date = DateTime.Now;
                        emailModel.qe_priority = 1;
                        emailModel.qe_sent = false;
                        emailModel.qe_status = "";
                        emailModel.qe_subject = "Last Result Status Notification";
                        emailModel.qe_to = user.Users_Email;
                        emailModel.qe_system_id = 1;
                        _emailTemplateRepository.SaveQueueEmail(emailModel);

                        // string url_template = emailTemplate.ET_Body.Replace("{RESET_URL}", callbackUrl).Replace("[USER_FULLNAME]", email);
                        _emailSender.SendHtmlEmailNotificationAsync(user.Users_Email, "Last Result Status Notification", emailTemplate.ET_Body);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return status;
        }

        [HttpGet]
        [Route("GetLastResultForSa")]

        public IActionResult GetLastResultForSa(int value, string title)
        {
            var result = _Lead_ListRepository.GetLastResultBySa(value, title);
            return Ok(result);
        }
    }
}