using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.emailtemplates;
using ds.core.logger;
using ds.portal.applications;
using ds.portal.applications.Models;
using ds.portal.companies;
using ds.portal.companies.Models;
using ds.portal.leads;
using ds.portal.ui.Controllers;
using ds.portal.ui.Controllers.Api;
using ds.portal.users;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationQuestionsApiController : ControllerBase
    {

        private readonly IQuestionsRepository _questionsRepository;
        readonly ILogger<ApplicationQuestionsApiController> _log;
        LogException LogException;
        public ApplicationQuestionsApiController(IHostingEnvironment hostingEnvironment
            , ILogger<ApplicationQuestionsApiController> log, IConfigurationRepository iConfig, IQuestionsRepository questionsRepository)
        {
            _log = log;
            LogException = new LogException(iConfig, _log);
            _questionsRepository = questionsRepository;
        }
        [HttpGet]
        [Route("GetApplicationQuestionStep")]
        public IActionResult GetApplicationQuestionStep()
        {
            try
            {
                var steps = _questionsRepository.GetApplicationStep();
                return Ok(steps);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetApplicationSection")]
        public IActionResult GetApplicationSection(int id)
        {
            try
            {
                var section = _questionsRepository.GetApplicationSection(id);
                return Ok(section);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        
        [HttpGet]
        [Route("RemoveQuestion")]
        public IActionResult RemoveQuestion(int id,int levelId)
        {
            try
            {
                var section = _questionsRepository.RemoveQuestionFromCourse(id,levelId);
                return Ok(section);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("SearchInQuestion")]
        public IActionResult SearchInQuestion([DataSourceRequest] DataSourceRequest request, string name)
        {
            try
            {
                var section = _questionsRepository.QuestionsSearch(name);

                var model = new List<QuestionModel>();
                foreach (var item in section)
                {
                    model.Add(new QuestionModel()
                    {
                        id = item.id,
                        sortOrder = item.sortOrder,
                        title = item.title,
                        encodedId = Encryption.Encrypt(item.id.ToString()),
                        type = item.type,
                        step = item.step,
                        section = item.section,
                        optheader_title = item.optheader_title
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
        [Route("GetQuestionCourseLevelId")]
        public IActionResult GetQuestionCourseLevelId([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var section = _questionsRepository.QuestionsByCourseLevel(id);

                var model = new List<QuestionModel>();
                foreach (var item in section)
                {
                    model.Add(new QuestionModel()
                    {
                        id = item.id,
                        sortOrder = item.sortOrder,
                        title = item.title,
                        encodedId = Encryption.Encrypt(item.id.ToString()),
                        type = item.type,
                        step = item.step,
                        section = item.section,
                        optheader_title = item.optheader_title
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
        [Route("GetQuestionByType")]
        public IActionResult GetQuestionByType([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var section = _questionsRepository.GetQuestionsByType(id);

                var model = new List<QuestionModel>();
                foreach (var item in section)
                {
                    model.Add(new QuestionModel()
                    {
                        id = item.id,
                        sortOrder = item.sortOrder,
                        title = item.title,
                        encodedId = Encryption.Encrypt(item.id.ToString()),
                        type = item.type,
                        step = item.step,
                        section = item.section,
                        optheader_title = item.optheader_title
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
        [Route("GetApplicationDepend")]
        public IActionResult GetApplicationDepend(int stepId,int sectionId,int courselevelId)
        {
            try
            {
                var section = _questionsRepository.GetApplicationDepend(stepId,sectionId, courselevelId);
                return Ok(section);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertUpdateQuestion")]
        [HistoryFilter]
        public ApplicationQuestions InsertUpdateQuestion(ApplicationQuestions application)
        {
            try
            {
              
                return _questionsRepository.InsertUpdateQuestionRecord(application);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("AssignBulk")]
        [HistoryFilter]
        public int AssignBulk(int levelFromId, int levelToId)
        {
            try
            {
                return _questionsRepository.AssignBulk(levelFromId,levelToId);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetQuestionsList")]
        public DataSourceResult GetQuestionsList([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var section = _questionsRepository.GetListQuestions();
                return section.ToDataSourceResult(request);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetQuestionById")]
        public IActionResult GetQuestionById(int id)
        {
            try
            {
                var company = _questionsRepository.GetQuestionById(id);
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
        [Route("CheckQuestionExist")]
        public IActionResult CheckQuestionExist(int levelId,int number)
        {
            try
            {
                var company = _questionsRepository.CheckQuestionExist(levelId,number);
                /// country default set - UK.
                //if (leads.Lead_Contact_Id_Country == 0)
                //{
                //    leads.Lead_Contact_Id_Country = 4;
                //}
                if(company != true)
                {
                    _questionsRepository.AssignSingleBulk(levelId, number);
                }
                return Ok(company);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("PreviewApp")]
        public IActionResult PreviewApp(int levelId)
        {
            try
            {
                var company = _questionsRepository.CheckAppQuestionExist(levelId);
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
        [Route("DeleteQuestionById")]
        [HistoryFilter]
        public IActionResult DeleteQuestionById(int id)
        {
            try
            {
                var retVal = _questionsRepository.DeleteQuestionById(id);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteHardQuestionById")]
        [HistoryFilter]
        public IActionResult DeleteHardQuestionById(int id)
        {
            try
            {
                var retVal = _questionsRepository.DeleteHardQuestionById(id);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpPost]
        [Route("InsertCopyQuestion")]
        [HistoryFilter]
        public ApplicationQuestions InsertCopyQuestion(ApplicationQuestions application)
        {
            try
            {

                return _questionsRepository.AddCopyQuestion(application);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

    }
}
