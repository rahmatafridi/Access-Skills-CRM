Vue.use(vuelidate.default)

var router = new VueRouter({
    mode: 'history',
    routes: []
});
var app = new Vue({
    router,
    el: '#dv_Portal_UserManage',
    data: {
        user: {
            Users_ID: 0,
            Users_UserName: '',
            Users_Email: '',
            Users_Password: '',
            Users_ConfirmPassword: '',
            Users_DisplayName: '',
            Users_IsActive: true,
            Users_RoleId: 0,
            Users_SeeLearnerCourceSummaryDocs: false,
            Users_Id_Care: 0,
            Users_ShowLearnersFrom: '',
            Users_Id_TrainingProvider: 0,
            Users_Id_AwardingBody:0
        },
        providers: [],
        awarding:[],
        isSubmitted: false,
        Roles: [],
        parameters: {},
        isExists: false,
        accountType:''
    },
    validations: {
        user: {
            Users_UserName: {
                required: validators.required
            },
            Users_Email: {
                required: validators.required,
                email: validators.email
            },
            Users_Password: {
                //required:  validators.required,
                required: validators.required,
                minLength: validators.minLength(6)
            },
            Users_DisplayName: {
                required: validators.required
            },
            Users_ConfirmPassword: {
                required: validators.required,
                sameAsPassword: validators.sameAs('Users_Password')
            },
            Users_RoleId: {
                required: validators.required
            }
        }
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
        submitted: function (type) {
            this.isSubmitted = true;
            if (app.accountType == "Training Provider") {
                if (this.user.Users_Id_AwardingBody != 0) {
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
                console.log(response);
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
            console.log(data);
            if (data == "Training Provider") {
                $("#traingingprovider").show();
                $("#awardingbody").hide();
               // $("#accouttype").hide();
                $("#tickdcourse").hide();
                $("#employerid").hide();
                $("#showlearner").show();
            }
            if (data == "Awarding Body") {
                $("#traingingprovider").hide();
                $("#awardingbody").show();
               // $("#accouttype").hide();
                $("#tickdcourse").hide();
                $("#employerid").hide();
                $("#showlearner").show();
            }
        },
        mounted: function () {
            parameters = this.$route.query;
            parameters.id = _userId;
            if (parameters.id > 0) {
                //if (!_canRoleManagerEditUser) {
                //    window.location.href = '/Error/PermissionDenied';
                //}
                app.parameters.id = parameters.id;
                $.ajax({
                    url: "/api/UserAdminApi/GetUserById",
                    data: { id: parameters.id },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    app.user = response;
                    $("#lblUserHeader").text(response.Users_DisplayName);
                    app.user.Users_ConfirmPassword = app.user.Users_Password;
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
            } else {
                
            }
        }
    }
});

app.PopulateRoles();
app.LoadOscaUser();
function saveUserAdmin(type) {
    app.submitted(type);
}
