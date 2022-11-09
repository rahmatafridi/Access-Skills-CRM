Vue.use(vuelidate.default)

var router = new VueRouter({
    mode: 'history',
    routes: []
});

var app = new Vue({
    router,
    el: '#dv_Create_User',
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
            Users_IsAccepting_Lead: true
        },

        isSubmitted: false,
        Roles: [],
        parameters: {},
        isExists : false
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
                url: "/api/RoleAdminApi/GetAllRoles",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Roles = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        submitted: function (type) {
            this.isSubmitted = true;

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

            if (data.Users_IsAccepting_Lead) {
                data.Users_IsAccepting_Lead = 1;
            } else {
                data.Users_IsAccepting_Lead = 0;
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
                        if (response === true)
                        {
                            toastr.success("User has been saved successfully.", "Success!");
                            //// if type is 1 then save and go back.
                            if (type === 1) {
                                setTimeout(function () { window.location.href = 'UserAdmin/Manage'; }, 3000);
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
        mounted: function () {
            parameters = this.$route.query;
            parameters.id = _userId;
            if (parameters.id > 0) {
                if (!_canRoleManagerEditUser) {
                    window.location.href = '/Error/PermissionDenied';
                }
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
                if (!_canRoleManagerCreateUser) {
                    window.location.href = '/Error/PermissionDenied';
                }
            }
        }
    }
});

app.PopulateRoles();

function saveUserAdmin(type) {
    app.submitted(type);
}
