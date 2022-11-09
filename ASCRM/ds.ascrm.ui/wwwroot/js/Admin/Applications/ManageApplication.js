Vue.use(vuelidate.default)

var app = new Vue({

    el: '#dv_Manage_Applications',
    data: {
        isSubmitted: false,
        buttonText: 'Add',
        app_id: 0,
        rejected_reason: '',
        deleted_reason: '',
        readytoenroll_comment: '',
        CourseLevels: [],
        app_officeuse1_courselevel: 0,
        Attechments:[]

    },
    validations: {
        rejected_reason: { required: validators.required },
        deleted_reason: { required: validators.required },
        readytoenroll_comment: { required: validators.required },
        EmailTemplate: {
            //ET_Body: {
            //    required: validators.required
            //}
            //,
        }
    },
    methods: {
        Populate: function () {
            //$.ajax({
            //    url: "/api/ListApi/GetDropdownOptionsByHeaderName",
            //    type: "GET",
            //    contentType: "application/json",
            //    data: { headerName: "CourseLevels" },
            //    dataType: "json",
            //}).done(function (response) {
            //    app.CourseLevels = response;
            //}).fail(function (xhr) {
            //    toastr.error(xhr.responseText, 'Error!');
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
        AddNewApplication: function () {

            
            if (this.app_officeuse1_courselevel == 0) {
            	toastr.error('Please select course level first.', 'Course Level!');
            	return false;
            }

            $.ajax({
                url: "/api/ApplicationApi/InsertNewApplication",
                data: { id: this.app_officeuse1_courselevel},
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.isSubmitted = false;
                if (response.AppUser_App_Id > 0) {
                    location.href = '/Application/ProgressAdmin?id=' + response.encoded_app_id;
                } else {
                    toastr.error('Application have not been created. Please try again.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformApplicationEdit: function (et_Id) {
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
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformReadyToEnroll: function () {
            this.isSubmitted = true;

            if (this.$v.readytoenroll_comment.$invalid) {
                toastr.error('Error in validation.', "Error!");
                return;
            }
            var reasonModel = { app_id: app.app_id, reason: app.readytoenroll_comment };
            $.ajax({
                url: "/api/ApplicationApi/ReadyToEnrollApplicationWithReason",
                data: JSON.stringify(reasonModel),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    app.isSubmitted = false;
                    $('#grid_submitted').data('kendoGrid').dataSource.read();
                    $('#grid_submitted').data('kendoGrid').refresh();
                    $("#kt_modal_ready_to_enroll").modal("hide");
                    toastr.success('Application status changed successfully.', 'Ready To Enroll');
                } else {
                    toastr.error('Application status cannot be changed. Please try again.', 'Ready To Enroll');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Ready To Enroll');
            });
        },
        PerformReject: function () {
            this.isSubmitted = true;

            if (this.$v.rejected_reason.$invalid) {
                toastr.error('Error in validation.', "Error!");
                return;
            }
            var reasonModel = { app_id: app.app_id, reason: app.rejected_reason };
            $.ajax({
                url: "/api/ApplicationApi/RejectApplicationWithReason",
                data: JSON.stringify(reasonModel),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    app.isSubmitted = false;
                    $('#grid_submitted').data('kendoGrid').dataSource.read();
                    $('#grid_submitted').data('kendoGrid').refresh();
                    $("#kt_modal_reject_reason").modal("hide");
                    toastr.success('Application rejected successfully.', 'Application Reject');
                } else {
                    toastr.error('Application cannot be rejected. Please try again.', 'Application Reject');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Application Reject');
            });
        },
        PerformApplicationDelete: function () {
            this.isSubmitted = true;

            if (this.$v.deleted_reason.$invalid) {
                toastr.error('Error in validation.', "Error!");
                return;
            }
            var reasonModel = { app_id: app.app_id, reason: app.deleted_reason };
            $.ajax({
                url: "/api/ApplicationApi/DeleteApplicationWithReason",
                data: JSON.stringify(reasonModel),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    app.isSubmitted = false;
                    $('#grid_submitted').data('kendoGrid').dataSource.read();
                    $('#grid_submitted').data('kendoGrid').refresh();
                    $("#kt_modal_delete_reason").modal("hide");
                    toastr.success('Application deleted successfully.', 'Application Delete');
                } else {
                    toastr.error('Application cannot be deleted. Please try again.', 'Application Delete');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Application Delete');
            });
        },
        PerformNotifyChanges: function (app_id) {
            this.isSubmitted = true;
            var changesModal = { app_firstname: applicationName, app_email: email, app_id: app_id };
            $.ajax({
                url: "/api/ApplicationApi/PerformNotifyChanges",
                data: { appid: app_id},
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    app.isSubmitted = false;
                    $('#grid_submitted').data('kendoGrid').dataSource.read();
                    $('#grid_submitted').data('kendoGrid').refresh();
                    $("#kt_modal_delete_reason").modal("hide");
                    toastr.success('Application changes sent successfully.', 'Notify Changes');
                } else {
                    toastr.error('Application changes cannot be sent. Please try again.', 'Notify Changes');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Notify Changes');
            });
        },
        RefreshSubmitted: function () {
            $('#grid_submitted').data('kendoGrid').dataSource.read();
            $('#grid_submitted').data('kendoGrid').refresh();
        },
        RefreshNotSubmitted: function () {
            $('#grid_notsubmitted').data('kendoGrid').dataSource.read();
            $('#grid_notsubmitted').data('kendoGrid').refresh();
        },
        RefreshRejected: function () {
            $('#grid_rejected').data('kendoGrid').dataSource.read();
            $('#grid_rejected').data('kendoGrid').refresh();
        },
        RefreshDeleted: function () {
            $('#grid_deleted').data('kendoGrid').dataSource.read();
            $('#grid_deleted').data('kendoGrid').refresh();
        },
        RefreshEnrolled: function () {
            $('#grid_enrolled').data('kendoGrid').dataSource.read();
            $('#grid_enrolled').data('kendoGrid').refresh();
        },
        DownloadAttechment: function (id) {
            var root = this;
            $.ajax({
                url: "/api/ApplicationApi/GetApplicationAttachment",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);
                if (response.length > 0) {
                    app.Attechments = response;
                    app.Attechments.forEach((value, index) => {
                        //var data = this.fileData;
                        var sampleArr = root.base64ToArrayBuffer(value.bin_answer);
                        root.saveByteArray(value.q_title, sampleArr, value.q_file_extension);
                    });

                } else {
                    toastr.error("Attechment Not Available", 'Error!');
                    return;
                }
               
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        
        base64ToArrayBuffer(base64) {
            var binaryString = window.atob(base64);
            var binaryLen = binaryString.length;
            var bytes = new Uint8Array(binaryLen);
            for (var i = 0; i < binaryLen; i++) {
                var ascii = binaryString.charCodeAt(i);
                bytes[i] = ascii;
            }
            return bytes;
        },
        saveByteArray(reportName, byte,ext) {
            var type = "";
            if (ext == "pdf") {
                type = "application/pdf";
            }
            if (ext == "docx") {
                type = "application/msword"
            }
            if (ext == "doc") {
                type = "application/msword"
            }
            if (ext == "xls") {
                type = "application/vnd.ms-excel"
            }
            if (ext == "xlsx") {
                type = "application/vnd.ms-excel"
            }

            if (ext == "ppt") {
                type = "application/vnd.ms-powerpoint"
            }

            if (ext == "JPEG") {
                type = "image/jpeg"
            }
            if (ext== "jpeg") {
                type = "image/jpeg"
            }
            if (ext == "jpg") {
                type = "image/jpeg"
            }
            if (ext == "png") {
                type = "image/png"
            }

            var blob = new Blob([byte], { type: type });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            var fileName = reportName;
            link.download = fileName;
            link.click();
        },
    }
});

function PerformApplicationEdit(encoded_app_id) {
    window.location = "/Application/ProgressAdmin?id=" + encoded_app_id;
}

function PerformDownloadAttach(id) {
//    //window.location = "/Application/Print?id=" + encoded_app_id;
    ////    window.open("/Application/Print?id=" + encoded_app_id, '_blank');
    app.DownloadAttechment(id)
}
function PerformPrint(encoded_app_id) {
    //window.location = "/Application/Print?id=" + encoded_app_id;
    window.open("/Application/Print?id=" + encoded_app_id, '_blank');
}
function PerformReadyToEnroll(AppUser_App_Id) {
    if (!_canEnrApp) {
        toastr.error('You cannot Enrolled please contact administrator', 'Permission denied!');
        return false;
    }
    app.readytoenroll_comment = "";
    app.app_id = AppUser_App_Id;
    $("#kt_modal_ready_to_enroll").modal("show");
}
function PerformNotifyChanges(applicationName, email, app_id) {
    app.PerformNotifyChanges(applicationName, email, app_id);
}
function PerformReject(AppUser_App_Id) {
    if (!_canRejApp) {
        toastr.error('You cannot Reject please contact administrator', 'Permission denied!');
        return false;
    }
    app.rejected_reason = "";
    app.app_id = AppUser_App_Id;
    $("#kt_modal_reject_reason").modal("show");
}
function PerformApplicationDelete(AppUser_App_Id) {
    if (!_canDeleteApp) {
        toastr.error('You cannot Delete please contact administrator', 'Permission denied!');
        return false;
    }
    app.deleted_reason = "";
    app.app_id = AppUser_App_Id;
    $("#kt_modal_delete_reason").modal("show");
}

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
        $('#grid_submitted').data('kendoGrid').dataSource.read();
        $('#grid_submitted').data('kendoGrid').refresh();
    }
}
app.Populate();