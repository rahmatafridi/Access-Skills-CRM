var app = new Vue({

    el: '#dv_Manage_QuestionValidation',
    data: {
        isSubmitted: false,
        Validations: [],
        SalesAdvisors: []
    },
    methods: {
        refreshData: function () {
            //init to do
        },

        Edit: function (id, activityTab) {
            //  alert("Edit Id : " + company_id);
         
                window.location.href = 'QuestionValidation/Edit?id=' + id;
           
        },
        Delete: function (id) {
            $.ajax({
                url: "/api/QuestionValidationApi/DeleteQuestionValidationById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#gridVQuestion').data('kendoGrid').dataSource.read();
                    $('#gridVQuestion').data('kendoGrid').refresh();
                    toastr.success("Validation has been deleted successfully.", "Success!");
                } else {
                    toastr.error("Validation cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },


    }
});

app.refreshData();
function PerformQVDelete(id) {
   
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
            app.Delete(id);
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
        $('#gridVQuestion').data('kendoGrid').dataSource.read();
        $('#gridVQuestion').data('kendoGrid').refresh();
    }
}

function PerformQVEdit(id, activityTab) {
    if (_canManagerEditQV) {
        app.Edit(id, activityTab);
    } else {
        toastr.error('You cannot edit please contact administrator', 'Permission denied!');
    }
}

