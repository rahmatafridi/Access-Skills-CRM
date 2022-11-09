Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_LearnerWork',
    data: {
        Notifications: [],
        notes:'',
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
           

        },
        Document: {
            topic_id: 0,
            isAssessor: true,
            learner_id:0
        },

        courseId: 0,
        files:[]
    },
    validations: {

    },
    methods: {
      
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
            var workid = app.id;
            this.Assesser.id = workid;
            this.Assesser.is_assessor_marking_valid = 1;
            this.Assesser.note = app.notes;
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
            debugger
            console.log(id);
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
                console.log(app.files);
                $("#kt_modal_files1").modal("show");

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        downlaodFile: function (path) {
            window.location = '/Document/DownloadLearnerUploadFile?filePath=' + path;
        }


    },
   
})

function PerformReject(id) {
    app.id = id;
    app.rejected_reason = "";
    $("#kt_modal_reject_reason").modal("show");
}
function DownlaodFile(id) {

}
function PerformApprove(id) {
    app.notes = "";
    
    app.id = id;
    $("#kt_modal_approve").modal("show");
}
function PerformUploadFile(topic_id,learnerId) {
    app.upload(topic_id, learnerId);
}
  function PerformFiles(id) {
        app.loadFiles(id);
    }