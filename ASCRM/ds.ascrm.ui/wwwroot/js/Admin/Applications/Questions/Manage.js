var app = new Vue({

    el: '#dv_Manage_Question',
    data: {
        isSubmitted: false,
        Questions: [],
        CourseLevel: [],
       
        question_type: '',
        
        CourseLevels1: [],
        CourseLevels2: [],
        course_LevelId: '',
        QuestionLevel:{
            course_LevelToId: '',
            course_LevelFromId: '',
        },
        q_Id: '',
        q_number: '',
        question: {
            q_id: 0,
            q_number: 0,
            q_title: '',
            q_sortorder: 0,
            q_id_appstep: 0,
            q_id_appsection: 0,
            q_type: 0,
            q_optheader_title: '',
            q_maxlength: 0,
            q_minlength: 0,
            q_id_dependency: 0,
            q_is_mandatory: 0,
            q_mapname: 0,
            q_class: '',
            q_courselevelId: 0,             
            q_isfuture: 0,
            q_is_admin: 0,
            q_validation_id: 0,
            q_depend_yesno: '',
            q_is_enrolled: 0

        },


    },
    validations: {
        QuestionLevel: {
         
          
      
          
            course_LevelFromId: {
                required: validators.required
            },
            course_LevelToId: {
                required: validators.required
            }
        }
    },

    methods: {
        refreshData: function () {
            //init to do

            $.ajax({
                url: "/api/ApplicationApi/GetAllCourseLevel",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.CourseLevel = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        Edit: function (questionId) {
            //  alert("Edit Id : " + company_id);
            
            window.location.href = 'ApplicationQuestions/Edit?id=' + questionId;
            
        },
        Delete: function (questionId) {
            $.ajax({
                url: "/api/ApplicationQuestionsApi/DeleteQuestionById",
                data: { id: questionId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#gridCompanies').data('kendoGrid').dataSource.read();
                    $('#gridCompanies').data('kendoGrid').refresh();
                    toastr.success("Question has been deleted successfully.", "Success!");
                } else {
                    toastr.error("Question cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        DeleteHard: function (questionId) {
            $.ajax({
                url: "/api/ApplicationQuestionsApi/DeleteHardQuestionById",
                data: { id: questionId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#gridCompanies').data('kendoGrid').dataSource.read();
                    $('#gridCompanies').data('kendoGrid').refresh();
                    toastr.success("Question has been deleted successfully.", "Success!");
                } else {
                    toastr.error("Question cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        mounted: function () {
            this.refreshData();
        },
        ChangeDropdown: function () {
            if (this.course_LevelId != "") {
                $.ajax({
                    url: "/api/ApplicationQuestionsApi/GetQuestionCourseLevelId",
                    data: { id: this.course_LevelId  },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    $("#gridCompanies").data("kendoGrid").dataSource.data([]);


                    let grid = $("#gridCompanies").data("kendoGrid");

                    for (var i = 0; i < response.Data.length; i++) {

                        grid.dataSource.add({
                            encodedId: response.Data[i].encodedId, id: response.Data[i].id, title: response.Data[i].title, type: response.Data[i].type, step: response.Data[i].step
                            , section: response.Data[i].section, sortOrder: response.Data[i].sortOrder, optheader_title: response.Data[i].optheader_title
                        });

                    }
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
            }
        },
        ChangeType: function () {
            if (this.question_type != "") {
                $.ajax({
                    url: "/api/ApplicationQuestionsApi/GetQuestionByType",
                    data: { id: this.question_type },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    $("#gridCompanies").data("kendoGrid").dataSource.data([]);


                    let grid = $("#gridCompanies").data("kendoGrid");

                    for (var i = 0; i < response.Data.length; i++) {

                        grid.dataSource.add({
                            encodedId: response.Data[i].encodedId, id: response.Data[i].id, title: response.Data[i].title, type: response.Data[i].type, step: response.Data[i].step
                            , section: response.Data[i].section, sortOrder: response.Data[i].sortOrder, optheader_title: response.Data[i].optheader_title
                        });

                    }

                    //if (response === true) {
                    //    $('#gridCompanies').data('kendoGrid').dataSource.read();
                    //    $('#gridCompanies').data('kendoGrid').refresh();
                    //    toastr.success("Question has been deleted successfully.", "Success!");
                    //} else {
                    //    toastr.error("Question cannot be deleted. Please try again.", "Error!");
                    //}
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
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
            var root = this;

            $.ajax({
                url: "/api/ApplicationApi/GetAllCourseLevel",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                debugger;
                root.CourseLevels1 = response;
                root.CourseLevels2 = response;
                console.log(root.CourseLevels1);
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        AssignBulik: function () {
        
            $("#kt_modal_template").modal("show");

        },
        BulkQuestion: function () {
            if (this.QuestionLevel.course_LevelFromId == this.QuestionLevel.course_LevelToId) {
                toastr.error("Please Select Differnet Level", 'Error!');

                return;
            }
            if (this.QuestionLevel.course_LevelFromId == "") {
                toastr.error("Please Select Level From");
                return;
            }
            if (this.QuestionLevel.course_LevelToId == "") {
                toastr.error("Please Select Level To");
                return;
            }
            var levelFromId = parseInt(this.QuestionLevel.course_LevelFromId);
            var levelToId = parseInt(this.QuestionLevel.course_LevelToId);

            $.ajax({
                url: "/api/ApplicationQuestionsApi/AssignBulk",
                data: {
                    levelFromId: levelFromId,
                    levelToId: levelToId
                },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                toastr.success("Question has been Assgined successfully.", "Success!");
                $("#kt_modal_template").modal("hide");

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        SingleBulk: function (questionId) {
            $.ajax({
                url: "/api/ApplicationQuestionsApi/GetQuestionById",
                data: { id: questionId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                //    app.question = response;
                app.q_number = response.q_number;
                $("#kt_modal_template_Single").modal("show");

            })
        },
        QuestionRemove: function (id) {
            var levelId = this.course_LevelId;
            if (levelId == "") {
                toastr.error("Please Select The Course Level", "warning");
                  return;
            }

            $.ajax({
                url: "/api/ApplicationQuestionsApi/RemoveQuestion?id=" + id + "&levelId=" + levelId,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                this.refreshData();

                //    app.question = response;
                //app.q_number = response.q_number;
                //if (response == true) {
                //    toastr.error("This Question Already Available", "warning");
                //    return;
                //}
                //else {
                //    $("#kt_modal_template_Single").modal("hide");

                //}

            })
        },
        SingleBulkQuestion: function () {
            $.ajax({
                url: "/api/ApplicationQuestionsApi/CheckQuestionExist",
                data: { levelId: app.QuestionLevel.course_LevelToId, number: this.q_number },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                //    app.question = response;
                //app.q_number = response.q_number;
                if (response == true) {
                    toastr.error("This Question Already Available", "warning");
                    return;
                }
                else {
                    $("#kt_modal_template_Single").modal("hide");

                }

            })
        },
        QuestionCopy: function (questionId) {
            $.ajax({
                url: "/api/ApplicationQuestionsApi/GetQuestionById",
                data: { id: questionId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                const d = new Date().toLocaleString('en-GB');

                app.question = response;
                app.question.q_title = "Copy " + d +" "+ app.question.q_title;
                var data = app.question;

                $.ajax({
                    url: "/api/ApplicationQuestionsApi/InsertCopyQuestion",
                    data: JSON.stringify(data),
                    type: "Post",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    if (response !== null) {
                        toastr.success("Question Copy successfully.", "Success!");
                    //    window.location = "/ApplicationQuestions/Manage"
                    }
                });
            })
        },
        PreviewApp: function () {
            var levelId = app.course_LevelId;
            if (levelId == "") {
                toastr.error("Please Select Course Level", "warning");
                return;
            }
            $.ajax({
                url: "/api/ApplicationQuestionsApi/PreviewApp?levelId=" + levelId ,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                debugger
                if (response.isApp == true) {
                    window.location = "/Application/AppPreview?id=" + response.encoded_app_id;

                }
                else {
                    toastr.error("Application not Available ", "warning");
                    return;
                }

            })
        }
    },
    mounted() {
       
        this.refreshData();
        this.GetCourseLevels();
    }
});
function PerformQuestionEdit(questionId) {
    //if (_canRoleManagerEditCompany) {
    app.Edit(questionId);
    //} else {
    //    toastr.error('You cannot edit please contact administrator', 'Permission denied!');
    //}
}
function PerformQuestionDelete(questionId) {
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
            app.Delete(questionId);
        }
    });
}
function PerformQuestionRemove(id) {
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
            app.QuestionRemove(id);
        }
    });

}
function PerformQuestionSingleBulk(questionId) {
    //if (_canRoleManagerEditCompany) {
    app.SingleBulk(questionId);
    //} else {
    //    toastr.error('You cannot edit please contact administrator', 'Permission denied!');
    //}
}
function PerformQuestionCopy(questionId) {
    //if (_canRoleManagerEditCompany) {
    app.QuestionCopy(questionId);
    //} else {
    //    toastr.error('You cannot edit please contact administrator', 'Permission denied!');
    //}
}
function PerformQuestionHardDelete(id) {
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
            app.DeleteHard(id);
        }
    });
}

