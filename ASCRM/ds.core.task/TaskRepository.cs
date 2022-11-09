using Dapper;
using ds.core.task.Models;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ds.core.task
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;

        public TaskRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }
        public IEnumerable<TaskModel> GetTasks(int userId)
        {
            IEnumerable<TaskModel> notifications = new List<TaskModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Task_GetAllTasks]";
                    /// 1 module id is for lead activity.
                    object dynamicParams = new { @user_id = userId, @module_id = 1 };
                    notifications = conn.Query<TaskModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return notifications;
        }
        public bool SnoozeTask(int task_id, int task_snooze_delay)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Task_Snooze]";
                    object dynamicParams = new { @task_id = task_id , @task_snooze_delay = task_snooze_delay };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }


        public bool TaskDone(long task_id, int lead_id, int user_id, string user_name)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_Task_Done]";
                    object dynamicParams = new
                    {
                        task_id = task_id,
                        task_id_lead = lead_id,
                        task_id_user = user_id,
                        task_done_by_user_id = user_id,
                        task_done_by_username = user_name
                        //task_date_done = DateTime.Now
                    };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;

        }

        public IEnumerable<TaskModel> GetPerDayTasks(int userId)
        {
            IEnumerable<TaskModel> tasks = new List<TaskModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Task_GetPerDayTasks]";
                    object dynamicParams = new { @user_id = userId };
                    tasks = conn.Query<TaskModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    var _count = 0;
                    foreach (var item in tasks)
                    {
                        if (_count == 0)
                        {
                            item.Class_Name = "1";
                            _count = 1;
                        }
                        else if (_count == 1)
                        {
                            item.Class_Name = "2";
                            _count = 2;
                        }
                        else if (_count == 2)
                        {
                            item.Class_Name = "3";
                            _count = 3;
                        }
                        else if (_count == 3)
                        {
                            item.Class_Name = "4";
                            _count = 0;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return tasks;
        }
        public IEnumerable<TaskModel> GetAllTasks(int userId)
        {
            IEnumerable<TaskModel> tasks = new List<TaskModel>();
            List<TaskModel> collections = new List<TaskModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Task_GetAllTasksByUsersId]";
                    object dynamicParams = new { @user_id = userId };
                    tasks = conn.Query<TaskModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    var _count = 0;

                    foreach (var item in tasks)
                    {
                        collections.Add(item);
                    }

                    var _uniqueTasks = collections.GroupBy(x => x.date).ToList();

                    foreach (var item in _uniqueTasks)
                    {
                        if (_uniqueTasks.Any(x => x.Key == Convert.ToString(item.Key)))
                        {
                            _count++;
                            foreach (var _task in item.ToList())
                            {
                                if (_count ==1 )
                                {
                                    _task.Class_Name = "fc-event-solid-danger";
                                }
                                else if (_count == 2)
                                {
                                    _task.Class_Name = "fc-event-brand";
                                }
                                else if (_count == 3)
                                {
                                    _task.Class_Name = "fc-event-info";
                                }
                                else if (_count == 4)
                                {
                                    _task.Class_Name = "fc-event-primary";
                                }
                                else if (_count == 5)
                                {
                                    _task.Class_Name = "fc-event-warning";
                                }
                                else if (_count == 6)
                                {
                                    _task.Class_Name = "fc-event-solid-danger fc-event-light";
                                    _count = 0;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return tasks;
        }
        public bool Insert(TaskModel taskModel)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    ////// Module type - 2
                    string storedProc = string.Empty;
                    var p = new DynamicParameters();
                    DateTime dt = new DateTime();
                    if (!string.IsNullOrEmpty(taskModel.date_time))
                    {
                        dt = DateTime.Parse(Convert.ToString(taskModel.date_time));
                    }
                        if (taskModel.task_id > 0)
                    {
                        storedProc = "SP_Task_UpdateByTaskId";
                        p.Add("@task_id", taskModel.task_id);
                        p.Add("@title", taskModel.title);
                        p.Add("@note", taskModel.note);
                        p.Add("@date_time",dt);
                        p.Add("@is_done", taskModel.is_done);
                        p.Add("@task_action_id", taskModel.task_action_id);
                        p.Add("@updated_by", taskModel.assigned_to);
                    }
                    else
                    {
                        storedProc = "[dbo].[SP_Task_Insert]";
                        p.Add("@title", taskModel.title);
                        p.Add("@note", taskModel.note);
                        p.Add("@date_time", dt);
                        p.Add("@is_snoozed", 0);
                        p.Add("@assigned_to", taskModel.assigned_to);
                        p.Add("@is_done", taskModel.is_done);
                        p.Add("@task_action_id", 0);
                        p.Add("@module_id", 2);
                        p.Add("@is_synced", 0);
                        p.Add("@sync_type_id", 0);
                        p.Add("@created_by", taskModel.assigned_to);
                        p.Add("@lead_id", 0);
                    }
                    int a = conn.Execute(storedProc, p, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public TaskModel GetTaskById(int taskId)
        {
            TaskModel taskModel = new TaskModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Task_GetTaskById]";
                    object dynamicParams = new { @task_id = taskId };
                    taskModel = conn.QueryFirstOrDefault<TaskModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return taskModel;
        }
        public bool DeleteTask(int taskId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Task_Delete]";
                    object dynamicParams = new { @task_id = taskId };
                     conn.Query<TaskModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public IEnumerable<TaskLeadModel> GetAllLeads()
        {
            IEnumerable<TaskLeadModel> leadModels = new List<TaskLeadModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_GetAllLead]";
                    leadModels = conn.Query<TaskLeadModel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return leadModels;
        }
    }
}
