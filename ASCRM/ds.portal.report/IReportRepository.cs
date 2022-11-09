using ds.portal.report.Model;
using System;
using System.Collections.Generic;

namespace ds.portal.report
{
    public interface IReportRepository
    {
        IEnumerable<ContactReportModel> GetContacts(int security_user_id, DateTime? startDate, DateTime? endDate);
    }
}
