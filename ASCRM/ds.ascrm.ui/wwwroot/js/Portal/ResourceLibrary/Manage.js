Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_ResourceLibrary',
    data: {
        Category: [],

    },
    validations: {

    },
    methods: {
        GetCategory: function () {

        



            $.ajax({
                url: "/api/PortalResourceLibraryApi/GetCategory",
                type: "GET",

                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Category = response;
                console.log(app.Category);
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        downlaodFile: function (path) {
            window.location = '/Document/DownloadLearnerUploadFile?filePath=' + path;
        },
        LoadOnCate: function () {
            $.ajax({
                url: "/api/PortalResourceLibraryApi/GetFileByCate",
                type: "GET",
                data: { id: app.cateId },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                $("#grid_docfiles").data("kendoGrid").dataSource.data([]);


                let grid = $("#grid_docfiles").data("kendoGrid");

                for (var i = 0; i < response.length; i++) {

                    grid.dataSource.add({
                        Docs_Id: response[i].Docs_Id,
                        Docs_Title: response[i].Docs_Title
                        , Docs_Version: response[i].Docs_Version
                        

                    });

                }

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }
    }
})
app.GetCategory();