using Dapper;
using ds.core.configuration.Models;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ds.core.configuration
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public ConfigurationRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }

        public IEnumerable<ConfigurationModel> GetAllConfigration()
        {
            IEnumerable<ConfigurationModel> configrations = new List<ConfigurationModel>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Core_Configuration_GetAll]";
                    configrations = conn.Query<ConfigurationModel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return configrations;
        }
        public ConfigurationModel GetConfigrationById(string config_id)
        {
            ConfigurationModel configration = new ConfigurationModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Core_Configuration_GetById]";
                    object dynamicParams = new { config_id = config_id };
                    configration = conn.QuerySingleOrDefault<ConfigurationModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return configration;
        }
        public string GetSingleConfigValueByConfigKey(string config_key)
        {
            string value = "";
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Core_Configuration_GetSingleValueByKey]";
                    object dynamicParams = new { config_key = config_key };
                    value = conn.QuerySingleOrDefault<string>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return value;
        }
        public bool InsertUpdateConfigration(ConfigurationModel configuration)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (configuration.config_id > 0)
                    {
                        string storedProc = "[dbo].[SP_Core_Configuration_Update]";
                        object dynamicParams = configuration;
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_Core_Configuration_Insert]";
                        object dynamicParams = configuration;
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public Tuple<bool, bool> CheckKeyAndValueExists(ConfigurationModel configuration)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Core_Configuration_CheckKeyAndValueExists]";
                    object dynamicParams = new
                    {
                        config_id = configuration.config_id,
                        config_key = configuration.config_key,
                        config_value = configuration.config_value
                    };

                    var dynamic = conn.QuerySingleOrDefault<dynamic>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    var fields = dynamic as IDictionary<string, object>;
                    return Tuple.Create<bool, bool>(Convert.ToBoolean(fields["KeyExists"]), Convert.ToBoolean(fields["ValueExists"]));
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public bool DeleteConfigrationById(int config_id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Core_Configuration_DeleteById]";
                    object dynamicParams = new { config_id = config_id};
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool InsertAuditConfiguration(AuditModel auditModel)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Audit_Insert]";
                    object dynamicParams = auditModel;
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public ProductModel GetProduct()
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_GetProduct]";
                    return conn.QuerySingleOrDefault<ProductModel>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public string GetConnectionString()
        {
            return _connString;
        }
    }
}
