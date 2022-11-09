using ds.portal.companies.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace crm.osca.Models
{
    public class Learner
    {
        public string mDate { get; set; }
        public string Genders_Title { get; set; }
        public int ID { get; set; }
        private string _encodedId { get; set; }
        public string encodedId
        {
            get
            {
                return Encryption.Encrypt(this.ID.ToString());
            }
            set
            {
                _encodedId = value;
            }
        }
        public string LearnerRef { get; set; }
        public string Learner_FirstName { get; set; }
        public string Learner_Surname { get; set; }
        public string LearnerName { get; set; }
        public string CandidateStatus { get; set; }
        public string Learner_Class { get; set; }
        public string CandidateStatus_Title { get; set; }
        public string CandidateStatus_Image3 { get; set; }
        public string CandidateStatus_BackColor { get; set; }
        public int CandidateStatus_Id { get; set; }
        public string Level { get; set; }
        public string Course { get; set; }

    }
    public class CourseDetail
    {
        public int LearnerCourses_id { get; set; }
        public string StatusDate { get; set; }
        public string Course_Title { get; set; }
        public string Course_SchemeCode { get; set; }
        public string CoursesStatus_Title { get; set; }
        public string TotalActiveDays { get; set; }
        public string LearnerCourses_IsFunded { get; set; }
    }
    public class LearnerData
    {
        public int Learner_Id { get; set; }

        public string Learner_Ref { get; set; }

        public int Learner_Id_Titles { get; set; }

        public string Learner_FirstName { get; set; }

        public string Learner_Middlename { get; set; }

        public string Learner_Surname { get; set; }

        public string Learner_Gender { get; set; }

        public DateTime Learner_DOB { get; set; }

        public byte Learner_IsActive { get; set; }

        public string Learner_Address1 { get; set; }

        public string Learner_Address2 { get; set; }

        public string Learner_Address3 { get; set; }

        public string Learner_Address4 { get; set; }

        public string Learner_Address5 { get; set; }

        public string Learner_PostCode1 { get; set; }

        public string Learner_PostCode2 { get; set; }

        public string Learner_Telephone { get; set; }

        public string Learner_Telephone2 { get; set; }

        public string Learner_Mobile1 { get; set; }

        public string Learner_Mobile2 { get; set; }

        public string Learner_TelephoneWork1 { get; set; }

        public string Learner_TelephoneWork2 { get; set; }

        public string Learner_Email1 { get; set; }

        public string Learner_Email2 { get; set; }

        public string Learner_Website { get; set; }

        public byte Learner_IsOverseas { get; set; }

        public DateTime? Learner_OCRDate { get; set; }

        public int? Learner_OCRNumber { get; set; }

        public byte? Learner_IsOCRRegistered { get; set; }

        public DateTime? Learner_ReceivedDate { get; set; }

        public DateTime? Learner_BookedDate { get; set; }

        public byte[] Learner_Picture { get; set; }

        public byte? Learner_IsFunded { get; set; }

        public byte? Learner_IsClaimed { get; set; }

        public string Learner_Occupation { get; set; }

        public int? Learner_Id_MaritalStatus { get; set; }

        public int? Learner_Id_Assessor1 { get; set; }

        public int? Learner_Id_Assessor2 { get; set; }

        public int? Learner_Id_Assessor3 { get; set; }

        public int? Learner_Id_Status { get; set; }

        public int? Learner_Id_DealingPerson { get; set; }

        public int? Learner_Id_Notes { get; set; }

        public int? Learner_Id_Ethnicity { get; set; }

        public int? Learner_Id_PriorityStatus { get; set; }

        public int? Learner_Id_Country { get; set; }

        public int? Learner_Id_Group { get; set; }

        public byte? Learner_NVQLevel { get; set; }

        public string Learner_AppNumber { get; set; }

        public int? Learner_Id_CandidateStatus { get; set; }

        public int? Learner_Id_TrainingProvider { get; set; }

        public int? Learner_Id_AccountStatus { get; set; }

        public byte? Learner_IsAgreementRcvd { get; set; }

        public DateTime? Learner_AgreementDate { get; set; }

        public byte? Learner_IsApplicationRcvd { get; set; }

        public DateTime? Learner_ApplicationRcvdDate { get; set; }

        public DateTime? Learner_ArrivalDate { get; set; }

        public DateTime? Learner_VisaStartDate { get; set; }

        public DateTime? Learner_VisaRenewDate { get; set; }

        public string Learner_VisaNumber { get; set; }

        public DateTime? Learner_AssessorNotifiedDate { get; set; }

        public DateTime? Learner_MandateDate { get; set; }

        public byte Learner_IsDeleted { get; set; }

        public DateTime? Learner_DeletedDate { get; set; }

        public int? Learner_DeletedByUser { get; set; }

        public DateTime? Learner_CreatedDate { get; set; }

        public int? Learner_CreatedByUser { get; set; }

        public DateTime? Learner_UpdatedDate { get; set; }

        public int? Learner_UpdatedByUser { get; set; }

        public int? Learner_Id_Regions { get; set; }

        public string Learner_CareAcademy_Login { get; set; }

        public string Learner_CareAcademy_Password { get; set; }

        public DateTime? Learner_CareAcademy_RequestDate { get; set; }

        public DateTime? Learner_CareAcademy_SignupDate { get; set; }

        public byte Learner_IsPBS { get; set; }

        public string Learner_IddRefNo { get; set; }

        public string Learner_IddABFNo { get; set; }

        public byte? Learner_IsIdCardSent { get; set; }

        public DateTime? Learner_IdCardSentDate { get; set; }

        public string Learner_Photo { get; set; }

        public byte? Learner_IsInProcess { get; set; }

        public string Learner_Class { get; set; }

        public byte? Learner_IsWelcomed { get; set; }

        public string Learner_Email1x { get; set; }

        public DateTime? Learner_2ndVisaStartDate { get; set; }

        public DateTime? Learner_2ndVisaRenewDate { get; set; }

        public string Learner_2ndVisaNumber { get; set; }

        public byte? Learner_IsVisaCopy { get; set; }

        public byte? Learner_IsPassportCopy { get; set; }

        public string Learner_CASNumber { get; set; }

        public byte Learner_IsUKResident { get; set; }

        public int? Learner_Id_CandidateGroup { get; set; }

        public byte? Learner_IsVisa2Copy { get; set; }

        public DateTime? Learner_DatePassportExpiry { get; set; }

        public DateTime? Learner_DateVisaExtSubm { get; set; }

        public DateTime? Learner_DateVisaRefusal { get; set; }

        public DateTime? Learner_DateVisaForgate { get; set; }

        public int? Learner_Id_VisaStatus { get; set; }

        public int? Learner_Id_VisaApplicationRoutes { get; set; }

        public int? Learner_Id_VisaExtensionRoutes { get; set; }

        public int? Learner_Id_VisaPermittedHours { get; set; }

        public DateTime? Learner_UpdatedDateImmigration { get; set; }

        public int? Learner_UpdatedByUserImmigration { get; set; }

        public byte? Learner_OptEmailInvoice { get; set; }

        public DateTime? Learner_StartDate { get; set; }

        public string Learner_Passport { get; set; }

        public int? Learner_Id_VisaTypes { get; set; }

        public int? Learner_Id_Nationality { get; set; }

        public byte? Learner_Eq_IsDifficulties { get; set; }

        public string Learner_Eq_Difficulties { get; set; }

        public byte? Learner_Eq_IsNeedAssessReq { get; set; }

        public string Learner_Eq_NeedAssessReq { get; set; }

        public byte? Learner_Eq_IsSocialDifficulties { get; set; }

        public string Learner_Eq_SocialDifficulties { get; set; }

        public int? Learner_Id_CaseloadAssignedUser { get; set; }

        public DateTime? Learner_BTEC_LastSchoolDate { get; set; }

        public int? Learner_Id_FinanceStatus { get; set; }

        public int? Learner_Id_ImmigrationStatus { get; set; }

        public byte Learner_IsWorkshopLearner { get; set; }

        public DateTime? Learner_Audit_FileDate { get; set; }

        public int? Learner_Audit_Id_FileByWho { get; set; }

        public DateTime? Learner_Audit_PassVerifDate { get; set; }

        public int? Learner_Audit_Id_PassVerifByWho { get; set; }

        public int? Learner_Idd1Day { get; set; }

        public int? Learner_Idd2Day { get; set; }

        public int? Learner_Idd3Day { get; set; }

        public string Learner_IddRefNo3 { get; set; }

        public string Learner_NI { get; set; }

        public DateTime? Learner_PlannedEndDate { get; set; }

        public byte? Learner_IsRegisteredAwardingBody { get; set; }

        public byte? Learner_IsObs1Completed { get; set; }

        public byte? Learner_IsObs2Completed { get; set; }

        public byte? Learner_IsAuditSample { get; set; }

        public byte? Learner_IsMidPoint { get; set; }

        public int? Learner_Id_CourseStatusDetails { get; set; }

        public int? Learner_Id_ObsAssessor { get; set; }

        public string Learner_PercentageCompleted { get; set; }

        public int? Learner_Id_LearnerObservationStatus { get; set; }

        public int? Learner_Id_SupportiveAssessor { get; set; }

        public DateTime? Learner_LastDayLearning { get; set; }

        public int? Learner_PercentageCompleted2 { get; set; }
        public string Learner_ULN { get; set; }

        public string Learner_TotalHours { get; set; }
        public string Learner_AnnualLeaveEntitlement { get; set; }
        public string Learner_HRSWorkPerWeek { get; set; }
        public string Learner_WeeksOnProgramme { get; set; }
        public int Learner_Id_ProjectType { get; set; }
        public int Learner_Id_EmploymentStatus { get; set; }
        public int Learner_Id_EmploymentIntensityIndicator { get; set; }
        public int Learner_Id_LengthOfEmployment { get; set; }
        public string Learner_JobRole { get; set; }
        public int Learner_IsEnterredILR { get; set; }
        public int Learner_IsEnterredACE { get; set; }
        public int Learner_IsConfirmedLLP { get; set; }
        public int Learner_Id_AddSupportiveAssessor { get; set; }
        public int Learner_Id_MarketingContactConsent { get; set; }
        public int Learner_Id_EmployerPaid { get; set; }
        public int Learner_IsObservationQuestionnaireSent { get; set; }
        public string Learner_ObservationQuestionnaireSentDate { get; set; }
        public int Learner_IsObservationQuestionnaireReceived { get; set; }
        public string Learner_ObservationQuestionnaireReceivedDate { get; set; }
        public string Learner_DBS_Number { get; set; }
        public int Learner_EPAGrade { get; set; }
        public int Learner_Id_UserSkillsAdvisors { get; set; }
        public int Learner_IsVisibleInPortal { get; set; }
        public int Learner_Id_RiskRating { get; set; }
        public int Learner_Id_AwardingBodyLearner { get; set; }
        public int Learner_Id_LearnerOutcome { get; set; }
        public int Learner_EnglishInitial_Assessment { get; set; }
        public int Learner_MathInitial_Assessment { get; set; }
        public int Learner_EnglishDiagnostic_Assessment { get; set; }
        public int Learner_MathDiagnostic_Assessment { get; set; }
        public int Learner_Id_LearnerHHS { get; set; }
        public int Learner_Id_LearnerPA { get; set; }
        public int Learner_Id_LearnerLLD { get; set; }

    }

    public class LearnerComman
    {
        public int Titles_Id { get; set; }
        public string Titles_Title { get; set; }

        public int Genders_Id { get; set; }
        public string Genders_Title { get; set; }

        public int MaritalStatus_Id { get; set; }
        public string MaritalStatus_Title { get; set; }

        public int IdRole { get; set; }
        public string CandidateStatus_Title { get; set; }

        public int CandidateGroup_Id { get; set; }
        public string CandidateGroup_Title { get; set; }

        public int LIS_Id { get; set; }
        public string LIS_Title { get; set; }

        public int Region_Id { get; set; }
        public string Region_Title { get; set; }

        public int Country_Id { get; set; }
        public string Country_Name { get; set; }

        public int TrainingProvider_Id { get; set; }
        public string TrainingProvider_Title { get; set; }

        public int Opt_Value { get; set; }
        public string Opt_Title { get; set; }

        public int AwardingBodyLearner_Id { get; set; }
        public string AwardingBodyLearner_Title { get; set; }

        public int LO_Id { get; set; }
        public string LO_Title { get; set; }

        public int RiskRating_Id { get; set; }
        public string RiskRating_Title { get; set; }

        public int LCS_Id { get; set; }
        public string LCS_Title { get; set; }

        public int AccountStatus_Id { get; set; }
        public string AccountStatus_Title { get; set; }

        public int IdRoles { get; set; }
        public string FS_Title { get; set; }
        public int LPA_Value { get; set; }
        public string title { get; set; }

        public int LHHS_Value { get; set; }
        public string LHHS_Title { get; set; }

        public int LearnerEthnicity_Id { get; set; }
        public string LearnerEthnicity_Title { get; set; }

        public int Nationality_Id { get; set; }
        public string Nationality_Title { get; set; }
        public int LLDD_Value { get; set; }
        public string LLDD_Title { get; set; }

        public string CourseTitle { get; set; }
        public int Course_Id { get; set; }


        public int CoursesStatus_Id { get; set; }
        public string CoursesStatus_Title { get; set; }

        public int LearnerCourses_id { get; set; }
        public string Course_Title { get; set; }


        public int ECD_ID { get; set; }
        public string ECD_Name { get; set; }
    }

    public class LearnerUploadDoc
    {
        public int LearnerUploadedDocsId { get; set; }
        public string FilePath { get; set; }
        public string DocTopic { get; set; }
        public string FileName { get; set; }
        public string FileTitle { get; set; }
        public string UploadedDate { get; set; }
    }

    public class IVEVDoc
    {
        public int LearnerDocs_Id { get; set; }
        public string LearnerDocs_Caption { get; set; }
        public string cat { get; set; }
        public string DocCat_Title { get; set; }
        public string Users_Username { get; set; }
        public string CreatedDate { get; set; }
        public string LearnerDocs_File { get; set; }
    }

    public class SinupDoc
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string cat { get; set; }
        public int learnerId { get; set; }
        public string UploadDate { get; set; }

    }
    public class DocUploadTopic
    {
        public string SSJLP_Topic { get; set; }
        public int SSJLP_Id { get; set; }
    }
    public class LearnerEmployer
    {
        public int EmployerId { get; set; }
        public int Employer_Id { get; set; }
        public string Employer { get; set; }
    }
    public class NoteCatgory
    {
        public int LNC_Id { get; set; }
        public string LNC_Title { get; set; }
    }
    public class LearnerNote
    {
        public int id { get; set; }
        public int cateId { get; set; }
        public string date { get; set; }
        public string note { get; set; }
        public int pinned { get; set; }
        public string NoteCategory { get; set; }
        public string NoteDate { get; set; }
        public string Reminder { get; set; }
        public int isChecked { get; set; }
        public int isPinned { get; set; }
        public string PinnedBy_User { get; set; }
        public int LearnerId { get; set; }

    }

    public class LearnerReview
    {
        public int SSQR_Id { get; set; }
        public int SSQR_Id_Learner { get; set; }
        public string SSQR_Id_LearnerCourse { get; set; }
        public string SSQR_ReviewNumber { get; set; }
        public string SSQR_ReviewDate { get; set; }
        public string SSQR_IsCompleted { get; set; }
        public string SSQR_Id_QRStatus { get; set; }
        public string due { get; set; }
    }
    public class LearnerUser
    {
        public int Users_Id { get; set; }
        public string Users_Username { get; set; }
    }
    public class CourseReview
    {
        public int LearnerCourses_id { get; set; }
    }
 
   
    public class TabHistory
    {
        public int TAP_ReviewNumber { get; set; }
        public string TAP_ReviewDate { get; set; }
        public string TAP_IsApprenticeship { get; set; }
        public string TAP_CoordinatorSignatureDate { get; set; }
        public string TAP_LearnerSignatureDate { get; set; }

    }

    public class Employer
    {
        public int Employer_Id { get; set; }
        public string Employer_Ref { get; set; }
        public string Employer_Name { get; set; }
        public string Employer_ContactName { get; set; }
        public string Employer_ContactNumber1 { get; set; }
        public string Employer_ContactName2 { get; set; }
        public string Employer_ContactNumber2 { get; set; }
        public string Employer_EDRSNumber { get; set; }
        public string Employer_Address { get; set; }
        public string Employer_PostCode { get; set; }
        public string Group_Name { get; set; }
        public string UpdatedDate { get; set; }
        public string Employer_Email1 { get; set; }
        public string Employer_Email2 { get; set; }
        public string Employer_NbEmployees { get; set; }
        public string Employer_IsLevyPayingEmployer { get; set; }
        public string ECD_ID { get; set; }
        public string ECD_Name { get; set; }
        public string ECD_Email { get; set; }
        public string ECD_Tel { get; set; }
        public string ECD_JobTitle { get; set; }

    }

    public class MatrixCourse
    {
        public int LearnerCourses_id { get; set; }
        public string LearnerCourse { get; set; }
    }
    public class DocLearnerCourseId
    {
        public int LearnerCourses_Id { get; set; }
    }

    public class OscaHeaderOption
    {
        public int Opt_Value { get; set; }
        public string Opt_Title { get; set; }
    }

    public class CombineData
    {
        public int Learner_Id_TrainingProvider { get; set; }
        public int Learner_Id_FinanceStatus { get; set; }
        public int Learner_Id_AccountStatus { get; set; }
        public string Learner_IsAgreementRcvd { get; set; }
        public string Learner_AgreementDate { get; set; }
        public string Learner_EstimatedCompletionDate { get; set; }
        public string Learner_IsApplicationRcvd { get; set; }
        public string Learner_ApplicationRcvdDate { get; set; }

        public string Learner_AssessorNotifiedDate { get; set; }
        public string Learner_MandateDate { get; set; }

        public string Learner_IsIdCardSent { get; set; }
        public string Learner_IdCardSentDate { get; set; }
        public string Learner_StartDate { get; set; }
        public string Learner_PlannedEndDate { get; set; }
        public string Learner_LastDayLearning { get; set; }
        public string Learner_PracticalEndDate { get; set; }
        public string Learner_EPAGrade { get; set; }
        public string Learner_EPACompletionEndDate { get; set; }
        public string Learner_DBS_Date { get; set; }
        public string Learner_DBS_Number { get; set; }
        public string Learner_Id_CaseloadAssignedUser { get; set; }
        public string caseloadassignedto { get; set; }


    }
    public class UpdateCourse
    {
        public int LearnerCourses_Id_CoursesStatus { get; set; }
        public DateTime? LearnerCourses_CompletedDate { get; set; }
        public int LearnerCourses_IsCompleted { get; set; }
        public int LearnerCourses_Id_Learner { get; set; }
        public int LearnerCourses_Id_Course { get; set; }
        public int LearnerCourses_IsFunded { get; set; }
        public int LearnerCourses_CreatedByUser { get; set; }
        public DateTime? LearnerCourses_CreatedDate { get; set; }
    }
    public class LeanerFinanceLastDate
    {
        public string LastDate { get; set; }
    }
    public class LeanerStatusChangeDate
    {
        public string StatusChangeDate { get; set; }
    }

}
