using ds.core.configuration;
using ds.core.logger;
using ds.portal.leads;
using ds.portal.leads.Models.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/ListApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ListApiController : ControllerBase
    {
        private readonly ILead_ListRepository _Lead_ListRepository;
        readonly ILogger<ListApiController> _log;
        LogException LogException;
        public ListApiController(ILead_ListRepository Lead_ListRepository
            , ILogger<ListApiController> log, IConfigurationRepository iConfig)
        {
            _Lead_ListRepository = Lead_ListRepository;
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
        [HttpPost]
        [Route("InsertUpdateOptionRecord")]
        [HistoryFilter]
        public bool InsertUpdateOptionRecord(DropdownOption dropdownOption)
        {
            try
            {
                return _Lead_ListRepository.InsertUpdateOptionRecord(dropdownOption);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetAllDropdownsHeaders")]
        public IActionResult GetAllDropdownsHeaders()
        {
            try
            {
                var dropdownHeaders = _Lead_ListRepository.GetDropdownOptionsHeaders();
                return Ok(dropdownHeaders);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetDropdownOptionsByHeaderId")]
        public IActionResult GetDropdownOptionsByHeaderId(int id)
        {
            try
            {
                var dropdownOptions = _Lead_ListRepository.GetDropdownOptionsByHeaderId(id);
                return Ok(dropdownOptions);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetDropdownOptionsByHeaderName")]
        public IActionResult GetDropdownOptionsByHeaderName(string headerName)
        {
            try
            {
                var dropdownOptions = _Lead_ListRepository.GetDropdownOptionsByHeaderName(headerName);
                return Ok(dropdownOptions);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
        [HttpGet]
        [Route("GetDropdownOptionsByOptionId")]
        public IActionResult GetDropdownOptionsByOptionId(int id)
        {
            try
            {
                var dropdownOptions = _Lead_ListRepository.GetDropdownOptionsByHeaderId(id);
                return Ok(dropdownOptions);
            }
            catch (Exception ex)
            {
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
                return _Lead_ListRepository.DeleteDropdownOptionByOptionId(id, UserId, UserName);
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
                return _Lead_ListRepository.CheckTitleAndValueExists(dropdownOption);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}