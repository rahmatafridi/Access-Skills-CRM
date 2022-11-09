using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.CrmModel
{
    public class Dashboard
    {
        public DateTime Learner_StartDate { get; set; }
        public DateTime Learner_PlannedEndDate { get; set; }
        public string Learner_PercentageCompleted { get; set; }
        public string NbNotCompleted { get; set; }
        public string NbTopicsCompleted { get; set; }
        public string NbCompleted { get; set; }
        public string TopicCompleted { get; set; }
        public string TopicNext { get; set; }
        public string GLH { get; set; }
        public int Learner_ID { get; set; }
        public string First_Name { get; set; }
        public string Surname { get; set; }
    }
}
