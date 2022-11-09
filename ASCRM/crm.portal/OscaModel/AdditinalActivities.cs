using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
   public class AdditinalActivities
    {
        public int PId { get; set; }
        public string Completed_Date { get; set; }
        public string Activity_Title { get; set; }
        public string Description { get; set; }
        public decimal Actual_OTJ { get; set; }
        public string Is_Locked { get; set; }
    }
}
