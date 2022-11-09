using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
   public class CompletedActivities
    {
        public int PId { get; set; }
        public string Completed_Date { get; set; }
        public decimal Planned_OTJ { get; set; }
        public  string Topic_Title { get; set; }
        public  decimal Actual_OTJ { get; set; }

        public string Is_Locked { get; set; }
        public string CPDR_IsCompletedByCoordinator { get; set; }
    }
}
