using System.Collections.Generic;

namespace ds.portal.dashboard.Models
{
    public class MyLastSevenDaysActivitiesModel
    {
        public int TotalCount { get; set; }
        public string Lead_DateCreated { get; set; }
    }
    public class LeadSalesAdvisorModel
    {
        public int users_id { get; set; }
        public int TotalCount { get; set; }
        public string Users_Username { get; set; }
        public string Opt_Title { get; set; }
    }
    public class LeadSalesAdvisorStatusModel
    {
        public IEnumerable<LeadSalesAdvisorModel> leadSalesAdvisorStatusModels { get; set; }
        public IList<SalesAdvisorStatusModel> salesAdvisors { get; set; }
    }

    public class SalesAdvisorStatusModel
    {
        public string Users_Username { get; set; }
        public int InProgress { get; set; }
        public int New { get; set; }
        public int Completed { get; set; }
        public int Deleted { get; set; }

        public decimal InProgressCalc { get; set; }
        public decimal NewCalc { get; set; }
        public decimal CompletedCalc { get; set; }
        public decimal DeletedCalc { get; set; }
    }

}
