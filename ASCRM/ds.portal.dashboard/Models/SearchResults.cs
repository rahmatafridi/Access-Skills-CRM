using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.dashboard.Models
{
    public class SearchResults
    {
        public IEnumerable<MyLeadsModel> Leads { get; set; }
        public IEnumerable<MyActivitiesModels> Activities { get; set; }
        public IEnumerable<NotesModel> Notes { get; set; }
        public IEnumerable<CompanyModel> Companies { get; set; }
        public IEnumerable<CompanyWorkplaces> companyWorkplaces { get; set; }
        public IEnumerable<CompanyContact> companyContacts { get; set; }
        public IEnumerable<CompanyContact> WorkplaceContacts { get; set; }
    }
}
