using System;
using System.Collections.Generic;
using System.Text;

namespace ds.core.uow
{
    public class UOW_Osca : IUOW_Osca
    {
        public string _connectionString;

        public UOW_Osca(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
