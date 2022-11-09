
var app = new Vue({

    el: '#dv_Learner_Detail',
    data: {
        learnerId:0,
        isSubmitted: false,
        Companies: [],
        SalesAdvisors: [],
        Learner:{
            Learner_Ref: '',
            Learner_Id_Titles: '',
            Learner_FirstName: '',
            Learner_Middlename: '',
            Learner_Surname: '',
            Learner_Gender: '',
            Learner_DOB: '',
            Learner_Address1: '',
            Learner_Address2: '',
            Learner_Address3: '',
            Learner_Address4: '',
            Learner_Address5: '',
            Learner_PostCode1: '',
            Learner_PostCode2: '',
            Learner_Telephone: '',
            Learner_Telephone2: '',
            Learner_Mobile1: '',
            Learner_Mobile2: '',
            Learner_TelephoneWork1: '',
            Learner_TelephoneWork2: '',
            Learner_Email1: '',
            Learner_Email2: '',
            Learner_Class: '',
            Learner_NI: '',
            Learner_ULN: '',
            Learner_Eq_Difficulties: '',
            Learner_Eq_NeedAssessReq: '',
            Learner_Eq_SocialDifficulties: '',
            Learner_Id_Nationality: '',
            Learner_Id_LearnerLLDD: '',
            Learner_Id_LearnerPA: '',
            Learner_Id_LearnerHHS: '',
            Learner_MathDiagnostic_Assessment: '',
            Learner_EnglishDiagnostic_Assessment: '',
            Learner_MathInitial_Assessment: '',
            Learner_EnglishInitial_Assessment: '',
            Learner_Id_AwardingBodyLearner: '',
            Learner_Id_LearnerOutcome: '',
            Learner_Id_RiskRating: '',
            Learner_Eq_IsDifficulties: '',
            Learner_Eq_IsNeedAssessReq: 0,
            Learner_Eq_IsSocialDifficulties: '',
            Learner_IsOverseas: '',
            Learner_IsPBS: '',
            Learner_IsUKResident: '',
            Learner_IsWorkshopLearner: '',
            Learner_IsVisibleInPortal: '',
            Learner_Id_MaritalStatus: '',
            Learner_Id_Status: '',
            Learner_Id_UserSkillsAdvisors: '',
            Learner_Id_Ethnicity: '',
            Learner_Id_PriorityStatus: '',
            Learner_Id_Country: '',
            Learner_Id_CandidateStatus: '',
            Learner_Id_CandidateGroup: '',
            Learner_Id_ImmigrationStatus: '',
            Learner_Id_TrainingProvider: '',
            Learner_Id_ProjectType: '',
            Learner_Id_EmploymentStatus: '',
            Learner_Id_EmploymentIntensityIndicator: '',
            Learner_Id_LengthOfEmployment: '',
            Learner_IddRefNo: '',
            Learner_IddABFNo: '',
            Learner_IddRefNo3: '',
            Learner_Idd1Day: '',
            Learner_Idd2Day: '',
            Learner_Idd3Day: '',
            Learner_JobRole: '',
            Learner_IsRegisteredAwardingBody: 0,
            Learner_IsMidPoint: 0,
            Learner_IsAuditSample: 0,
            Learner_IsEnterredILR: 0,
            Learner_IsEnterredACE: 0,
            Learner_IsConfirmedLLP: 0,
            Learner_Id_ObsAssessor: '',
            Learner_Id_SupportiveAssessor: '',
            Learner_Id_AddSupportiveAssessor: '',
            Learner_Id_CourseStatusDetails: '',
            Learner_PercentageCompleted: '',
            Learner_Id_LearnerObservationStatus: '',
            Learner_Id_MarketingContactConsent: '',
            Learner_Id_EmployerPaid: '',
            Learner_Id_Regions: '',
            Learner_IsObservationQuestionnaireSent: '',
            Learner_ObservationQuestionnaireSentDate: '',
            Learner_IsObservationQuestionnaireReceived: '',
            Learner_ObservationQuestionnaireReceivedDate: '',
            Learner_DBS_Number: '',
            Learner_IsIdCardSent: '',
            Learner_IdCardSentDate: '',
            Learner_EstimatedCompletionDate: '',
            Learner_StartDate: '',
            Learner_LastDayLearning: '',
            Learner_PlannedEndDate: '',
            Learner_PracticalEndDate: '',
            Learner_Id_FinanceStatus: '',
            Learner_Id_AccountStatus: '',
            Learner_Id_LearnerHHS: '',
            Learner_HRSWorkPerWeek: '',
            Learner_WeeksOnProgramme: '',
            Learner_AnnualLeaveEntitlement: '',
            Learner_TotalHours: '',
            Learner_PracticalEndDate: '',
            Learner_Id:0

           

        },
        FinanceLastDate: '',
        StatusChangeDate:'',
        caseloadassignedto: '',
        Genders: [],
        Groups: [],
        Status: [],
        Regions: [],
        ImmigrationStatus: [],
        Maritals: [],
        Titles: [],
        Country: [],
        Advisors: [],
        FundingTypes: [],
        ProjectTypes:[],
        AwardingBodys: [],
        OutComes: [],
        Riskings: [],
        CourseStatus: [],
        ObservationAssessors: [],
        AccountStatus: [],
        EmployerPaids: [],
        FinanceStaus: [],
        PaymentStaus: [],
        PriorAttainment: [],
        LearnerHHS: [],
        LearnerLLDD: [],
        Nationality: [],
        Ethnicity: [],
        AppHPW: '',
        WOP: '',
        LALE: '',
        TH: '',
        MOJC: '',
        matrixtypeid: '',
        matrixlearnercourseid: '',
        matrixtopicid:0,
        EmployerId:0,
        CourseDetail: {
            LearnerCourses_id: '',
            StatusDate: '',
            Course_SchemeCode: '',
            Course_Title: '',
            CoursesStatus_Title: '',
            LearnerCourses_IsFunded: '',
            TotalActiveDays: '',

        },
        QualCourses: [],
        CourseStatus1: [],
        ReviewCourse: [],
        ListReview: [],
        ReviewCourseId: '',
        CPD: {
            date_start: '',
            date_practical_end: '',
            weekly_hrs: '',
            daily_hrs: '',
            actual_otj_hrs: '',
            min_20_otj_hrs: '',
            remaining_otj_hrs: '',
            otj_working_percentage: '',
            otj_working_status:''
        },
        CompleteActivites: [],
        AdditionalActivites: [],
        TabHistory: [],
        Employer: {
            Employer_Id: 0,
            UpdatedDate: '',
            Group_Name: '',
            Employer_Name: '',
            ECD_ID: '',
            ECD_Tel: '',
            ECD_Email: '',
            ECD_JobTitle: '',
            Employer_Address: '',
            Employer_PostCode: '',
            Employer_NbEmployees: '',
            Employer_IsLevyPayingEmployer: '',
            Employer_EDRSNumber:''
        },
        PrimaryContatcs: [],
        Employers: [],
        AutoEmployer: [],
        ProfileActivty: [],
        IVEVS: [],
        SingupDocs: [],
        docTopicId: '',
        docTopics: [],
        NoteCates: [],
        Note: {
            cateId: 0,
            date: '',
            note: '',
            pinned:0
        },
        NoteList: [],
        MatrixLearnerCourses: [],
        MatrixTopic: [],
        MathsInitial: [],
        MarketingContactConsent: [],
        EmploymentStatus: [],
        EmploymentIntensity: [],
        LengthOfEmployment: [],
        CPD: {
            Actual_Otj: '',
            Activity_Title: '',
            Completed_Date: '',
            Description: '',
            CPD_Id: 0,
            Learner_Id:0
        },
        CourseUpdate: {
            LearnerCourses_Id_CoursesStatus: 0,
            LearnerCourses_CompletedDate: '',
            LearnerCourses_IsCompleted: 0,
            LearnerCourses_Id_Learner: 0,
            LearnerCourses_Id_Course: 0,
            LearnerCourses_IsFunded: 0,
            
        },
         ruta: '',

    },
    methods: {
        loadNoteList: function (learnerid,note,cateid) {
            $.ajax({
                url: "/api/LearnersApi/GetNoteList",
                data: { learnerId: learnerid, note: note, cateid: cateid },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.docTopics = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadcaseassigned: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetCombineData",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);
                app.caseloadassignedto = response.caseloadassignedto;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadStatusChangeDate: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetStatusChangeDate",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);
                app.StatusChangeDate = response.StatusChangeDate;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadGetFinanceLastDate: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetFinanceLastDate",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);
                app.FinanceLastDate = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loaddocTopics: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetdocTopics",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.docTopics = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadNoteCates: function () {
            $.ajax({
                url: "/api/LearnersApi/GetLoadNoteCatgory",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.NoteCates = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadProfileActivty: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetProfileActivty",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.ProfileActivty = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadSingupDocs: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetSingupDocs",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.SingupDocs = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadIVEVS: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetIVEVS",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.IVEVS = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadPrimaryContatcs: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetPrimaryContatcs",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.PrimaryContatcs = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        searchEmployer: function (name) {
            $.ajax({
                url: "/api/LearnersApi/GetEmployerbyName",
                data: { name: name },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                //app.AutoEmployer = [];
                app.Employers = response;
                for (var i = 1; i < app.Employers.length; i++) {
                    app.AutoEmployer.push({ value: app.Employers[i].Employer, data: app.Employers[i].Employer_Id });
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        UpdateCourseDetail: function () {
           
            var data = this.CourseUpdate;
            data.LearnerCourses_Id_Learner = app.learnerId;
            $.ajax({
                url: "/api/LearnersApi/UpdateCourse",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    if (app.Note.Note_Id > 0) {
                        toastr.success("updated successfully.", "Updated!");
                        $("#note_model").modal("hide");

                    } else {
                        toastr.success(" inserted successfully.", "Inserted!");
                        $("#note_model").modal("hide");

                    }
                } else {
                    toastr.error("Cannot submitted, please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },


        loadTitle: function () {
            $.ajax({
                url: "/api/LearnersApi/GetTitle",
               
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Titles = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadCourseStatus1: function () {
            $.ajax({
                url: "/api/LearnersApi/GetCourseStatus1",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.CourseStatus1 = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadQualCourses: function () {
            $.ajax({
                url: "/api/LearnersApi/GetQualCourses",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.QualCourses = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadGender: function () {
            $.ajax({
                url: "/api/LearnersApi/GetGender",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Genders = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadGroup: function () {
            $.ajax({
                url: "/api/LearnersApi/GetGroup",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Groups = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        loadCourseDetail: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetCourseDetail",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);
                app.CourseDetail = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadStatus: function () {
            $.ajax({
                url: "/api/LearnersApi/GetStatus",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Status = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadMarital: function () {
            $.ajax({
                url: "/api/LearnersApi/GetMarital",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Maritals = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadImmigrationStatus: function () {
            $.ajax({
                url: "/api/LearnersApi/GetImmigrationStatus",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.ImmigrationStatus = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadRegion: function () {
            $.ajax({
                url: "/api/LearnersApi/GetRegion",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Regions = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadDetial: function (id) {
            $.ajax({
                url: "/api/LearnersApi/LoadDeatil",
                data: {id:id},
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Learner = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadAdvisor: function () {
            $.ajax({
                url: "/api/LearnersApi/GetSkillsAdvisor",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Advisors = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadCountry: function () {
            $.ajax({
                url: "/api/LearnersApi/GetCountry",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Country = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadProjectType: function () {
            $.ajax({
                url: "/api/LearnersApi/GetProjectType",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.ProjectTypes = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadFundingType: function () {
            $.ajax({
                url: "/api/LearnersApi/GetFundingType",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.FundingTypes = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadOutcome: function () {
            $.ajax({
                url: "/api/LearnersApi/GetOutcome",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.OutComes = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadAwrdingBody: function () {
            $.ajax({
                url: "/api/LearnersApi/GetAwardingBody",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.AwardingBodys = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadFinanceStaus: function () {
            $.ajax({
                url: "/api/LearnersApi/GetFinanceStaus",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.FinanceStaus = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadRisking: function () {
            $.ajax({
                url: "/api/LearnersApi/GetRisking",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Riskings = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadObservationAssessors: function () {
            $.ajax({
                url: "/api/LearnersApi/GetObservationAssessors",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.ObservationAssessors = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadAccountStatus: function () {
            $.ajax({
                url: "/api/LearnersApi/GetAccountStatus",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.AccountStatus = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        loadEthnicity: function () {
            $.ajax({
                url: "/api/LearnersApi/GetEthnicity",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Ethnicity = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadNationality: function () {
            $.ajax({
                url: "/api/LearnersApi/GetNationality",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Nationality = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadLearnerLLDD: function () {
            $.ajax({
                url: "/api/LearnersApi/GetLearnerLLDD",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.LearnerLLDD = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
       

        loadPaymentStaus: function () {
            $.ajax({
                url: "/api/LearnersApi/GetPaymentStaus",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.PaymentStaus = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadCourseStatus: function () {
            $.ajax({
                url: "/api/LearnersApi/GetCourseStatus",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.CourseStatus = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadEmployerPaid: function () {
            $.ajax({
                url: "/api/LearnersApi/GetEmployerPaid",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.EmployerPaids = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadLearnerHHS: function () {
            $.ajax({
                url: "/api/LearnersApi/GetLearnerHHS",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.LearnerHHS = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadPriorAttainment: function () {
            $.ajax({
                url: "/api/LearnersApi/GetPriorAttainment",

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.PriorAttainment = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadReviewCourse: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetReviewCourse",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.ReviewCourse = response;
                app.loadReviewList(id, app.ReviewCourse[0].LearnerCourses_id);
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadReviewList: function (id, courseId) {
            debugger;
            $.ajax({
                url: "/api/LearnersApi/GetReviewList?id=" + id + "&courseId=" + courseId,

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                
                app.ListReview = response;

            }).fail(function (xhr) {
                
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadcpdPrime: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetCPDPrime",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.CPD.date_start = response.date_start;
                app.CPD.date_practical_end = response.date_practical_end;
                app.CPD.weekly_hrs = response.weekly_hrs;
                app.CPD.daily_hrs = response.daily_hrs;
                app.CPD.actual_otj_hrs = response.actual_otj_hrs;
                app.CPD.min_20_otj_hrs = response.min_20_otj_hrs;
                app.CPD.remaining_otj_hrs = response.remaining_otj_hrs;
                app.CPD.otj_working_percentage = response.otj_working_percentage;
                app.CPD.otj_working_status = response.otj_working_status;
                
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }  ,
        loadCompleteActivites: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetCompleteActivites",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.CompleteActivites = response;
                
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }   ,
        loadAdditionalActivites: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetAdditionalActivites",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.AdditionalActivites = response;
                
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadTabHistory: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetTabHistory",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.TabHistory = response;
                
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadEmployer: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetEmployer",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Employer = response;
                
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        NewNote: function () {
            $("#note_model").modal("show");

        },
        AddNote: function () {
            debugger;
            if (this.Note.pinned == true) {
                this.Note.pinned = 1;
            } else {
                this.Note.pinned = 0;

            }
            var data = this.Note;
            $.ajax({
                url: "/api/LearnersApi/InsertNotes",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    if (app.Note.Note_Id > 0) {
                        toastr.success("Note updated successfully.", "Updated!");
                        $("#note_model").modal("hide");

                    } else {
                        toastr.success("Note inserted successfully.", "Inserted!");
                        $("#note_model").modal("hide");

                    }
                } else {
                    toastr.error("Cannot submitted, please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadMatrixLearnerCourses: function (id) {
            $.ajax({
                url: "/api/LearnersApi/GetMatrixCourses",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.MatrixLearnerCourses = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadMatrixLearnerTopic: function (id,type) {
            $.ajax({
                url: "/api/LearnersApi/GetMatrixCourses",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.MatrixLearnerCourses = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        loadMathsInitial: function () {
            $.ajax({
                url: "/api/LearnersApi/GetHeaders",
                data: { header: "BKSB Assessment" },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.MathsInitial = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadMarketingContactConsent: function () {
            $.ajax({
                url: "/api/LearnersApi/GetHeaders",
                data: { header: "Marketing Contact Consent" },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.MarketingContactConsent = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadEmploymentStatus: function () {
            $.ajax({
                url: "/api/LearnersApi/GetHeaders",
                data: { header: "Employment Status" },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.EmploymentStatus = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadEmploymentIntensity: function () {
            $.ajax({
                url: "/api/LearnersApi/GetHeaders",
                data: { header: "Employment Intensity Indicator" },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.EmploymentIntensity = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadLengthOfEmployment: function () {
            $.ajax({
                url: "/api/LearnersApi/GetHeaders",
                data: { header: "Length Of Employment" },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.LengthOfEmployment = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GenerateCourseSummary: function () {
            var id = app.learnerId;
            $.ajax({
                url: "/api/LearnersApi/GetCourseSummaryReport",
                data: { id: id },

                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                debugger;
                console.log(response);
                app.ruta = response;
                var sampleArr = app.base64ToArrayBuffer(response);
                app.saveByteArray(app.learnerId, sampleArr);
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        base64ToArrayBuffer: function (base64) {
            var binaryString = window.atob(base64);
            var binaryLen = binaryString.length;
            var bytes = new Uint8Array(binaryLen);
            for (var i = 0; i < binaryLen; i++) {
                var ascii = binaryString.charCodeAt(i);
                bytes[i] = ascii;
            }
            return bytes;
        },
        saveByteArray: function (reportName, byte) {
            var blob = new Blob([byte], {
                type: "application/pdf"
            });
            //var link = document.createElement('a');
            //link.href = window.URL.createObjectURL(blob);
            //const data = window.URL.createObjectURL(blob);

            var fileName = reportName;
            //link.download = fileName;
            //link.click();
            //window.URL.revokeObjectURL(data);
            //link.remove();
            var file = new Blob([byte], { type: 'application/pdf' });
            var fileURL = URL.createObjectURL(file);
            window.open(fileURL);

            //const data = window.URL.createObjectURL(blob);
            //const link = document.createElement('a');
            //document.body.appendChild(link);
            //link.href = data;
            //link.download = `${fileName}.pdf`;
            //link.click();
            //window.URL.revokeObjectURL(data);
            //link.remove();
        },
        Save: function () {
            var data = app.Learner;
            data.Learner_Id = app.learnerId;

            $.ajax({
                url: "/api/LearnersApi/UpdateLearner",
                data: JSON.stringify(data),

                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                toastr.success("Learner Updated.", "Success!");
                location.reload();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        updateEmployer: function () {
            debugger;
            var employerId = app.EmployerId;
            var learnerId = app.learnerId;
            if (learnerId == 0 ) {
                toastr.error("Provide the Learner Id.", "error!");
                return;
            }

            if (employerId == 0 ) {
                toastr.error("Provide the Employer Id.", "error!");
                return;
            }
            $.ajax({
                url: "/api/LearnersApi/UpdateEmployer?learnerId=" + learnerId + "&employerId=" + employerId,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                
                toastr.success("Employer Updated.", "Success!");
                location.reload();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        updateAssessment: function () {
            debugger;
            var employerId = app.EmployerId;
            var learnerId = app.learnerId;
            if (learnerId == 0 ) {
                toastr.error("Provide the Learner Id.", "error!");
                return;
            }

            if (employerId == 0 ) {
                toastr.error("Provide the Employer Id.", "error!");
                return;
            }
            $.ajax({
                url: "/api/LearnersApi/UpdateAssessment?learnerId=" + learnerId + "&employerId=" + employerId,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                
                toastr.success("Employer Updated.", "Success!");
                location.reload();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        
        addNew: function () {
            $("#cpd_model").modal("show");
        },
        AddCPD: function () {
            var data = app.CPD;
            data.Learner_Id = app.learnerId;

            $.ajax({
                url: "/api/LearnersApi/AddCPD",
                data: JSON.stringify(data),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                toastr.success("CPD Added.", "Success!");
                $("#cpd_model").modal("hide");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        ReviewDetail: function () {
            $("#cpd_ReviewDetail").modal("show");
        }
    },
    
})
//app.loadPrimaryContatcs();
app.loadLengthOfEmployment();
app.loadEmploymentIntensity();
app.loadEmploymentStatus();
app.loadMathsInitial();
app.loadMarketingContactConsent();
app.loadNoteCates();
app.loadRisking();
app.loadFinanceStaus();
app.loadCourseStatus1();
app.loadQualCourses();
app.loadGender();
app.loadTitle();
app.loadGroup();
app.loadMarital();
app.loadRegion();
//app.loadStatus();
app.loadAdvisor();
app.loadCountry();
app.loadImmigrationStatus();
app.loadFundingType();
app.loadProjectType();
app.loadAwrdingBody();
app.loadOutcome();
app.loadCourseStatus();
app.loadObservationAssessors();
app.loadAccountStatus();
app.loadPaymentStaus();
app.loadEmployerPaid();
app.loadPriorAttainment();
app.loadLearnerHHS();

app.loadEthnicity();
app.loadNationality();
app.loadLearnerLLDD();