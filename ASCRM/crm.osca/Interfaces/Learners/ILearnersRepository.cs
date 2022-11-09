using crm.osca.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.osca.Interfaces.Learners
{
   public interface ILearnersRepository
    {
        List<Learner> LoadLearners();
        List<LearnerComman> LoadTitle();
        List<LearnerComman> LoadGender();
        List<LearnerComman> LoadGroup();
        List<LearnerComman> LoadMarital();
        List<LearnerComman> LoadStatus();
        List<LearnerComman> LoadImmigrationStatus();
        List<LearnerComman> LoadRegion();
        List<LearnerComman> LoadCountry();
        List<LearnerComman> LoadFundingType();
        List<LearnerComman> LoadProjectType();
        List<LearnerComman> LoadAwardingBody();
        List<LearnerComman> LoadQualCourses();
        List<LearnerComman> LoadCourseStatus1();
        List<LearnerComman> LoadLearnerOutcome();
        List<LearnerComman> LoadRisking();
        List<LearnerComman> LoadFinanceStaus();
        List<LearnerComman> LoadCourseStatus();
        List<LearnerUser> LoadSkillsAdvisors();
        List<LearnerUser> LoadObservationAssessors();
        List<LearnerComman> LoadAccountStatus();
        List<LearnerComman> LoadPaymentStaus();
        List<LearnerComman> LoadPriorAttainment();
        List<LearnerComman> LoadLearnerHHS();
        List<LearnerComman> LoadEthnicity();
        List<LearnerComman> LoadNationality();
        List<LearnerComman> LoadLearnerLLDD();
        List<LearnerComman> LoadEmployerPaid();
        List<LearnerComman> LoadReviewCourse(int id);
        List<DocUploadTopic> LoadDocUploadTopic(int id);
        List<SinupDoc> LoadSingupDocs(int id);
        List<IVEVDoc> LoadIVEVDOC(int id);
        List<LearnerUploadDoc> LoadProfileActivty(int id);
        List<LearnerComman> LoadPrimaryContact(int id);
        List<CompleteActivites> LoadCompleteActivites(int id);
        List<AdditionalActivites> LoadAdditionalActivites(int id);
        Employer LoadEmployer(int id);
        List<TabHistory> LoadTabHistory(int id);
        List<LearnerReview> LoadReviewList(int id,int courseId);
        List<LearnerEmployer> SearchEmployer(string name);
        List<NoteCatgory> LoadNoteCatgory();
        bool AddNote(LearnerNote learnerNote);
        bool AddAdditionalActivites(AdditionalActivites model);
        List<LearnerNote> NoteList(int leanerId, string note, int cateId);
        List<MatrixCourse> LoadCourses(int learnerId);
        LearnerData LoadLearnerDatail(int id);
        PrimeCPD LoadPrimeCPD(int id);
        DocLearnerCourseId LoadDocLearnerCourseId(int learnerId);
        List<CourseDetail> LoadCourseDatail(int id);
        CombineData LoadCombineData(int id);
        LeanerFinanceLastDate LoadLastDate(int id);
        bool UpdateEmployer(int learnerId, int employerId);
        bool UpdateAssessment(int learnerId, int employerId);
        LeanerStatusChangeDate LoadStatusChangeDate(int id);
        LearnerData UpdateLearnerData(LearnerData learnerData);
        bool UpdateCourse(UpdateCourse updateCourse);
        bool InsertCourse(UpdateCourse updateCourse);
        List<UpdateCourse> CheckUpdateCouser(int learnerId, int courseId);
        List<OscaHeaderOption> LoadOscaHeader(string header);
        Doc_CourseSummary GenerateCourseSummary(int learnerId);
    }
}
