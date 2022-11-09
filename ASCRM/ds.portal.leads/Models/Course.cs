using System;
using System.Collections.Generic;
using System.Text;

namespace ds.portal.leads.Models
{
    public class Course
    {
        public int Course_Id { get; set; }
        public string Course_Title { get; set; }
        public string Course_Level { get; set; }
        public int Course_SchemeCode { get; set; }
        public string Course_Ref { get; set; }
        public int Course_Id_Sector { get; set; }
        public int Course_IsActive { get; set; }
        public int Course_WeightOrder { get; set; }
        public int Course_NbObs { get; set; }
        public int Course_Group { get; set; }
    }
}
