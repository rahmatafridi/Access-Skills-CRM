using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.companies.Models
{
   public  class AssignedLearnerList
    {
        public int ID { get; set; }
        public string LearnerName { get; set; }
        public string CandidateStatus { get; set; }
        public string EnrolmentDate { get; set; }
        public string WhosePaying { get; set; }
        public string CompletionEnrolledCancelledDate { get; set; }
        public string CourseLevel { get; set; }
        
        //public string Cancelled { get; set; }
        //public string Link { get; set; }
    }
    public class RootObject
    {
        public List<AssignedLearnerList> SPs { get; set; }
    }
}
