using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.dashboard.Models
{

    public class DashChartLeadsCountModel
    {
        public int lead_status_id { get; set; }
        public string lead_status_title { get; set; }
        public string lead_status_class { get; set; }
        public string lead_color { get; set; }
        public int lead_total { get; set; }
    }

    public class AllLeadsModel
    {
        public int Leads_Total { get; set; }
        public int Leads_New { get; set; }
        public int Leads_InProgress { get; set; }
        public int Leads_Completed { get; set; }
        public int Leads_Deleted { get; set; }
    }

    public class LastResultModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string LastResult { get; set; }
    }

    public class AgreedPaymentsModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string AgreedPaymentScheme { get; set; }
    }
}
