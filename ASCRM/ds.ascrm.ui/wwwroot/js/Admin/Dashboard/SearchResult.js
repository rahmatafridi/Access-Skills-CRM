Vue.use(vuelidate.default)

var router = new VueRouter({
    mode: 'history',
    routes: []
});
var app = new Vue({
    router,
    el: '#dv_SearchResults',
    data: {
        parameters: {},
        leads_result: [],
        notes_result: [],
        company_result: [],
        workPlace_result:[],
        activities_result: [],
        search_query: '',
        companyContact_result: [],
        workplaceContact_result:[]
    },
    validations: {
    },
    methods: {
        Mounted: function () {
            $("#activity-tab").hide();
            parameters = this.$route.query;
            if (parameters.s !== null) {
                app.search_query = parameters.s;
                app.GetSearchResults();
            }
        },
        GetSearchResults: function () {
            $.ajax({
                url: "/api/DashboardApi/GetSearchResults",
                type: "GET",
                contentType: "application/json",
                data: { searchQuery: app.search_query },
                dataType: "json",
            }).done(function (response) {
                //app.results = response;
                app.leads_result = response.Leads;
                app.notes_result = response.Notes;
                app.activities_result = response.Activities;
                app.company_result = response.Companies;
                app.workPlace_result = response.companyWorkplaces;
                app.companyContact_result = response.companyContacts;
                app.workplaceContact_result = response.WorkplaceContacts;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GoToLead: function (encodeLeadId, activityTab) {
            //alert(encodeLeadId);
            if (!activityTab) {
                window.location.href = 'Lead/Edit?id=' + encodeLeadId;
            } else {
                window.location.href = 'Lead/Edit?id=' + encodeLeadId + '&Act=Y';
            }
        },
        GoToCompany: function (encodeCompanyId, activityTab) {
            //alert(encodeLeadId);
            if (!activityTab) {
                window.location.href = 'Company/Edit?id=' + encodeCompanyId;
            } else {
                window.location.href = 'Company/Edit?id=' + encodeCompanyId + '&Act=Y';
            }
        },
        GoToViewLead: function (encodeLeadId, activityTab) {
            //alert(encodeLeadId);
            if (!activityTab) {
                window.location.href = 'Lead/Edit?id=' + encodeLeadId;
            } else {
                window.location.href = 'Lead/Edit?id=' + encodeLeadId + '&Act=Y';
            }
        },

        PerformDeleteNow: function (lead_id) {
            //alert(lead_id);
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
                    app.Delete(lead_id);
                }
            });
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
                    app.Mounted();
                    //$('#leadsGrid').data('kendoGrid').dataSource.read();
                    //$('#leadsGrid').data('kendoGrid').refresh();
                    toastr.success("Lead has been deleted successfully.", "Success!");
                } else {
                    toastr.error("Lead cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        

    }
});
 
app.Mounted();
