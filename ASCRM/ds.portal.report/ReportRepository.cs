using Dapper;
using ds.core.uow;
using ds.portal.report.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ds.portal.report
{
    public class ReportRepository : IReportRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public ReportRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }

        public IEnumerable<ContactReportModel> GetContacts(int currentUserId, DateTime? startDate, DateTime? endDate)
        {
            IEnumerable<ContactReportModel> contactreport = new List<ContactReportModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Report_GetContacts]";
                    object dynamicParams = new { @security_user_id = currentUserId, @start_date = startDate, @end_date = endDate };
                    contactreport = conn.Query<ContactReportModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return contactreport;
        }
    }
}
