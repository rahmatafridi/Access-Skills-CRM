using crm.portal.Interfaces.User;
using crm.portal.OscaModel;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.ui.Models;
using ds.portal.users;
using ds.portal.users.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/UserAdminApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class UserAdminApiController : ControllerBase
    {
        private readonly ILead_UserRepository _userRepository;
        private readonly ILoginRepository _loginRepository;
        private readonly IMailService _emailSender;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        readonly ILogger<UserAdminApiController> _log;
        private IPortalUserRepository _portalUserRepository;
        LogException LogException;
        public UserAdminApiController(IPortalUserRepository portalUserRepository, ILead_UserRepository userRepository
            , ILoginRepository loginRepository, IMailService emailSender, IEmailTemplateRepository emailTemplateRepository
            , ILogger<UserAdminApiController> log, IConfigurationRepository iConfig)
        {
            _userRepository = userRepository;
            _loginRepository = loginRepository;
            _emailSender = emailSender;
            _emailTemplateRepository = emailTemplateRepository;
            _log = log;
            _portalUserRepository = portalUserRepository;
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
        [Route("GetAllUsers")]
        public DataSourceResult GetAllUsers([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return _userRepository.GetAllUsers().ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertUpdateUserRecord")]
        [HistoryFilter]
        public bool InsertUpdateUserRecord(Users user)
        {
            try
            {
                return _userRepository.InsertUpdateUserRecord(user);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetUserById")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var contact = _userRepository.GetUserById(id);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("AwardingBody")]
        public IActionResult AwardingBody(int id)
        {
            try
            {
                var contact = _portalUserRepository.GetAwardingBodyLearners();
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        

        [HttpGet]
        [Route("GetTrainingProviderUser")]
        public IActionResult GetTrainingProviderUser(int id,string type)
        {
            try
            {
                var contact = _portalUserRepository.GetProviderAwadingBodyUsers(id,type);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("TrainingProviders")]
        public IActionResult TrainingProviders(int id)
        {
            try
            {
                var contact = _portalUserRepository.GetTrainingProviders();
                return Ok(contact);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("CheckEmailAlreadyExists")]
        public IActionResult CheckEmailAlreadyExists(Users user)
        {
            try
            {
                var isExists = _userRepository.CheckEmailAlreadyExists(user.Users_ID, user.Users_UserName);
                return Ok(isExists);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("CheckEmailAlreadyExistsPortal")]
        public IActionResult CheckEmailAlreadyExistsPortal(Users user)
        {
            try
            {
                var isExists = _userRepository.CheckEmailAlreadyExistsPortal(user.Users_ID, user.Users_UserName);
                return Ok(isExists);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("SendResetEmail")]
        [HistoryFilter]
        public IActionResult SendResetEmail(string email)
        {
            try
            {
                var retTuple = _loginRepository.GetRestPasswordLinkByEmail(email);
                if (retTuple.Item1 && retTuple.Item2 != "0")
                {
                    string callbackUrl = Url.Link("Default", new { controller = "Account", action = "ResetPassword", code = HttpUtility.UrlEncode(retTuple.Item2) });
                    var emailTemplate = _emailTemplateRepository.GetEmailTemplateByCode("PW_RESET");
                    if (emailTemplate != null)
                    {
                        if (!string.IsNullOrEmpty(emailTemplate.ET_Body))
                        {
                            string url_template = emailTemplate.ET_Body.Replace("{RESET_URL}", callbackUrl).Replace("[USER_FULLNAME]", email);
                            _emailSender.SendEmail(retTuple.Item3, "Password Reset Email", url_template);
                        }
                    }
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteUser")]
        [HistoryFilter]
        public IActionResult DeleteUser(int userId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userId > 0)
                    {
                        _userRepository.DeleteUser(userId);
                    }
                    return Ok(true);
                }
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("LoadAssignLearner")]

        public DataSourceResult LoadAssignLearner([DataSourceRequest] DataSourceRequest request)
        {
            var user = _portalUserRepository.GetUserById(UserId);
            var data = _portalUserRepository.LoadAssignLearner(0, (DateTime)user.Users_ShowLearnersFrom);

           return data.ToDataSourceResult(request);
        }
        [HttpGet]
        [Route("LoadAssignLearnerForAccount")]

        public DataSourceResult LoadAssignLearnerForAccount([DataSourceRequest] DataSourceRequest request)
        {
            var user = _portalUserRepository.GetUserById(UserId);
            var data = _portalUserRepository.LoadAssignLearnerByTrainingProviderAccounts(UserId, (DateTime)user.Users_ShowLearnersFrom, (int)user.Users_Id_TrainingProvider);

            return data.ToDataSourceResult(request);
        }
        [HttpGet]
        [Route("LoadAssignLearnerForEmployer")]

        public DataSourceResult LoadAssignLearnerForEmployer([DataSourceRequest] DataSourceRequest request)
        {
            var empIds = _portalUserRepository.LoadEmployerLearnerHidenByUser(UserId);
            string learnerIds = "";
            int i = 0;
            foreach (var item in empIds)
            {
                if (i == 0)
                {
                    learnerIds += item.Learner_Id;
                }
                else
                {
                    learnerIds += ", " + item.Learner_Id;
                }
                i++;
            }

            var emp = _portalUserRepository.LoadEmployerByUser(UserId);
            string employerIds = "";
            int j = 0;
            foreach (var item in emp)
            {
                if (j == 0)
                {
                    employerIds += item.Employer_Id;
                }
                else
                {
                    employerIds += ", " + item.Employer_Id;
                }
                j++;
            }


            var data = _portalUserRepository.LoadAssignLearnerForEmployer(employerIds, learnerIds);

            return data.ToDataSourceResult(request);
        }
        [HttpGet]
        [Route("LoadAssignLearnerForAccountSearch")]

        public IActionResult LoadAssignLearnerForAccountSearch(string name)
        {
            var user = _portalUserRepository.GetUserById(UserId);
            var data = _portalUserRepository.LoadAssignLearnerByTrainingProviderAccountsSearch(name,UserId, (DateTime)user.Users_ShowLearnersFrom, (int)user.Users_Id_TrainingProvider);
            var list = new List<Learner>();
            foreach (var item in data)
            {
                list.Add(new Learner()
                {
                    CandidateStatus = item.CandidateStatus,
                    CandidateStatus_BackColor= item.CandidateStatus_BackColor,
                    CourseName= item.CourseName,
                    CurrentLevel= item.CurrentLevel,
                    Employer_PostCode1 = item.Employer_PostCode1,
                    encodedId = Encryption.Encrypt(item.Learner_ID.ToString()),
                    IsAssigned = item.IsAssigned,
                    Learner_ID= item.Learner_ID,
                    Id=item.Id,
                    LearnerCourseId = item.LearnerCourseId,
                    LearnerName= item.LearnerName,
                    Learner_Class =item.Learner_Class,
                    Learner_FirstName = item.Learner_FirstName,
                    Learner_Surname =item.Learner_Surname,
                    Percentage= item.Percentage,
                    Status= item.Status,
                    Titles_Title= item.Titles_Title,
                    Users_Id_LearnerContacts= item.Users_Id_LearnerContacts
                });
            }
            return Ok(list);
        }
        [HttpGet]
        [Route("LoadAssignLearnerByProviderUser")]

        public IActionResult LoadAssignLearnerByProviderUser(int id)
        {
            var user = _portalUserRepository.GetUserById(id);
            var data = _portalUserRepository.LoadAssignLearnerByTrainingProvider(id, (DateTime)user.Users_ShowLearnersFrom, (int)user.Users_Id_TrainingProvider);

           return Ok(data);
        }
        [HttpGet]
        [Route("LoadAssignLearnerByProviderUserSearch")]

        public IActionResult LoadAssignLearnerByProviderUserSearch(int id,string name)
        {
            var user = _portalUserRepository.GetUserById(id);
            var data = _portalUserRepository.LoadAssignLearnerByTrainingProviderSearch(name,id, (DateTime)user.Users_ShowLearnersFrom, (int)user.Users_Id_TrainingProvider);

           return Ok(data);
        }
        [HttpGet]
        [Route("LoadAssignLearnerByAwardUser")]

        public IActionResult LoadAssignLearnerByAwardUser(int id)
        {
            var user = _portalUserRepository.GetUserById(id);
            var data = _portalUserRepository.LoadAssignLearnerByAwardingBody(id, (DateTime)user.Users_ShowLearnersFrom, (int)user.Users_Id_AwardingBody);

           return Ok(data);
        }
        [HttpGet]
        [Route("LoadAssignLearnerByAwardUserSearch")]

        public IActionResult LoadAssignLearnerByAwardUserSearch(int id,string name)
        {
            var user = _portalUserRepository.GetUserById(id);
            var data = _portalUserRepository.LoadAssignLearnerByAwardingBodySearch(name,id, (DateTime)user.Users_ShowLearnersFrom, (int)user.Users_Id_AwardingBody);

           return Ok(data);
        }
        [HttpGet]
        [Route("CheckAssignLearner")]
        public IActionResult CheckAssignLearner(int userid,int learnerid)
        {
            var data = _portalUserRepository.CheckAssignLearner(userid, learnerid);
            return Ok(data);
        } 
        [HttpGet]
        [Route("UpdateAssignLearner")]
        public IActionResult UpdateAssignLearner(int userid,int learnerid)
        {
            var data = _portalUserRepository.UpdateAssignLearner(userid, learnerid);
            return Ok(data);
        }
        [HttpGet]
        [Route("UpdateEmployerLearner")]
        public IActionResult UpdateEmployerLearner(int userid, int employerid)
        {
            var data = _portalUserRepository.UpdateAssignEmployer(userid, employerid);
            return Ok(data);
        }
        [HttpGet]
        [Route("UpdateHideEmployerLearner")]
        public IActionResult UpdateHideEmployerLearner(int userid, int learnerid, string type)
        {
            if (type == "No")
            {
                var data = _portalUserRepository.HideLearner(userid, learnerid);
                return Ok(data);
            }
            else if(type =="Yes")
            {
                var data = _portalUserRepository.UnHideLearner(userid, learnerid);
                return Ok(data);
            }
            else
            {
                return Ok(null);
            }
        }
        [HttpGet]
        [Route("GetEmployerLearnerUser")]
        public IActionResult GetEmployerLearnerUser(int id)
        {
            var empIds = _portalUserRepository.LoadEmployerByUser(id);
            string employerIds = "";
            int i = 0;
            foreach (var item in empIds)
            {
                if (i == 0)
                {
                    employerIds += item.Employer_Id;
                }
                else
                {
                    employerIds += ", " + item.Employer_Id;
                }
                i++;
            }
            if (employerIds != "")
            {
                var data = _portalUserRepository.LoadEmployerLearnerById(employerIds);
                return Ok(data);
            }
            else
            {
                return Ok(null);
            }
        }
        [HttpGet]
        [Route("GetEmployerLearner")]
        public IActionResult GetEmployerLearner(int id)
        {
            var empIds = _portalUserRepository.LoadEmployerLearnerHidenByUser(id);
            string learnerIds = "";
            int i = 0;
            foreach (var item in empIds)
            {
                if (i == 0)
                {
                    learnerIds += item.Learner_Id;
                }
                else
                {
                    learnerIds += ", " + item.Learner_Id;
                }
                i++;
            }

            var emp = _portalUserRepository.LoadEmployerByUser(id);
            string employerIds = "";
            int j = 0;
            foreach (var item in emp)
            {
                if (j == 0)
                {
                    employerIds += item.Employer_Id;
                }
                else
                {
                    employerIds += ", " + item.Employer_Id;
                }
                j++;
            }

            if (employerIds != "")
            {
                var data = _portalUserRepository.LoadEmployerLearners(employerIds,learnerIds);
                return Ok(data);
            }
            else
            {
                return Ok(null);
            }
        }
        [HttpGet]
        [Route("GetEmployerUser")]
        public IActionResult GetEmployerUser(int id)
        {
            
           
                var data = _portalUserRepository.LoadEmployerUsers();
                return Ok(data);
            
        }
        [HttpGet]
        [Route("LoadAccountUser")]
        public IActionResult LoadAccountUser()
        {
            var data = _portalUserRepository.GetAccountUsers();
            return Ok(data);
        }
    }
}