Vue.use(vuelidate.default)

var router = new VueRouter({
    mode: 'history',
    routes: []
});

var app = new Vue({
    router,
    el: '#dv_UserProfile',
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
            Mobile: ''
        },

        isSubmitted: false,
        Roles: [],
        parameters: {},
        isExists: false
    },
    validations: {
        user: {
            Users_UserName: {
                required: validators.required,
                email: validators.email
            },
            Users_Email: {
                required: validators.required,
                email: validators.email
            },
            Users_Password: {
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
            },
            Mobile: {
                required: validators.required,
                maxLength: validators.maxLength(13)
            }
        }
    },
    methods: {

        PopulateRoles: function () {
            $.ajax({
                url: "/api/UserProfileApi/GetUserById",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.user = response;
                _userProfileUrl = '';
                if (app.user.Users_Image === null) {
                    _userProfileUrl = '/Images/user2-160x160.jpg';
                    app.user.Users_Image = '/Images/user2-160x160.jpg';
                }
                else {
                    _userProfileUrl = app.user.Users_Image;
                }
                $(".kt-avatar__holder").css("background-image", "url(" + app.user.Users_Image + ")");
                $("#userProfileImg").attr("src", app.user.Users_Image);
                $("#userLogoImg").attr("src", app.user.Users_Image);
                app.user.Users_ConfirmPassword = app.user.Users_Password;

                if (response.Users_IsActive === 1) {
                    $("#txtStatus").val("Active");
                }
                else { $("#txtStatus").val("Inactive"); }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        submitted: function () {

            this.isSubmitted = true;

            if (this.$v.user.$invalid) {
                toastr.error("Error in validation.", 'Error!', { positionClass: 'toast-top-right' });
                return;
            }

            var phoneno = /^\d{11}$/;
            if (!this.user.Mobile.match(phoneno)) {
                toastr.error("Mobile number is invalid", 'Error!');
                return false;
            }

            //check if email already exists for user

            var data = this.user;

            if (data.Users_IsActive) {
                data.Users_IsActive = 1;
            } else {
                data.Users_IsActive = 0;
            }

            $.ajax({
                url: "/api/UserProfileApi/CheckEmailAlreadyExists",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === false) {
                    data.Users_Image = _userProfileUrl;

                    app.isExists = false;
                    $.ajax({
                        url: "/api/UserProfileApi/InsertUpdateUserRecord",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        if (response === true) {
                            toastr.success("User have been saved successfully.", 'Success!');
                            /// update user profile.
                            getUserProfile();
                        } else {
                            toastr.error("User have not been saved. Please try again.", 'Error!');
                        }
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                } else {
                    app.isExists = true;
                    toastr.warning("Email already exists. Please change email.", 'Warning!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }
    }
});



app.PopulateRoles();
var _userProfileUrl = '';



function getUserProfile() {
    $.ajax({
        url: "/api/UserProfileApi/GetUserById",
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {

        app.user = response;
        _userProfileUrl = '';
        _userProfileUrl = '';
        if (app.user.Users_Image === null) {
            _userProfileUrl = '/Images/user2-160x160.jpg';
            app.user.Users_Image = '/Images/user2-160x160.jpg';
        }
        else {
            _userProfileUrl = app.user.Users_Image;
        }
        $(".kt-avatar__holder").css("background-image", "url(" + app.user.Users_Image + ")");
        $("#userProfileImg").attr("src", app.user.Users_Image);
        $("#userLogoImg").attr("src", app.user.Users_Image);
        app.user.Users_ConfirmPassword = app.user.Users_Password;

        if (response.Users_IsActive === 1) {
            $("#txtStatus").val("Active");
        }
        else { $("#txtStatus").val("Inactive"); }
    }).fail(function (xhr) {
        toastr.error(xhr.responseText, 'Error!');
    });
}