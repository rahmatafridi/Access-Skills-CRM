using ds.core.task.Models;
using System.Collections.Generic;

namespace ds.core.task
{
    public interface ITaskRepository
    {
        IEnumerable<TaskModel> GetTasks(int userId);
        bool SnoozeTask(int task_id, int task_snooze_delay);
        IEnumerable<TaskModel> GetPerDayTasks(int userId);
        IEnumerable<TaskModel> GetAllTasks(int userId);
        bool Insert(TaskModel taskModel);
        TaskModel GetTaskById(int taskId);
        bool DeleteTask(int taskId);
        IEnumerable<TaskLeadModel> GetAllLeads();

        bool TaskDone(long task_id, int lead_id, int user_id, string user_name);


       
    }
}
