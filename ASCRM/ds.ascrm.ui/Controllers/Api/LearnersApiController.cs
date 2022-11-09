using crm.osca.Interfaces.Learners;
using crm.osca.Models;
using crm.portal.Interfaces.User;
using ds.core.configuration;
using ds.core.logger;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Novacode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/LearnersApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class LearnersApiController : ControllerBase
    {
        private readonly ILearnersRepository _learnersRepository;
        readonly ILogger<LearnersApiController> _log;
        LogException LogException;
        private IConfigurationRepository configuration;
        private readonly IPortalUserRepository _portalUserRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public LearnersApiController(IHostingEnvironment hostingEnvironment,IPortalUserRepository portalUserRepository,IConfigurationRepository iConfig,ILogger<LearnersApiController> log,ILearnersRepository learnersRepository)
        {
            _learnersRepository = learnersRepository;
            _log = log;
            LogException = new LogException(iConfig, _log);
            _portalUserRepository = portalUserRepository;
            _hostingEnvironment = hostingEnvironment;
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
        [Route("LoadLearners")]
        [HttpGet]
        public ActionResult LoadLearners([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var courses = _learnersRepository.LoadLearners();
                return Ok(courses.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetTitle")]
        [HttpGet]
        public ActionResult GetTitle()
        {
            try
            {
                var comman = _learnersRepository.LoadTitle();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("GetGender")]
        [HttpGet]
        public ActionResult GetGender()
        {
            try
            {
                var comman = _learnersRepository.LoadGender();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetGroup")]
        [HttpGet]
        public ActionResult GetGroup()
        {
            try
            {
                var comman = _learnersRepository.LoadGroup();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
         [Route("GetStatus")]
        [HttpGet]
        public ActionResult GetStatus()
        {
            try
            {
                var comman = _learnersRepository.LoadStatus();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        } 
        [Route("GetMarital")]
        [HttpGet]
        public ActionResult GetMarital()
        {
            try
            {
                var comman = _learnersRepository.LoadMarital();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetImmigrationStatus")]
        [HttpGet]
        public ActionResult GetImmigrationStatus()
        {
            try
            {
                var comman = _learnersRepository.LoadImmigrationStatus();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetRegion")]
        [HttpGet]
        public ActionResult GetRegion()
        {
            try
            {
                var comman = _learnersRepository.LoadRegion();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("GetCountry")]
        [HttpGet]
        public ActionResult GetCountry()
        {
            try
            {
                var comman = _learnersRepository.LoadCountry();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [Route("GetSkillsAdvisor")]
        [HttpGet]
        public ActionResult GetSkillsAdvisor()
        {
            try
            {
                var comman = _learnersRepository.LoadSkillsAdvisors();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetFundingType")]
        [HttpGet]
        public ActionResult GetFundingType()
        {
            try
            {
                var comman = _learnersRepository.LoadFundingType();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetProjectType")]
        [HttpGet]
        public ActionResult GetProjectType()
        {
            try
            {
                var comman = _learnersRepository.LoadProjectType();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("GetAwardingBody")]
        [HttpGet]
        public ActionResult GetAwardingBody()
        {
            try
            {
                var comman = _learnersRepository.LoadAwardingBody();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetQualCourses")]
        [HttpGet]
        public ActionResult GetQualCourses()
        {
            try
            {
                var comman = _learnersRepository.LoadQualCourses();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }  
        [Route("GetCourseStatus1")]
        [HttpGet]
        public ActionResult GetCourseStatus1()
        {
            try
            {
                var comman = _learnersRepository.LoadCourseStatus1();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetCourseDetail")]
        [HttpGet]
        public ActionResult GetCourseDetail(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadCourseDatail(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetCourseSummaryReport")]
        [HttpGet]
        public ActionResult GetCourseSummaryReport(int id)
        {
            try
            {
                Doc_CourseSummary oDoc = new Doc_CourseSummary();

                var user = _portalUserRepository.GetUserById(UserId);
                oDoc.us_UpdaterName = user.Users_Username;
                oDoc.us_UpdaterEmail = user.Users_Email;
                string strNumberNumber = GenerateRandomPasswordUsingGUID(6);
                string strPath = _hostingEnvironment.WebRootPath + "\\Assets\\Temp\\coursesummary\\";
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                strPath = strPath + "\\coursesummary-" + id + "-" + strNumberNumber + ".docx";
                string sTemplateFile = _hostingEnvironment.WebRootPath + "\\Assets\\Docs\\LearnerCourseSummary.docx";
                if (System.IO.File.Exists(strPath)) { System.IO.File.Delete(strPath); }
                System.IO.File.Copy(sTemplateFile, strPath);



                var comman = _learnersRepository.GenerateCourseSummary(id);
                using (DocX document = DocX.Load(strPath))
                {
                    document.ReplaceText("[LEARNERNAME]", comman.LearnerName, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[LEARNERID]", id.ToString(), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[COURSE]", comman.Learner_CurrentCourse, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                    //    document.ReplaceText("[TOTALCOMPLETED]", TotalCompletedTopics.ToString(), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    //    document.ReplaceText("[TOTALNOTCOMPLETED]", TotalNotCompletedTopics.ToString(), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[STARTDATE]", comman.Learner_StartDate, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[ENDDATE]", comman.Learner_PlannedEndDate, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                    document.ReplaceText("[PERCENT]", comman.Percentage, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);


                    int iMaxItems = 41;
                    int i = 0;
                    foreach (var mx_item in comman.listItem)
                    {
                        i++;
                        document.ReplaceText("[TOPICNAME" + i + "]", mx_item._SSJLP_Topic, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        string sCompleteDate = mx_item._duedate_completed_label;
                        // sCompleteDate = sCompleteDate.Split(' ')[0];
                        document.ReplaceText("[DATE" + i + "]", (sCompleteDate == "" ? "" : sCompleteDate), false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[STATUS" + i + "]", mx_item._filter_label_caption, false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                    }

                    for (int j = i; j <= iMaxItems; j++)
                    {
                        document.ReplaceText("[TOPICNAME" + j + "]", "", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[DATE" + j + "]", "", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);
                        document.ReplaceText("[STATUS" + j + "]", "", false, System.Text.RegularExpressions.RegexOptions.None, null, null, MatchFormattingOptions.SubsetMatch);

                    }

                    // Save changes made to this document.
                    document.Save();


                }
                string sMainTempPath3 = _hostingEnvironment.WebRootPath + "\\Assets\\Temp\\";
                var pdfPath = sMainTempPath3 + +id + "-" + strNumberNumber + ".pdf";

                //var pdfPath = strPath + "\\coursesummary-" + id + "-" + strNumberNumber + ".pdf";
                Spire.Doc.Document document1 = new Spire.Doc.Document();
                document1.LoadFromFile(strPath, Spire.Doc.FileFormat.Auto);
                document1.SaveToFile(pdfPath, Spire.Doc.FileFormat.PDF);


                byte[] sendFile = GetFile(pdfPath);
                return Ok(sendFile);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        [Route("GetReviewCourse")]
        [HttpGet]
        public ActionResult GetReviewCourse(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadReviewCourse(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }   
        [Route("GetdocTopics")]
        [HttpGet]
        public ActionResult GetdocTopics(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadDocUploadTopic(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }    
        [Route("GetMatrixCourses")]
        [HttpGet]
        public ActionResult GetMatrixCourses(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadCourses(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }     
        [Route("InsertNotes")]
        [HttpPost]
        public ActionResult InsertNotes(LearnerNote note)
        {
            try
            {
                return Ok(_learnersRepository.AddNote(note));
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [Route("GetLoadNoteCatgory")]
        [HttpGet]
        public ActionResult GetLoadNoteCatgory()
        {
            try
            {
                var comman = _learnersRepository.LoadNoteCatgory();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }     
        [Route("GetNoteList")]
        [HttpGet]
        public ActionResult GetNoteList(int learnerId,string note, int cateid)
        {
            try
            {
                var comman = _learnersRepository.NoteList(learnerId,note,cateid);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }   
        [Route("GetSingupDocs")]
        [HttpGet]
        public ActionResult GetSingupDocs(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadSingupDocs(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }   
        [Route("GetIVEVS")]
        [HttpGet]
        public ActionResult GetIVEVS(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadIVEVDOC(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }    
        [Route("GetProfileActivty")]
        [HttpGet]
        public ActionResult GetProfileActivty(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadProfileActivty(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetEmployerbyName")]
        [HttpGet]
        public ActionResult GetEmployerbyName(string name)
        {
            try
            {
                var comman = _learnersRepository.SearchEmployer(name);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }  
        [Route("GetPrimaryContatcs")]
        [HttpGet]
        public ActionResult GetPrimaryContatcs(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadPrimaryContact(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }  
        [Route("GetCompleteActivites")]
        [HttpGet]
        public ActionResult GetCompleteActivites(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadCompleteActivites(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }   
        [Route("GetAdditionalActivites")]
        [HttpGet]
        public ActionResult GetAdditionalActivites(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadAdditionalActivites(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("GetEmployer")]
        [HttpGet]
        public ActionResult GetEmployer(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadEmployer(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [Route("GetTabHistory")]
        [HttpGet]
        public ActionResult GetTabHistory(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadTabHistory(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }  


        [Route("GetCPDPrime")]
        [HttpGet]
        public ActionResult GetCPDPrime(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadPrimeCPD(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("GetReviewList")]
        [HttpGet]
        public ActionResult GetReviewList(int id,int courseId)
        {
            try
            {
                var comman = _learnersRepository.LoadReviewList(id,courseId);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("LoadDeatil")]
        public IActionResult LoadDeatil(int id)
        {
            var detail = _learnersRepository.LoadLearnerDatail(id);
            return Ok(detail);
        }
        [Route("GetOutcome")]
        [HttpGet]
        public ActionResult GetOutcome()
        {
            try
            {
                var comman = _learnersRepository.LoadLearnerOutcome();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        
        [Route("GetFinanceStaus")]
        [HttpGet]
        public ActionResult GetFinanceStaus()
        {
            try
            {
                var comman = _learnersRepository.LoadFinanceStaus();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        
        [Route("GetRisking")]
        [HttpGet]
        public ActionResult GetRisking()
        {
            try
            {
                var comman = _learnersRepository.LoadRisking();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [Route("GetCourseStatus")]
        [HttpGet]
        public ActionResult GetCourseStatus()
        {
            try
            {
                var comman = _learnersRepository.LoadCourseStatus();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetCombineData")]
        [HttpGet]
        public ActionResult GetCombineData(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadCombineData(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetFinanceLastDate")]
        [HttpGet]
        public ActionResult GetFinanceLastDate(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadLastDate(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("UpdateLearner")]
        [HttpPost]
        public ActionResult UpdateLearner(LearnerData data)
        {
            try
            {
                var comman = _learnersRepository.UpdateLearnerData(data);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("UpdateEmployer")]
        [HttpGet]
        public ActionResult UpdateEmployer(int learnerId,int employerId)
        {
            try
            {
                var comman = _learnersRepository.UpdateEmployer(learnerId,employerId);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        
        [Route("UpdateAssessment")]
        [HttpGet]
        public ActionResult UpdateAssessment(int learnerId,int employerId)
        {
            try
            {
                var comman = _learnersRepository.UpdateAssessment(learnerId,employerId);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("UpdateCourse")]
        [HttpPost]
        public ActionResult UpdateCourse(UpdateCourse model)
        {
            try
            {
                var data = _learnersRepository.CheckUpdateCouser(model.LearnerCourses_Id_Learner, model.LearnerCourses_Id_Course);
                if(data.Count() >1)
                {
                    var comman = _learnersRepository.UpdateCourse(model);
                    return Ok(comman);
                }
                else
                {
                    var data1 = _learnersRepository.InsertCourse(model);
                    return Ok(data1);
                }
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("GetStatusChangeDate")]
        [HttpGet]
        public ActionResult GetStatusChangeDate(int id)
        {
            try
            {
                var comman = _learnersRepository.LoadStatusChangeDate(id);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("GetAccountStatus")]
        [HttpGet]
        public ActionResult GetAccountStatus()
        {
            try
            {
                var comman = _learnersRepository.LoadAccountStatus();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        } 
        [Route("GetPriorAttainment")]
        [HttpGet]
        public ActionResult GetPriorAttainment()
        {
            try
            {
                var comman = _learnersRepository.LoadPriorAttainment();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        } 
        [Route("GetLearnerHHS")]
        [HttpGet]
        public ActionResult GetLearnerHHS()
        {
            try
            {
                var comman = _learnersRepository.LoadLearnerHHS();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }  
        [Route("GetLearnerLLDD")]
        [HttpGet]
        public ActionResult GetLearnerLLDD()
        {
            try
            {
                var comman = _learnersRepository.LoadLearnerLLDD();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }  
        [Route("GetNationality")]
        [HttpGet]
        public ActionResult GetNationality()
        {
            try
            {
                var comman = _learnersRepository.LoadNationality();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("GetHeaders")]
        [HttpGet]
        public ActionResult GetHeaders(string header)
        {
            try
            {
                var comman = _learnersRepository.LoadOscaHeader(header);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [Route("GetEthnicity")]
        [HttpGet]
        public ActionResult GetEthnicity()
        {
            try
            {
                var comman = _learnersRepository.LoadEthnicity();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("GetPaymentStaus")]
        [HttpGet]
        public ActionResult GetPaymentStaus()
        {
            try
            {
                var comman = _learnersRepository.LoadPaymentStaus();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        } 
        [Route("GetObservationAssessors")]
        [HttpGet]
        public ActionResult GetObservationAssessors()
        {
            try
            {
                var comman = _learnersRepository.LoadObservationAssessors();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [Route("GetEmployerPaid")]
        [HttpGet]
        public ActionResult GetEmployerPaid()
        {
            try
            {
                var comman = _learnersRepository.LoadEmployerPaid();
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [Route("AddCPD")]
        [HttpPost]
        public ActionResult AddCPD(AdditionalActivites model)
        {
            try
            {
                var comman = _learnersRepository.AddAdditionalActivites(model);
                return Ok(comman);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        public static string GenerateRandomPasswordUsingGUID(int length)
        {
            // Get the GUID
            string guidResult = System.Guid.NewGuid().ToString();

            // Remove the hyphens
            guidResult = guidResult.Replace("-", string.Empty);

            // Make sure length is valid
            /*   if (length <= 0 || length > guidResult.Length)
                   throw new ArgumentException("Length must be between 1 and " + guidResult.Length);
               */
            // Return the first length bytes
            return guidResult.Substring(0, length).ToUpper();
        }

    }
}
