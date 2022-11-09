var app = new Vue({

    el: '#dv_Manage_Companies',
    data: {
        isSubmitted: false,
        Companies: [],
        SalesAdvisors: []
    },
    methods: {
        refreshData: function () {
            //init to do
        },
       
        Edit: function (company_id, activityTab) {
          //  alert("Edit Id : " + company_id);
            if (!activityTab) {
                window.location.href = 'Company/Edit?id=' + company_id;
            } else {
                window.location.href = 'Company/Edit?id=' + company_id + '&Act=Y';
            }
        },
        Delete: function (company_id) {
            $.ajax({
                url: "/api/CompanyApi/DeleteCompanyById",
                data: { id: company_id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#gridCompanies').data('kendoGrid').dataSource.read();
                    $('#gridCompanies').data('kendoGrid').refresh();
                    toastr.success("Company has been deleted successfully.", "Success!");
                } else {
                    toastr.error("Lead cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GetDuplicateLead: function (id) {

            $.ajax({
                url: "/api/CompanyApi/GetAllSalesAdvisorUsers",
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
        DeleteCompany: function () {
            var data = this.duplicateLead;
            var ldId = data.LD_Id;
            var company_id = data.Lead_Id;
            var leadDuplicateId = data.Duplicate_Lead_Id;

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
                        url: "/api/CompanyApi/DeleteDuplicateLeadById",
                        data: { ld_id: ldId, lead_id: company_id, lead_duplicate_id: leadDuplicateId },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        $("#kt_modal_duplicate").modal("hide");
                        $('#gridCompanies').data('kendoGrid').dataSource.read();
                        $('#gridCompanies').data('kendoGrid').refresh();
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });

        },
        DeleteDuplicateLead: function () {
            var data = this.duplicateLead;
            var ldId = data.LD_Id;
            var company_id = data.Duplicate_Lead_Id;
            var leadDuplicateId = data.Lead_Id;

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
                        url: "/api/CompanyApi/DeleteDuplicateLeadById",
                        data: { ld_id: ldId, lead_id: company_id, lead_duplicate_id: leadDuplicateId },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        $("#kt_modal_duplicate").modal("hide");
                        $('#gridCompanies').data('kendoGrid').dataSource.read();
                        $('#gridCompanies').data('kendoGrid').refresh();
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });
        },
        leadAdvisorChange: function () {
            var data = this.duplicateLead;
            var company_id = data.Lead_Id;
            var salesAdvisorId = data.Sales_Advisor_Id;

            $.ajax({
                url: "/api/CompanyApi/UpdateSalesAdvisor",
                data: { lead_id: company_id, sales_advisor_id: salesAdvisorId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#kt_modal_duplicate").modal("hide");
                $('#gridCompanies').data('kendoGrid').dataSource.read();
                $('#gridCompanies').data('kendoGrid').refresh();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        leadDuplicateAdvisorChange: function () {

            var data = this.duplicateLead;
            var company_id = data.Duplicate_Lead_Id;
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
                        data: { lead_id: company_id, sales_advisor_id: salesAdvisorId },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        $("#kt_modal_duplicate").modal("hide");
                        $('#gridCompanies').data('kendoGrid').dataSource.read();
                        $('#gridCompanies').data('kendoGrid').refresh();
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });
        },
        removeDuplicateLead: function (type) {

            var _company_id = 0;
            _company_id = (type === 0) ? app.duplicateLead.Lead_Id : app.duplicateLead.Duplicate_Lead_Id;

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
                        url: "/api/CompanyApi/RemoveLeadDuplicate",
                        data: { lead_id: _company_id, ld_id: _ldId },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        $("#kt_modal_duplicate").modal("hide");
                        $('#gridCompanies').data('kendoGrid').dataSource.read();
                        $('#gridCompanies').data('kendoGrid').refresh();
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });

        },
        selectSalesAdvisor: function (id, company_id) {

            $.ajax({
                url: "/api/CompanyApi/GetAllSalesAdvisorUsers",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.SalesAdvisors = response;
                app.duplicateLead.Sales_Advisor_Id = id;
                app.duplicateLead.Lead_Id = company_id;
                $("#kt_modal_SalesAdvisor").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        salesAdvisorChange: function () {
            var data = this.duplicateLead;
            var company_id = data.Lead_Id;
            var salesAdvisorId = data.Sales_Advisor_Id;

            $.ajax({
                url: "/api/CompanyApi/UpdateSalesAdvisor",
                data: { lead_id: company_id, sales_advisor_id: salesAdvisorId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $("#kt_modal_SalesAdvisor").modal("hide");
                $('#gridCompanies').data('kendoGrid').dataSource.read();
                $('#gridCompanies').data('kendoGrid').refresh();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },


    }
});

app.refreshData();
function PerformCompanyDelete(company_Id) {
    if (!_canRoleManagerDeleteCompany) {
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
            app.Delete(company_Id);
        }
    });
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
        $('#gridCompanies').data('kendoGrid').dataSource.read();
        $('#gridCompanies').data('kendoGrid').refresh();
    }
}

function PerformCompanyEdit(company_Id, activityTab) {
    if (_canRoleManagerEditCompany) {
        app.Edit(company_Id, activityTab);
    } else {
        toastr.error('You cannot edit please contact administrator', 'Permission denied!');
    }
}



function duplicateOnclick(id, Lead_Id) {
    if (id === null) return;
    app.GetDuplicateLead(Lead_Id);
}

function removeDuplicateLead(type) {
    app.removeDuplicateLead(type);
}

function openSalesAdvisor(salesAdvisorId, company_id) {
    if (salesAdvisorId === null) return;
    app.selectSalesAdvisor(salesAdvisorId, company_id);
}


