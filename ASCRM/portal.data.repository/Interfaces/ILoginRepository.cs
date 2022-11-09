using crm.portal.CrmModel;
using portal.models.Account;
using System;
using System.Collections.Generic;

namespace portal.data.repository.Interfaces
{
    public interface ILoginRepository
    {
        User ValidateUser(string email, string password, bool rememberMe);
        

        IEnumerable<Role> GetRolesByUserName(string userName);
        bool IsUserInRole(string userName, string roleName);
        bool IsPasswordResetLinkValid(string GUID);
        bool ChangePassword(string GUID, string password);
        Tuple<bool, string, string> GetRestPasswordLinkByEmail(string email);
        bool ValidateUser(string email, string password);
        string GetUserEmail(string code);

    }
}
