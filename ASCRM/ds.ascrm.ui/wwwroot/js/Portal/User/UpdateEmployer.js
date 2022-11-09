Vue.use(vuelidate.default)
var app = new Vue({
    el: '#dv_Portal_updateEmployer',
    data: {
        Users: [],
        search12: '',
        search1: '',
        userId: '',
        AssgendList: [],
        
    },
    methods: {

        GetUsers: function () {
            $.ajax({
                url: "/api/UserAdminApi/LoadAccountUser",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Users = response;
                app.userId = app.Users[1].Users_Id;
                app.LoadUsers();
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        LoadUserEmployer: function () {
            $.ajax({
                url: "/api/UserAdminApi/GetEmployerUser",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.AssgendList = response;
            })
        },
        LoadUsers: function () {
            $.ajax({
                url: "/api/UserAdminApi/GetEmployerLearnerUser",
                data: { id: app.userId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.LoadEmployerLearner();
                var data = app.AssgendList;
                $("#grid_employer").data("kendoGrid").dataSource.data([]);

                let grid = $("#grid_employer").data("kendoGrid");
                for (var i = 0; i < response.length; i++) {

                    var assign = '';
                    var findassign = app.AssgendList.filter(function (x) {
                        if (x.Employer_Id == response[i].Employer_Id) return x.Employer_Id;
                    })
                    if (findassign.length > 0) {
                        assign = 'Yes';
                    } else {
                        assign = 'No';

                    }
                            grid.dataSource.add({
                                Employer_Id: response[i].Employer_Id
                                , Employer_Name: response[i].Employer_Name,
                                TotalAssigned: response[i].TotalAssigned,
                                IsAssigned: assign
                            });

                }
             
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        UpdateLearner: function (id) {
            if (app.userid == '') {
                toastr.warning('Please Select User', 'Warning!');
                return;
            }
            
            $.ajax({
                url: "/api/UserAdminApi/UpdateEmployerLearner",
                type: "GET",
                data: { userid: app.userId, employerid: id },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                toastr.success("Assigned successfully.", "Success!");

                app.LoadUsers();
                app.LoadEmployerLearner();
                //app.Users = [];
                //app.Users = response;
                //console.log(app.awarding)

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        LoadEmployerLearner: function () {
            $.ajax({
                url: "/api/UserAdminApi/GetEmployerLearner",
                data: { id: app.userId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                console.log(response);
                $("#grid_Learner").data("kendoGrid").dataSource.data([]);

                let grid = $("#grid_Learner").data("kendoGrid");
                for (var i = 0; i < response.length; i++) {
                    grid.dataSource.add({
                        Learner_ID: response[i].Learner_ID
                        , Learner_FirstName: response[i].Learner_FirstName,
                        Learner_Surname: response[i].Learner_Surname,
                        Learner_Class: response[i].Learner_Class,
                        CandidateStatus: response[i].CandidateStatus,
                        Percentage: response[i].Percentage,
                        IsHidden: response[i].IsHidden
                    });
                }
            })
        },
        onChange: function () {
            app.LoadUsers();
            app.LoadEmployerLearner();
        },
        Hidden: function (id,type) {
            if (app.userid == '') {
                toastr.warning('Please Select User', 'Warning!');
                return;
            }

            $.ajax({
                url: "/api/UserAdminApi/UpdateHideEmployerLearner",
                type: "GET",
                data: { userid: app.userId, learnerid: id, type: type },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                toastr.success("Hide successfully.", "Success!");

                app.LoadUsers();
                app.LoadEmployerLearner();
                //app.Users = [];
                //app.Users = response;
                //console.log(app.awarding)

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

    }

})
function PerfromUpdate(id) {
    app.UpdateLearner(id);
}
function PerfromHide(id,type) {
    app.Hidden(id,type);
}