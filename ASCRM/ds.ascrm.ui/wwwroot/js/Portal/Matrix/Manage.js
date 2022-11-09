Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_ViewMatrix',
    data: {
        MatrixLable: [],
        matrixId: '',
        IsSummary: false,
        Matrix:'',
        Summary: [],
        FullSummary: [],
        SSM_Name: '',
        Id_MatrixLabel: '',
        LMatrixId: '',
        TopId: '',
        htmlContent:''

    },
    validations: {

    },
    methods: {
        GetMetricLable: function () {
            $.ajax({
                url: "/api/PortalMatrixApi/GetMatrixLable",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.MatrixLable = response;
                console.log(app.MatrixLable);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        changeMatrixLable: function () {

            $.ajax({
                url: "/api/PortalMatrixApi/GetMatrix",
                type: "GET",
                data: { matrixId: app.matrixId},
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#matrixdata").show();
                app.TopId = response[0].SSML_Id_MatrixLabel;
                app.LMatrixId = response[0].SSM_Id;
                app.SSM_Name = response[0].SSM_Name;
                console.log(response);
                app.GetFullSummary();

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

            $.ajax({
                url: "/api/PortalMatrixApi/GetMatrixSummary",
                type: "GET",
                data: { matrixId: app.matrixId },
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
            debugger;
            $.ajax({
                url: "/api/PortalMatrixApi/GetMatrixFullSummary",
                type: "GET",
                data: {
                    matrixId: app.LMatrixId,
                    topicid: app.TopId
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
            $.ajax({
                url: "/Document/DownloadCompleteSummary",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                //app.Summary = response;
                //app.IsSummary = true;
                console.log(response);
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
            $.ajax({
                url: "/Document/DownloadCompleteSummary",
                type: "GET",
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
        }
    }
})
app.GetMetricLable();