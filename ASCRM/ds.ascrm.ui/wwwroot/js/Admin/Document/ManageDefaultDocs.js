Vue.use(vuelidate.default)

var app = new Vue({

    el: '#dv_Manage_DefaultDocuments',
    data: {
        isDocumentSubmitted: false,
        buttonText: 'Add',
        MaxDefaultFileSize: 0,
        isValueExists: false,
        isKeyExists: false,
        Document: { DefaultDoc_Id: 0, DefaultDoc_Name: '', DefaultDoc_Description: '', DefaultDoc_FilePath: '', DefaultDoc_Id_Category: null },
        DocumentCategories: {}
    },
    validations: {
        Document: {
            DefaultDoc_Name: {
                required: validators.required
            },
            DefaultDoc_Id_Category: {
                required: validators.required
            },
            DefaultDoc_Description: {
                required: validators.required
            }
        }
    },
    methods: {
        GetDefaultDocumentCategories: function () {
            $.ajax({
                url: "/api/ListApi/GetDropdownOptionsByHeaderName",
                type: "GET",
                contentType: "application/json",
                data: { headerName: "DefaultDocCategories" },
                dataType: "json",
            }).done(function (response) {
                app.DocumentCategories = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        GetDefaultDocumentMaxSize: function () {
            $.ajax({
                url: "/api/CoreConfigurationApi/GetSingleConfigValueByConfigKey",
                type: "GET",
                contentType: "application/json",
                data: { config_key: "MaxDefaultFileSize" },
                dataType: "json"
            }).done(function (response) {
                app.MaxDefaultFileSize = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        NewDocument: function () {

            app.isDocumentSubmitted = false;
            app.Document = { DefaultDoc_Id: 0, DefaultDoc_Id_Lead: 0, DefaultDoc_Name: '', DefaultDoc_Description: '', DefaultDoc_FilePath: '' };            
            $('label[id*="lbl_docName"]').text('');
            $("#inputDocument").val("");
            if (!_canRoleManagerCreateDocument) {
                toastr.error('You cannot add new document please contact administrator', 'Permission denied!');
                return false;
            }
            var upload = $("#files").data("kendoUpload");
            upload.clearAllFiles();
            $("#kt_modal_document").modal("show");
        },
        submitDocument: function () {
            if (!_canRoleManagerCreateDocument)
            {
                toastr.error('You cannot add new document please contact administrator', 'Permission denied!');
                return false;
            }
            this.isDocumentSubmitted = true;
            if (this.$v.Document.DefaultDoc_Name.$invalid) {
                toastr.error('Error in validation', 'Error!');
                return;
            }

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

            $.ajax({
                url: "/api/DocumentApi/UploadDefaultDocumentsAjax",
                data: formData,
                type: "Post",
                contentType: false,
                processData: false,
            }).done(function (response) {
                if (response == true) {
                    app.isDocumentSubmitted = false;
                    app.Document = { DefaultDoc_Id: 0, DefaultDoc_Id_Lead: 0, DefaultDoc_Name: '', DefaultDoc_Description: '', DefaultDoc_FilePath: '' };
                    $("#kt_modal_document").modal("hide");
                    $('label[id*="lbl_docName"]').text('');
                    $("#inputDocument").val("");
                    $('#DocumentsGrid').data('kendoGrid').dataSource.read();
                    $('#DocumentsGrid').data('kendoGrid').refresh();
                    toastr.success("Document uploaded successfully.", "Success!");
                } else {
                    toastr.success('Document cannot uploaded. Please try again.', "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformDocumentDelete: function (id) {
            $.ajax({
                url: "/api/DocumentApi/DeleteDocumentById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#DocumentsGrid').data('kendoGrid').dataSource.read();
                    $('#DocumentsGrid').data('kendoGrid').refresh();
                    toastr.success("Document deleted successfully.", "Success!");
                } else {
                    toastr.success("Document cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformDocumentDownload: function (filePath) {
            if (!_canRoleManagerDownloadDocument) {
                toastr.error('You cannot download please contact administrator', 'Permission denied!');
                return false;
            }
            $.ajax({
                url: "/api/DocumentApi/DownloadFile",
                data: { filename: filePath },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                   
                } else {
                    toastr.error('Document cannot be deleted. Please try again.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }
    }
});


function error_handler(e) {
    if (e.errors) {
        var message = "Errors:\n";
        $.each(e.errors, function (key, value) {
            if ('errors' in value) {
                $.each(value.errors, function () {
                    message += this + "\n";
                });
            }
        });
        toastr.error(message, 'Error!');
        $('#notesGrid').data('kendoGrid').dataSource.read();
        $('#notesGrid').data('kendoGrid').refresh();
    }
}
function PerformDocumentDelete(document_Id) {
    if (!_canRoleManagerDeleteDocument) {
        toastr.error('You cannot delete please contact administrator', 'Permission denied!');
        return false;
    }
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            app.PerformDocumentDelete(document_Id);
        }
    });
}
function PerformDocumentDownload(filePath) {
    app.PerformDocumentDownload(filePath);
}
app.GetDefaultDocumentCategories();
app.GetDefaultDocumentMaxSize();