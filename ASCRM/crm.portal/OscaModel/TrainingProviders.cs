using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.OscaModel
{
   public class TrainingProviders
    {
        public int TrainingProvider_Id { get; set; }

        public string TrainingProvider_Title { get; set; }

        public string TrainingProvider_ForeColour { get; set; }

        public string TrainingProvider_BGColour { get; set; }

        public int TrainingProvider_WeightOrder { get; set; }

        public string TrainingProvider_Workshop_Template { get; set; }

        public string TrainingProvider_ShortCourse_Template { get; set; }

        public bool TrainingProvider_IsDeleted { get; set; }
    }
}
