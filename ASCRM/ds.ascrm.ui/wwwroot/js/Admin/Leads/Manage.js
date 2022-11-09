var app = new Vue({

    el: '#dv_Manage_Lead',
    data: {
        isSubmitted: false,
        Leads: [],
        SalesAdvisors: [],
        duplicateLead: {
            ContactName: '',
            Duplicate_ContactName: '',
            Mobile: '',
            Duplicate_Mobile: '',
            CourseLevel: '',
            Duplicate_CourseLevel: '',
            LastResult: '',
            Duplicate_LastResult: '',
            Status: '',
            Duplicate_Status: '',
            EnquiryDate: '',
            Duplicate_EnquiryDate: '',
            Lead_Id: 0,
            Duplicate_Lead_Id: 0,
            LD_Id: 0,
            Sales_Advisor_Id: 0,
            Duplicate_Sales_Advisor_Id:0
        }
    },
    methods: {
        refreshData: function () {

        },
        Edit: function (leadId, activityTab) {
            //alert("Edit Id : " + leadId);
            if (!activityTab) {
                window.location.href = 'Lead/Edit?id=' + leadId;
            } else {
                window.location.href = 'Lead/Edit?id=' + leadId + '&Act=Y';
            }
        },
        Delete: function (leadId) {
            $.ajax({
                url: "/api/LeadApi/DeleteLeadById",
                data: { id: leadId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#leadsGrid').data('kendoGrid').dataSource.read();
                    $('#leadsGrid').data('kendoGrid').refresh();
                    toastr.success("Lead has been deleted successfully.", "Success!");
                } else {
                    toastr.error("Lead cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GetDuplicateLead: function (id) {

            $.ajax({
                url: "/api/LeadApi/GetAllSalesAdvisorUsers",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.SalesAdvisors = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
                });

            $.ajax({
                url: "/api/LeadApi/GetDuplicateLeadLeadById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.duplicateLead = response;

                $("#kt_modal_duplicate").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        DeleteLead: function () {
            var data = this.duplicateLead;
            var ld_id = data.LD_Id;
            var lead_id = data.Lead_Id;
            //var leadDuplicateId = data.Duplicate_Lead_Id;

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
                    $.ajax({
                        url: "/api/LeadApi/DeleteDuplicateLeadById",
                        data: { ld_id: ld_id, lead_id: lead_id},
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        $("#kt_modal_duplicate").modal("hide");
                        $('#leadsGrid').data('kendoGrid').dataSource.read();
                        $('#leadsGrid').data('kendoGrid').refresh();
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });
            
        },
        DeleteDuplicateLead: function ()
        {
            var data = this.duplicateLead;
            var ld_id = data.LD_Id;
            var lead_id = data.Duplicate_Lead_Id;
           // var leadDuplicateId = data.Lead_Id;

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
                    $.ajax({
                        url: "/api/LeadApi/DeleteDuplicateLeadById",
                        data: { ld_id: ld_id, lead_id: lead_id},
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        $("#kt_modal_duplicate").modal("hide");
                        $('#leadsGrid').data('kendoGrid').dataSource.read();
                        $('#leadsGrid').data('kendoGrid').refresh();
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });
        },
        leadAdvisorChange: function ()
        {
            var data = this.duplicateLead;
            var leadId = data.Lead_Id;
            var salesAdvisorId = data.Sales_Advisor_Id;

            $.ajax({
                url: "/api/LeadApi/UpdateSalesAdvisor",
                data: { lead_id: leadId, sales_advisor_id: salesAdvisorId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#kt_modal_duplicate").modal("hide");
                $('#leadsGrid').data('kendoGrid').dataSource.read();
                $('#leadsGrid').data('kendoGrid').refresh();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        leadDuplicateAdvisorChange: function () {
            
            var data = this.duplicateLead;
            var leadId = data.Duplicate_Lead_Id;
            var salesAdvisorId = data.Duplicate_Sales_Advisor_Id;

            Swal.fire({
                title: 'Are you sure?',
                text: "You want to update sales advisor",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, update it!'
            }).then((result) => {
                if (result.value) {
                    $.ajax({
                        url: "/api/LeadApi/UpdateSalesAdvisor",
                        data: { lead_id: leadId, sales_advisor_id: salesAdvisorId },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        $("#kt_modal_duplicate").modal("hide");
                        $('#leadsGrid').data('kendoGrid').dataSource.read();
                        $('#leadsGrid').data('kendoGrid').refresh();
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });
        },
        removeDuplicateLead: function (type) {

            var _leadId = 0;
            _leadId = (type === 0) ? app.duplicateLead.Lead_Id : app.duplicateLead.Duplicate_Lead_Id;

            var _ldId = app.duplicateLead.LD_Id;

            Swal.fire({
                title: 'Are you sure?',
                text: "You want to remove duplicate!",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, remove it!'
            }).then((result) => {
                if (result.value) {
                    $.ajax({
                        url: "/api/LeadApi/RemoveLeadDuplicate",
                        data: { lead_id: _leadId, ld_id: _ldId },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        $("#kt_modal_duplicate").modal("hide");
                        $('#leadsGrid').data('kendoGrid').dataSource.read();
                        $('#leadsGrid').data('kendoGrid').refresh();
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });

        }
        , selectSalesAdvisor: function (id, leadId) {

            $.ajax({
                url: "/api/LeadApi/GetAllSalesAdvisorUsers",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.SalesAdvisors = response;
                app.duplicateLead.Sales_Advisor_Id = id;
                app.duplicateLead.Lead_Id = leadId;
                $("#kt_modal_SalesAdvisor").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }
        ,salesAdvisorChange: function ()
        {
            var data = this.duplicateLead;
            var leadId = data.Lead_Id;
            var salesAdvisorId = data.Sales_Advisor_Id;

            $.ajax({
                url: "/api/LeadApi/UpdateSalesAdvisor",
                data: { lead_id: leadId, sales_advisor_id: salesAdvisorId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#kt_modal_SalesAdvisor").modal("hide");
                $('#leadsGrid').data('kendoGrid').dataSource.read();
                $('#leadsGrid').data('kendoGrid').refresh();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }
    }
});

app.refreshData();

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

function PerformLeadEdit(lead_Id, activityTab) {
    if (_canRoleManagerEditLead) {
        app.Edit(lead_Id, activityTab);
    } else {
        toastr.error('You cannot edit please contact administrator', 'Permission denied!');
    }
}

function PerformLeadDelete(lead_Id) {
    if (!_canRoleManagerDeleteLead) {
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
            app.Delete(lead_Id);
        }
    });
}

function duplicateOnclick(id, Lead_Id) {
    if (id === null) return;
    app.GetDuplicateLead(Lead_Id);
}

function removeDuplicateLead(type) {
    app.removeDuplicateLead(type);
}

function openSalesAdvisor(salesAdvisorId, leadId) {
    if (salesAdvisorId === null) return;
    app.selectSalesAdvisor(salesAdvisorId, leadId);
}
