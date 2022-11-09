Vue.use(vuelidate.default)

var router = new VueRouter({
    mode: 'history',
    routes: []
});
var app = new Vue({
    router,
    el: '#dv_Portal_AssignLearner',
    data: {
        user: {
            Users_ID: 0,
            Users_UserName: '',
            Users_Email: '',
            Users_Password: '',
            Users_ConfirmPassword: '',
            Users_DisplayName: '',
            Users_IsActive: true,
            Users_RoleId: '',
            Users_SeeLearnerCourceSummaryDocs: false,
            Users_Id_Care: 0,
            Users_ShowLearnersFrom: '',
            Users_Id_TrainingProvider: 0,
            AwardingBodyLearner_Id: 0
        },
        providers: [],
        awarding: [],
        isSubmitted: false,
        Roles: [],
        parameters: {},
        isExists: false,
        accountType: '',
        Users: [],
        userSelectedId: '',
        AccountId: '',
        userId: '',
        search1:''


    },
    validations: {
    },
    methods: {
        PopulateRoles: function () {
            $.ajax({
                url: "/api/RoleAdminApi/GetAllRolesPortal",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Roles = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        LoadOscaUser: function () {
            $.ajax({
                url: "/api/UserAdminApi/TrainingProviders",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.providers = response;
                //console.log(app.awarding)

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
          
        },
        LoadOscaUserAwading() {
            $.ajax({
                url: "/api/UserAdminApi/AwardingBody",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.awarding = response;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        LoadTrainingProviderUser: function (id) {
            $.ajax({
                url: "/api/UserAdminApi/GetTrainingProviderUser",
                type: "GET",
                data: { id: id,type:"Provider"},
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Users = [];
                app.Users = response;
                //console.log(app.awarding)

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
           
        },
        LoadAwrdingUser: function (id) {
            $.ajax({
                url: "/api/UserAdminApi/GetTrainingProviderUser",
                type: "GET",
                data: { id: id, type: "AwardBody" },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Users = [];
                app.Users = response;
                //console.log(app.awarding)

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        providerChange: function () {
            $("#LearnerGrid").data("kendoGrid").dataSource.read();

            app.LoadTrainingProviderUser(app.user.Users_Id_TrainingProvider);
        },
        awardingChange: function () {

            app.LoadAwrdingUser(app.user.AwardingBodyLearner_Id);

        },
        submitted: function (type) {
            this.isSubmitted = true;
            if (app.accountType == "Training Provider") {
                if (this.user.AwardingBodyLearner_Id != 0) {
                    toastr.warning('Please Refresh the Page', 'Warning!');
                    return;
                }
            }
            if (app.accountType == "Awarding Body") {
                if (this.user.Users_Id_TrainingProvider != 0) {
                    toastr.warning('Please Refresh the Page', 'Warning!');
                    return;
                }
            }
            //this.$v.$touch();
            if (this.$v.user.$invalid) {
                toastr.error("Error in validation.", 'Error!');
                return;
            }
            //check if email already exists for user

            var data = this.user;

            if (data.Users_IsActive) {
                data.Users_IsActive = 1;
            } else {
                data.Users_IsActive = 0;
            }



            $.ajax({
                url: "/api/UserAdminApi/CheckEmailAlreadyExists",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === false) {
                    app.isExists = false;
                    $.ajax({
                        url: "/api/UserAdminApi/InsertUpdateUserRecord",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        if (response === true) {
                            toastr.success("User has been saved successfully.", "Success!");
                            //// if type is 1 then save and go back.
                            if (type === 1) {
                                setTimeout(function () { window.location.href = 'Portal/ManageUser'; }, 3000);
                            }
                        } else {
                            toastr.error("User has not been saved. Please try again.", "Error!");
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
        changeRole: function () {
            $.ajax({
                url: "/api/RoleAdminApi/GetRoleById",
                type: "GET",
                data: { id: app.user.Users_RoleId },

                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response.identifier == "1300") {
                    $("#tickdcourse").show();
                    $("#employerid").show();
                    $("#showlearner").show();
                    $("#accouttype").hide();

                }
                else if (response.identifier == "1100" || response.identifier == "1200") {

                    $("#accouttype").show();
                    $("#tickdcourse").hide();
                    $("#employerid").hide();
                    $("#showlearner").hide();
                }
                else {
                    $("#accouttype").hide();
                    $("#tickdcourse").hide();
                    $("#employerid").hide();
                    $("#showlearner").hide();
                }

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        onChange: function (e) {
            var data = event.target.value;
            app.accountType = data;
            if (data == "Training Provider") {
                $("#traingingprovider").show();
                $("#awardingbody").hide();
                // $("#accouttype").hide();
                $("#tickdcourse").hide();
                $("#employerid").hide();
                $("#showlearner").show();
                app.loadDefult();
            }
            if (data == "Awarding Body") {
                $("#traingingprovider").hide();
                $("#awardingbody").show();
                // $("#accouttype").hide();
                $("#tickdcourse").hide();
                $("#employerid").hide();
                $("#showlearner").show();
                app.loadDefultAwadingBoday();
            }
        },
        loadLearner: function () {
            $.ajax({
                url: "/api/UserAdminApi/LoadAssignLearnerByProviderUser",
                type: "GET",
                data: { id: app.userId },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                $("#LearnerGrid").data("kendoGrid").dataSource.data([]);


                let grid = $("#LearnerGrid").data("kendoGrid");

                for (var i = 0; i < response.length; i++) {

                    grid.dataSource.add({
                       
                        Learner_ID: response[i].Learner_ID
                        , Learner_FirstName: response[i].Learner_FirstName,
                        Learner_Surname: response[i].Learner_Surname
                        , Learner_Class: response[i].Learner_Class, CandidateStatus: response[i].CandidateStatus
                        , IsAssigned: response[i].IsAssigned


                    });

                }


      
                //console.log(app.awarding)

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
                url: "/api/UserAdminApi/CheckAssignLearner",
                type: "GET",
                data: { userid: app.userId,learnerid: id },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response.length >0) {
                    toastr.warning('This Learner Already Assigned.', 'Warning!');

                }
                else {
                    $.ajax({
                        url: "/api/UserAdminApi/UpdateAssignLearner",
                        data: { userid: app.userId, learnerid: id },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        if (response === true) {
                            toastr.success("Assigned successfully.", "Success!");
                            //// if type is 1 then save and go back.
                            //if (type === 1) {
                            //    setTimeout(function () { window.location.href = 'Portal/ManageUser'; }, 3000);
                            //}
                        } else {
                            toastr.error("User has not been saved. Please try again.", "Error!");
                        }
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
                //app.Users = [];
                //app.Users = response;
                //console.log(app.awarding)

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        loadDefult: function () {
            $.ajax({
                url: "/api/UserAdminApi/TrainingProviders",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.providers = response;
                //console.log(app.awarding)
                var provder = app.providers[0];
                $("#traingingprovider").show();
                $.ajax({
                    url: "/api/UserAdminApi/GetTrainingProviderUser",
                    type: "GET",
                    data: { id: provder.TrainingProvider_Id, type: "Provider" },
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    app.Users = [];
                    app.Users = response;
                    app.user.Users_Id_TrainingProvider = provder.TrainingProvider_Id;
                    var userid = app.Users[0];
                    if (app.Users.length > 0) {
                        app.userId = userid.Users_Id;
                        $.ajax({
                            url: "/api/UserAdminApi/LoadAssignLearnerByProviderUser",
                            type: "GET",
                            data: { id: userid.Users_Id },
                            contentType: "application/json",
                            dataType: "json"
                        }).done(function (response) {

                            $("#LearnerGrid").data("kendoGrid").dataSource.data([]);


                            let grid = $("#LearnerGrid").data("kendoGrid");

                            for (var i = 0; i < response.length; i++) {

                                grid.dataSource.add({

                                    Learner_ID: response[i].Learner_ID
                                    , Learner_FirstName: response[i].Learner_FirstName,
                                    Learner_Surname: response[i].Learner_Surname
                                    , Learner_Class: response[i].Learner_Class, CandidateStatus: response[i].CandidateStatus
                                    , IsAssigned: response[i].IsAssigned

                                });

                            }



                            //console.log(app.awarding)

                        }).fail(function (xhr) {
                            toastr.error(xhr.responseText, 'Error!');
                        });
                    } else {
                        $("#LearnerGrid").data("kendoGrid").dataSource.read();

                    }
                    //console.log(app.awarding)

                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
                //var users = app.LoadTrainingProviderUser(provder.TrainingProvider_Id);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
           

        },
        loadDefultAwadingBoday: function () {

            $.ajax({
                url: "/api/UserAdminApi/AwardingBody",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.awarding = response;
                //console.log(app.awarding)
                var awards = app.awarding[0];
                //$("#traingingprovider").show();
                $.ajax({
                    url: "/api/UserAdminApi/GetTrainingProviderUser",
                    type: "GET",
                    data: { id: awards.AwardingBodyLearner_Id, type: "AwardBody" },
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    app.Users = [];
                    app.Users = response;
                    app.user.AwardingBodyLearner_Id = awards.AwardingBodyLearner_Id;
                    var userid = app.Users[0];
                    if (app.Users.length > 0) {
                        app.userId = userid.Users_Id;
                        $.ajax({
                            url: "/api/UserAdminApi/LoadAssignLearnerByAwardUser",
                            type: "GET",
                            data: { id: userid.Users_Id },
                            contentType: "application/json",
                            dataType: "json"
                        }).done(function (response) {

                            $("#LearnerGrid").data("kendoGrid").dataSource.data([]);


                            let grid = $("#LearnerGrid").data("kendoGrid");

                            for (var i = 0; i < response.length; i++) {

                                grid.dataSource.add({

                                    Learner_ID: response[i].Learner_ID
                                    , Learner_FirstName: response[i].Learner_FirstName,
                                    Learner_Surname: response[i].Learner_Surname
                                    , Learner_Class: response[i].Learner_Class, CandidateStatus: response[i].CandidateStatus
                                    , IsAssigned: response[i].IsAssigned

                                });

                            }



                            //console.log(app.awarding)

                        }).fail(function (xhr) {
                            toastr.error(xhr.responseText, 'Error!');
                        });
                    }
                    else {
                        $("#LearnerGrid").data("kendoGrid").dataSource.read();

                    }
                    //console.log(app.awarding)

                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
                //var users = app.LoadTrainingProviderUser(provder.TrainingProvider_Id);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });


        },
        LearnerSearch: function () {
            $("#LearnerGrid").data("kendoGrid").refresh();
            debugger; // eslint-disable-line
            var type = $("#account").val();
            var id = app.search1;

            if (type == "Training Provider") {
                if (id != "") {
                    $.ajax({
                        url: "/api/UserAdminApi/LoadAssignLearnerByProviderUserSearch",
                        data: { id: app.userId, name: id },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json",
                        async: false
                    }).done(function (response) {
                        $("#LearnerGrid").data("kendoGrid").dataSource.data([]);


                        let grid = $("#LearnerGrid").data("kendoGrid");

                        for (var i = 0; i < response.length; i++) {

                            grid.dataSource.add({
                                Learner_ID: response[i].Learner_ID
                                , Learner_FirstName: response[i].Learner_FirstName,
                                Learner_Surname: response[i].Learner_Surname
                                , Learner_Class: response[i].Learner_Class, CandidateStatus: response[i].CandidateStatus
                                , IsAssigned: response[i].IsAssigned
                            });

                        }


                    });
                }
                else {
                    app.loadDefult();
                    $("#LearnerGrid").data("kendoGrid").dataSource.read();
                }
            }
            if (type == "Awarding Body") {
                if (id != "") {
                    $.ajax({
                        url: "/api/UserAdminApi/LoadAssignLearnerByAwardUserSearch",
                        data: { id: app.userId, name: id },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json",
                        async: false
                    }).done(function (response) {
                        $("#LearnerGrid").data("kendoGrid").dataSource.data([]);


                        let grid = $("#LearnerGrid").data("kendoGrid");

                        for (var i = 0; i < response.length; i++) {

                            grid.dataSource.add({
                                Learner_ID: response[i].Learner_ID
                                , Learner_FirstName: response[i].Learner_FirstName,
                                Learner_Surname: response[i].Learner_Surname
                                , Learner_Class: response[i].Learner_Class, CandidateStatus: response[i].CandidateStatus
                                , IsAssigned: response[i].IsAssigned
                            });

                        }


                    });
                }
                else {
                    app.loadDefultAwadingBoday();
                    $("#LearnerGrid").data("kendoGrid").dataSource.read();
                }
            }
        },
        mounted: function () {
           // app.LoadOscaUser();
           // app.LoadOscaUserAwading();
        }
    }
});
app.PopulateRoles();
function saveUserAdmin(type) {
   // app.submitted(type);
}
function PerfromUpdate(id) {
    app.UpdateLearner(id);
}
app.loadDefult();
