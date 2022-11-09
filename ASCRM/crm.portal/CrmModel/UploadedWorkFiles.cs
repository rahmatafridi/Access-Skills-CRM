using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.CrmModel
{
    public class UploadedWorkFiles
    {
        public int id { get; set; }
        public string topic_id { get; set; }
        public int? work_upload_id { get; set; }

        public string file_name { get; set; }

        public string file_type { get; set; }

        public string file_size { get; set; }

        public string file_ext { get; set; }

        public DateTime? created_on { get; set; }

        public int? created_by { get; set; }

        public string file_path { get; set; }
        public bool isAssessor { get; set; }
        public int learner_id
        {
            get; set;

        }
        public string Status { get; set; }
        public string Topic { get; set; }
        public int assessor_rejected { get; set; }
        public string due_date { get; set; }
        public DateTime? due_date1 { get; set; }
        public DateTime completed_date { get; set; }
        public string LearnerName { get; set; }
        public int is_admin_approved { get; set; }
        public bool is_assessorUpload { get; set; }
        public int is_submited { get; set; }
        public string assessor_name { get; set; }
        public int learner_submit_count { get; set; }

        public string user_name { get; set; }
    

        public int? is_work_checked { get; set; }

        public int? is_ready_for_marking { get; set; }

        public int? is_marking_validated { get; set; }

        public int? is_rejected { get; set; }

        public int? is_assessor_marking_valid { get; set; }

        public int assessor_assigned_user_id { get; set; }

        public string rejected_reason { get; set; }

        public string note { get; set; }
    }
}