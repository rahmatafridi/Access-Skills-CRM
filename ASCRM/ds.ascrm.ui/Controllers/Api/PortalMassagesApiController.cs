using crm.portal.CrmModel;
using crm.portal.Interfaces.Message;
using ds.core.configuration;
using ds.core.logger;
using ds.portal.companies.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ds.ascrm.ui.Controllers.Api
{
    [Route("api/PortalMassagesApi")]
    [ApiController]

    [EnableCors("AllowOrigin")]
    public class PortalMassagesApiController : ControllerBase
    {
        private readonly IMassageRepository _massageRepository;
        readonly ILogger<PortalMassagesApiController> _log;
        LogException LogException;
        public PortalMassagesApiController(ILogger<PortalMassagesApiController> log, IConfigurationRepository iConfig, IMassageRepository massageRepository)
        {
            _massageRepository = massageRepository;
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
        private string RoleCode
        {
            get
            {
                var securityUserName = "";

                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("RoleCode")))
                {
                    securityUserName = HttpContext.Session.GetString("RoleCode");
                }

                return securityUserName;
            }
            set {; }
        }
        private string DisplayName
        {
            get
            {
                var securityUserName = "";

                if (HttpContext != null && HttpContext.Session.Keys.Any(a => a.Equals("DisplayName")))
                {
                    securityUserName = HttpContext.Session.GetString("DisplayName");
                }

                return securityUserName;
            }
            set {; }
        }

        private int LearnerContactsId
        {
            get
            {
                var securityUserId = 0;

                if (HttpContext.Session.Keys.Any(a => a.Equals("Users_Id_LearnerContacts")))
                {
                    securityUserId = HttpContext.Session.GetInt32("Users_Id_LearnerContacts") ?? 0;
                }


                return securityUserId;
            }
            set {; }
        }


        #endregion


        [HttpGet]
        [Route("GetMessages")]
        public IActionResult GetMessages()
        {
            try
            {
                var list = new List<Messages>();
                if (RoleCode == "900")
                {
                    var result = _massageRepository.GetMessages(Convert.ToInt32(UserName));

                    foreach (var item in result)
                    {
                        list.Add(new Messages()
                        {
                            id = item.id,
                            created_date = item.created_date,
                            encodedId = Encryption.Encrypt(item.id.ToString()),
                            is_question = item.is_question,
                            message = item.message,
                            topic = item.topic,
                            subject = item.subject,
                            user_name = item.user_name,
                            topic_id = item.topic_id,
                            learner_id = item.learner_id,
                            user_id = item.user_id
                        });
                    }
                }
                if(RoleCode == "100")
                {
                    var result = _massageRepository.GetMessagesAdmin();

                    foreach (var item in result)
                    {
                        list.Add(new Messages()
                        {
                            id = item.id,
                            created_date = item.created_date,
                            encodedId = Encryption.Encrypt(item.id.ToString()),
                            is_question = item.is_question,
                            message = item.message,
                            topic = item.topic,
                            subject = item.subject,
                            user_name = item.user_name,
                            topic_id = item.topic_id,
                            learner_id = item.learner_id,
                            user_id = item.user_id
                        });
                    }
                }
                return Ok(list);

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetTopic")]
        public IActionResult GetTopic(int? learnerid)
        {
            try
            {
                if (RoleCode != "100")
                {
                    return Ok(_massageRepository.courseTopics(Convert.ToInt32(UserName)));
                }
                else
                {
                    return Ok(_massageRepository.courseTopicsByAdmin(learnerid));
                }
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
            
        }
        [HttpGet]
        [Route("GetMessageById")]
        public IActionResult GetMessageById(int id)
        {
            try
            {
                return Ok(_massageRepository.GetMessagesById(id));

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }

        }

        [HttpGet]
        [Route("GetMessageDetail")]
        public IActionResult GetMessageDetail(int id)
        {
            try
            {
                return Ok(_massageRepository.messageDetails(id));

            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpPost]
        [Route("InsertMassage")]
        public IActionResult InsertMassage(Messages massages)
        {
            try
            {
                if (RoleCode != "100")
                {
                    massages.learner_id = Convert.ToInt32(UserName);
                    massages.user_name = DisplayName;
                    massages.user_id = Convert.ToInt32(UserName);
                }
                else
                {
                    massages.user_id = UserId;
                    massages.user_name = UserName;
                }
                var result = _massageRepository.InsertMassage(massages);
                return Ok(result);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
           
        }
    }
}
