using ds.core.configuration;
using ds.core.document;
using ds.core.logger;
using ds.core.task;
using ds.core.task.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/TaskApi")]
    [ApiController]
    public class TaskApiController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        readonly ILogger<DocumentApiController> _log;
        LogException LogException;
        public TaskApiController(ITaskRepository taskRepository, IHostingEnvironment hostingEnvironment
            , ILogger<DocumentApiController> log, IConfigurationRepository iConfig
            , IDocument document)
        {
            _taskRepository = taskRepository;
            _hostingEnvironment = hostingEnvironment;
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

        [HttpGet]
        [Route("GetTasks")]

        public IActionResult GetTasks()
        {
            try
            {
                var tasks = _taskRepository.GetTasks(UserId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("SnoozeTask")]
        public IActionResult SnoozeTask(int task_id, int task_snooze_delay)
        {
            try
            {
                _taskRepository.SnoozeTask(task_id, task_snooze_delay);
                return Ok(true);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }


        [HttpGet]
        [Route("TaskDone")]
        public IActionResult TaskDone(long task_id, int lead_id)
        {
            try
            {
                var retVal = _taskRepository.TaskDone(task_id, lead_id, UserId, UserName);
                return Ok(retVal);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetPerDayTasks")]
        public IActionResult GetPerDayTasks()
        {
            try
            {
                var tasks = _taskRepository.GetPerDayTasks(UserId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllTasks")]
        public IActionResult GetAllTasks()
        {
            try
            {
                var tasks = _taskRepository.GetAllTasks(UserId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpPost]
        [Route("InsertTask")]
        [HistoryFilter]
        public IActionResult InsertTask(TaskModel taskModel)
        {
            try
            {
                taskModel.assigned_to = UserId;
                var tasks = _taskRepository.Insert(taskModel);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetTaskById")]
        public IActionResult GetTaskById(int taskId)
        {
            try
            {
                var tasks = _taskRepository.GetTaskById(taskId);
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteTask")]
        public IActionResult DeleteTask(int taskId)
        {
            try
            {
                var task = _taskRepository.DeleteTask(taskId);
                return Ok(task);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("GetAllLeads")]
        public IActionResult GetAllLeads()
        {
            try
            {
                var allLeads = _taskRepository.GetAllLeads();
                return Ok(allLeads);
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}