using crm.portal.OscaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.MyCPD
{
   public interface IMyCPDRepository
    {
        PrimeReviewStats CPDGetPrimeReviewStats(int Id);
        IEnumerable<CompletedActivities> GetCompletedActivities(int Id);
        List<AdditinalActivities> GetAdditinalActivities(int Id);
        bool UpdateCompletedActivity(CompletedActivities act);
        bool UpdateAdditinalActivity(AdditinalActivities act,int userId);

    }
}
