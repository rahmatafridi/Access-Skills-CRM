using System;
using System.Collections.Generic;
using System.Text;

namespace ds.core.uow
{
   public class UOW_Portal: IUOW_Portal
    {
        public string _connectionString;

        public UOW_Portal(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

    }
}
