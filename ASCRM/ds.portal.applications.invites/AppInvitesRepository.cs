using Dapper;
using ds.core.configuration.Models;
using ds.core.uow;
using ds.portal.applications.invites.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ds.portal.applications.invites
{
    public class AppInvitesRepository : IAppInvitesRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        public AppInvitesRepository(IUOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }
        public IEnumerable<AppInvites> GetAllApplicationInvites()
        {
            IEnumerable<AppInvites> appInvites = new List<AppInvites>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_AppInvites_GetAll]";
                    object dynamicParams = new { lead_id = 0 };

                    appInvites = conn.Query<AppInvites>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return appInvites;
        }
        public IEnumerable<AppInvites> GetAllApplicationInvitesForLead(int lead_id)
        {
            IEnumerable<AppInvites> appInvites = new List<AppInvites>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_AppInvites_Get_ByLead]";
                    object dynamicParams = new { lead_id = lead_id };

                    appInvites = conn.Query<AppInvites>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return appInvites;
        }
        public AppInvites GetApplicationInviteById(int API_Id)
        {
            AppInvites appInvite = new AppInvites();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_AppInvites_GetById]";
                    object dynamicParams = new { api_id = API_Id };
                    appInvite = conn.QuerySingleOrDefault<AppInvites>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return appInvite;
        }
        public bool InsertUpdateApplicationInvite(AppInvites appInvite)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (appInvite.api_id > 0)
                    {
                        string storedProc = "[dbo].[SP_AppPortal_AppInvites_Update]";
                        object dynamicParams = new
                        {
                            api_id = appInvite.api_id,
                            api_id_courselevel = appInvite.api_id_courselevel,
                            api_courseleveltitle = appInvite.api_courseleveltitle,
                            api_updatedbyuserid = appInvite.api_updatedbyuserid,
                            api_updatedbyusername = appInvite.api_updatedbyusername
                        };
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_AppPortal_AppInvites_Insert]";
                        object dynamicParams = new
                        {
                            api_id_courselevel = appInvite.api_id_courselevel,
                            api_courseleveltitle = appInvite.api_courseleveltitle,
                            api_email = appInvite.api_email,
                            api_firstname = appInvite.api_firstname,
                            api_lastname = appInvite.api_lastname,
                            api_password = appInvite.api_password,
                            api_emailbody = appInvite.api_emailbody,
                            api_createdbyuserid = appInvite.api_createdbyuserid,
                            api_createdbyusername = appInvite.api_createdbyusername,
                            lead_id=appInvite.lead_id
                        };
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
        public bool DeleteApplicationInviteById(int API_Id, int userId, string userName)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_AppPortal_AppInvites_DeleteById]";
                    object dynamicParams = new { api_id = API_Id, api_deletedbyuserid = userId, api_deletedbyusername = userName };
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public ConfigurationModel LoadConfigData()
        {
            ConfigurationModel appInvite = new ConfigurationModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string key = "api_source_url";
                    string storedProc = "[dbo].[SP_Load_Config_Data]";
                    object dynamicParams = new { key = key };
                    appInvite = conn.QuerySingleOrDefault<ConfigurationModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return appInvite;
        }
    }
}
