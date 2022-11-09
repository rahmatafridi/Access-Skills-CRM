using Dapper;
using ds.core.uow;
using ds.portal.sqlupgrade.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;

namespace ds.portal.sqlupgrade
{
    public class SqlUpgradeRepository : ISqlUpgradeRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        private readonly IHostingEnvironment _env;

        public SqlUpgradeRepository(IUOW unitOfWork,IHostingEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
            _env = env;
        }

        public IEnumerable<Product> GetProductVersion()
        {
            IEnumerable<Product> products = new List<Product>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_Product_Get]";
                    products = conn.Query<Product>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return products;
        }

        public bool UpdateSQLVersion()
        {
            try
            {
                updatedatabase();
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        private void updatedatabase()
        {
            SqlConnection conn = new SqlConnection(_connString);
            try
            {
                conn.Open();

                var productVersions = GetProductVersion();

                string webRootPath = _env.WebRootPath;
                foreach (var files in Directory.GetFiles(webRootPath + "/Assets/SQLUpgrade", "*.sql"))
                {
                    FileInfo info = new FileInfo(files);
                    var filePath = info.FullName;
                    var fileName = Path.GetFileName(info.FullName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (fileName.Contains(".sql"))
                        {
                            var collection = fileName.ToString().Replace("po_v_", "").Replace(".sql", "").Split('.');
                            if (collection.Length == 3)
                            {
                                var version = Convert.ToInt32(collection[2]);
                                foreach (var pro in productVersions)
                                {
                                    if (pro.Patch < version)
                                    {
                                        string script = File.ReadAllText(filePath);

                                        IEnumerable<string> commandStrings = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                                        foreach (string commandString in commandStrings)
                                        {
                                            if (commandString.Trim() != "")
                                            {
                                                new SqlCommand(commandString, conn).ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException er)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
