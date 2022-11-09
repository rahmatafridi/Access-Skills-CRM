Vue.use(vuelidate.default);

var app = new Vue({

    el: '#dv_Dashboard_Main',
    data: {
        AllLeadsModel: {},
        AllAgreedPaymentSchemeModel: 1
    },
    methods: {
        //GetAllWidgets: function () {
        //    $.ajax({
        //        url: "/api/DashboardApi/GetAllLeadsForDashboard",
        //        type: "GET",
        //        contentType: "application/json",
        //        dataType: "json"
        //    }).done(function (response) {
        //        $("#lblLeadTotal").text(response.Leads_Total);
        //        app.AllLeadsModel = response;
        //    }).fail(function (xhr) {
        //        toastr.error(xhr.responseText, 'Error!');
        //    });
        //},
        GetAllTasks: function () {
            $.ajax({
                url: "/api/TaskApi/GetPerDayTasks",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                var _html = '';
                for (var count = 0; count < response.length; count++)
                {
                    _html = '';
                    _html += '<div class="kt-timeline-v2__item">';
                    _html += '<span class="kt-timeline-v2__item-time">' + response[count].task_date + '</span>';
                    _html += '<div class="kt-timeline-v2__item-cricle">';
                    if (response[count].Class_Name === "1")
                    {
                        _html += '<i class="fa fa-genderless kt-font-danger"></i>';
                    }
                    else if (response[count].Class_Name === "2") {
                        _html += '<i class="fa fa-genderless kt-font-success"></i>';
                    }
                    else if (response[count].Class_Name === "3") {
                        _html += '<i class="fa fa-genderless kt-font-brand"></i>';
                    }
                    else if (response[count].Class_Name === "4") {
                        _html += '<i class="fa fa-genderless kt-font-warning"></i>';
                    }
                    _html += ' </div>';

                    _html += '<div class="kt-timeline-v2__item-text  kt-padding-top-5">';
                    _html += '<span style="font-weight:700">' + response[count].title + '</span>';
                    _html += '<br>';
                    _html += '<span style="font-weight:500">' + response[count].note + '</span>';
                    _html += '</div>';

                    $("#divTasks").append(_html);
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText);
            });
        }
    }
});

//app.GetAllWidgets();
app.GetAllTasks();

function PerformLeadEdit(lead_Id, activityTab) {
    if (_canRoleManagerEditLead) {
        if (!activityTab) {
            window.location.href = 'Lead/Edit?id=' + lead_Id;
        } else {
            window.location.href = 'Lead/Edit?id=' + lead_Id + '&Act=Y';
        }
    } else {
        toastr.error('You cannot edit please contact administrator', 'Permission denied!');
    }
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
        toastr.error(message, 'Permission denied!');
        $('#leadsGrid').data('kendoGrid').dataSource.read();
        $('#leadsGrid').data('kendoGrid').refresh();
    }
}



function show_agreed_payment_scheme_leads(id) {
    $("#agreed_pay_val").val(id);

   // alert($("#agreed_pay_val").val());
    $('#ModalGridAgreedPaymentSchemeLeads').data('kendoGrid').dataSource.read();
    $('#ModalGridAgreedPaymentSchemeLeads').data('kendoGrid').refresh();

    $("#kt_modal_agreedpaymentscheme").modal("show");
    
}
function getSelectedValueAgreedPaymentScheme() {
    return {
        agreed_pay_val: $("#agreed_pay_val").val()         
    }

}


function show_last_result_leads(id) {
    $("#last_result_val").val(id);
    $('#ModalGridLastResultsLeads').data('kendoGrid').dataSource.read();
    $('#ModalGridLastResultsLeads').data('kendoGrid').refresh();
    $("#kt_modal_lastresults").modal("show");    
}

function getSelectedValueLastResult() {
    return {
        last_result_val: $("#last_result_val").val()
    }
}

