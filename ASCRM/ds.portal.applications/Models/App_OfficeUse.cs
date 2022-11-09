using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.applications.Models
{
    public class App_OfficeUse
    {
        public int app_id { get; set; }
        public int? app_officeuse1_courselevel { get; set; }
        public string app_officeuse_uniquelearnerreference { get; set; }
        public string app_officeuse_startdate { get; set; }
        public string app_officeuse_enddate { get; set; }
        //public DateTime? app_officeuse_startdate { get; set; }
        //public DateTime? app_officeuse_enddate { get; set; }
        public string app_officeuse_apprenticeshipframework { get; set; }
        public int? app_officeuse_learnerid { get; set; }
        public string app_officeuse_cqcinspectiondate { get; set; }
        //public DateTime? app_officeuse_cqcinspectiondate { get; set; }
        public string app_officeuse_ukprn { get; set; }
        public string app_officeuse_employerid { get; set; }
        public string app_officeuse_referenceid { get; set; }
        public bool app_officeuse_isevidenceseen { get; set; }
        public string app_officeuse_referenceidtype { get; set; }
        //public DateTime? app_officeuse_referencedate { get; set; }
        public string app_officeuse_referencedate { get; set; }
        public int? app_officeuse_fundingid { get; set; }
        public string app_officeuse_fundingtitle { get; set; }
    }
}
