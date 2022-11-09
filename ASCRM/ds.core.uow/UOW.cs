namespace ds.core.uow
{
    public sealed class UOW : IUOW
    {
        public string _connectionString;

        public UOW(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
