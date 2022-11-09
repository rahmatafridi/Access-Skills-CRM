Vue.use(vuelidate.default)

var app = new Vue({

    el: '#dv_Manage_List',
    data: {
        isSubmitted: false,
        buttonText: 'Add',
        DropdownHeaders: [],
        DropdownOptions: [],
        Option: { Opt_Id: 0, Opt_Value: null, Opt_Title: null, Opt_SortOrder: 0, Opt_Id_OptHeader: 0 },
        Selected: null,
        isValueExists: false,
        isTitleExists: false
    },
    validations: {
        Option: {
            Opt_Value: {
                required: validators.required
            },
            Opt_Title: {
                required: validators.required
            },
            Opt_SortOrder: {
                required: validators.required,
                integer: validators.integer
            }
        }
    },
    methods: {
        GetAllDropdownsHeaders: function () {
            $.ajax({
                url: "/api/ListApi/GetAllDropdownsHeaders",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                var collection = [];
                var obj = new Object();
                obj.OptHeader_Id = -1;
                obj.OptHeader_Title = "Please select header name to edit";
                collection.push(obj);
                for (var count = 0; count < response.length; count++)
                {
                    obj = new Object();
                    obj.OptHeader_Id = response[count].OptHeader_Id;
                    obj.OptHeader_Title = response[count].OptHeader_Title;
                    collection.push(obj);
                }
                
                app.DropdownHeaders = collection;
                app.Selected = -1;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        ChangeDropdown: function () {
            if (app.Selected !== null) {
                $.ajax({
                    url: "/api/ListApi/GetDropdownOptionsByHeaderId",
                    data: { id: app.Selected },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    app.DropdownOptions = response;
                    app.CancelEdit();
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
            }
        },
        AddEditOption: function () {
            if (this.Option.Opt_Id === 0 && !_canRoleManagerCreateList) {
                toastr.error('You cannot add new option please contact administrator', 'Permission denied!');
                return false;
            }
            this.isSubmitted = true;
            app.isTitleExists = false;
            app.isValueExists = false;

            if (app.Selected === null) {
                toastr.error('Please select header dropdown first', 'Error!');
                this.isSubmitted = false;
                return;
            }

            if (this.$v.Option.$invalid) {
                toastr.error('Error in validation', 'Error!');
                return;
            }
            this.Option.Opt_Id_OptHeader = app.Selected;
            var data = this.Option;

            $.ajax({
                url: "/api/ListApi/CheckTitleAndValueExists",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response.Item1 === false && response.Item2 === false) {
                    $.ajax({
                        url: "/api/ListApi/InsertUpdateOptionRecord",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        if (response === true) {
                            app.ChangeDropdown();
                            app.isSubmitted = false;
                            if (app.Option.Opt_Id > 0)
                            {
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
                    app.isTitleExists = response.Item1;
                    app.isValueExists = response.Item2;
                    toastr.error('Title or value already exists.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        EditOption: function (option) {
            if (!_canRoleManagerEditList) {
                toastr.error("You cannot edit please contact administrator", 'Permission denied!');
                return false;
            }
            app.buttonText = "Update";
            app.Option = option;
        },
        Delete: function (optionId) {

            if (!_canRoleManagerDeleteList) {
                toastr.error("You cannot delete please contact administrator", 'Permission denied!');
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
                $.ajax({
                    url: "/api/ListApi/DeleteDropdownOptionByOptionId",
                    data: { id: optionId },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    if (response === true) {
                        toastr.success("Option deleted successfully.", "Deleted!");
                        app.ChangeDropdown();
                    } else {
                        toastr.error("Option cannot deleted. Please try again.", "Deleted!");
                    }
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Deleted!');
                });
            });

        },
        CancelEdit: function () {
            app.buttonText = "Add";
            app.isSubmitted = false;
            app.isTitleExists = false;
            app.isValueExists = false;
            app.Option = { Opt_Id: 0, Opt_Value: null, Opt_Title: null, Opt_SortOrder: 0, Opt_Id_OptHeader: 0 };
        }
    }
});

app.GetAllDropdownsHeaders();
app.ChangeDropdown();