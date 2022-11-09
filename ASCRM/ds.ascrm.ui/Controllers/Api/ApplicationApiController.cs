using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.configuration.Models;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.applications;
using ds.portal.applications.Models;
using ds.portal.leads;
using ds.portal.leads.Models.Shared;
using ds.portal.ui.Models;
using ds.portal.users;
using ds.portal.users.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/ApplicationApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ApplicationApiController : ControllerBase
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILead_ListRepository _lead_ListRepository;
        readonly ILogger<ListApiController> _log;
        LogException LogException;
        private readonly IConfiguration _Configuration;
        IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMailService _emailSender;
        private readonly ILead_UserRepository _lead_UserRepository;
        private readonly ILead_ListRepository _Lead_ListRepository;
        public ApplicationApiController(IApplicationRepository applicationRepo, ILead_ListRepository lead_ListRep
            , ILogger<ListApiController> log, IConfigurationRepository iConfig, IConfiguration configuration, IEmailTemplateRepository emailTemplateRep
            , IMailService emailSender, ILead_UserRepository lead_UserRep, ILead_ListRepository Lead_ListRepository)
        {
            _applicationRepository = applicationRepo;
            _lead_ListRepository = lead_ListRep;
            _log = log;
            LogException = new LogException(iConfig, _log);
            _Configuration = configuration;
            _emailTemplateRepository = emailTemplateRep;
            _emailSender = emailSender;
            _lead_UserRepository = lead_UserRep;
            _Lead_ListRepository = Lead_ListRepository;
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
        [Route("GetAllApplicationsForAdmin")]
        public DataSourceResult GetAllApplicationsForAdmin([DataSourceRequest]DataSourceRequest request, string type)
        {
            try
            {
                return _applicationRepository.GetAllApplicationsForAdminByType(type).ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllApplicationsByUserId")]
        [HistoryFilter]
        public IEnumerable<ApplicationModel> GetAllApplicationsByUserId()
        {
            try
            {
                return _applicationRepository.GetAllApplicationsByUserId(UserId);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        //[HttpGet]
        //[Route("GetApplicationById")]
        //[HistoryFilter]
        //public Application GetApplicationById(int id)
        //{
        //    try
        //    {
        //        return _applicationRepository.GetApplicationById(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogException.Log(ex, this.ControllerContext);
        //        throw new Exception(ex.ToString());
        //    }
        //}

        [HttpGet]
        [Route("GetApplicationAttachment")]
        [HistoryFilter]
        public IEnumerable<ApplicationQuestions>  GetApplicationAttachment(int id)
        {
            try
            {
                return _applicationRepository.GetApplicationAttachment(id);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetExtension")]
        [HistoryFilter]
        public ConfigurationModel GetExtension()
        {
            try
            {
                return _applicationRepository.GetConfigs();
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        
        [HttpGet]
        [Route("GetEnrolledQurestions")]
        [HistoryFilter]
        public IEnumerable<QuestionModel> GetEnrolledQurestions(int id)
        {
            try
            {
                return _applicationRepository.GetEnrolledQuestions(id);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetQuestionAnswerById")]
        [HistoryFilter]
        public IEnumerable<QuestionModel> GetQuestionAnswerById(int id)
        {
            try
            {
                return _applicationRepository.GetQuestionAnswerById(id);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("InsertUpdateApplication")]
        [HistoryFilter]
        public bool InsertUpdateApplication(List<SaveAnswer> application)
        {
            try
            {

                //foreach (var item in application)
                //{

                //}
                //byte[] bytes = Convert.FromBase64String(application.dataUri.Split(',')[1]);
                //application.Signature = bytes;
                //application.Username = UserName;
                //application.dataUri = "";

                //CheckChangeIfAny(application);
                var appId = application.Where(x => x.q_mapname == "id").FirstOrDefault();

                var data = _applicationRepository.GetCourseLevelForPortal(Convert.ToInt32(appId.q_answer));
                if (data.appuser_istrackchange == 1)
                {
                    CheckChangeIfAny(application, Convert.ToInt32(appId.q_answer));
                }


                return _applicationRepository.InsertUpdateApplication(application);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertUpdateEnrollmentApplication")]
        [HistoryFilter]
        public bool InsertUpdateEnrollmentApplication(List<SaveAnswer> application)
        {
            try
            {

              

                return _applicationRepository.InsertUpdateEnrollmentApplication(application);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetCourseLevelById")]
        [HistoryFilter]
        public CourseLevels GetCourseLevelById(int id)
        {
            try
            {
                return _applicationRepository.GetCourseLevelById(id);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetCourseLevelByIdOnPrint")]
        [HistoryFilter]
        public CourseLevels GetCourseLevelByIdOnPrint(int id)
        {
            try
            {
                int course_level_id = _applicationRepository.GetCourseLevelIdByApplicationId(id);


                return _applicationRepository.GetCourseLevelById(course_level_id);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertUpdateCourseLevel")]
        [HistoryFilter]
        public bool InsertUpdateCourseLevel(CourseLevels course)
        {
            try
            {
                
                return _applicationRepository.InsertUpdateCourseLevel(course);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteCourseLevelById")]
        [HistoryFilter]
        public IActionResult DeleteCourseLevelById(int id)
        {
            try
            {
                var retVal = _applicationRepository.DeleteCourseLevel(id);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("InsertNewApplication")]
        [HistoryFilter]
        public ApplicationModel InsertNewApplication(int id)
        {
            try
            {
                ApplicationModel applicationModel = new ApplicationModel();

                //Get the course level id first and then save in the application
                int course_level_id = 0;
                if (id > 0)
                {
                    course_level_id = id;
                }
                else
                {
                    course_level_id = _applicationRepository.GetCourseLevelIdByUserId(UserId);
                }

                int app_id = _applicationRepository.InsertNewApplication(UserId, UserName, course_level_id);
                if (app_id > 0)
                {
                    applicationModel.AppUser_App_Id = app_id;
                    string encrypted_id = Encryption.Encrypt(app_id.ToString());
                }
                else
                {
                    applicationModel.AppUser_App_Id = 0;
                }
                return applicationModel;
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
                var courses = _applicationRepository.GetAllCourses();
                return Ok(courses);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }



        [HttpGet]
        [Route("GetQuestionType")]
        public IActionResult GetQuestionType()
        {
            var courses = _applicationRepository.LoadQuestionType();
            return Ok(courses);
        }

        [HttpGet]
        [Route("GetAllCourseLevel")]
        public IActionResult GetAllCourseLevel()
        {
            var courses = _applicationRepository.LoadCourseLevel();
            return Ok(courses);
        }
        [HttpGet]
        [Route("LoadCourseLevel")]
        public IActionResult LoadCourseLevel([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var courses = _applicationRepository.LoadCourseLevel();
                return Ok(courses.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetApplicationNoticeQuestion")]
        [HistoryFilter]
        public ApplicationQuestions GetApplicationNoticeQuestion(int id)
        {
            try
            {
                return _applicationRepository.GetApplicationNoticeQuestion(id);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("LoadSignatureData")]
        [HistoryFilter]
        public ApplicationSignature LoadSignatureData(int id)
        {
            try
            {
                return _applicationRepository.LoadSignatureData(id);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("GetDependentAnswerById")]
        [HistoryFilter]
        public IEnumerable<QuestionModel> GetDependentAnswerById(int id)
        {
            try
            {
                var data = _applicationRepository.GetDependentAnswerById(id);

                return data;


            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        
        [HttpGet]
        [Route("GetConfirmQuestions")]
        [HistoryFilter]
        public IEnumerable<QuestionModel> GetConfirmQuestions(int id)
        {
            try
            {
                var data = _applicationRepository.GetConfirmQuestionsAnswer(id);

                return data;


            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetDependentAnswerForAdminById")]
        [HistoryFilter]
        public IEnumerable<QuestionModel> GetDependentAnswerForAdminById(int id)
        {
            try
            {
                var data = _applicationRepository.GetDependentAnswerForAdminById(id);

                return data;


            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetApplicationStepsForLearner")]
        [HistoryFilter]
        public IEnumerable<ApplicationSteps> GetApplicationStepsForLearner(int id)
        {
            try
            {
                int course_level_id = _applicationRepository.GetCourseLevelIdByApplicationId(id);

                var steps = _applicationRepository.GetListStepsForLearner(course_level_id,id);

                return steps;

               
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetApplicationForAdmin")]
        [HistoryFilter]
        public IEnumerable<ApplicationSteps> GetApplicationForAdmin(int id)
        {
            try
            {
                int course_level_id = _applicationRepository.GetCourseLevelIdByApplicationId(id);

                var steps = _applicationRepository.GetAdminQuestion(course_level_id, id);

                return steps;

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("AddHeader")]
        [HistoryFilter]
        public bool AddHeader(string text)
        {
            try
            {

                return _applicationRepository.InsertUpdateHeader(text);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("CheckHeader")]
        [HistoryFilter]
        public bool CheckHeader(string text)
        {
            try
            {

                return _applicationRepository.CheckHeader(text);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("SubmittedCheck")]
        [HistoryFilter]
        public bool SubmittedCheck()
        {
            try
            {
                return _applicationRepository.SubmittedCheck(UserId);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("ReadyToEnrollApplicationWithReason")]
        [HistoryFilter]
        public bool ReadyToEnrollApplicationWithReason(ReasonModel reasonModel)
        {
            try
            {
                reasonModel.user_id = UserId;
                reasonModel.user_name = UserName;
                return _applicationRepository.ReadyToEnrollApplicationWithReason(reasonModel);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("RejectApplicationWithReason")]
        [HistoryFilter]
        public bool RejectApplicationWithReason(ReasonModel reasonModel)
        {
            try
            {
                reasonModel.user_id = UserId;
                reasonModel.user_name = UserName;
                return _applicationRepository.RejectApplicationWithReason(reasonModel);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("DeleteApplicationWithReason")]
        [HistoryFilter]
        public bool DeleteApplicationWithReason(ReasonModel reasonModel)
        {
            try
            {
                reasonModel.user_id = UserId;
                reasonModel.user_name = UserName;
                return _applicationRepository.DeleteApplicationWithReason(reasonModel);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("UpdateOfficeUseOnly")]
        [HistoryFilter]
        public bool UpdateOfficeUseOnly(App_OfficeUse app_OfficeUse)
        {
            try
            {
                return _applicationRepository.UpdateOfficeUseOnly(app_OfficeUse);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
       

        [HttpGet]
        [Route("GetSignatureByAppId")]
        [HistoryFilter]
        public string GetSignatureByAppId(int id)
        {
            try
            {
                var signature = _applicationRepository.GetSignatureByAppId(id);
                return signature;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpPost]
        [Route("PerformNotifyChanges")]
        [HistoryFilter]
        public bool PerformNotifyChanges(int appid)
        {
            try
            {
                bool retVal = false;
                var appTrackChanges = _applicationRepository.GetApplicationsTrackChanges(appid);
                var data = _applicationRepository.GetApplicationById(appid);

                string tbl = "<table>";
                tbl += "    <tbody>";
                tbl += "        <tr>";
                tbl += "            <th>Change From</th>";
                tbl += "            <th>Change To</th>";
                tbl += "        </tr>";
                tbl += "    </tbody>";
                tbl += "    <tbody>";

                foreach (var appChange in appTrackChanges)
                {
                    tbl += "<tr style='background-color:#d9edf7;'>";
                    tbl += "     <td colspan='2'>" + appChange.ATC_FieldName + "</td>";
                    tbl += " </tr>";
                    tbl += " <tr>";
                    tbl += "     <td>" + appChange.ATC_ValueFrom + "</td>";
                    tbl += "     <td>" + appChange.ATC_ValueTo + "</td>";
                    tbl += " </tr>";
                }
                tbl += "    </tbody>";
                tbl += "</table>";

                bool isTestDevelopment = Convert.ToBoolean(_Configuration["Test_Development"]);
                Hashtable ht = new Hashtable();
                var firstName = data.FirstOrDefault(x => x.q_title.Contains("First"));
                var firstNameAns = "";
                if(firstName != null)
                {
                    firstNameAns = firstName.answer;
                }
                var lastName = data.FirstOrDefault(x => x.q_title.Contains("Surname"));
                var lastNameAns = "";
                if (lastName != null)
                {
                    lastNameAns = lastName.answer;
                }

                ht.Add("[LEARNER]", firstNameAns + " " + lastNameAns);
                ht.Add("[TABLEROWS]", tbl);

                Tuple<string, string> tuple = _emailTemplateRepository.GetSubjectAndEmailTemplateBodyReplacedByCode("NCL001", ht);

               // retVal = _emailSender.SendEmail(_app.app_email.Trim().ToLower(), tuple.Item1, tuple.Item2);

                //change application status for changes
                if (retVal)
                {
                    _applicationRepository.UpdateApplicationStatusAfterNotifiedChanges(appid, UserId);
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
        [Route("SubmitApplication")]
        [HistoryFilter]
        public bool SubmitApplication(int id)
        {
            try
            {
                //var data = _applicationRepository.GetCourseLevelForPortal(id);
                //if (data.appuser_istrackchange == 1)
                //{

                //}

                return _applicationRepository.SubmitApplication(id, UserName);
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

            // Return the first length bytes
            return guidResult.Substring(0, length).ToUpper();
        }
        private bool SendEmailForFailedQCSRegistration(string email, string userName, string message)
        {
            Hashtable ht = new Hashtable();
            ht.Add("[USERNAME]", userName);
            ht.Add("[ERRORMESSAGE]", message);

            Tuple<string, string> tuple = _emailTemplateRepository.GetSubjectAndEmailTemplateBodyReplacedByCode("RFQCS", ht);

            try
            {
                _emailSender.SendEmail(email.Trim().ToLower(), tuple.Item1, tuple.Item2);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
            return true;
        }


        List<ApplicationsTrackChanges> applicationsTrackChangesList;
        private bool CheckChangeIfAny(List<SaveAnswer> list, int appId)
        {
            applicationsTrackChangesList = new List<ApplicationsTrackChanges>();
            bool isAmended = false;
            //int appId = _App.app_id;

            var data = _applicationRepository.GetApplicationById(appId);
            foreach (var item in list)
            {
               var result = data.FirstOrDefault(x => x.q_mapname == item.q_mapname);
                if (result != null)
                {
                    applicationsTrackChangesList.Add(new ApplicationsTrackChanges()
                    {

                        atc_q_mapname = item.q_mapname,
                        ATC_FieldName = result.q_title,
                        ATC_ValueFrom = result.answer,
                        ATC_ValueTo = item.q_answer,

                    });
                }
            }

            if (applicationsTrackChangesList.Count > 0)
            {
                _applicationRepository.SaveChangesToApplicationsTrackChanges(applicationsTrackChangesList, appId, UserId);
            }

            return true;
        }

        //private bool SetValueOfChanges(string valueFrom, string valueTo, string fieldName, string fieldDescription)
        //{
        //    applicationsTrackChangesList.Add(new ApplicationsTrackChanges
        //    {
        //        ATC_ValueFrom = valueFrom,
        //        ATC_ValueTo = valueTo,
        //        ATC_FieldName = fieldName,
        //        ATC_FieldDesciption = fieldDescription
        //    });
        //    return true;
        //}
        private string GetYesNo(string value)
        {
            string retVal = "";

            switch (value)
            {
                case "null":
                    retVal = "Please select";
                    break;
                case "1":
                    retVal = "Yes";
                    break;
                case "0":
                    retVal = "No";
                    break;
                default:
                    break;
            }

            return retVal;
        }
        [HttpGet]
        [Route("GetApplicationAllDropdownsHeaders")]
        public IActionResult GetApplicationAllDropdownsHeaders()
        {
            try
            {
                var dropdownHeaders = _applicationRepository.GetApplicationDropdownOptionsHeaders();
                return Ok(dropdownHeaders);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


       

        [HttpGet]
        [Route("GetApplicationDropdownOptionsByHeaderId")]
        public IActionResult GetApplicationDropdownOptionsByHeaderId(int id)
        {
            try
            {
                var dropdownOptions = _applicationRepository.GetApplicationDropdownOptionsByHeaderId(id);
                return Ok(dropdownOptions);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("CheckTitleAndValueExists")]
        public Tuple<bool, bool> CheckTitleAndValueExists(DropdownOption dropdownOption)
        {
            try
            {
                return _applicationRepository.CheckTitleAndValueExists(dropdownOption);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertUpdateOptionRecord")]
        [HistoryFilter]
        public bool InsertUpdateOptionRecord(DropdownOption dropdownOption)
        {
            try
            {
                return _applicationRepository.InsertUpdateOptionRecord(dropdownOption);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteDropdownOptionByOptionId")]
        [HistoryFilter]
        public bool DeleteDropdownOptionByOptionId(int id)
        {
            try
            {
                return _applicationRepository.DeleteDropdownOptionByOptionId(id, UserId, UserName);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

    }
}