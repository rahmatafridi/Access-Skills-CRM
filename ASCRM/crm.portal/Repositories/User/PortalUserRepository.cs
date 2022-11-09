using crm.portal.CrmModel;
using crm.portal.Interfaces.User;
using crm.portal.OscaModel;
using Dapper;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace crm.portal.Repositories.User
{
    public class PortalUserRepository : IPortalUserRepository
    {
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;

        public PortalUserRepository(IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();

        }

        public string GetLearnerNameSurname(string username)
        {
           // var UserLearnerId = GetUserLearnerId(username);


            var data = new Learner();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_LearnerName]";
                    object dynamicParams = new
                    {
                        sUsername = username
                    };

                    data = conn.QueryFirstOrDefault<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return data.LearnerName;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }
        public Portal_User GetUserById(int id)
        {
            Portal_User user = new Portal_User();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_mdlead_Users_GetById]";
                    object dynamicParams = new { Users_Id = id };

                    user = conn.QuerySingleOrDefault<Portal_User>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
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
        //public bool InsertUpdateUserRecord(Portal_User user)
        //{
        //    using (SqlConnection conn = new SqlConnection(_connString))
        //    {
        //        try
        //        {
        //            if (user.Users_Id > 0)
        //            {
        //                //TODO: MR InsertUpdateUserRecord
        //                //Get user from service not passed from call as it could be of been tampered by user
        //                var orginalUser = GetUserById(user.Users_Id);
        //                //Update password if it has changed. If the same set from db
        //                //Encoding pass again changes the user password
        //                user = (orginalUser.Users_Username == user.Users_Username) ? orginalUser.Users_Password : EncodePassword(user.Users_Password);
        //                string storedProc = "[dbo].[SP_mdlead_Users_Update]";
        //                object dynamicParams = user;
        //                conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
        //            }
        //            else
        //            {
        //                user.Users_Password = EncodePassword(user.Users_Password);
        //                string storedProc = "[dbo].[SP_mdlead_Users_Insert]";
        //                object dynamicParams = user;
        //                conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
        //    return true;
        //}

        private string EncodePassword(string users_Password)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PortalUserModel> GetListUsersForPortal(int security_user_id)
        {
            IEnumerable<PortalUserModel> result = new List<PortalUserModel>(100);

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[UP_Web_UserRole_ListUsersPortal]";
                    object dynamicParams = new { security_user_id = security_user_id };
                    result = conn.Query<PortalUserModel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);

                    
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public List<MatrixLabel> GetMatrixLabels(int LearnerCourseId)
        {
            var data = new List<MatrixLabel>();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_SS_GetListLearnerAssignedMatrixes]";
                    object dynamicParams = new
                    {
                        LearnerCourseId = LearnerCourseId
                    };

                    data = (List<MatrixLabel>)conn.Query<MatrixLabel>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public int GetUserLearnerId(string sUsername)
        {
            var data = new Learner();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_USER_LearnerContacts]";
                    object dynamicParams = new
                    {
                        sUsername = sUsername
                    };

                    data = conn.QueryFirstOrDefault<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    return data.Users_Id_LearnerContacts;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<AwardingBodyLearner> GetAwardingBodyLearners()
        {
            IEnumerable<AwardingBodyLearner> result = new List<AwardingBodyLearner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_AwardingBody]";
                    object dynamicParams = new { };
                    result = conn.Query<AwardingBodyLearner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public IEnumerable<TrainingProviders> GetTrainingProviders()
        {
            IEnumerable<TrainingProviders> result = new List<TrainingProviders>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    object dynamicParams = new { };
                    result = conn.Query<TrainingProviders>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public IEnumerable<Learner> LoadAssignLearner(int id,DateTime dateTime)
        {
            IEnumerable<Learner> result = new List<Learner>();
            return result;
        }

        public IEnumerable<Learner> LoadAssignLearnerByAwardingBody(int id, DateTime date, int awardingId)
        {
            IEnumerable<Learner> result = new List<Learner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_GET_PORTAL_TrainingProvider_AwrdingBody]";

                    object dynamicParams = new {userid=id, dtStartDate = date,awardingId=awardingId };
                    result = conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }
        public IEnumerable<Learner> LoadAssignLearnerByAwardingBodySearch(string search,int id, DateTime date, int awardingId)
        {
            IEnumerable<Learner> result = new List<Learner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_GET_PORTAL_TrainingProvider_AwrdingBodySearch]";

                    object dynamicParams = new {name=search, userid=id, dtStartDate = date,awardingId=awardingId };
                    result = conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public IEnumerable<Learner> LoadAssignLearnerByTrainingProviderAccounts(int id, DateTime date, int providerid)
        {

            IEnumerable<Learner> result = new List<Learner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_GET_PORTAL_TrainingProvider_ForAccount]";

                    object dynamicParams = new {userid=id, dtStartDate = date, providerId = providerid };
                    result = conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }
        public IEnumerable<Learner> LoadAssignLearnerByTrainingProvider(int id, DateTime date, int providerid)
        {

            IEnumerable<Learner> result = new List<Learner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_GET_PORTAL_TrainingProvider_BYProvider]";

                    object dynamicParams = new {userid=id, dtStartDate = date, providerId = providerid };
                    result = conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public List<Portal_User> GetProviderAwadingBodyUsers(int id,string type)
        {
           List<Portal_User> user = new List<Portal_User>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    if (type == "Provider")
                    {
                        string storedProc = "[dbo].[SP_PORATL_GET_PROVIDER_USER]";
                        object dynamicParams = new { providerId = id };

                        user = (List<Portal_User>)conn.Query<Portal_User>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    if (type == "AwardBody")
                    {
                        string storedProc = "[dbo].[SP_PORATL_GET_AWARDBODY_USER]";
                        object dynamicParams = new { awardingId = id };

                        user = (List<Portal_User>)conn.Query<Portal_User>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return user;
        }

        public IEnumerable<LinkLearnerABTPUser> CheckAssignLearner(int userid, int learnerid)
        {
            IEnumerable<LinkLearnerABTPUser> result = new List<LinkLearnerABTPUser>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_GET_PORTAL_Link_Learner_User]";

                    object dynamicParams = new { userid = userid, learnerid = learnerid };
                    result = conn.Query<LinkLearnerABTPUser>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public bool UpdateAssignLearner(int userid, int learnerid)
        {
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_PORTAL_Insert_Link_Learner_User]";

                    object dynamicParams = new { userid = userid, learnerid = learnerid };
                     //= conn.Query<LinkLearnerABTPUser>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
                return true;
            }
        }

        public IEnumerable<Learner> LoadAssignLearnerByTrainingProviderAccountsSearch(string name, int id, DateTime date, int providerid)
        {
            IEnumerable<Learner> result = new List<Learner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_GET_PORTAL_TrainingProvider_ForAccountSearch]";

                    object dynamicParams = new {name=name, userid = id, dtStartDate = date, providerId = providerid };
                    result = conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public IEnumerable<Learner> LoadAssignLearnerByTrainingProviderSearch(string name, int id, DateTime date, int providerid)
        {
            IEnumerable<Learner> result = new List<Learner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_GET_PORTAL_TrainingProvider_BYProviderSearch]";

                    object dynamicParams = new {name=name, userid = id, dtStartDate = date, providerId = providerid };
                    result = conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return result;
        }

        public IEnumerable<Employer> LoadEmployerLearnerById(string id)
        {
            IEnumerable<Employer> result = new List<Employer>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_Portal_ListAllCareOnSearch]";

                    object dynamicParams = new { LId = "", LCareHome = "", LPostCode = "", LContactName = "",
                        LStatus=1,
                        LGroupId=1,
                        EmployerIds=id,
                        LContactNumber ="", LContactEmail="" };
                    result = conn.Query<Employer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return result;
        }

        public List<Portal_User> GetAccountUsers()
        {
            List<Portal_User> user = new List<Portal_User>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    string storedProc = "[dbo].[SP_Portal_GetEmployerUsersByFilter]";
                    object dynamicParams = new { };

                    user = (List<Portal_User>)conn.Query<Portal_User>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return user;
        }

        public IEnumerable<Employer> LoadEmployerByUser(int id)
        {
            IEnumerable<Employer> result = new List<Employer>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_Portal_Get_EmpoyerByUser]";

                    object dynamicParams = new
                    {
                       userId= id
                    };
                    result = conn.Query<Employer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return result;
        }

        public IEnumerable<Employer> LoadEmployerUsers()
        {
            IEnumerable<Employer> result = new List<Employer>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_Portal_Get_EmpoyerUsers]";

                    object dynamicParams = new
                    {
                    };
                    result = conn.Query<Employer>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return result;
        }

        public bool UpdateAssignEmployer(int userid, int employerid)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_PORTAL_Insert_Link_Employer_User]";

                    object dynamicParams = new { userId = userid, employerId = employerid };
                    //= conn.Query<LinkLearnerABTPUser>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
                return true;
            }
        }

        public IEnumerable<EmployerLearner> LoadEmployerLearnerHidenByUser(int id)
        {
            IEnumerable<EmployerLearner> result = new List<EmployerLearner>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_Portal_Get_EmpoyerLearnerByUser]";

                    object dynamicParams = new
                    {
                        userId=id
                    };
                    result = conn.Query<EmployerLearner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return result;
        }

        public IEnumerable<Learner> LoadEmployerLearners(string employes, string learners)
        {
            IEnumerable<Learner> result = new List<Learner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_Portal_LoadUserLearner]";
                    if(learners == "")
                    {
                        learners = "0";
                    }
                   
                    object dynamicParams = new
                    {
                        
                        EmployerIds = employes.ToString(),
                        LearnerIds = learners
                    };
                    result = conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return result;
        }

        public IEnumerable<Learner> LoadAssignLearnerForEmployer(string employers, string learners)
        {
            IEnumerable<Learner> result = new List<Learner>();

            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_Portal_Get_EmployerLearner]";
                    if (learners == "")
                    {
                        learners = "0";
                    }
                    //object dynamicParams = new
                    //{
                    //    @EmployerIds = employes,
                    //    @LearnerIds =learners
                    //};
                    object dynamicParams = new
                    {

                        @Employers = employers.ToString(),
                        @Learners = learners
                    };
                    result = conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    // result = conn.Query<Learner>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return result;
        }

        public bool HideLearner(int userid, int learnerid)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_PORTAL_Insert_Hidden_Learner]";

                    object dynamicParams = new { userid = userid, learnerid = learnerid };
                    //= conn.Query<LinkLearnerABTPUser>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
                return true;
            }
        }

        public bool UnHideLearner(int userid, int learnerid)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    //string storedProc = "[dbo].[SP_PORTAL_GET_TraningProvider]";
                    string storedProc = "[dbo].[SP_PORTAL_Delete_HiddenLearner]";

                    object dynamicParams = new { userid = userid, learnerid = learnerid };
                    //= conn.Query<LinkLearnerABTPUser>(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw;
                }
                return true;
            }
        }
    }
}
