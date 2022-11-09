using ds.portal.sqlupgrade.Models;
using System.Collections.Generic;

namespace ds.portal.sqlupgrade
{
    public interface ISqlUpgradeRepository
    {
        IEnumerable<Product> GetProductVersion();
        bool UpdateSQLVersion();
    }
}
