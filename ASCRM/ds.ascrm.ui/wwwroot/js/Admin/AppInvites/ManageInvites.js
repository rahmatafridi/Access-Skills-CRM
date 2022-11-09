Vue.use(vuelidate.default)

var app = new Vue({

    el: '#dv_Manage_AppInvites',
    data: {
        isSubmitted: false,
        buttonText: 'Add',
        TemplateTypes: '',
        CourseLevels: [],
        AppInvite: { api_id: 0, api_id_courselevel: null, api_courseleveltitle: null, api_email: null, api_firstname: null, api_lastname: null, api_emailbody: null, api_password: null },
        htmlEmail: '',
        ModelForm: false,
        ModelViwe:false
    },
    validations: {
        AppInvite: {
            api_firstname: {
                required: validators.required
            },
            api_lastname: {
                required: validators.required
            },
            api_email: {
                required: validators.required
            },
            api_password: {
                required: validators.required
            },
            api_id_courselevel: {
                required: validators.required
            }
        }
    },
    methods: {

        
        GetCourseLevels: function () {
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
                url: "/api/ApplicationApi/GetAllCourseLevel",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.CourseLevels = response;
                console.log(app.CourseLevels);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GetRandom8DigitPassword: function () {
            $.ajax({
                url: "/api/ApplicationInviteApi/GetRandom8DigitPassword",
                type: "GET",
                contentType: "application/json",
                //dataType: "json"
            }).done(function (response) {
                app.AppInvite.api_password = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText);
            });
        },
        NewTemplate: function () {

            app.isSubmitted = false;
            app.buttonText = "Add";
            app.AppInvite = { api_id: 0, api_id_courselevel: null, api_courseleveltitle: null, api_email: null, api_firstname: null, api_lastname: null, api_emailbody: null, api_password: null };
            if (!_canRoleManagerCreateAppInvite) {
                toastr.error('You cannot add new invite please contact administrator', "Permission denied!");
                return false;
            }
            app.GetRandom8DigitPassword();
            $("#kt_modal_template").modal("show");
        },
        AddEditAppInvite: function () {

            this.isSubmitted = true;

            if (this.$v.AppInvite.$invalid) {
                toastr.error('Error in validation.', "Error!");
                return;
            }
            var data = this.AppInvite;
            data.api_courseleveltitle = $("#ddl_CourseLevel option:selected").text();

            $.ajax({
                url: "/api/UserAdminApi/CheckEmailAlreadyExists",
                data: JSON.stringify({ Users_ID: 0, Users_UserName: data.api_email }),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === false) {
                    app.isExists = false;
                    $.ajax({
                        url: "/api/ApplicationInviteApi/InsertUpdateApplicationInvite",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        if (response === true) {
                            app.isSubmitted = false;
                            $("#kt_modal_template").modal("hide");
                            $("#kt_modal_invite_view1").modal("hide");
                            
                            if (app.AppInvite.api_id > 0) {
                                toastr.success("Invite updated successfully.", "Updated!");
                            } else {
                                toastr.success("Invite added successfully.", "Inserted!");
                            }
                            app.CancelEdit();
                            $('#templateGrid').data('kendoGrid').dataSource.read();
                            $('#templateGrid').data('kendoGrid').refresh();
                            app.GetRandom8DigitPassword();
                        } else {
                            toastr.error("Record have not been saved. Please try again.", 'Error!');
                        }
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                } else {
                    app.isExists = true;
                    toastr.warning('Email already exists. Please change email.', 'Warning!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        EditAppInvite: function () {

            this.isSubmitted = true;

            if (this.$v.AppInvite.api_id_courselevel.$invalid) {
                toastr.error('Error in validation.', "Error!");
                return;
            }
            var data = this.AppInvite;
            data.api_courseleveltitle = $("#ddl_CourseLevel option:selected").text();

            app.isExists = false;
            $.ajax({
                url: "/api/ApplicationInviteApi/InsertUpdateApplicationInvite",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    app.isSubmitted = false;
                    $("#kt_modal_edit_course_level").modal("hide");
                    toastr.success("Invite updated successfully.", "Updated!");
                    app.CancelEdit();
                    $('#templateGrid').data('kendoGrid').dataSource.read();
                    $('#templateGrid').data('kendoGrid').refresh();
                } else {
                    toastr.error("Record have not been saved. Please try again.", 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformInviteView: function (aPI_Id) {
            $.ajax({
                url: "/api/ApplicationInviteApi/GetApplicationInviteById",
                data: { id: aPI_Id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.htmlEmail = response.api_emailbody;
                $("#kt_modal_invite_view").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        InviteView: function () {
            if (this.AppInvite.api_firstname == '') {
                toastr.error('Please Enter First Name', 'Error!');
                return;
            }
            if (this.AppInvite.api_lastname == '') {
                toastr.error('Please Enter Last Name', 'Error!');
                return;
            }

            if (this.AppInvite.api_id_courselevel == 0) {
                toastr.error('Please Select the Course ', 'Error!');
                return;
            }
            if (this.AppInvite.api_email == '') {
                toastr.error('Please Enter Email ', 'Error!');
                return;
            }
            if (this.AppInvite.api_password == '') {
                toastr.error('Please Enter Password ', 'Error!');
                return;
            }
            var name = this.AppInvite.api_firstname + " " + this.AppInvite.api_lastname;
            var pass = this.AppInvite.api_password;

            var valObj = this.CourseLevels.filter(function (elem) {
                if (elem.CL_Id == app.AppInvite.api_id_courselevel) return elem.CL_Id;
            });
            if (valObj.length > 0) {
                this.AppInvite.api_courseleveltitle = valObj[0].CL_Title;

            }

            $.ajax({
                url: "/api/ApplicationInviteApi/GetApplicationInviteTemplate",
                data: {
                    name: name,
                    password: pass,
                    coruselevel: this.AppInvite.api_courseleveltitle,
                    email: this.AppInvite.api_email
                },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                //console.log(response);
                //debugger;
                //app.htmlEmail = response;
                //$("#kt_modal_invite_view1").modal("show");
            }).fail(function (xhr) {
                debugger;
                app.htmlEmail = xhr.responseText;
                $("#kt_modal_invite_view1").modal("show");
                $("#kt_modal_template").modal("hide");
               // toastr.error(xhr.responseText, 'Error!');
            });
        },
        PreviousView: function () {
            $("#kt_modal_invite_view1").modal("hide");
            $("#kt_modal_template").modal("show");
           
       
        },
        PerformInviteResend: function (aPI_Id) {
            if (!_canRoleManagerResendAppInvite) {
                toastr.error('You cannot resend please contact administrator', 'Permission denied!');
                return false;
            }
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, resend it!'
            }).then((result) => {
                if (!result.value) {
                    return;
                }
                $.ajax({
                    url: "/api/ApplicationInviteApi/ResendApplicationInviteById",
                    data: { id: aPI_Id },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    if (response === true) {
                        toastr.success('Application invite resend successfully.', 'Resend');
                    } else {
                        toastr.error('Application invite cannot be resend. Please try again.', 'Resend');
                    }
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
            });
        },
        PerformInviteEdit: function (aPI_Id) {
            if (!_canRoleManagerEditAppInvite) {
                toastr.error('You cannot edit please contact administrator', 'Permission denied!');
                return false;
            }
            app.CancelEdit();

            $.ajax({
                url: "/api/ApplicationInviteApi/GetApplicationInviteById",
                data: { id: aPI_Id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.AppInvite.api_id = response.api_id;
                app.AppInvite.api_id_courselevel = response.api_id_courselevel;
                $("#kt_modal_edit_course_level").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformInviteDelete: function (aPI_Id) {

            if (!_canRoleManagerDeleteAppInvite) {
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
                    url: "/api/ApplicationInviteApi/DeleteApplicationInviteById",
                    data: { id: aPI_Id },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    if (response === true) {
                        $('#templateGrid').data('kendoGrid').dataSource.read();
                        $('#templateGrid').data('kendoGrid').refresh();
                        toastr.success('Application invite deleted successfully.', 'Deleted');
                    } else {
                        toastr.error('Application invite cannot be deleted. Please try again.', 'Deleted');
                    }
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Deleted');
                });
            });
        },
        CancelEdit: function () {
            app.buttonText = "Add";
            app.isSubmitted = false;
            app.AppInvite = { api_id: 0, api_id_courselevel: null, api_courseleveltitle: null, api_email: null, api_firstname: null, api_lastname: null, api_emailbody: null, api_password: null };
        }
    }
});

function PerformInviteEdit(aPI_Id) {
    app.PerformInviteEdit(aPI_Id);
}
function PerformInviteDelete(aPI_Id) {
    app.PerformInviteDelete(aPI_Id);
}
function PerformInviteResend(aPI_Id) {
    app.PerformInviteResend(aPI_Id);
}

function PerformInviteView(aPI_Id) {
    if (!_canRoleManagerViewAppInvite) {
        toastr.error('You cannot view please contact administrator', 'Permission denied!');
        return false;
    }
    app.PerformInviteView(aPI_Id);
}

app.GetCourseLevels();
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
