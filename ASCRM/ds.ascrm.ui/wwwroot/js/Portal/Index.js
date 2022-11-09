
Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_Deshboard',
    data: {
        Notifications: [],
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
        totalcompletedb: '',
        notes: '',
        id: 0,
        rejected_reason: '',
        Assesser: {
            id: 0,
            assessor_assigned_user_id: 0,
            rejected_reason: '',
            due_date: '',
            note: '',
            assessor_rejected: 0,
            is_assessor_marking_valid: 0,
            traningAssessment: {
                tap_content_activty: '',
                tap_core_skil: '',
                tap_referencing: '',
                tap_development: '',
                tap_assessment_criteria: '',
                tap_resubmission: '',
                tap_assessor_signature: '',
                tap_assessor_sign :''

            }

        },
        Document: {
            topic_id: 0,
            isAssessor: true,
            learner_id: 0
        },

        courseId: 0,
        files: [],
        isResubmission: false,
        totalapprove:0,
        totalrejected: 0,
        fileHistory: []

    },
    validations: {

    },
    methods: {
        getNotification: function () {
            $.ajax({
                url: "/api/PortalApi/GetNotification",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Notifications = response;
                

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GoToSummary: function (id) {
            window.location = '/Portal/CourseSummary/MyCourseSummary#'+id;
        },
        GetAssosserSummary: function () {
            $.ajax({
                url: "/api/PortalApi/GetAssosserSummary",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);
                app.totalapprove = response.assessor_approve_count;
                app.totalrejected = response.assessor_reject_count;
                
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GetDashboardData: function () {
            $.ajax({
                url: "/api/PortalApi/GetDashboardData",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
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
               
                app.CompletedGLH = response.GLH;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        formatDateEN: function(date) {
    const options = { month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' }
        return new Date(date).toLocaleDateString('en-GB')
        },
        workReject: function () {

            var workid = parseInt(app.id);
            this.Assesser.id = workid;
            this.Assesser.rejected_reason = app.rejected_reason;
            this.Assesser.assessor_rejected = 1;
            var data = this.Assesser;

            $.ajax({
                url: "/api/WorkUploadApi/Reject",
                data: JSON.stringify(data),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#kt_modal_reject_reason").modal("hide");

                $('#grid_submitted').data('kendoGrid').dataSource.read();
                $('#grid_submitted').data('kendoGrid').refresh();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        approve: function () {
            var ids = $("#newId").val();
            var workid = parseInt(app.id);
            
            this.Assesser.id = workid;
            
            this.Assesser.is_assessor_marking_valid = 1;
            this.Assesser.note = app.notes;
            if (app.isResubmission == true) {
                var value = $("#resubmission").val();
                if (value == "") {
                    toastr.error("Please Select Days");
                    return;
                }
                this.Assesser.traningAssessment.tap_resubmission = value;
            }


            var sing = $("#printName").val();
            if (sing == "") {
                toastr.error("Please Enter the Sign", 'Error!');
                return;
            }
            var canvas = document.getElementById('signaturepad');
            var context = canvas.getContext('2d');
            var dataUri = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");  // here is the most important part because if you dont replace you will get a DOM 18 exception.

            $("#sketch_data").val(dataUri);
            $("#sketch_data").val(dataUri);

            this.Assesser.traningAssessment.tap_assessor_signature = dataUri;
            this.Assesser.traningAssessment.assessor_sign = sing;
        

            var data = this.Assesser;

            $.ajax({
                url: "/api/WorkUploadApi/Approve",
                data: JSON.stringify(data),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#kt_modal_approve").modal("hide");
                toastr.success("Approved successfully.", "Approved!");

                /* $("#kt_modal_reject_reason").modal("hide");*/
                $('#grid_submitted').data('kendoGrid').dataSource.read();
                $('#grid_submitted').data('kendoGrid').refresh();

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        upload: function (id, learnerId) {
       
            this.Document.topic_id = id;
            this.Document.learner_id = learnerId;
            var upload = $("#files").data("kendoUpload");
            $("#kt_modal_Create").modal("show");

        },
        Submit: function () {
            debugger;
            var formData = new FormData();
            var upload = $("#files").getKendoUpload();
            var files = upload.getFiles();

            if (files.length === 0) {
                toastr.error('Please select file first', 'Error!');
                return;
            }

            if (files.length > 0) {
                for (var count = 0; count < files.length; count++) {
                    formData.append('files', files[count].rawFile);

                }
            }
            else {
                formData.append('files', files[0].rawFile);
            }
            var data = this.Document;
            formData.append("documentData", JSON.stringify(data));
            //var data = this.Topic;
            // var data = this.Document;
            //formData.append("documentData", JSON.stringify(data));

            $.ajax({
                url: "/api/PortalCourseSummaryApi/UploadDocumentsAjax",
                data: formData,
                type: "Post",
                contentType: false,
                processData: false
            }).done(function (response) {
                if (response === true) {

                    toastr.success("Uploaded successfully.", "Success!");
                    $("#kt_modal_Create").modal("hide");
                    //    location.reload();
                }
            })

        },
        loadFiles: function (id) {
            var ids = parseInt(id);
            $.ajax({
                url: "/api/WorkUploadApi/GetWorkFiles",
                type: "GET",
                data: {
                    work_upload_id: ids
                },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.files = response;
                $("#kt_modal_files1").modal("show");

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        downlaodFile: function (path) {
            window.location = '/Document/DownloadLearnerUploadFile?filePath=' + path;
        },
        assessmentchange: function (event) {
            var data = event.target.value;
            if (data == 'no') {
                app.isResubmission = true;
                $("#resubmissiondata").show();
            }
            if (data == 'yes') {
                $("#resubmissiondata").hide();
                app.isResubmission = false;

            }

        },
        Sign: function (id) {
            
            app.tabs.TAP_Id = id;
            app.tabs.learnerComment = '';
            $("#printName").val('');
            $("#myModal").modal("show");

        },
        txtPrintName: function (e) {
            var value = e.target.value;
            this.printName = e.target.value;
            var stt = value;
            var lng = stt.length;
            var fnt = 30;
            //max 20!
            if (lng < 20) { fnt = 30; }
            else if (lng < 30) { fnt = 20; }
            else { fnt = 15; }

            var canvas = document.getElementById('signaturepad');
            var context = canvas.getContext('2d');
            context.clearRect(0, 0, canvas.width, canvas.height);
            var fntsize = fnt + 'pt';
            context.font = 'italic ' + fntsize + ' Sacramento, cursive ';
            context.fillText(stt, 50, 60);
        },
        ViewHistory: function (learner_id, topic_id) {
            $.ajax({
                url: "/api/WorkUploadApi/GetHistory?learner_id=" + learner_id + "&topic_id=" + topic_id,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                app.fileHistory = response;
                console.log(response);
                $("#kt_modal_history").modal("show");


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });


        },
        DownloadViewHistory: function (data, learner_id) {
            var sampleArr = app.base64ToArrayBuffer(data);
            app.saveByteArray(learner_id, sampleArr);
        }
    }
})
app.getNotification();
app.GetDashboardData();
app.GetAssosserSummary();
function HistoryFileView(learner_id, topic_id) {
    app.ViewHistory(learner_id, topic_id);
}
