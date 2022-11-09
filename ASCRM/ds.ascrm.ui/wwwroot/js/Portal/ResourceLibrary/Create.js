
//var router = new VueRouter({
//    mode: 'history',
//    routes: []
//});
var app = new Vue({
    //router,
    el: '#dv_Resource',
    data: {
        Cat: {
            DocCat_Id:0,
            DocCat_Title: '',
            DocCat_Access:0
        },
        Files: {
            Docs_FileDate: '',
            Docs_Id: 0,
            Docs_Id_DocCategories: 0,
            Docs_Version:''
        },
        Categories:[],
        Topices: [],
        messageDetails: [],
        cateId: 0,

        firstMassage: {
            message: '',
            user_name: '',
            created_date: '',
            subject: '',
            topic: '',
        }
    },
    methods: {
   
        SubmitCat: function () {

           

           
          
            var data = app.Cat;
            $.ajax({
                url: "/api/PortalApi/AddUpdateCategory",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response == true) {
                    $("#kt_modal_view").modal("hide");
                    $('#grid_categroy').data('kendoGrid').dataSource.read();
                    $('#grid_categroy').data('kendoGrid').refresh();
                }
            })
        },
        addnewcat: function () {
            app.Cat.DocCat_Title = "";
            app.Cat.DocCat_Access = 0;
            app.DocCat_Id=0
            $("#kt_modal_view").modal("show");

        },
        addnewfile: function () {
            app.Files.Docs_FileDate = "";
            app.Files.Docs_Id = 0;
            app.Files.Docs_Version = "";
            app.Files.Docs_Id_DocCategories = 0;

         
            $("#kt_modal_Create").modal("show");

        },
        Edit: function (id) {

            $.ajax({
                url: "/api/PortalApi/GetCategoryById",
                data: { id:id},
                type: "Get",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
              
                    app.Cat = response
                    $("#kt_modal_view").modal("show");
                    //$('#grid_categroy').data('kendoGrid').dataSource.read();
                    //$('#grid_categroy').data('kendoGrid').refresh();
                
            })
        },
        LoadCat: function () {
            $.ajax({
                url: "/api/PortalApi/GetCategoryList",
                type: "Get",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                app.Categories = response
                //$('#grid_categroy').data('kendoGrid').dataSource.read();
                //$('#grid_categroy').data('kendoGrid').refresh();

            })
        },
        SubmitFile: function () {
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
            var data = this.Files;

            formData.append("documentData", JSON.stringify(data));

            $.ajax({
                url: "/api/PortalResourceLibraryApi/UploadDocumentsAjax",
                data: formData,
                type: "Post",
                contentType: false,
                processData: false
            }).done(function (response) {
                if (response === true) {
                    $('#grid_docfiles').data('kendoGrid').dataSource.read();
                    $('#grid_docfiles').data('kendoGrid').refresh();
                    toastr.success("Resource Added successfully.", "Success!");

               
                    $("#kt_modal_Create").modal("hide");

                }
            })
        },
        EditFile: function (id) {

            $.ajax({
                url: "/api/PortalResourceLibraryApi/GetResourseFileById",
                data: { id: id },
                type: "Get",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                app.Files = response
                $("#kt_modal_Create").modal("show");
                //$('#grid_categroy').data('kendoGrid').dataSource.read();
                //$('#grid_categroy').data('kendoGrid').refresh();

            })
        },
        LoadOnCate: function () {
            debugger;
            $.ajax({
                url: "/api/PortalApi/GetDocFilesById",
                type: "GET",
                data: { id: app.cateId },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);
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
function PerformEdit(id) {
    app.Edit(id);
}
function PerformEditFile(id) {
    app.EditFile(id);
}
app.LoadCat();