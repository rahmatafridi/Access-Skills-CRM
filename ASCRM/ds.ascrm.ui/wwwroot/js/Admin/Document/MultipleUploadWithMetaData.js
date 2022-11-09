Vue.use(vuelidate.default)

var app = new Vue({

    el: '#dv_MultipleUpload',
    data: {
        isDocumentSubmitted: false,
        buttonText: 'Add',
        MaxDefaultFileSize: 0,
        isValueExists: false,
        isKeyExists: false,
        Document: { DefaultDoc_Id: 0, DefaultDoc_Name: '', DefaultDoc_Description: '', DefaultDoc_FilePath: '', DefaultDoc_Id_Category: null },
        DocumentCategories: {},
        DocumentsList: []
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
        AddDocument: function () {

            var fileUpload = $("#FileUpload1").get(0);

            if (fileUpload.files.length == 0) {
                return;
            }
            var file = fileUpload.files[0];

            var date = new Date();
            var timestamp = date.getTime();

            app.Document.DefaultDoc_Name = timestamp;
            app.Document.File = file;

            var reader = new FileReader();
            reader.readAsDataURL(file);

            reader.onload = function () {
                //console.log(reader.result);//base64encoded string
                app.Document.DefaultDoc_FilePath = reader.result;
                app.DocumentsList.push(app.Document);
                app.Document = { DefaultDoc_Id: 0, DefaultDoc_Id_Lead: 0, DefaultDoc_Name: '', DefaultDoc_Description: '', DefaultDoc_FilePath: '' };
            };



        },
        SubmitDocument: function () {
            var formData = new FormData();

            $.each(this.DocumentsList, function (k, v) {
                formData.append(v.DefaultDoc_Name, v.File);
            });


            var data = this.DocumentsList;
            formData.append("documentData", JSON.stringify(data));

            $.ajax({
                url: "/api/DocumentApi/UploadMultipleDocumentsAjax",
                data: formData,
                type: "Post",
                contentType: false,
                processData: false,
            }).done(function (response) {
                if (response == true) {
                    app.isDocumentSubmitted = false;
                    toastr.success("Document uploaded successfully.", "Success!");
                } else {
                    toastr.success('Document cannot uploaded. Please try again.', "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        SubmitDocumentBase64: function () {
            var formData = new FormData();
            var fileUpload = $("#FileUpload1").get(0);


            $.each(app.DocumentsList, function (k, v) {
                //var reader = new FileReader();
                //formData.append(v.DefaultDoc_Name, v.File);                               
                //reader.readAsDataURL(fileUpload.files[0]);
                //reader.readAsDataURL(v.File);

                //v.document_object = await toBase64(v.File);

                //v.document_object = reader.result;
                v.DefaultDoc_Name = v.File.name;

                //reader.addEventListener("load", function () {
                //    // convert image file to base64 string
                //    v.DefaultDoc_Name = reader.result;
                //}, false);

                //if (v.File) {
                //    reader.readAsDataURL(v.File);
                //}
            });


            var data = app.DocumentsList;
            formData.append("documentData", JSON.stringify(data));

            $.ajax({
                url: "/api/DocumentApi/UploadMultipleDocumentsBase64",
                //data: formData,
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response == true) {
                    app.isDocumentSubmitted = false;
                    toastr.success("Document uploaded successfully.", "Success!");
                } else {
                    toastr.success('Document cannot uploaded. Please try again.', "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }
    }
});

app.GetDefaultDocumentCategories();
app.GetDefaultDocumentMaxSize();

function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

const toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
});