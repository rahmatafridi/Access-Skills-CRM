Vue.use(vuelidate.default)

var app = new Vue({

    el: '#dv_Manage_Configration',
    data: {
        isSubmitted: false,
        buttonText: 'Add',
        ConfigrationList: [],
        Configration: { config_id: 0, config_key: null, config_value: null, config_description: null },
        isValueExists: false,
        isKeyExists: false
    },
    validations: {
        Configration: {
            config_key: {
                required: validators.required
            },
            config_value: {
                required: validators.required
            }
            //,
            //config_description: {
            //    required: validators.required,
            //    integer: validators.integer
            //}
        }
    },
    methods: {
        GetAllConfigrations: function () {
            $.ajax({
                url: "/api/CoreConfigurationApi/GetAllConfigrations",
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.ConfigrationList = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        AddEditConfigration: function () {
            if (this.Configration.config_id === 0 && !_canRoleManagerCreateConfig) {
                toastr.error('You cannot add new Configuration please contact administrator.', 'Permission denied!');
                return false;
            }
            this.isSubmitted = true;
            app.isKeyExists = false;
            app.isValueExists = false;

            if (this.$v.Configration.$invalid) {
                toastr.error('Error in validation.', 'Error!');
                return;
            }
            var data = this.Configration;

            $.ajax({
                url: "/api/CoreConfigurationApi/CheckKeyAndValueExists",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response.Item1 === false && response.Item2 === false) {
                    $.ajax({
                        url: "/api/CoreConfigurationApi/InsertUpdateConfigration",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json",
                    }).done(function (response) {
                        if (response === true) {
                            app.GetAllConfigrations();
                            app.isSubmitted = false;
                            if (app.Configration.config_id > 0) {
                                toastr.success("Record updated successfully.", "Updated!");
                            } else {
                                toastr.success("Record inserted successfully.", "Inserted!");
                            }
                            app.CancelEdit();
                        } else {
                            toastr.error('Record have not been saved. Please try again.', 'Error!');
                        }
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                } else {
                    app.isKeyExists = response.Item1;
                    app.isValueExists = response.Item2;
                    toastr.error('Title or value already exists.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        EditConfigration: function (Configration) {
            if (!_canRoleManagerEditConfig) {
                toastr.error('You cannot edit please contact administrator', 'Permission denied!');
                return false;
            }
            app.buttonText = "Update";
            app.Configration = Configration;
        },
        Delete: function (config_id) {

            if (!_canRoleManagerDeleteConfig) {
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
                if (!result.value) {
                    return;
                }
                $.ajax({
                    url: "/api/CoreConfigurationApi/DeleteConfigrationById",
                    data: { id: config_id },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    if (response === true) {
                        app.GetAllConfigrations();
                        toastr.success("Configuration deleted successfully.", "Deleted!");
                    } else {
                        toastr.success("Configuration cannot deleted. Please try again.", "Deleted!");
                    }
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Deleted!');
                });
            });
        },
        CancelEdit: function () {
            app.buttonText = "Add";
            app.isSubmitted = false;
            app.isKeyExists = false;
            app.isValueExists = false;
            app.Configration = { config_id: 0, config_key: null, config_value: null, config_description: null };
        }
    }
});


app.GetAllConfigrations();