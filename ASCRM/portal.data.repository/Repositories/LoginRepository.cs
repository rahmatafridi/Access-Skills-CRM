using crm.portal.CrmModel;
using Dapper;
using portal.data.repository.Interfaces;
using portal.models.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;


namespace portal.data.repository.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        private readonly string _connString;
        private readonly IData_UOW _unitOfWork;
        public LoginRepository(IData_UOW unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _connString = unitOfWork.GetConnectionString();
        }
        public User ValidateUser(string email, string password, bool rememberMe)
        {
            User result = null;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string md5PW = EncodePassword(password);
                    //md5PW = "4B-49-8F-35-CA-7A-EB-79-DA-1B-80-E6-F3-6B-60-9D";
                    string storedProc = "[dbo].[SP_mdlead_Users_ValidateUser]";
                    object dynamicParams = new { Users_UserName = email, Users_Password = md5PW };
                    result = conn.QuerySingleOrDefault<User>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return result;
        }

        public IEnumerable<Role> GetRolesByUserName(string userName)
        {
            IEnumerable<Role> roles = null;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[aspnet_UsersInRoles_GetRolesForUser]";
                    object dynamicParams = new { ApplicationName = "/", UserName = userName };
                    roles = conn.Query<Role>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return roles;
        }
        public bool IsUserInRole(string userName, string roleName)
        {
            bool isExists = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "aspnet_UsersInRoles_IsUserInRole";
                    object dynamicParams = new { ApplicationName = "/", UserName = userName, RoleName = roleName };
                    isExists = conn.QueryFirstOrDefault<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    var list = conn.Query<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure).AsList();
                    isExists = (list.Count > 0) ? true : false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return isExists;
        }        

        public bool IsPasswordResetLinkValid(string GUID)
        {
            bool isExists = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_IsPasswordResetLinkValid]";
                    object dynamicParams = new { GUID = GUID };
                    isExists = conn.QueryFirstOrDefault<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return isExists;
        } 

        public bool ChangePassword(string GUID, string password)
        {
            bool isExists = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string md5PW = EncodePassword(password);
                    string storedProc = "[dbo].[SP_User_ChangePassword]";
                    object dynamicParams = new { GUID = GUID, Password = md5PW };
                    isExists = conn.QueryFirstOrDefault<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return isExists;
        }
        public Tuple<bool, string, string> GetRestPasswordLinkByEmail(string email)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {               
                try
                {
                    string storedProc = "[dbo].[SP_ResetPassword]";
                    object dynamicParams = new { UserName = email };
                    var dynamic = conn.QuerySingleOrDefault<dynamic>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    var fields = dynamic as IDictionary<string, object>;
                    return Tuple.Create<bool, string, string>(Convert.ToBoolean(fields["ReturnCode"]), fields["UniqueId"].ToString(), fields["Email"].ToString());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private static string EncodePassword(string originalPassword)
        {
            originalPassword = "stusPasswordSalt" + originalPassword;
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes);
        }
        public bool ValidateUser(string email, string password)
        {
            User result = null;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string md5PW = EncodePassword(password);
                    string storedProc = "[dbo].[SP_CRUD_mdlead_Users]";
                    object dynamicParams = new { Type = "ValidateUser", Users_UserName = email, Users_Password = md5PW };
                    result = conn.QuerySingleOrDefault<User>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw;
                }
            }
            return result == null ? false : true;
        }

        public string GetUserEmail(string code)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_GetUserEmailByCode]";
                    object dynamicParams = new { @Code = code};
                    return conn.QuerySingleOrDefault<string>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    throw;
                }
            }
        }
    }
}
