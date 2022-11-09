using System;
using System.Collections.Generic;
using System.Text;

namespace crm.osca.Models
{
    public class Doc_CourseSummary_TopicItems
    {
        public int _SSJLP_Id { get; set; }
        public int _SSJLP_Id_Learner { get; set; }
        public int _SSJLP_Id_LearnerCourse { get; set; }
        public string _SSJLP_Topic { get; set; }
        public string _IsCompleted { get; set; }

        public string _CompletedDate { get; set; }
        //public string _HasDocuments;
        public string _Checkpointdate { get; set; }
        public string _AlertColour { get; set; }

        public string _filter_label { get; set; }
        public string _filter_label_caption { get; set; }
        public string _duedate_completed_label { get; set; }
    }
}
