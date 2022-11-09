using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.companies.Models
{
    public class CompanyAssignedLearner
    {
        public int id { get; set; }

        public int? wp_id { get; set; }

        public int? learner_id { get; set; }

        public DateTime? date_assigned { get; set; }

        public bool? is_active { get; set; }

    }
    public class Employeer
    {
        public int Employer_Id { get; set; }
        public string Employer_Name { get; set; }
        public string Group_Name { get; set; }
        public string Employer_Address { get; set; }
        public string Employer_ContactName { get; set; }
        public string Employer_ContactNumber1 { get; set; }
        public string Employer_EDRSNumber { get; set; }
        public string Employer_Email1 { get; set; }
        public string ECD_Tel { get; set; }
        public string ECD_JobTitle { get; set; }
        public string Employer_NbEmployees { get; set; }
        public string Employer_IsLevyPayingEmployer { get; set; }
        public string ModDate { get; set; }
        public  string Postcode { get; set; }
    }
}
