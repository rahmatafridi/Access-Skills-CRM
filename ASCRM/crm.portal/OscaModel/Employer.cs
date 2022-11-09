using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
   public  class Employer
    {
        public int Employer_Id { get; set; }
        public int User_Id { get; set; }
        public string Employer_Name { get; set; }
        public string Employer_ContactName { get; set; }
        public string Employer_Address1 { get; set; }
        public string Employer_Address2 { get; set; }
        public string Employer_Address3 { get; set; }
        public string Employer_Address5 { get; set; }
        public string Employer_Address4 { get; set; }
        public string Employer_PostCode1 { get; set; }
        public string Employer_PostCode2 { get; set; }
        public string Employer_ContactNumber1 { get; set; }
        public string Employer_Email1 { get; set; }
        public string IsSelected { get; set; }
        public string TotalAssigned { get; set; }
        public string IsAssigned { get; set; }
    }
}
