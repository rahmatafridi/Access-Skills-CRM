
Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_MyCPD',
    data: {
        LearnerStartDate: '',
        PracticalEndDate: '',
        HoursWorkedperWeek: '',
        LearnerStartDate: '',
        DailyHours: '',
        Min20OTJHours: '',
        ActualOTJHours: '',
        RemainingOTJHours:'',
        Completed: [],
        completedActivity: {
            Actual_OTJ: '',
            PId:''
        },
        additionalActivity: {
            PId: 0,
            Completed_Date: '',
            Activity_Title: '',
            Description: '',
            Actual_OTJ: '',
            Is_Locked:''
        },

        CompletedActivities:[],
        AdditionalActivities: [],
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
                url: "/api/PortalMyCPDApi/CompletedActivitiesList",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.CompletedActivities = response;
            })

            $.ajax({
                url: "/api/PortalMyCPDApi/AdditionalActivitiesList",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.AdditionalActivities = response;
            })


            $.ajax({
                url: "/api/PortalMyCPDApi/GetReviewStats",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.LearnerStartDate = response.date_start;
                app.PracticalEndDate = response.date_practical_end;
                app.HoursWorkedperWeek = response.weekly_hrs;
                app.DailyHours = response.daily_hrs;
                app.Min20OTJHours = response.min_20_otj_hrs;
                app.ActualOTJHours = response.actual_otj_hrs;
                app.RemainingOTJHours = response.remaining_otj_hrs;

                console.log(response);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        SubmitCompletedActivity: function () {
            if (app.completedActivity.Actual_OTJ == "") {
                toastr.error('Please Enter Value. Please try again.', 'Error!');

                return;
            }
            debugger;
            app.completedActivity.PId = parseInt(app.completedActivity.PId);
            var data = app.completedActivity;
            $.ajax({
                url: "/api/PortalMyCPDApi/UpdateCompletedActivity",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                if (response === true) {
                    app.completedActivity.Actual_OTJ = '';
                    app.completedActivity.PId = '';
                    toastr.success("Submitted successfully.", "Success!");

                    $("#kt_modal_Completed_Activities").modal("hide");
                    $('#gridCompletedActivities').data('kendoGrid').dataSource.read();
                    $('#gridCompletedActivities').data('kendoGrid').refresh();

                } else {
                    toastr.error('Record have not been saved. Please try again.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        Edit: function (id) {
            var data = app.CompletedActivities.filter(function (a) {
                if (a.PId == id) return a.PId;
            });
            app.completedActivity.Actual_OTJ = data[0].Actual_OTJ;
            app.completedActivity.PId = data[0].PId;
            $("#kt_modal_Completed_Activities").modal("show");

        },
        onlyNumber($event) {
            //console.log($event.keyCode); //keyCodes value
            let keyCode = ($event.keyCode ? $event.keyCode : $event.which);
            if ((keyCode < 48 || keyCode > 57) && keyCode !== 46) { // 46 is dot
                $event.preventDefault();
            }
        },
        addAdditional: function (id) {
            if (id != 0) {
                var data = app.AdditionalActivities.filter(function (a) {
                    if (a.PId == id) return a.PId;
                });
                app.additionalActivity.PId = data[0].PId;
                app.additionalActivity.Completed_Date = data[0].Completed_Date;
                app.additionalActivity.Activity_Title = data[0].Activity_Title;
                app.additionalActivity.Description = data[0].Description;
                app.additionalActivity.Actual_OTJ = data[0].Actual_OTJ;
              //  app.additionalActivity.Actual_OTJ = data[0].Actual_OTJ;

            }

            $("#kt_modal_Additinal_Activities").modal("show");

        },
        SubmitAdditional: function () {


            if (app.additionalActivity.Completed_Date == "") {
                toastr.error('Please Enter Date. Please try again.', 'Error!');

                return;
            }
            if (app.additionalActivity.Activity_Title == "") {
                toastr.error('Please Enter Activity. Please try again.', 'Error!');

                return;
            }
            if (app.additionalActivity.Actual_OTJ == "") {
                toastr.error('Please Actual. Please try again.', 'Error!');

                return;
            }
            if (app.additionalActivity.Activity_Title.length <= 5) {
                toastr.error('Please Enter Activity more than 5 characters . Please try again.', 'Error!');

                return;
            }
           /* app.additionalActivity.PId = parseInt(app.additionalActivity.PId);*/
            var data = app.additionalActivity;
            $.ajax({
                url: "/api/PortalMyCPDApi/UpdateAdditinalActivity",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                if (response === true) {
                    app.additionalActivity.Actual_OTJ = '';
                    app.additionalActivity.PId = '';
                    app.additionalActivity.Activity_Title = '';
                    app.additionalActivity.Completed_Date = '';
                    app.additionalActivity.Description = '';
                    toastr.success("Submitted successfully.", "Success!");

                    $("#kt_modal_Additinal_Activities").modal("hide");
                    $('#gridAdditional').data('kendoGrid').dataSource.read();
                    $('#gridAdditional').data('kendoGrid').refresh();

                } else {
                    toastr.error('Record have not been saved. Please try again.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
    },
    mounted: function () {
        $('#completeDate').datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
            todayHighlight: true
        }).on('changeDate', () => { app.additionalActivity.Completed_Date = $('#completeDate').val(); });

    }
})

app.GetMyDocument();
function PerformEditComplete(id) {
    app.Edit(id);
}
function PerformEditAdditinal(id) {
    app.addAdditional(id);
}