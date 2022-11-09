using System;
using System.Collections.Generic;
using System.Text;

namespace portal.data.repository
{
    public sealed class Data_UOW : IData_UOW
    {
        public string _connectionString;

        public Data_UOW(string connectionString)

        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
