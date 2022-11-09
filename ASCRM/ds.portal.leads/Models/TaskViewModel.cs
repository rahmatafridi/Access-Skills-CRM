using System;

namespace ds.portal.leads.Models
{
    public class TaskViewModel
    {
        public int task_id { get; set; }
        public int task_id_lead { get; set; }
        public int task_id_user { get; set; }       
        public int task_id_optheader { get; set; }
        public int task_id_activity { get; set; }
        public string task_start { get; set; }
        public DateTime? task_end { get; set; }
        public string task_scheduled_with { get; set; }
        public string task_note { get; set; }
        public int? task_created_by_user_id { get; set; }
        public string task_created_by_username { get; set; }
        public DateTime task_date_created { get; set; }
        public int? task_updated_by_user_id { get; set; }
        public string task_updated_by_username { get; set; }
        public DateTime? task_date_updated { get; set; }
        public bool? task_is_deleted { get; set; }
        public int? task_deleted_by_user_id { get; set; }
        public string task_deleted_by_username { get; set; }
        public DateTime? task_date_deleted { get; set; }         
        public string task_activity { get; set; }
        public string task_company { get; set; }
 
        public string task_start_str { get; set; }


        public bool? task_is_done { get; set; }

        public int? task_done_by_user_id { get; set; }

        public string task_done_by_username { get; set; }

        public DateTime? task_date_done { get; set; }
        public string task_date_done_str { get; set; }

        
    }

}
