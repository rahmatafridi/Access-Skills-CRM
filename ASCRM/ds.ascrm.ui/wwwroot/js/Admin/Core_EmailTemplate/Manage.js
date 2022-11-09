Vue.use(vuelidate.default)

var app = new Vue({

    el: '#dv_Manage_EmailTemplates',
    data: {
        isSubmitted: false,
        buttonText: 'Add',
        TemplateTypes: '',
        EmailTemplates: [],
        EmailTemplate: { ET_Id: 0, ET_Title: null, ET_Body: null, ET_Code: null, ET_Subject: null, ET_Type: null },
        isValueExists: false,
        isKeyExists: false
    },
    validations: {
        EmailTemplate: {
            ET_Type: {
                required: validators.required
            },
            ET_Title: {
                required: validators.required
            },
            //ET_Body: {
            //    required: validators.required
            //}
            //,
            ET_Subject: {
                required: validators.required
            }
        }
    },
    methods: {
        GetEmailTemplateTypes: function () {
            $.ajax({
                url: "/api/ListApi/GetDropdownOptionsByHeaderName",
                type: "GET",
                contentType: "application/json",
                data: { headerName: "EmailTemplateTypes" },
                dataType: "json"
            }).done(function (response) {
                app.TemplateTypes = response;
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText);
            });
        },
        NewTemplate: function () {
            
            app.isSubmitted = false;
            app.buttonText = "Add";
            $('#editor').summernote('code', "");
            app.EmailTemplate = { ET_Id: 0, ET_Title: null, ET_Body: null, ET_Code: null, ET_Subject: null, ET_Type: null };
            if (!_canRoleManagerCreateTemplate) {
                toastr.error('You cannot add new template please contact administrator', "Permission denied!");
                return false;
            }
            $("#kt_modal_template").modal("show");
        },
        AddEditEmailTemplate: function () {
            
            this.isSubmitted = true;
            app.isKeyExists = false;
            app.isValueExists = false;

            if (this.$v.EmailTemplate.$invalid) {
                toastr.error('Error in validation.', "Error!");
                return;
            }
            var data = this.EmailTemplate;
            data.ET_Body = $('#editor').summernote('code');
            if (data.ET_Body === "") {
                toastr.error('Please insert template body.', "Error!");
                return false;
            }

            $.ajax({
                url: "/api/Core_EmailTemplateApi/CheckTitleAndCodeExists",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response.Item1 === false && response.Item2 === false) {
                    $.ajax({
                        url: "/api/Core_EmailTemplateApi/InsertUpdateEmailTemplate",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        if (response === true) {
                            app.isSubmitted = false;
                            $("#kt_modal_template").modal("hide");
                            if (app.EmailTemplate.ET_Id > 0) {
                                toastr.success("Template updated successfully.","Updated!");
                            } else {
                                toastr.success("Template added successfully.", "Inserted!");
                            }
                            app.CancelEdit();
                            $('#templateGrid').data('kendoGrid').dataSource.read();
                            $('#templateGrid').data('kendoGrid').refresh();
                        } else {
                            toastr.error("Record have not been saved. Please try again.", 'Error!');
                        }
                        }).fail(function (xhr) {
                            toastr.error(xhr.responseText, 'Error!');
                    });
                } else {
                    app.isKeyExists = response.Item1;
                    app.isValueExists = response.Item2;
                    toastr.error("Title or code already exists.", 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformTemplateEdit: function (et_Id) {
            if (!_canRoleManagerEditTemplate) {
                toastr.error('You cannot edit please contact administrator', 'Permission denied!');
                return false;
            }
            app.buttonText = "Update";
            $.ajax({
                url: "/api/Core_EmailTemplateApi/GetEmailTemplateById",
                data: { id: et_Id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.EmailTemplate = response;
                $('#editor').summernote('code', response.ET_Body);
                $("#kt_modal_template").modal("show");
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText,'Error!');
            });
        },
        PerformTemplateDelete: function (et_Id) {

            if (!_canRoleManagerDeleteTemplate) {
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
                if (!result.value) {
                    return;
                }
                $.ajax({
                    url: "/api/Core_EmailTemplateApi/DeleteEmailTemplateById",
                    data: { id: et_Id },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    if (response === true) {
                        $('#templateGrid').data('kendoGrid').dataSource.read();
                        $('#templateGrid').data('kendoGrid').refresh();
                        toastr.success('Email template deleted successfully.', 'Deleted');
                    } else {
                        toastr.error('Template cannot be deleted. Please try again.', 'Deleted');
                    }
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Deleted');
                });
            });
        },
        CancelEdit: function () {
            app.buttonText = "Add";
            app.isSubmitted = false;
            app.isKeyExists = false;
            app.isValueExists = false;
            app.EmailTemplate = { ET_Id: 0, ET_Title: null, ET_Body: null, ET_Code: null, ET_Subject: null, ET_Type: null };
        }
    }
});

function PerformTemplateEdit(et_Id) {
    app.PerformTemplateEdit(et_Id);
}
function PerformTemplateDelete(et_Id) {
    app.PerformTemplateDelete(et_Id);
}

app.GetEmailTemplateTypes();
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
        toastr.error(message, 'Error');
        $('#templateGrid').data('kendoGrid').dataSource.read();
        $('#templateGrid').data('kendoGrid').refresh();
    }
}
