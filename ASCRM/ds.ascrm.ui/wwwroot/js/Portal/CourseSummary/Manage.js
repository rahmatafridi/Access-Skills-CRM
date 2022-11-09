
Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_CourseContentsSummary',
    data: {
        CourseLevels: [],
        Document: {
            topic_id: 0,
            isAssessor: false

        }
       
    },
    validations: {

    },
    methods: {

        upload: function (id) {
          
            this.Document.topic_id  = id;
            var upload = $("#files").data("kendoUpload");
            $("#kt_modal_Create").modal("show");

        },
        Submit: function () {
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
                    location.reload();
                }
            })

        },
    }
})
function uploaddoc(id) {
    app.upload(id);
}
function PerformCourseContentEdit(id) {
    app.Edit(id);
}
