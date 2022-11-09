using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
   public class CourseTopicSummary
    {
        public int LearnerId { get; set; }
        public int LearnerCourseId { get; set; }
       public List<CourseTopic> CourseTopics { get; set; }

    }
    public class CourseTopic
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string TopicStatus { get; set; }
        public string HasDocuments { get; set; }
        public int SSJLP_Id { get; set; }
        public int SSJLP_Id_Learner { get; set; }
        public int SSJLP_Id_LearnerCourse { get; set; }
        public string IsCompleted { get; set; }
        public string CompletedDate { get; set; }
        public string SSJLP_Topic { get; set; }
        public string Checkpointdate { get; set; }

        public string AlertColour { get; set; }

        public List<CourseTopicDocuments> Documents { get; set; }
        public bool isWorkSubmit { get; set; }
        public int AP_TopicId { get;set; }

    }
    public class CourseTopicDocuments
    {
        public int DocumentId { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string DocTopic { get; set; }
        //public int LearnerId { get; set; }
        //public int LearnerCourseId { get; set; }


    }
}
