using Dapper;
using ds.core.uow;
using ds.portal.dashboard.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ds.portal.dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public DashboardRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }
        public IEnumerable<DashChartLeadsCountModel> GetAllLeadsForDashboard(int userId, bool isAdmin)
        {
            IEnumerable<DashChartLeadsCountModel> lead_count_data = new List<DashChartLeadsCountModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Lead_Dashboard_GetLeadCounts]";//SP_mdlead_Dashboard_GetAllLeads
                    object dynamicParams = new { @user_id = userId, @is_admin = isAdmin };// object dynamicParams = new { @UserId = userId, @isAdmin = isAdmin };
                    lead_count_data = conn.Query<DashChartLeadsCountModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lead_count_data;
        }
        public SearchResults GetSearchResults(string searchQuery, int userId, bool isAdmin, string userName)
        {
            SearchResults searchResults = new SearchResults();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedPrcoSearch = "[dbo].[SP_GetSearchRecords]";
                    string storedProcCompany = "[dbo].[SP_GetSearchRecordsFor_Compnay]";
                    string storedProcWorkPlace = "[dbo].[SP_GetSearchWorkPlace]";
                    string storedProcCompContact = "[dbo].[SP_GetSearch_CompanyContact]";
                    string storedProcWPContact = "[dbo].[SP_GetSearch_WP_Contact]";

                    object dynamicParamsSearch = new { search_key = searchQuery, user_id = userId, user_name = userName };

                    //object dynamicParamsCompany = new { search_key = searchQuery, user_id = userId, user_name = userName };
                    //object dynamicParams3 = new { search_key = searchQuery, user_id = userId, user_name = userName };

                    var data = conn.QueryMultiple(storedPrcoSearch, param: dynamicParamsSearch, commandType: CommandType.StoredProcedure);
                    var item1 = data.Read<MyLeadsModel>();
                    var item2 = data.Read<NotesModel>();
                    var item3 = data.Read<MyActivitiesModels>();


                    searchResults.Leads = item1;
                    searchResults.Notes = item2.ToList();
                    searchResults.Activities = item3.ToList();
                    conn.Close();
                    conn.Open();
                    var company = conn.QueryMultiple(storedProcCompany, param: dynamicParamsSearch, commandType: CommandType.StoredProcedure);
                    var Item4 = company.Read<CompanyModel>();
                    searchResults.Companies = Item4;

                    conn.Close();
                    conn.Open();

                    var workplace = conn.QueryMultiple(storedProcWorkPlace, param: dynamicParamsSearch, commandType: CommandType.StoredProcedure);
                    var Item5 = workplace.Read<CompanyWorkplaces>();
                    searchResults.companyWorkplaces = Item5;

                    conn.Close();
                    conn.Open();

                    var companyContact = conn.QueryMultiple(storedProcCompContact, param: dynamicParamsSearch, commandType: CommandType.StoredProcedure);
                    var Item6 = companyContact.Read<CompanyContact>();
                    searchResults.companyContacts = Item6;



                    conn.Close();
                    conn.Open();

                    var wpContact = conn.QueryMultiple(storedProcWPContact, param: dynamicParamsSearch, commandType: CommandType.StoredProcedure);
                    var Item7 = wpContact.Read<CompanyContact>();
                    searchResults.WorkplaceContacts = Item7;


                    searchResults.Leads.ToList().ForEach(e => { e.CanEdit = (isAdmin || e.SalesAdvisorId == userId ? true : false);  e.CanDelete = (isAdmin ? true : false); });
                    searchResults.Notes.ToList().ForEach(e => e.CanEdit = (isAdmin || e.SalesAdvisorId == userId ? true : false));
                    searchResults.Activities.ToList().ForEach(e => e.CanEdit = (isAdmin || e.SalesAdvisorId == userId ? true : false));

                }
                catch (Exception ex)
                {
                      throw;
                }
            }
            return searchResults;
        }
        public IEnumerable<MyLeadsModel> GetMyLeadsForDashboard(int user_Id)
        {
            IEnumerable<MyLeadsModel> myLeadsModel = new List<MyLeadsModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Dashboard_GetMyLeads]";
                    object dynamicParams = new { Users_Id = user_Id };

                    myLeadsModel = conn.Query<MyLeadsModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return myLeadsModel;
        }
        public IEnumerable<MyActivitiesModels> GetMyActivitiesForDashboard(int user_Id)
        {
            IEnumerable<MyActivitiesModels> myActivities = new List<MyActivitiesModels>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Dashboard_GetMyActivities]";
                    object dynamicParams = new { Users_Id = user_Id };

                    myActivities = conn.Query<MyActivitiesModels>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return myActivities;
        }
        public IEnumerable<MyLastSevenDaysActivitiesModel> GetLastSevenDaysLead(int userId, bool isAdmin)
        {
            IEnumerable<MyLastSevenDaysActivitiesModel> myActivities = new List<MyLastSevenDaysActivitiesModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Dashboard_GetLastSevenDaysLead]";
                    object dynamicParams = new { @UserId = userId, @isAdmin = isAdmin };
                    myActivities = conn.Query<MyLastSevenDaysActivitiesModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return myActivities;
        }
        public LeadSalesAdvisorStatusModel GetLeadSalesAdvisor()
        {
            LeadSalesAdvisorStatusModel leadSalesAdvisorStatusModel = new LeadSalesAdvisorStatusModel();
            leadSalesAdvisorStatusModel.leadSalesAdvisorStatusModels = new List<LeadSalesAdvisorModel>();
            leadSalesAdvisorStatusModel.salesAdvisors = new List<SalesAdvisorStatusModel>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Dashboard_GetLeadSalesAdvisorStatus]";
                    leadSalesAdvisorStatusModel.leadSalesAdvisorStatusModels = conn.Query<LeadSalesAdvisorModel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            var _uniqueSalesAdvisor = leadSalesAdvisorStatusModel.leadSalesAdvisorStatusModels.GroupBy(x => x.users_id);
            foreach (var item in _uniqueSalesAdvisor)
            {
                var _total = 0;
                var _completed = item.Where(x => x.Opt_Title == "Completed").Sum(x=>x.TotalCount);
                var _deleted = item.Where(x => x.Opt_Title == "Deleted").Sum(x => x.TotalCount);
                var _inProgress = item.Where(x => x.Opt_Title == "In Progress").Sum(x => x.TotalCount);
                var _new = item.Where(x => x.Opt_Title == "New").Sum(x => x.TotalCount);
                _total = _completed + _deleted + _inProgress + _new;
                if (_total <= 0) { _total = 1; }
                leadSalesAdvisorStatusModel.salesAdvisors.Add(new SalesAdvisorStatusModel
                {
                    Users_Username = item.FirstOrDefault().Users_Username,
                    Completed = _completed,
                    Deleted = _deleted,
                    InProgress = _inProgress,
                    New = _new,

                    CompletedCalc = Math.Round(Convert.ToDecimal(_completed * 100) / _total),
                    DeletedCalc = Math.Round(Convert.ToDecimal(_deleted * 100) / _total),
                    InProgressCalc = Math.Round(Convert.ToDecimal(_inProgress * 100) / _total),
                    NewCalc = Math.Round(Convert.ToDecimal(_new * 100) / _total),
                });

            }

            return leadSalesAdvisorStatusModel;
        }
        public IEnumerable<LastResultModel> GetLastResults(int userId, bool isAdmin)
        {
            IEnumerable<LastResultModel> lastResults  = new List<LastResultModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Dashboard_GetLastResultsCount]";
                    object dynamicParams = new { @UserId = userId, @isAdmin = isAdmin };
                    lastResults = conn.Query<LastResultModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return lastResults;
        }
        public IEnumerable<AgreedPaymentsModel> GetAgreedPaymentSchemes(int userId, bool isAdmin)
        {
            IEnumerable<AgreedPaymentsModel> agreedPayments  = new List<AgreedPaymentsModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Dashboard_GetAgreedPaymentSchemeCount]";
                    object dynamicParams = new { @UserId = userId, @isAdmin = isAdmin };
                    agreedPayments = conn.Query<AgreedPaymentsModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return agreedPayments;
        }
        public IEnumerable<MyLeadsModel> GetContactAttempts(int userId, bool isAdmin, string lastResult)
        {
            IEnumerable<MyLeadsModel> myLeadsModel = new List<MyLeadsModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Dashboard_GetContactAttempts]";
                    object dynamicParams = new
                    {
                        user_id = userId,
                        is_Admin = isAdmin,
                        last_result = lastResult
                    };
                    myLeadsModel = conn.Query<MyLeadsModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return myLeadsModel;
        }
    }
}
