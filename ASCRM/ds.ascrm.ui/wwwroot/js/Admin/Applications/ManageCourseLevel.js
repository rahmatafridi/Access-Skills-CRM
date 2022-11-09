var app = new Vue({

    el: '#dv_Manage_CourseLevel',
    data: {
        isSubmitted: false,
        Questions: [],
    },
    methods: {
        refreshData: function () {
            //init to do
        },
        
        Edit: function (id) {
            //  alert("Edit Id : " + company_id);

            window.location.href = 'Application/EditCourse?id=' + id;

        },
        Delete: function (id) {
            $.ajax({
                url: "/api/ApplicationApi/DeleteCourseLevelById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#gridCourseLevel').data('kendoGrid').dataSource.read();
                    $('#gridCourseLevel').data('kendoGrid').refresh();
                    toastr.success("Course Level has been deleted successfully.", "Success!");
                } else {
                    toastr.error("Question cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },


    }
});
function PerformCourseLevelEdit(id) {
    //if (_canRoleManagerEditCompany) {
    app.Edit(id);
    //} else {
    //    toastr.error('You cannot edit please contact administrator', 'Permission denied!');
    //}
}
function PerformCourseLevelDelete(id) {
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