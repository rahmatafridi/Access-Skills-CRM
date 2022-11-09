
Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_MyDocument',
    data: {
        documents: [],
        
    },
    validations: {

    },
    methods: {
        GetMyDocument: function () {
            //$.ajax({
            //    url: "/api/ListApi/GetDropdownOptionsByHeaderName",
            //    type: "GET",
            //    contentType: "application/json",
            //    data: { headerName: "CourseLevels" },
            //    dataType: "json"
            //}).done(function (response) {
            //    app.CourseLevels = response;
            //}).fail(function (xhr) {
            //    toastr.error(xhr.responseText);
            //});


            $.ajax({
                url: "/api/PortalMyDocumentApi/GetMyDocument",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.documents = response;
                console.log(app.documents);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        downlaodFile: function (id) {
            debugger
            var valObj = app.documents.Data.filter(function (elem) {
                if (elem.Id == id) return elem.Id;
            });
            if (valObj.length > 0) {
                window.location = '/Document/DownloadLocalFile?filePath=' + valObj[0].FilePath;
            }
        }
     
    }
})
function downlaodFile(id) {
    app.downlaodFile(id)
   // window.location = '/Document/DownloadLocalFile?filePath=' + path;
}
app.GetMyDocument();