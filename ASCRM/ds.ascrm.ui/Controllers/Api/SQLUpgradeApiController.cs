using ds.core.configuration;
using ds.core.logger;
using ds.portal.sqlupgrade;
using ds.portal.sqlupgrade.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ds.portal.ui.Controllers.Api
{
    [Route("api/SQLUpgradeApi")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class SQLUpgradeApiController : ControllerBase
    {
        private readonly ISqlUpgradeRepository _sqlUpgrade;
        private readonly IHostingEnvironment _env;
        readonly ILogger<SQLUpgradeApiController> _log;
        LogException LogException;
        public SQLUpgradeApiController(ISqlUpgradeRepository sqlUpgrade, IHostingEnvironment env
            , ILogger<SQLUpgradeApiController> log, IConfigurationRepository iConfig)
        {
            _sqlUpgrade = sqlUpgrade;
            _env = env;
            _log = log;
            LogException = new LogException(iConfig, _log);
        }

        [HttpPost]
        [Route("InsertProductVersion")]
        [HistoryFilter]
        public bool InsertProductVersion()
        {
            try
            {
                return _sqlUpgrade.UpdateSQLVersion();
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
       
        [HttpGet]
        [Route("GetProductVersions")]
        public List<ProductVersion> GetProductVersions()
        {
            try
            {
                var productVersions = _sqlUpgrade.GetProductVersion();
                List<ProductVersion> versions = new List<ProductVersion>();

                ArrayList arrayList = new ArrayList();
                string webRootPath = _env.WebRootPath;

                foreach (var files in Directory.GetFiles(webRootPath + "/Assets/SQLUpgrade", "*.sql"))
                {
                    FileInfo info = new FileInfo(files);
                    var fileName = Path.GetFileName(info.FullName);
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        if (fileName.Contains(".sql"))
                        {
                            arrayList.Add(fileName);
                        }
                    }
                }

                foreach (var item in arrayList)
                {
                    var fileName = string.Empty;
                    fileName = item.ToString();

                    var collection = item.ToString().Replace("po_v_", "").Replace(".sql", "").Split('.');
                    if (collection.Length == 3)
                    {
                        var version = Convert.ToInt32(collection[2]);
                        foreach (var pro in productVersions)
                        {
                            if (pro.Patch < version)
                            {
                                versions.Add(new ProductVersion
                                {
                                    SQLScriptVersion = fileName,
                                });
                            }
                        }
                    }
                }
               
                return versions;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }

        [HttpGet]
        [Route("DeleteSqlFile")]
        [HistoryFilter]
        public bool DeleteSqlFile(string deletedFileName)
        {
            try
            {
                string webRootPath = _env.WebRootPath;
                foreach (var files in Directory.GetFiles(webRootPath + "/Assets/SQLUpgrade", "*.sql"))
                {
                    FileInfo info = new FileInfo(files);
                    var filePath = info.FullName;
                    var _fileName = Path.GetFileName(info.FullName);
                    if (!string.IsNullOrEmpty(_fileName))
                    {
                        if (_fileName.Contains(".sql"))
                        {
                            if (_fileName == deletedFileName)
                            {
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                            }
                        }
                    }
                }

               
                return true;
            }
            catch (Exception ex)
            {
                LogException.Log(ex, this.ControllerContext);
                throw new Exception(ex.ToString());
            }
        }
    }
}