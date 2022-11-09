using Dapper;
using ds.core.common;
using ds.core.uow;
using ds.portal.users.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ds.portal.users
{
    public class Lead_UserRepository : ILead_UserRepository
    {
        private readonly string _connString;
        private readonly IUOW _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public Lead_UserRepository(IUOW unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _connString = unitOfWork.GetConnectionString();
        }

        public IEnumerable<Users> GetAllUsers()
        {
            IEnumerable<Users> optionsHeaders = new List<Users>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Users_GetAll]";

                    optionsHeaders = conn.Query<Users>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return optionsHeaders;
        }
        public bool InsertUpdateUserRecord(Users user)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //if (user.Users_ShowLearnersFrom != "")
                    //{
                    //    var date = Convert.ToDateTime(user.Users_ShowLearnersFrom);
                    //    DateTime dt = new DateTime(2015, 12, 31);
                    //    TimeSpan ts = new TimeSpan(25, 20, 55);
                    //    var newDate = date.Add(ts);
                    //    user.Users_ShowLearnersFrom = newDate.ToLongDateString();
                    //}
                    
                    if (user.Users_ID > 0)
                    {
                        //if(user.Users_IsCare != 0)
                        //{
                        //    string storedProc1 = "[dbo].[Sp_Portal_Update_EmployerUser]";
                        //    object dynamicParams1 = new { userId = user.Users_ID,employeeId= user.Users_IsCare };

                        //    conn.Query(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                        //}
                        //TODO: MR InsertUpdateUserRecord
                        //Get user from service not passed from call as it could be of been tampered by user
                        var orginalUser = GetUserById(user.Users_ID);
                        //Update password if it has changed. If the same set from db
                        //Encoding pass again changes the user password
                        user.Users_Password = (orginalUser.Users_Password == user.Users_Password) ? orginalUser.Users_Password : EncodePassword(user.Users_Password);
                        string storedProc = "[dbo].[SP_mdlead_Users_Update]";
                        object dynamicParams = user;
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        if(user.Users_Id_Care != 0)
                        {
                            user.Users_IsCare = 1;

                           
                        }
                        user.Users_Password = EncodePassword(user.Users_Password);
                        string storedProc = "[dbo].[SP_mdlead_Users_Insert]";
                        object dynamicParams = user;
                      var id= conn.QueryFirstOrDefault<int>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                        if(user.Users_Id_Care != 0)
                        {
                            string storedProc1 = "[dbo].[Sp_Portal_Create_EmployerUser]";
                            object dynamicParams1 = new { userId = id, employerId = user.Users_Id_Care };

                            conn.Query(storedProc1, param: dynamicParams1, commandType: CommandType.StoredProcedure);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return true;
        }

        public Users GetUserById(int id)
        {
            Users user = new Users();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Users_GetById]";
                    object dynamicParams = new { Users_Id = id };

                    user = conn.QuerySingleOrDefault<Users>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return user;
        }

        public bool CheckEmailAlreadyExists(int users_ID, string users_UserName)
        {
            bool retVal = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Users_CheckEmailAlreadyExists]";
                    object dynamicParams = new { Users_Id = users_ID, Users_Username = users_UserName };

                    retVal = conn.QuerySingleOrDefault<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retVal;
        }
        public bool CheckEmailAlreadyExistsPortal(int users_ID, string users_UserName)
        {
            bool retVal = false;
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Users_CheckEmailAlreadyExists]";
                    object dynamicParams = new { Users_Id = users_ID, Users_Username = users_UserName };

                    retVal = conn.QuerySingleOrDefault<bool>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retVal;
        }

        public IEnumerable<Users> GetAllSalesAdvisorUsers(bool _isAdmin)
        {
            IEnumerable<Users> optionsHeaders = new List<Users>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdLead_SalesAdvisors_GetAll]";
                    object dynamicParams = new { @is_admin = _isAdmin};
                    optionsHeaders = conn.Query<Users>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return optionsHeaders;
        }

        public bool DeleteUser(int userId)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_User_Delete]";
                    object dynamicParams = new { @user_id = userId };

                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return true;
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

        public IEnumerable<Users> GetAllSalesAdvisor()
        {
            IEnumerable<Users> optionsHeaders = new List<Users>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_SalesAdvisors_GetAll_Search]";
                    optionsHeaders = conn.Query<Users>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return optionsHeaders;
        }

        public Users GetUserByUserName(string id)
        {
            Users user = new Users();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Get_User_BY_UserName]";
                    object dynamicParams = new { userName = id };

                    user = conn.QuerySingleOrDefault<Users>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return user;
        }

        public IEnumerable<Users> GetPortalAllUsers()
        {
            IEnumerable<Users> optionsHeaders = new List<Users>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Portal_Users_GetAll]";

                    optionsHeaders = conn.Query<Users>(storedProc, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return optionsHeaders;
        }
    }
}
