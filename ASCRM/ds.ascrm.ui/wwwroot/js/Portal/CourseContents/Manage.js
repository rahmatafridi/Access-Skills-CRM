
Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_CourseContents',
    data: {
        CourseLevels: [],
        courseContent: {
            cc_id_course_level: 0,
            cc_sortorder: 0,
            cc_description: '',
            cc_name: '',
            cc_file_size: 0,
            cc_type: '',
            file_path: '',
            cc_id:0
        },
        courses: [],
        courseLevelId:0
    },
    validations: {

    },
    methods: {
        CourseChange: function () {
            $("#gridCourseContent").data("kendoGrid").refresh();

            var id = app.courseLevelId;
            if (id != "") {
                $.ajax({
                    url: "/api/PortalCourseContentApi/GetCourseContents",
                    data: {  id: id },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json",
                    async: false
                }).done(function (response) {
                    $("#gridCourseContent").data("kendoGrid").dataSource.data([]);
                    console.log(response);

                    let grid = $("#gridCourseContent").data("kendoGrid");

                    for (var i = 0; i < response.length; i++) {

                        grid.dataSource.add({
                            CC_Id: response[i].CC_Id, CC_Name: response[i].CC_Name,
                            CC_Description: response[i].CC_Description
                            , CC_Type: response[i].CC_Type,
                            CC_File_Size: response[i].CC_File_Size
                            , CC_SortOrder: response[i].CC_SortOrder
                            , CC_FilePath: response[i].CC_FilePath

                        });

                    }


                });
            }
            else {
                $("#gridCourseContent").data("kendoGrid").dataSource.read();
            }
        },
        GetCourseLevels: function () {
            //$.ajax({
            //    url: "/api/ListApi/GetDropdownOptionsByHeaderName",
            //    type: "GET",
            //    contentType: "application/json",
            //    data: { headerName: "CourseLevels" },
            //    dataType: "json"
            //}).done(function (response) {
            //    app.CourseLevels = response;
            //}).fail(function (xhr) {
            //    toastr.error(xhr.responseText);
            //});


            $.ajax({
                url: "/api/PortalCourseContentApi/GetCourses",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.CourseLevels = response;
                console.log(app.CourseLevels);
              
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        AddCourseContent: function () {
            if (!_canRoleManagerAccessContentCreate) {
                toastr.error('You cannot Create please contact administrator', 'Permission denied!');
                return;
            } else {
                app.GetCourseLevels();
                var upload = $("#files").data("kendoUpload");
                upload.clearAllFiles();
                $("#kt_modal_Create").modal("show");
            }
        },
        Submit: function () {

            var formData = new FormData();
            var upload = $("#files").getKendoUpload();
            var files = upload.getFiles();
            
            if (files.length === 0) {
                toastr.error('Please select file first', 'Error!');
                return;
            }

            if (files.length > 0) {
                for (var count = 0; count < files.length; count++) {
                    formData.append('files', files[count].rawFile);
                }
            }
            else {
                formData.append('files', files[0].rawFile);
            }
            var data = this.courseContent;

            formData.append("documentData", JSON.stringify(data));

            $.ajax({
                url: "/api/PortalCourseContentApi/UploadDocumentsAjax",
                data: formData,
                type: "Post",
                contentType: false,
                processData: false
            }).done(function (response) {
                if (response === true) {
                    $('#gridCourseContent').data('kendoGrid').dataSource.read();
                    $('#gridCourseContent').data('kendoGrid').refresh();
                    toastr.success("Course Content Sumitted successfully.", "Success!");
                    
                    app.courseContent.cc_description = "";
                    app.courseContent.cc_id = 0;
                    app.courseContent.cc_name = "";
                    app.courseContent.cc_sortorder =0;
                    app.courseContent.cc_id_course_level = 0;
                    $("#kt_modal_Create").modal("hide");

                }
            })

        },
        Delete: function (leadId) {
            debugger;

            $.ajax({
                url: "/api/PortalCourseContentApi/DeleteCourseContentById",
                data: { id: leadId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#gridCourseContent').data('kendoGrid').dataSource.read();
                    $('#gridCourseContent').data('kendoGrid').refresh();
                    toastr.success("Course Content has been deleted successfully.", "Success!");
                } else {
                    toastr.error("Course Content cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        Edit: function (id) {

            $.ajax({
                url: "/api/PortalCourseContentApi/GetCourseContentById",
                type: "GET",
                data: { id: id },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.GetCourseLevels();
                app.courseContent = response;
                app.courseContent.cc_description = response.CC_Description;
                app.courseContent.cc_id = response.CC_Id;
                app.courseContent.cc_name = response.CC_Name;
                app.courseContent.cc_sortorder = response.CC_SortOrder;
                app.courseContent.cc_id_course_level = response.CC_Id_Course_Level;
                console.log(app.courseContent);
                $("#kt_modal_Create").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        downlaodFile: function (path) {
            window.location.href = '/api/PortalCourseContentApi/DownloadFile?id=' + path;

        }
        
    }
})
function PerformCourseContentDelete(id) {
    //if (!_canRoleManagerDeleteLead) {
    //    toastr.error('You cannot delete please contact administrator', 'Permission denied!');
    //    return false;
    //}
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
function PerformCourseContentEdit(id) {
    app.Edit(id);
}
app.GetCourseLevels();
function PerformDelete(id) {
    if (!_canRoleMangerAccessContentDelete) {
        toastr.error('You cannot delete please contact administrator', 'Permission denied!');
        return;
    }
    else {
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

   
   // app.Delete(id);
}
function PerformEdit(id) {
    if (!_canRoleManagerAccessContentCreate) {
        toastr.error('You cannot edit please contact administrator', 'Permission denied!');
        return;
    } else {
        app.Edit(id);
    }
}
function PerformDownlaod(id) {
    app.downlaodFile(id);
}