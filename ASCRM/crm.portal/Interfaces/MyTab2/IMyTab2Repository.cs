using crm.portal.OscaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.MyTab2
{
   public  interface IMyTab2Repository
    {
        List<TrainingAssessmentPlan> TrainingAssessmentPlanGet(int Id);
        TrainingAssessmentPlan TrainingAssessmentPlanGetBy_Id(int Id);
        int SaveSignatur(Tab tab);
    }
}
