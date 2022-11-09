using ds.portal.sqlupgrade.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ds.portal.sqlupgrade
{
    public class SqlUpgradeRepository : ISqlUpgrade
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public SqlUpgradeRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }

        public IEnumerable<Product> GetProductVersion()
        {
            IEnumerable<Product> products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdLead_GetAll]";
                    products = conn.Query<Product>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return leads;
        }
    }
}
