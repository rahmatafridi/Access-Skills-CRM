using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
 public  class Courses
    {
        public int Course_Id { get; set; }

        public string Course_Title { get; set; }

        public string Course_Level { get; set; }

        public int? Course_SchemeCode { get; set; }

        public string Course_Ref { get; set; }

        public int Course_Id_Sector { get; set; }

        public byte Course_IsActive { get; set; }

        public int Course_WeightOrder { get; set; }

        public int? Course_NbObs { get; set; }

        public int? Course_Group { get; set; }
    }
}
