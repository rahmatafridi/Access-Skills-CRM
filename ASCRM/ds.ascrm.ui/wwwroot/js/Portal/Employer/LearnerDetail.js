Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_EmployerLearnerDetail',
    data: {
        MatrixLable: [],
        matrixId: '',
        IsSummary: false,
        Matrix: '',
        Summary: [],
        FullSummary: [],
        SSM_Name: '',
        Id_MatrixLabel: '',
        LMatrixId: '',
        TopId: '',
        htmlContent: '',
        Docs: [],
        docId: '',
        LearnerEnrolDate: '',
        Learner_PlannedEndDate: '',
        Learner_PercentageCompleted: '',
        iPercentageCompletion: '',
        NbTopicsCompleted: '',
        NbTopicsToBeCompleted: '',
        TitleTopicCompleted: '',
        lblCompletedGLH: '',
        TitleTopicNext: '',
        CompletedGLH: '',
        Name: '',
        LearnerId: '',
        LearnerNameAndId: '',


        Employer_Address1: '',
        Employer_Address2: '',
        Employer_Address3: '',
        Employer_Address4: '',
        Employer_ContactName: '',
        Employer_ContactNumber1: '',
        Employer_Email1: '',
        Employer_Id: '',
        Employer_Name: '',
        Employer_PostCode1: '',
        Employer_PostCode2: '',
        Employer_Address:''


    },
    validations: {

    },
    methods: {
        LoadLearnerProfile: function () {
            var _learnerId = $("#learnerId").val();
            $.ajax({
                url: "/api/PortalApi/GetDashboardDataForAccount" ,
                type: "GET",
                data: { id: _learnerId },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                console.log(response);
                app.LearnerId = response.Learner_ID;
                app.Name = response.First_Name +" "+ response.Surname;
                app.LearnerEnrolDate = app.formatDateEN(response.Learner_StartDate);
                app.Learner_PlannedEndDate = app.formatDateEN(response.Learner_PlannedEndDate);
                app.Learner_PercentageCompleted = response.Learner_PercentageCompleted;
                app.NbTopicsCompleted = response.NbCompleted;
                app.NbTopicsToBeCompleted = response.NbNotCompleted;
                app.TitleTopicCompleted = response.TopicCompleted;
                app.TitleTopicNext = response.TopicNext;
                if (app.Learner_PercentageCompleted == "") {
                    app.iPercentageCompletion = 0;
                } else {
                    app.iPercentageCompletion = app.Learner_PercentageCompleted;
                }
                app.LearnerNameAndId = app.Name + "(" + response.Learner_ID + ")";
                app.CompletedGLH = response.GLH;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        LoadEmployeeProfile: function () {
            var _learnerId = $("#learnerId").val();
            $.ajax({
                url: "/api/PortalApi/GetEmployeeDataForAccount" ,
                type: "GET",
                data: { id: _learnerId },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Employer_Address = response.Employer_Address1 + " " + response.Employer_Address2 + " " + response.Employer_Address3;
                app.Employer_ContactName = response.Employer_ContactName;
                app.Employer_ContactNumber1 = response.Employer_ContactNumber1;
                app.Employer_Email1 = response.Employer_Email1;
                app.Employer_Id = response.Employer_Id;
                app.Employer_Name = response.Employer_Name;
                app.Employer_PostCode1 = response.Employer_PostCode1 +" "+ response.Employer_PostCode1;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GetMetricLable: function () {
            $.ajax({
                url: "/api/PortalMatrixApi/GetMatrixLableForAccount",
                type: "GET",
                data: { id: _learnerId },

                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.MatrixLable = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        formatDateEN: function (date) {
            const options = { month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' }
            return new Date(date).toLocaleDateString('en-GB')
        },
        getOnLoad: function () {

            var _learnerId = $("#learnerId").val();
            $.ajax({
                url: "/api/PortalMatrixApi/GetMatrixLableForAccount",
                data: { id: _learnerId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                var MatrixLable = response;
                
                if (MatrixLable.length > 0) {
                    app.matrixId = MatrixLable[0].SSMB_Id;
                    app.changeMatrixLable();
               
                }

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        changeMatrixLable: function () {
            var _learnerId = $("#learnerId").val();
            
            $.ajax({
                url: "/api/PortalMatrixApi/GetMatrixForAccount",
                type: "GET",
                data: { matrixId: app.matrixId, id: _learnerId },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#matrixdata").show();
                app.TopId = response[0].SSML_Id_MatrixLabel;
                app.LMatrixId = response[0].SSM_Id;
                app.SSM_Name = response[0].SSM_Name;
                app.GetFullSummary();

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

            $.ajax({
                url: "/api/PortalMatrixApi/GetMatrixSummaryForAccount",
                type: "GET",
                data: { matrixId: app.matrixId, id: _learnerId },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Summary = response;
                app.IsSummary = true;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });


        },
        GetFullSummary: function () {
            var _learnerId = $("#learnerId").val();

            $.ajax({
                url: "/api/PortalMatrixApi/GetMatrixFullSummaryForAccount",
                type: "GET",
                data: {
                    matrixId: app.LMatrixId,
                    topicid: app.TopId,
                    id: _learnerId
                },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                app.FullSummary = response;
                console.log(app.FullSummary);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        DownlaodCompleteSummary: function () {
            var _learnerId = $("#learnerId").val();
            var url = window.location.href;
            let url1 = new URL(url);
            let search_params = url1.searchParams;
            $.ajax({
                url: "/Document/DownloadCompleteSummaryForAccount",
                type: "GET",
                data: { id: search_params.get('id') },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                //app.Summary = response;
                //app.IsSummary = true;
                app.htmlContent = response;
                var opt = {
                    margin: [3, 0, 3, 0],

                    filename: 'CourseSummary.pdf',
                    html2canvas: {
                        scale: 2

                    },
                    pagebreak: { avoid: 'label' },
                    /*image: { type: 'jpeg', quality: 0.98 },*/

                    jsPDF: { format: 'a4', orientation: 'portrait' },

                };



                html2pdf().set(opt).from(response).save();


            }).fail(function (xhr) {
                console.log(xhr.responseText);
                app.htmlContent = xhr.responseText;
                var opt = {
                    margin: [3, 0, 3, 0],

                    filename: 'CourseSummary.pdf',
                    html2canvas: {
                        scale: 2

                    },
                    pagebreak: { avoid: 'label' },
                    /*image: { type: 'jpeg', quality: 0.98 },*/

                    jsPDF: { format: 'a4', orientation: 'portrait' },

                };



                html2pdf().set(opt).from(xhr.responseText).save();
                // toastr.error(xhr.responseText, 'Error!');
            });

            // window.location = '/Document/DownloadCompleteSummary?courseId1=' + app.matrixId;

        },
        ViewSummary: function () {
            var _learnerId = $("#learnerId").val();
            var url = window.location.href;
            let url1 = new URL(url);
            let search_params = url1.searchParams;

            // get value of "id" parameter
            // "100"
            console.log(search_params.get('id'));
            $.ajax({
                url: "/Document/DownloadCompleteSummaryForAccount",
                type: "GET",
                data: { id: search_params.get('id') },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                //app.Summary = response;
                //app.IsSummary = true;
                console.log(response);
                app.htmlContent = response;


            }).fail(function (xhr) {
                app.htmlContent = xhr.responseText;
                $("#matrixdata").hide();
                $("#matrixview").show();
                $("#btnview").hide();
                $("#btncancel").show();

                // toastr.error(xhr.responseText, 'Error!');
            });

            // window.location = '/Document/DownloadCompleteSummary?courseId1=' + app.matrixId;

        },
        ViewCancel: function () {
            $("#matrixdata").show();
            $("#matrixview").hide();
            $("#btnview").show();
            $("#btncancel").hide();
        },
        DocTypeChange: function () {
            var catevalue = $("#docId").val();
            if (catevalue != "0") {
                $("#doctable").show();
                var _learnerId = $("#learnerId").val();

                $.ajax({
                    url: "/api/PortalCourseSummaryApi/GetLearnerDocForAccount",
                    type: "GET",
                    data: { id: _learnerId},
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {

                    app.Docs = response;

                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
            }
        }
    }
})
//app.getOnLoad();
//app.GetMetricLable();