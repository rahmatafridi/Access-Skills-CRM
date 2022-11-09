using portal.data.repository.Interfaces;
using portal.models.RoleAdmin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Linq;

namespace portal.data.repository.Repositories
{
    public class RoleAdminRepository : IRoleAdminRepository
    {
        private readonly string _connString;

        private readonly IData_UOW _unitOfWork;
        public RoleAdminRepository(IData_UOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }
        public IEnumerable<Role> GetAllRoles()
        {
            IEnumerable<Role> roles = new List<Role>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_GetRoles]";
                    roles = conn.Query<Role>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return roles;
        }
        public IEnumerable<Role> GetAllRolesPortal()
        {
            IEnumerable<Role> roles = new List<Role>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_GetRoles_Portal]";
                    roles = conn.Query<Role>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return roles;
        }
        public bool InsertUpdateRole(Role role)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (role.security_role_id > 0)
                    {
                        string storedProc = "[dbo].[UP_Web_UserRole_Update]";
                        object dynamicParams = new { Name = role.name, Description = role.description, Is_active = true, security_role_id = role.security_role_id };

                        conn.Query<Role>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    }
                    else
                    {
                        string storedProc = "[dbo].[UP_Web_UserRole_Insert]";
                        object dynamicParams = new { Name = role.name, Description = role.description, Is_active = true };

                        conn.Query<Role>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public Role GetRoleById(int id)
        {
            Role role = new Role();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_GetRoleById]";
                    object dynamicParams = new { role_id = id };

                    role = conn.QuerySingleOrDefault<Role>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return role;
        }
        public Role GetSecurityRoleByUserId(int security_user_id)
        {
            Role role = null;

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_SelectByUserId]";
                    object dynamicParams = new { security_user_id = security_user_id };

                    role = conn.QueryFirstOrDefault<Role>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return role;
        }
        public RoleFPModel GetListFeaturesForRole(int roleId, int security_user_id)
        {
            RoleFPModel fpModel = new RoleFPModel();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_GetListFeaturesPermissionsForRole]";
                    object dynamicParams = new { role_id = roleId, security_user_id = security_user_id };
                    var data = conn.QueryMultiple(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    var item1 = data.Read<RolePermissionModel>();
                    var item2 = data.Read<RoleFeatureModel>();

                    var features_permissions = item2.Select(x => new RoleFeatureModel
                    {
                        Feature_Id = x.Feature_Id,

                        Feature_Name = x.Feature_Name,
                        Permissions = item1.Where(y => y.Parent_Feature_Id == x.Feature_Id)
                        .Select(
                        z => new RolePermissionModel { Permission_Id = z.Permission_Id, Permission_Desc = z.Permission_Desc, is_checked = z.is_checked }

                     ).ToList()
                    });

                    fpModel.features = features_permissions;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return fpModel;
        }

        public Permissions FetchRolePermissionsByRoleId(int roleId)
        {
            Dictionary<string, bool> values = new Dictionary<string, bool>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_Security_FetchRolePermissions]";
                    object dynamicParams = new { role_id = roleId };
                    var permissions = conn.Query<string>(storedProc, dynamicParams, commandType: CommandType.StoredProcedure).ToDictionary(i => i, i => true);

                    return new Permissions(permissions)
                    {
                        ID = roleId
                    };

                }
                catch (Exception)
                {
                    throw;
                }
            }
                return new Permissions(values);
        }

        public IEnumerable<UsersInRoleModel> GetListUsersInRoleForClient(int security_user_id)
        {
            IEnumerable<UsersInRoleModel> result = new List<UsersInRoleModel>(100);

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_ListUsersInRoleForClient]";
                    object dynamicParams = new { security_user_id = security_user_id };
                    var data = conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    result = data.Select(x => new UsersInRoleModel
                    {
                        RoleId = x.security_role_id,
                        RoleName = x.role_name,
                        UserId = x.security_user_id,
                        UserName = x.username,
                    });
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return result;
        }
       
        public bool SavePermissions(int roleId, string permsList, int security_user_id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_UpdatePermissionsForClientRole]";
                    object dynamicParams = new { @role_id = roleId, @perm_list = permsList, @security_user_id = security_user_id };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }
        public bool DeletePermissions(int roleId, string permsList)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_DeletePermissionsForClientRole]";
                    object dynamicParams = new { @role_id = roleId, @perm_list = permsList };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool InsertPermissions(int roleId, string permsList)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_InsertPermissionsForClientRole]";
                    object dynamicParams = new { @role_id = roleId, @perm_list = permsList };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool CheckMandatoryRolePermissionByFeatureId(int featureId, int security_user_id)
        {
            bool restrictPermissionUncheck = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_MandatoryPermissions_CheckByFeatureId]";
                    object dynamicParams = new { @feature_id = featureId, @security_user_id = security_user_id };

                    restrictPermissionUncheck = conn.ExecuteScalar<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return restrictPermissionUncheck;
        }
        public bool CheckMandatoryRolePermission(string permission, int security_user_id)
        {
            bool restrictPermissionUncheck = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_MandatoryPermissions_Check]";
                    object dynamicParams = new { @permission_id = permission, @security_user_id = security_user_id };

                    restrictPermissionUncheck = conn.QuerySingleOrDefault<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return restrictPermissionUncheck;
        }

        public bool DeleteRole(int roleId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_Role_Delete]";
                    object dynamicParams = new
                    {
                        @role_id = roleId,
                    };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }


        #region "owner of lead"        
        public bool IsOwnerLead(int user_id, int lead_id)
        {
            bool is_owner = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_IsLeadOwner]";
                    object dynamicParams = new { @user_id = user_id, @lead_id = lead_id };

                    is_owner = conn.ExecuteScalar<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex )
                {
                    var s = ex.Message.ToString();
                    throw;
                }
            }
            return is_owner;
        }


        #endregion



    }
}
