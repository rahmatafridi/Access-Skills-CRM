Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_LearnerCPD',
    data: {
        LearnerStartDate: '',
        PracticalEndDate: '',
        HoursWorkedperWeek: '',
        LearnerStartDate: '',
        DailyHours: '',
        Min20OTJHours: '',
        ActualOTJHours: '',
        RemainingOTJHours: '',
        Completed: [],
        completedActivity: {
            Actual_OTJ: '',
            PId: ''
        },
        additionalActivity: {
            PId: 0,
            Completed_Date: '',
            Activity_Title: '',
            Description: '',
            Actual_OTJ: '',
            Is_Locked: ''
        },

        CompletedActivities: [],
        AdditionalActivities: [],
    },
    methods: {
        LoadQuickStats: function () {
            var _learnerId = $("#learnerId").val();
            $.ajax({
                url: "/api/PortalApi/GetQuickStats",
                type: "GET",
                data: { id: _learnerId },
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

    }

})
app.LoadQuickStats();