using ds.core.configuration.Models;
using System;
using System.Collections.Generic;

namespace ds.core.configuration
{
    public interface IConfigurationRepository
    {
        IEnumerable<ConfigurationModel> GetAllConfigration();
        ConfigurationModel GetConfigrationById(string config_id);
        string GetSingleConfigValueByConfigKey(string config_key);
        bool InsertUpdateConfigration(ConfigurationModel configuration);
        Tuple<bool, bool> CheckKeyAndValueExists(ConfigurationModel configuration);
        bool DeleteConfigrationById(int config_id, int userId, string userName);
        string GetConnectionString();
        bool InsertAuditConfiguration(AuditModel auditModel);
        ProductModel GetProduct();
    }
}
