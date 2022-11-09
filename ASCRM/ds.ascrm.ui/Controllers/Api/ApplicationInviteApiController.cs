using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.applications.invites;
using ds.portal.applications.invites.Models;
using ds.portal.leads;
using ds.portal.users;
using ds.portal.users.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/ApplicationInviteApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ApplicationInviteApiController : ControllerBase
    {
        private readonly IAppInvitesRepository _appInvitesRepository;
        readonly ILogger<ApplicationInviteApiController> _log;
        LogException LogException;
        IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMailService _emailSender;
        private readonly ILead_UserRepository _lead_UserRepository;

        public ApplicationInviteApiController(IAppInvitesRepository appInvitesRepository
            , ILogger<ApplicationInviteApiController> log, IConfigurationRepository iConfig,
            IEmailTemplateRepository emailTemplateRep, IMailService emailSender, ILead_UserRepository lead_UserRep)
        {
            _appInvitesRepository = appInvitesRepository;
            _log = log;
            LogException = new LogException(iConfig, _log);
            _emailTemplateRepository = emailTemplateRep;
            _emailSender = emailSender;
            _lead_UserRepository = lead_UserRep;
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
        [Route("GetRandom8DigitPassword")]
        [HistoryFilter]
        public string GetRandom8DigitPassword()
        {
            try
            {
                var randomPassword = GenerateRandomPasswordUsingGUID(8);
                return randomPassword;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        private string GenerateRandomPasswordUsingGUID(int length)
        {
            // Get the GUID
            string guidResult = Guid.NewGuid().ToString();

            // Remove the hyphens
            guidResult = guidResult.Replace("-", string.Empty);

            // Make sure length is valid
            /*   if (length <= 0 || length > guidResult.Length)
                   throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
               */
            // Return the first length bytes
            return guidResult.Substring(0, length).ToUpper();
        }        
        [HttpPost]
        [Route("InsertUpdateApplicationInvite")]
        [HistoryFilter]
        public bool InsertUpdateApplicationInvite(AppInvites appInvite)
        {
            try
            {
                bool retValInvite = false;
                bool retValUser = false;
                bool isNew = false;

                if (appInvite.api_id > 0)
                {
                    appInvite.api_updatedbyuserid = UserId;
                    appInvite.api_updatedbyusername = UserName;

                    retValInvite = _appInvitesRepository.InsertUpdateApplicationInvite(appInvite);
                }
                else
                {
                    appInvite.api_createdbyuserid = UserId;
                    appInvite.api_createdbyusername = UserName;

                    Users user = new Users();
                    user.Users_UserName = appInvite.api_email.Trim().ToLower();
                    user.Users_Password = appInvite.api_password.Trim();
                    user.Users_Email = appInvite.api_email.Trim().ToLower();
                    user.Users_IsActive = 1;
                    user.Users_RoleId = 14;
                    user.Users_Id_CourseLevel = appInvite.api_id_courselevel;
                    user.Users_DisplayName = appInvite.api_firstname + "  " + appInvite.api_lastname;
                    retValUser = _lead_UserRepository.InsertUpdateUserRecord(user);

                    var data = _appInvitesRepository.LoadConfigData();
                    if (retValUser)
                    {
                        Hashtable ht = new Hashtable();
                        ht.Add("[LEARNER]", appInvite.api_firstname.Trim() + " " + appInvite.api_lastname.Trim());
                        ht.Add("[COURSELEVEL]", appInvite.api_courseleveltitle.Trim());
                        ht.Add("[LEARNEREMAIL]", appInvite.api_email.Trim().ToLower());
                        ht.Add("[LEARNERPASSWORD]", appInvite.api_password.Trim());
                        ht.Add("[APPPORTALURL]", data.config_value);

                        Tuple<string, string> tuple = _emailTemplateRepository.GetSubjectAndEmailTemplateBodyReplacedByCode("API001", ht);
                        appInvite.api_emailbody = tuple.Item2;

                        retValInvite = _appInvitesRepository.InsertUpdateApplicationInvite(appInvite);
                          _emailSender.SendEmail(appInvite.api_email.Trim().ToLower(), tuple.Item1, tuple.Item2);


                    }
                }
                return retValInvite;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("ResendApplicationInviteById")]
        [HistoryFilter]
        public bool ResendApplicationInviteById(int id)
        {
            try
            {
                var appInvite = _appInvitesRepository.GetApplicationInviteById(id);

                Hashtable ht = new Hashtable();
                Tuple<string, string> tuple = _emailTemplateRepository.GetSubjectAndEmailTemplateBodyReplacedByCode("API001", ht);

                return _emailSender.SendEmail(appInvite.api_email.Trim().ToLower(), tuple.Item1, appInvite.api_emailbody);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllApplicationInvites")]
        [HistoryFilter]
        public DataSourceResult GetAllApplicationInvites([DataSourceRequest]DataSourceRequest request,int lead_Id)
        {
            try
            {
                return _appInvitesRepository.GetAllApplicationInvites().ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllApplicationInvitesForLead")]
        [HistoryFilter]
        public DataSourceResult GetAllApplicationInvitesForLead([DataSourceRequest] DataSourceRequest request, int lead_Id)
        {
            try
            {
                return _appInvitesRepository.GetAllApplicationInvitesForLead(lead_Id).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetApplicationInviteById")]
        [HistoryFilter]
        public AppInvites GetApplicationInviteById(int id)
        {
            try
            {
                return _appInvitesRepository.GetApplicationInviteById(id);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetApplicationInviteTemplate")]
        [HistoryFilter]
        public IActionResult GetApplicationInviteTemplate(string name, string password,string coruselevel,string email )
        {
            try
            {
                var data = _appInvitesRepository.LoadConfigData();
                Hashtable ht = new Hashtable();
                ht.Add("[LEARNER]", name);
                ht.Add("[COURSELEVEL]", coruselevel);
                ht.Add("[LEARNEREMAIL]", email);
                ht.Add("[LEARNERPASSWORD]", password);
                ht.Add("[APPPORTALURL]", data.config_value);

               Tuple<string, string> tuple = _emailTemplateRepository.GetSubjectAndEmailTemplateBodyReplacedByCode("API001", ht);
               var data1=  tuple.Item2;


                return Ok(data1);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("DeleteApplicationInviteById")]
        [HistoryFilter]
        public bool DeleteApplicationInviteById(int id)
        {
            try
            {
                return _appInvitesRepository.DeleteApplicationInviteById(id, UserId, UserName);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}