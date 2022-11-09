using ds.portal.dashboard.Models;
using System.Collections.Generic;

namespace ds.portal.dashboard
{
    public interface IDashboardRepository
    {
        IEnumerable<DashChartLeadsCountModel> GetAllLeadsForDashboard(int userId, bool isAdmin);
        SearchResults GetSearchResults(string searchQuery, int userId, bool isAdmin, string userName);
        IEnumerable<MyLeadsModel> GetMyLeadsForDashboard(int user_Id);
        IEnumerable<MyActivitiesModels> GetMyActivitiesForDashboard(int user_Id);
        IEnumerable<MyLastSevenDaysActivitiesModel> GetLastSevenDaysLead(int userId, bool isAdmin);
        LeadSalesAdvisorStatusModel GetLeadSalesAdvisor();
        IEnumerable<LastResultModel> GetLastResults(int userId, bool isAdmin);
        IEnumerable<AgreedPaymentsModel> GetAgreedPaymentSchemes(int userId, bool isAdmin);
        IEnumerable<MyLeadsModel> GetContactAttempts(int userId, bool isAdmin, string lastResult);
    }
}
