Vue.use(vuelidate.default)

var app = new Vue({

    el: '#dv_Manage_LearnerWork',
    data: {
        isSubmitted: false,
        buttonText: 'Add',
        app_id: 0,
        id: 0,
        learnerid:0,
        rejected_reason: '',
        deleted_reason: '',
        readytoenroll_comment: '',
        CourseLevels: [],
        app_officeuse1_courselevel: 0,
        Attechments: [],
        Acceccers: [],
        assesserId: 0,
        Assesser: {
            id: 0,
            assessor_assigned_user_id: 0,
            rejected_reason: '',
            due_date: '',
            note: '',
            is_rejected: 1,
            is_admin_approved:1,
            assessor_name: '',
            learner_id: 0,
            traningAssessment: {
                tap_content_activty: '',
                tap_core_skil: '',
                tap_referencing: '',
                tap_development: '',
                tap_assessment_criteria: '',
                tap_resubmission: '',
                tap_assessor_signature: '',
                tab_assessor_sign: '',
                id: 0,
                tap_admin_signature:''

            }
        },
         dueDate: '',
        notes: '',
        files: [],
        ruta: '',
        fileHistory:[]
    },
    methods: {
        loadAccesser: function () {
            $.ajax({
                url: "/api/WorkUploadApi/GetAccesser",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Acceccers = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        assesserChange: function (e) {
            var ids = e.target.value;
            app.assesserId = ids;
        },
        assignAccesser: function () {
            debugger
            this.Assesser.learner_id = app.learnerid;

            var workid = parseInt(app.id);
            var assessid = app.assesserId
            this.Assesser.id = workid;
            this.Assesser.assessor_assigned_user_id = assessid;
            this.Assesser.note = app.notes;
            const s = app.dueDate;
            const d = new Date(s);

            this.Assesser.due_date = app.dueDate;

            //this.Assesser.due_date =  app.dueDate;
            var data = this.Assesser;

            $.ajax({
                url: "/api/WorkUploadApi/AssignToAssesser",
                data: JSON.stringify(data),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                toastr.success("Assign successfully.", "Success!");

                $("#kt_modal_assign").modal("hide");
                $('#grid_submitted').data('kendoGrid').dataSource.read();
                $('#grid_submitted').data('kendoGrid').refresh();

                //$('#grid_rejected').data('kendoGrid').dataSource.read();
                //$('#grid_rejected').data('kendoGrid').refresh();

                $('#grid_readytochecked').data('kendoGrid').dataSource.read();
                $('#grid_readytochecked').data('kendoGrid').refresh();

                $('#grid_approved').data('kendoGrid').dataSource.read();
                $('#grid_approved').data('kendoGrid').refresh();

                $('#grid_validated').data('kendoGrid').dataSource.read();
                $('#grid_validated').data('kendoGrid').refresh();

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        workReject: function () {
           
            var workid = parseInt(app.id);
            this.Assesser.id = workid;
            this.Assesser.rejected_reason = app.rejected_reason;
            this.Assesser.is_rejected = 1;
            var data = this.Assesser;

            $.ajax({
                url: "/api/WorkUploadApi/Reject",
                data: JSON.stringify(data),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#kt_modal_reject_reason").modal("hide");

                $('#grid_submitted').data('kendoGrid').dataSource.read();
                $('#grid_submitted').data('kendoGrid').refresh();

                //$('#grid_rejected').data('kendoGrid').dataSource.read();
                //$('#grid_rejected').data('kendoGrid').refresh();

                $('#grid_readytochecked').data('kendoGrid').dataSource.read();
                $('#grid_readytochecked').data('kendoGrid').refresh();

                $('#grid_approved').data('kendoGrid').dataSource.read();
                $('#grid_approved').data('kendoGrid').refresh();

                $('#grid_validated').data('kendoGrid').dataSource.read();
                $('#grid_validated').data('kendoGrid').refresh();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        approve: function () {
            var workid = app.id;
            this.Assesser.id = workid;
            this.Assesser.note = app.notes;
            this.Assesser.is_admin_approved = 1;
            var data = this.Assesser;

            $.ajax({
                url: "/api/WorkUploadApi/Approve",
                data: JSON.stringify(data),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                /* $("#kt_modal_reject_reason").modal("hide");*/
                toastr.success("Approved successfully.", "Approved!");

                $("#kt_modal_approve").modal("hide");
                $('#grid_submitted').data('kendoGrid').dataSource.read();
                $('#grid_submitted').data('kendoGrid').refresh();

                //$('#grid_rejected').data('kendoGrid').dataSource.read();
                //$('#grid_rejected').data('kendoGrid').refresh();

                $('#grid_readytochecked').data('kendoGrid').dataSource.read();
                $('#grid_readytochecked').data('kendoGrid').refresh();


                $('#grid_approved').data('kendoGrid').dataSource.read();
                $('#grid_approved').data('kendoGrid').refresh();

                $('#grid_validated').data('kendoGrid').dataSource.read();
                $('#grid_validated').data('kendoGrid').refresh();

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        loadFiles: function (id) {
            $.ajax({
                url: "/api/WorkUploadApi/GetWorkFiles",
                type: "GET",
                data: {
                    work_upload_id:id
                },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.files = response;
                $("#kt_modal_files").modal("show");

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        downlaodFile: function (path) {
            debugger;
            window.location = '/Document/DownloadLearnerUploadFile?filePath=' + path;
        },
        deleteFile: function (id) {
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
                    app.Delete(id);
                }
            });
        },
        Delete: function (id) {

            $.ajax({
                url: "/api/PortalApi/DeleteWorkUploadById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                toastr.success("Deleted successfully.", "Success!");
                $("#kt_modal_files").modal("hide");

                //if (response === true) {
                //    $('#gridCourseContent').data('kendoGrid').dataSource.read();
                //    $('#gridCourseContent').data('kendoGrid').refresh();
                //    toastr.success("Course Content has been deleted successfully.", "Success!");
                //} else {
                //    toastr.error("Course Content cannot be deleted. Please try again.", "Error!");
                //}
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        approveWordEdit(learner_id, topic_id) {
            $.ajax({
                url: "/api/WorkUploadApi/LoadTainingAssessment?learner_id=" + learner_id+"&topic_id="+topic_id,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);
                app.Assesser.traningAssessment = response.traningAssessment;
                if (app.Assesser.traningAssessment.tap_resubmission != "") {
                    $("#assessmentcheck2").prop("checked", true);
                    $("#assessmentcheck1").prop("checked", false);
                    if (app.Assesser.traningAssessment.tap_resubmission == "7") {
                        $("#resubmission1").prop("checked", true);
                        $("#resubmission2").prop("checked", false);

                    }
                    if (app.Assesser.traningAssessment.tap_resubmission == "14") {
                        $("#resubmission2").prop("checked", true);
                        $("#resubmission1").prop("checked", false);
                    }
                    $("#resubmissiondata").show();


                }
                $("#kt_modal_approve1").modal("show");

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        assessmentchange: function (event) {
            var data = event.target.value;
            if (data == 'no') {
                app.isResubmission = true;
                $("#resubmissiondata").show();
            }
            if (data == 'yes') {
                $("#resubmissiondata").hide();
                app.isResubmission = false;

            }

        },
        Sign: function (id) {
            app.tabs.TAP_Id = id;
            app.tabs.learnerComment = '';
            $("#printName").val('');
            $("#myModal").modal("show");

        },
        txtPrintName: function (e) {
            var value = e.target.value;
            this.printName = e.target.value;
            console.log(this.printName);
            var stt = value;
            var lng = stt.length;
            var fnt = 30;
            //max 20!
            if (lng < 20) { fnt = 30; }
            else if (lng < 30) { fnt = 20; }
            else { fnt = 15; }

            var canvas = document.getElementById('signaturepad');
            var context = canvas.getContext('2d');
            context.clearRect(0, 0, canvas.width, canvas.height);
            var fntsize = fnt + 'pt';
            context.font = 'italic ' + fntsize + ' Sacramento, cursive ';
            context.fillText(stt, 50, 60);
        },
        approveUpdate: function () {
            var workid = app.id;
            this.Assesser.id = workid;
            this.Assesser.is_assessor_marking_valid = 1;
            this.Assesser.note = app.notes;
            if (app.isResubmission == true) {

            }


            var sing = $("#printName").val();
            if (sing == "") {
                toastr.error("Please Enter the Sign", 'Error!');
                return;
            }
            var canvas = document.getElementById('signaturepad');
            var context = canvas.getContext('2d');
            var dataUri = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");  // here is the most important part because if you dont replace you will get a DOM 18 exception.

            $("#sketch_data").val(dataUri);
            $("#sketch_data").val(dataUri);

            this.Assesser.traningAssessment.tap_assessor_signature = dataUri;
            this.Assesser.traningAssessment.tap_assessor_sign = sing;


            var data = this.Assesser;

            $.ajax({
                url: "/api/WorkUploadApi/ApproveUpdate",
                data: JSON.stringify(data),
                type: "POST",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#kt_modal_approve").modal("hide");
                toastr.success("Approved successfully.", "Approved!");
                $("#kt_modal_approve1").modal("hide");

                /* $("#kt_modal_reject_reason").modal("hide");*/
                $('#grid_submitted').data('kendoGrid').dataSource.read();
                $('#grid_submitted').data('kendoGrid').refresh();

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        WorkView: function (learner_id, topic_id) {
            $.ajax({
                url: "/api/WorkUploadApi/LoadFileData?learner_id=" + learner_id + "&topic_id=" + topic_id,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.ruta = response;
                var sampleArr = app.base64ToArrayBuffer(response);
                app.saveByteArray(learner_id, sampleArr);
                // $("#kt_modal_fileView").show();
              ///  $("#kt_modal_fileView").modal("show");

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        base64ToArrayBuffer: function(base64) {
            var binaryString = window.atob(base64);
            var binaryLen = binaryString.length;
            var bytes = new Uint8Array(binaryLen);
            for (var i = 0; i < binaryLen; i++) {
                var ascii = binaryString.charCodeAt(i);
                bytes[i] = ascii;
            }
            return bytes;
        },
        saveByteArray:function(reportName, byte) {
            var blob = new Blob([byte], {
                type: "application/pdf" });
            //var link = document.createElement('a');
            //link.href = window.URL.createObjectURL(blob);
            //const data = window.URL.createObjectURL(blob);

            var fileName = reportName;
            //link.download = fileName;
            //link.click();
            //window.URL.revokeObjectURL(data);
            //link.remove();
            var file = new Blob([byte], { type: 'application/pdf' });
            var fileURL = URL.createObjectURL(file);
            window.open(fileURL);

            //const data = window.URL.createObjectURL(blob);
            //const link = document.createElement('a');
            //document.body.appendChild(link);
            //link.href = data;
            //link.download = `${fileName}.pdf`;
            //link.click();
            //window.URL.revokeObjectURL(data);
            //link.remove();
        },
        SendFileToLearner: function (learner_id, topic_id) {
            $.ajax({
                url: "/api/WorkUploadApi/SendFileToLearner?learner_id=" + learner_id + "&topic_id=" + topic_id,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                toastr.success("File Sended", "Success");
                /*app.ruta = response;*/
                //var sampleArr = app.base64ToArrayBuffer(response);
                //app.saveByteArray(learner_id, sampleArr);
                // $("#kt_modal_fileView").show();
                ///  $("#kt_modal_fileView").modal("show");

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        ViewHistory: function (learner_id, topic_id) {
            $.ajax({
                url: "/api/WorkUploadApi/GetHistory?learner_id=" + learner_id + "&topic_id=" + topic_id,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                app.fileHistory = response;
                $("#kt_modal_history").modal("show");


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });


        },
        DownloadViewHistory: function (data, learner_id) {
            debugger;
            var sampleArr = app.base64ToArrayBuffer(data);
            app.saveByteArray(learner_id, sampleArr);
        },
        formatDateEN: function (date) {
    const options = { month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' }
            return new Date(date).toLocaleDateString('en-GB')
}

    },
    mounted: function () {
        
        $('#completeDate').datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
            todayHighlight: true
        }).on('changeDate', () => { app.dueDate = $('#completeDate').val(); });

    }

})
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
function PerformAssign(id, learner_id) {
    if (!_canRoleManagerWorkSubmittedAssign) {
        toastr.error('You cannot Assign please contact administrator', 'Permission denied!');
        return;
    }
    else {
        app.loadAccesser();
        app.id = id;
        app.learnerid = learner_id;
        $("#kt_modal_assign").modal("show");
        }
        
   
}
function PerformReject(id) {
    if (!_canRoleManagerWorkSubmittedReject) {
        toastr.error('You cannot Reject please contact administrator', 'Permission denied!');
        return;
    } else {
        app.id = id;
        app.rejected_reason = "";
        $("#kt_modal_reject_reason").modal("show");
    }
}
function DownlaodFile(id) {
    
}
function PerformApprove(id) {
    if (!_canRoleManagerWorkSubmittedApprove) {
        toastr.error('You cannot Approve please contact administrator', 'Permission denied!');
        return;
    } else {
        app.notes = "";

        app.id = id;
        $("#kt_modal_approve").modal("show");
    }
}
function PerformFiles(id) {
    app.loadFiles(id);
}
function PerformDelete(id) {
    if (!_canRoleManagerWorkSubmittedDelete) {
        toastr.error('You cannot delete please contact administrator', 'Permission denied!');
        return;
    } else {
        app.Delete(id);
    }
}

function ApproveFileEdit(learner_id, topic_id, id) {
    app.id = id;
    app.approveWordEdit(learner_id, topic_id);
}
function ApproveFileView(learner_id, topic_id) {
    app.WorkView(learner_id, topic_id);
}
function HistoryFileView(learner_id, topic_id) {
    app.ViewHistory(learner_id, topic_id);
}
function SendFile(learner_id, topic_id) {
    app.SendFileToLearner(learner_id, topic_id);
}