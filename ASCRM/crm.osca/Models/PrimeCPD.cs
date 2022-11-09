using System;
using System.Collections.Generic;
using System.Text;

namespace crm.osca.Models
{
   public class PrimeCPD
    {
        public int learner_id { get; set; }
        public string learner_name { get; set; }
        public string date_start { get; set; }
        public string date_practical_end { get; set; }
        public string weekly_hrs { get; set; }
        public string daily_hrs { get; set; }
        public string min_20_otj_hrs { get; set; }
        public string actual_otj_hrs_completed { get; set; }
        public string actual_otj_hrs_add { get; set; }
        public string otj_working_percentage { get; set; }
        public string otj_weeks_on_programme { get; set; }
        public string nb_weeks_now { get; set; }
        public string remaining_otj_hrs { get; set; }
        public string actual_otj_hrs { get; set; }
        public string otj_working_status { get; set; }
    }
}
