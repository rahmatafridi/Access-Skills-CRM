using crm.portal.CrmModel;
using crm.portal.OscaModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.portal.Interfaces.User
{
  public   interface IPortalUserRepository
    {
        int GetUserLearnerId(string sUsername);
        string GetLearnerNameSurname(string iLearner);
        IEnumerable<PortalUserModel> GetListUsersForPortal(int security_user_id);
        bool CheckEmailAlreadyExists(int users_ID, string users_UserName);
        Portal_User GetUserById(int id);
        List<Portal_User> GetProviderAwadingBodyUsers(int id,string type);
        List<Portal_User> GetAccountUsers();
        bool UpdateAssignLearner(int userid, int learnerid);
        bool UpdateAssignEmployer(int userid, int learnerid);
        bool HideLearner(int userid, int learnerid);
        bool UnHideLearner(int userid, int learnerid);

        IEnumerable<AwardingBodyLearner> GetAwardingBodyLearners();
        IEnumerable<TrainingProviders> GetTrainingProviders();
        IEnumerable<Learner> LoadAssignLearner(int id,DateTime date);
        IEnumerable<Learner> LoadAssignLearnerByAwardingBody(int id, DateTime date,int awardingId);
        IEnumerable<Learner> LoadAssignLearnerByAwardingBodySearch(string name,int id, DateTime date,int awardingId);
        IEnumerable<Learner> LoadAssignLearnerByTrainingProvider(int id, DateTime date, int providerid);
        IEnumerable<Learner> LoadAssignLearnerByTrainingProviderSearch(string name,int id, DateTime date, int providerid);
        IEnumerable<Learner> LoadAssignLearnerByTrainingProviderAccounts(int id, DateTime date, int providerid);
        IEnumerable<Learner> LoadAssignLearnerForEmployer(string employers,string learners);
        IEnumerable<Learner> LoadAssignLearnerByTrainingProviderAccountsSearch(string name,int id, DateTime date, int providerid);
        IEnumerable<LinkLearnerABTPUser> CheckAssignLearner(int userid,int learnerid);
        ///IEnumerable<Employer> CheckAssignEmployer(int userid, int learnerid);

        IEnumerable<Employer> LoadEmployerLearnerById(string id);
        IEnumerable<Learner> LoadEmployerLearners(string employes,string learners);
        IEnumerable<Employer> LoadEmployerByUser(int id);
        IEnumerable<EmployerLearner> LoadEmployerLearnerHidenByUser(int id);
        IEnumerable<Employer> LoadEmployerUsers();

    }
}
