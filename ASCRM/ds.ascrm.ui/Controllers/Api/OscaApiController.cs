using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using ds.core.comms.Mail;
using ds.core.configuration;
using ds.core.document;
using ds.core.logger;
using ds.portal.companies;
using ds.portal.companies.Models;
using ds.portal.leads;
using ds.portal.users;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using portal.data.repository.Interfaces;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/OscaApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]

    public class OscaApiController : ControllerBase
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ILead_ListRepository _Lead_ListRepository;
        private readonly ILead_UserRepository _Lead_UserRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMailService _emailSender;
        private IConfigurationRepository configuration;
        private string _fromEmailId = string.Empty;
        private readonly ILoginRepository _loginRepository;
        readonly ILogger<OscaApiController> _log;
        LogException LogException;
        IDocument _document;
        private readonly IRoleAdminRepository _roleAdminRepository;
        public OscaApiController(ICompanyRepository companyRepository, ILeadRepository leadRepository, ILead_ListRepository Lead_ListRepository, IHostingEnvironment hostingEnvironment,
           ILogger<OscaApiController> log, IMailService emailSender
            , ILead_UserRepository Lead_UserRepository, IConfigurationRepository iConfig
            , ILoginRepository loginRepository, IDocument document, IRoleAdminRepository roleAdminRepository)
        {
            _companyRepository = companyRepository;
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
        }
        [HttpGet]
        [Route("GetJobTitle")]
        public IActionResult GetJobTitle()
        {
            var jobTitles = _Lead_ListRepository.GetDropdownOptionsByHeaderName("JobTitles");
            return Ok(jobTitles);
        }
        [HttpGet]
        [Route("GetNoteCategory")]
        public IActionResult GetNoteCategory()
        {
            var jobTitles = _Lead_ListRepository.GetDropdownOptionsByHeaderName("CompanyNotes");
            return Ok(jobTitles);
        }
        [HttpGet]
        [Route("GetTitle")]
        public IActionResult GetTitle()
        {
            var jobTitles = _Lead_ListRepository.GetDropdownOptionsByHeaderName("Title");
            return Ok(jobTitles);
        }

        [HttpGet]
        [Route("GetSalesAdviser")]
        public IActionResult GetSalesAdviser()
        {
            var salesAdvisors = _Lead_UserRepository.GetAllSalesAdvisorUsers(true);
            return Ok(salesAdvisors);
        }
        [HttpGet]
        [Route("GetAllCompanies")]
        public IActionResult GetAllCompanies(string name)
        {
          
            var companies = _companyRepository.GetCompaniesForOsca(name);
            return Ok(companies);
        }

        [HttpGet]
        [Route("GetAllCompanyNotes")]
        public IActionResult GetAllCompanyNotes(int company_Id)
        {
            try
            {
              var notes=  _companyRepository.GetAllCompanyNotes(company_Id);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllCompanyContacts")]
        public IActionResult GetAllCompanyContacts(int company_Id)
        {
            try
            {
                return Ok(_companyRepository.GetAllCompanyContacts(company_Id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllAssignedLearner")]
        public IActionResult GetAllAssignedLearner(int company_Id)
        {
            try
            {
                return Ok(_companyRepository.GetAllAssignedLearner(company_Id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllWorkplaceContacts")]
        public IActionResult GetAllWorkplaceContacts(int company_Id, int wp_idd)
        {
            try
            {
                return Ok(_companyRepository.GetAllWorkplaceContacts(company_Id, wp_idd));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllCompanyWorkPlace")]
        public IActionResult GetAllCompanyWorkPlace(int company_Id)
        {
            try
            {
                return Ok(_companyRepository.GetAllCompanyWorkplaceses(company_Id));
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("CompanySearch")]
        public IActionResult CompanySearch(string name)
        {
            try
            {
                var id = "";
                var sale_id = 0;
                var company = _companyRepository.GetCompaniesForOsca(name);

                return Ok(company);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("AddUpdateAssignedLearnerEmployer")]
        public IActionResult AddUpdateAssignedLearnerEmployer(CompanyAssignedLearnerModel model)
        {
            try
            {
                var addupdate = _companyRepository.InssertUpdateAssignLearnerEmployer(model);
                return Ok(addupdate);
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }
        [HttpPost]
        [Route("AddUpdateAssignedLearnerAssessment")]
        public IActionResult AddUpdateAssignedLearnerAssessment(CompanyAssignedLearnerModel model)
        {
            try
            {
                var addupdate = _companyRepository.InssertUpdateAssignLearnerAssessment(model);
                return Ok(addupdate);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetEmployer")]
        public IActionResult GetEmployer(int id)
        {
            try
            {
                var employeer = _companyRepository.GetEmployeerByLearner(id);
                return Ok(employeer);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("GetAssessment")]
        public IActionResult GetAssessment(int id)
        {
            try
            {
                var employeer = _companyRepository.GetAssessmentByLearner(id);
                return Ok(employeer);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("PreviousEmployers")]
        public IActionResult PreviousEmployers(int id)
        {
            try
            {
                var employeer = _companyRepository.GetPreviousEmployers(id);
                return Ok(employeer);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("PreviousAssessment")]
        public IActionResult PreviousAssessment(int id)
        {
            try
            {
                var employeer = _companyRepository.GetPreviousAssessments(id);
                return Ok(employeer);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("GetWorkPlace")]
        public IActionResult GetWorkPlace(int id)
        {
            var workplace = _companyRepository.GetCompanyWorkplaceById(id);
            return Ok(workplace);
        }
        [HttpGet]
        [Route("GetContactOnWorkPlace")]
        public IActionResult GetWorkPlaceContact(int companyId, int workplaceid)
        {
            var contact = _companyRepository.GetAllWorkplaceContacts(companyId, workplaceid);
            return Ok(contact);
        }
        [HttpGet]
        [Route("UpdateAssignedLearnerOnChangeContact")]
        public IActionResult UpdateAssignedLearnerOnChangeContact(int companyId, int learnerId, int contactId, int? wp_id)
        {
            var contact = _companyRepository.UpdateAssignedLearnerOnContactChange(companyId, learnerId,contactId,wp_id);
            return Ok(contact);
        }
        [Route("DeleteCompanyContactById")]
        public IActionResult DeleteCompanyContactById(int id, int userid, string username)
        {
            try
            {
                var retVal = _companyRepository.DeleteCompanyContactById(id, userid, username);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteNoteById")]
        public IActionResult DeleteNoteById(int id,int userid,string username)
        {
            try
            {
                var retVal = _companyRepository.DeleteNoteById(id, userid, username);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteCompanyWorkPlaceById")]
        public IActionResult DeleteCompanyWorkPlaceById(int id,int userid,string username)
        {
            try
            {
                var retVal = _companyRepository.DeleteCompanyWorkPlaceById(id, userid, username);
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
            var contact = _companyRepository.GetCompanyContactById(id);
            return Ok(contact);
        }
       [HttpGet]
       [Route("CheckCompanyExist")]
       public IActionResult CheckCompanyExist(string name,string postcode)
        {
            var duplicateCompanyCount = _companyRepository.CheckCompanyForCrm(name,postcode);
            return Ok(duplicateCompanyCount);
        }
    }
}
