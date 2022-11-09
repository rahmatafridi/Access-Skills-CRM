using ds.portal.users.Models;
using System.Collections.Generic;

namespace ds.portal.users
{
    public interface ILead_UserRepository
    {
        IEnumerable<Users> GetAllUsers();
        bool InsertUpdateUserRecord(Users user);
        Users GetUserById(int id);
        Users GetUserByUserName(string id);
        bool CheckEmailAlreadyExists(int users_ID, string users_UserName);
        bool CheckEmailAlreadyExistsPortal(int users_ID, string users_UserName);

        IEnumerable<Users> GetAllSalesAdvisorUsers(bool _isAdmin);
        IEnumerable<Users> GetAllSalesAdvisor();
        IEnumerable<Users> GetPortalAllUsers();

        bool DeleteUser(int userId);
    }
}
