namespace ds.portal.sqlupgrade.Models
{
    public class Product
    {
        public string ProductName { get; set; }
        public string Edition { get; set; }
        public int Release { get; set; }
        public int Build { get; set; }
        public int Patch { get; set; }
    }

    public class ProductVersion
    {
        public string SQLScriptVersion { get; set; }
    }
}
