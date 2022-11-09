using ds.core.configuration.Models;
using ds.portal.applications.Models;
using ds.portal.leads.Models;
using ds.portal.leads.Models.Shared;
using System;
using System.Collections.Generic;

namespace ds.portal.applications
{
    public interface IApplicationRepository
    {
        IEnumerable<ApplicationModel> GetAllApplicationsForAdminByType(string type);
        IEnumerable<ApplicationSteps> GetAdminQuestion(int course_level_id,int app_id);
        IEnumerable<ApplicationSteps> GetListStepsForLearner(int course_level_id, int app_id);

        IEnumerable<ApplicationQuestions> GetApplicationByIdForLearner(int id);
        bool InsertUpdateApplication(List<SaveAnswer> application);
        bool InsertUpdateHeader(string text);
        bool CheckHeader(string text);
        bool SubmittedCheck(int id);
        bool InsertUpdateEnrollmentApplication(List<SaveAnswer> application);

        bool InsertUpdateCourseLevel(CourseLevels course);
        bool DeleteCourseLevel(int id);
        int InsertNewApplication(int user_id, string user_name, int course_level_id);
        IEnumerable<ApplicationModel> GetAllApplicationsByUserId(int userId);
        IEnumerable<ApplicationQuestions> GetApplicationAttachment(int id);

        IEnumerable <ApplicationQuestions> GetApplicationById(int app_Id);

        CourseLevels GetCourseLevelById(int app_Id);

        ApplicationQuestions GetApplicationNoticeQuestion(int app_Id);
        ApplicationSignature LoadSignatureData(int app_Id);

        ConfigurationModel GetConfigs();
        int GetLearnerAdvisorId(string sUsername);
        int GetLearnerCourseLevelId(string sUsername);
        bool ReadyToEnrollApplicationWithReason(ReasonModel reasonModel);
        bool RejectApplicationWithReason(ReasonModel reasonModel);
        bool DeleteApplicationWithReason(ReasonModel reasonModel);
        bool UpdateOfficeUseOnly(App_OfficeUse app_OfficeUse);
        bool UpdateOfficeUse1(App_OfficeUse1 app_OfficeUse1);
        string GetSignatureByAppId(int app_Id);
        bool SaveChangesToApplicationsTrackChanges(List<ApplicationsTrackChanges> ApplicationsTrackChangesList, int app_id, int user_id);
        IEnumerable<ApplicationsTrackChanges> GetApplicationsTrackChanges(int app_id);
        bool UpdateApplicationStatusAfterNotifiedChanges(int app_id, int user_id);
        int GetCourseLevelIdByApplicationId(int app_id);
        int GetCourseLevelIdByUserId(int user_id);
        bool SubmitApplication(int app_id, string user_name);
        IEnumerable<DropdownOptionsHeader> GetApplicationDropdownOptionsHeaders();
        IEnumerable<DropdownOption> GetApplicationDropdownOptionsByHeaderId(int headerId);
        Tuple<bool, bool> CheckTitleAndValueExists(DropdownOption dropdownOption);
        bool InsertUpdateOptionRecord(DropdownOption dropdownOption);
        bool DeleteDropdownOptionByOptionId(int id, int userId, string userName);
        IEnumerable<Course> GetAllCourses();
        IEnumerable<CourseLevels> LoadCourseLevel();
        IEnumerable<QuestionType> LoadQuestionType();
        IEnumerable<QuestionModel> GetQuestionAnswerById(int app_id);
        IEnumerable<QuestionModel> GetEnrolledQuestions(int app_id);

        IEnumerable<QuestionModel> GetDependentAnswerById(int app_id);
        IEnumerable<QuestionModel> GetConfirmQuestionsAnswer(int app_id);

        IEnumerable<QuestionModel> GetDependentAnswerForAdminById(int app_id);
        ApplicationUser GetCourseLevelForPortal(int id);
    }
}
