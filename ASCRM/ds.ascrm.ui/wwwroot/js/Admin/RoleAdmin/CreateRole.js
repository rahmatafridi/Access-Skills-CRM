Vue.use(vuelidate.default)

var router = new VueRouter({
    mode: 'history',
    routes: []
});
var app = new Vue({
    router,
    el: '#dv_Create_Role',
    data: {
        role: {
            name: '',
            description: ''
        },

        isSubmitted: false,        
    },
    validations: {
        role: {
            name: {
                required: validators.required
            }
        }        
    },
    methods: {
        refreshData: function () {
            this.isSubmitted = true;
        },
        submitted: function (type) {
            this.isSubmitted = true;

            if (this.$v.role.$invalid) {
                toastr.error("Error in validation", 'Error!');
                return;
            }

            var data = this.role;
            $.ajax({
                url: "/api/RoleAdminApi/InsertUpdateRole",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true)
                {
                    toastr.success("Role have been saved successfully.", 'Success!');
                    //// if type is 1 then save and go back.
                    if (type === 1) {
                        window.location.href = 'RoleAdmin/Index';
                    }
                } else {
                    toastr.error("Role have not been saved. Please try again.", 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        mounted: function () {
            parameters = this.$route.query;
            parameters.id = _roleId;

            if (parameters.id > 0) {
                if (!_canRoleManagerEditRole) {
                    window.location.href = '/Error/PermissionDenied';                    
                }                
                $.ajax({
                    url: "/api/RoleAdminApi/GetRoleById",
                    data: { id: parameters.id },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    app.role = response;
                    $("#lblRoleHeader").text(response.name);
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
            } else {
                if (!_canRoleManagerCreateRole) {
                    window.location.href = '/Error/PermissionDenied';
                }
            }
        }
    }
});

function saveRole(type) {
    app.submitted(type);
}