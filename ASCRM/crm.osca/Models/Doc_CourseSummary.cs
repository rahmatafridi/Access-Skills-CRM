using System;
using System.Collections.Generic;
using System.Text;

namespace crm.osca.Models
{
   public class Doc_CourseSummary
    {
        public int _tapId { get; set; }

        public int LearnerId { get; set; }
        public int LearnerCourseId { get; set; }
        public bool is_error { get; set; }
        public string error_message { get; set; }
        public string LearnerName { get; set; }
        public string todaydate { get; set; }

        public string LearnerEmail { get; set; }
        public string LearnerDob { get; set; }
        public string LearnerClass { get; set; }
        public string CurrentCourse { get; set; }

        public string CourseStartDate { get; set; }
        public string CourseEndDate { get; set; }
        public string CourseEstimationEndDate { get; set; }
        public string EmployerWorkplace { get; set; }
        public string EmployerWorkplaceName { get; set; }
        public string EmployerWorkplaceEmail { get; set; }
        public string CoordinatorName { get; set; }
        public string Percentage { get; set; }
        public string NextTopic { get; set; }
        public string NextTopicDueDate { get; set; }
        public string Planning { get; set; }
        public string CompletedTopics { get; set; }

        public string us_UpdaterName { get; set; }              // who updated it
        public string us_UpdaterEmail { get; set; } // updater email
        public string us_UpdatedOn { get; set; }                // date of the update

        public string Mail_HTML_BODY { get; set; }
        public string EmailRecipients { get; set; }

        public string Tap2Documentid { get; set; }

        public string Learner_Class { get; set; }
        public string Learner_StartDate { get; set; }
        public string Learner_PlannedEndDate { get; set; }
        public string Learner_EstimatedEndDate { get; set; }
        public string LearnerDOB { get; set; }
        public string Learner_CurrentCourse { get; set; }


        public int TotalCompletedTopics { get; set; }
        public int TotalNotCompletedTopics { get; set; }
        public List<Doc_CourseSummary_TopicItems> listItem { get; set; }
    }
}
