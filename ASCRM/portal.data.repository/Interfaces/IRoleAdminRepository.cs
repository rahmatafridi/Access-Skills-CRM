using portal.models.RoleAdmin;
using System.Collections.Generic;

namespace portal.data.repository.Interfaces
{
    public interface IRoleAdminRepository
    {
        IEnumerable<Role> GetAllRoles();
        IEnumerable<Role> GetAllRolesPortal();

        bool InsertUpdateRole(Role role);
        Role GetRoleById(int id);
        Role GetSecurityRoleByUserId(int security_user_id);
        RoleFPModel GetListFeaturesForRole(int roleId, int security_user_id);
        Permissions FetchRolePermissionsByRoleId(int roleId);
        IEnumerable<UsersInRoleModel> GetListUsersInRoleForClient(int security_user_id);
        bool SavePermissions(int roleId, string permsList, int security_user_id);
        bool DeletePermissions(int roleId, string permsList);
        bool InsertPermissions(int roleId, string permsList);
        bool CheckMandatoryRolePermissionByFeatureId(int featureId, int security_user_id);
        bool CheckMandatoryRolePermission(string permission, int security_user_id);
        bool DeleteRole(int roleId);

        bool IsOwnerLead(int user_id, int lead_id); 

    }
}
