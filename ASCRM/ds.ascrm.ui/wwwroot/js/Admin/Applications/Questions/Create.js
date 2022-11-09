var router = new VueRouter({
    mode: 'history',
    routes: []
});
var app = new Vue({
    router,
    el: '#dv_Create_Question',
    data: {
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
            q_is_admin:0,
            q_validation_id: 0,
            q_depend_yesno: '',
            q_is_enrolled: 0,
            q_is_confirm: 0,
            q_html_lable:''
        },
        
        parameters: {},

        isSubmitted: false,
    
        CourseLevel: [],
        DropdownHeaders: [],
        Step: [],
        Section: [],
        QuestionType: [],
        q_TypeId: 0,
        Validation:[],
        validationId:0
    },
    methods: {



        GetAllDropdownsHeaders: function () {
         
            $.ajax({
                url: "/api/ApplicationApi/GetApplicationAllDropdownsHeaders",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                var collection = [];
                var obj = new Object();
                //obj.OptHeader_Id = -1;
                //obj.OptHeader_Title = "Please select header name to edit";
                for (var count = 0; count < response.length; count++) {
                    obj = new Object();
                    obj.OptHeader_Id = response[count].OptHeader_Id;
                    obj.OptHeader_Title = response[count].OptHeader_Title;
                    collection.push(obj);
                }

                app.DropdownHeaders = collection;
                var ddl = '';

                ddl = $('#ddl_Options').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.DropdownHeaders);
                    ddl.refresh();
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
           
        },
        GetApplicationSetion: function (id) {
           
            $.ajax({
                url: "/api/ApplicationQuestionsApi/GetApplicationSection",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response != null) {
                    var collection = [];
                    var obj = new Object();
                    
                    for (var count = 0; count < response.length; count++) {
                        obj = new Object();
                        obj.ase_id = response[count].ase_id;
                        obj.ase_title = response[count].ase_title;
                        collection.push(obj);
                    }

                    app.Section = collection;
                    var ddl = '';

                    ddl = $('#ddl_Section').data("kendoDropDownList");
                    if (ddl !== undefined) {
                        ddl.setDataSource(app.Section);
                        ddl.refresh();
                    }


                } else {
                    toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        GetCourseLevel: function () {

           

        },
        ChangeQType: function () {
            debugger;
            if (this.question.q_type != 0) {
                var id = this.question.q_type;
                if (id == 1) {
                    $(".minmax").show();
                    $("#dd_answer").hide();
                    $("#dd_depend").hide();
                    $("#isfuture").hide();
                    $("#YesNoDepend").hide();
                    $("#htmlediter").hide();

                }
                if (id == 2) {
                    $(".minmax").show();
                    $("#dd_answer").hide();
                    $("#dd_depend").show();
                    $("#YesNoDepend").show();
                    $("#htmlediter").hide();

                   // app.GetAllDropdownsHeaders();
                    $("#isfuture").hide();
                }
                if (id == 3) {
                    $(".minmax").hide();
                    $("#dd_answer").show();
                    $("#dd_depend").show();
                    $("#YesNoDepend").show();
                    $("#htmlediter").hide();

                    app.GetAllDropdownsHeaders();
                    $("#isfuture").hide();
                }
                if (id == 4) {
                    $(".minmax").hide();
                    $("#dd_answer").hide();
                    $("#dd_depend").show();
                    $("#YesNoDepend").show();
                    $("#htmlediter").hide();

                    app.GetAllDropdownsHeaders();
                    $("#isfuture").hide();
                }
                if (id == 5) {
                    $("#isfuture").show();
                    $("#dd_depend").show();
                    $("#YesNoDepend").show();

                    $("#dd_answer").hide();
                    $("#htmlediter").hide();

                }
                if (id == 6) {
                    $("#isfuture").hide();
                    $("#dd_depend").hide();
                    $("#dd_answer").hide();
                    $(".minmax").show();
                    $("#YesNoDepend").hide();
                    $("#htmlediter").hide();


                }
                if (id == 7) {
                    $(".minmax").show();
                    $("#YesNoDepend").hide();

                    $("#isfuture").hide();
                    $("#dd_depend").hide();
                    $("#dd_answer").hide();
                    $("#htmlediter").hide();


                }
                if (id == 8)
                {
                    $(".minmax").hide();
                    $("#YesNoDepend").hide();

                    $("#isfuture").hide();
                    $("#dd_depend").hide();
                    $("#dd_answer").hide();
                    $("#htmlediter").hide();

                }
                if (id == 9) {
                    $(".minmax").hide();
                    $("#YesNoDepend").hide();

                    $("#isfuture").hide();
                    $("#dd_depend").hide();
                    $("#dd_answer").hide();
                    $("#htmlediter").hide();

                }
                if (id == 11) {
                    $(".minmax").hide();
                    $("#YesNoDepend").hide();

                    $("#isfuture").hide();
                    $("#dd_depend").hide();
                    $("#dd_answer").show();
                    $("#htmlediter").hide();
                    app.GetAllDropdownsHeaders();

                }
                if (id == 12) {
                    $(".minmax").hide();
                    $("#YesNoDepend").hide();

                    $("#isfuture").hide();
                    $("#dd_depend").hide();
                    $("#dd_answer").hide();
                    $("#dd_step").hide();
                    $("#dd_section").hide();
                    $("#htmlediter").show();

                }
                if (id == 13) {
                    $(".minmax").hide();
                    $("#YesNoDepend").hide();

                    $("#isfuture").hide();
                    $("#dd_depend").hide();
                    $("#dd_answer").hide();
                    $("#dd_step").hide();
                    $("#dd_section").hide();
                    $("#htmlediter").hide();
                }
            }
        },
        Populate: function () {
            var root = this;
            $.ajax({
                url: "/api/LeadApi/GetAllDropdowns",
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                //  app.Countries = response.Countries;


                //app.CourseLevel = response.CourseLevels;

                var ddl = '';

                //ddl = $('#ddl_CourseLevel').data("kendoDropDownList");
                //if (ddl !== undefined) {
                //    ddl.setDataSource(app.CourseLevel);
                //    ddl.refresh();
                //}
               

                app.mounted();

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
            $.ajax({
                url: "/api/QuestionValidationApi/GetQustionValidationAll",
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                //  app.Countries = response.Countries;

                root.Validation = response;


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
            $.ajax({
                url: "/api/ApplicationApi/GetAllCourseLevel",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.CourseLevel = response;

                console.log(app.CourseLevel);
                //app.CourseLevel = response.CourseLevels;

                var ddl = '';

                ddl = $('#ddl_CourseLevel').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.CourseLevel);
                    ddl.refresh();
                }


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });


            $.ajax({
                url: "/api/ApplicationApi/GetQuestionType",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.QuestionType = response;

                console.log(app.QuestionType);
                //app.CourseLevel = response.CourseLevels;

                var ddl = '';

                ddl = $('#ddl_QuestionType').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.QuestionType);
                    ddl.refresh();
                }


            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });




            $.ajax({
                url: "/api/ApplicationQuestionsApi/GetApplicationQuestionStep",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                var collection = [];
                var obj = new Object();
                //obj.OptHeader_Id = -1;
                //obj.OptHeader_Title = "Please select header name to edit";
                //collection.push(obj);
                for (var count = 0; count < response.length; count++) {
                    obj = new Object();
                    obj.as_id = response[count].as_id;
                    obj.as_title = response[count].as_title;
                    if (obj.ase_title != 'undifined') {
                        collection.push(obj);
                    }
                }

                app.Step = collection;
                var ddl = '';

                ddl = $('#ddl_Step').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.Step);
                    ddl.refresh();
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        submitted: function () {
            this.isSubmitted = true;
            
            debugger; // eslint-disable-line
           
            //  this.Company.Company_sales_advisor_id = parseInt($('#ddl_SalesAdvisor').val());
            if (this.question.q_is_confirm != 1) {
                if ($('#ddl_Step').val() === "") {
                    toastr.warning("Please Select Step.", "warning!");
                    return;
                }
            }
            if ($('#ddl_Step').val() != "" && $('#ddl_Section').val() ==="") {
                toastr.warning("Please Select Section.", "warning!");
                return;
            }
            if (this.question.q_type == 12) {
                if (this.question.q_is_confirm == 0) {
                    toastr.warning("Please Select Is Confirmation.", "warning!");
                    return;
                }
                else {
                    this.question.q_html_lable = $("#message").val();
                }

            }
            if ($('#ddl_CourseLevel').val() === "") {
                toastr.warning("Please Select Course Level.", "warning!");
                return;
            }
            
            this.question.q_type = parseInt(this.question.q_type);

            if (this.question.q_type == 0) {
                toastr.warning("Please Select Question type.", "warning!");
                return;
            }
            this.question.q_optheader_title = $("#ddl_Options").data("kendoDropDownList").text();

            if (this.question.q_type == 1) {
                this.question.q_optheader_title = "";
            }
            if (this.question.q_type == 2) {
                this.question.q_maxlength = 0;
                this.question.q_minlength = 0;
            }
            this.question.q_id_appstep = parseInt($('#ddl_Step').val());
            this.question.q_id_appsection = parseInt($('#ddl_Section').val());
            this.question.q_courselevelId = parseInt($('#ddl_CourseLevel').val());
            if ($('#ddl_depend').val() != "") {
                this.question.q_id_dependency = parseInt($('#ddl_depend').val());
            } else {
                this.question.q_id_dependency = 0;
            }
            if (this.question.q_is_admin == "") {
                this.question.q_is_admin = 0;
            }
            
            var data = this.question;

            $.ajax({
                url: "/api/ApplicationQuestionsApi/InsertUpdateQuestion",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response !== null) {
                    toastr.success("Question Added successfully.", "Success!");
                    window.location ="/ApplicationQuestions/Manage"
                }
            });
      
        },


        mounted: function () {
            parameters = this.$route.query;

            if (_questionId !== undefined) {

                parameters.id = _questionId;
                if (parameters.id > 0) {
                    //if (!_canRoleManagerEditCompany) {
                    //    window.location.href = '/Error/PermissionDenied';
                    //}

                    app.parameters.id = parameters.id;
                    $.ajax({
                        url: "/api/ApplicationQuestionsApi/GetQuestionById",
                        data: { id: parameters.id },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        app.question = response;
                        app.GetAllDropdownsHeaders();
                        app.GetCourseLevel();
                        debugger;
                        $("#ddl_CourseLevel").data("kendoDropDownList").value(response.q_courselevelId);
                        //$("#q_typeId").val(response.q_type);
                        if (response.q_type != 12 && response.q_type != 13) {
                            $("#ddl_Step").data("kendoDropDownList").value(response.q_id_appstep);
                            $("#ddl_Options").data("kendoDropDownList").text(response.q_optheader_title);


                            app.GetApplicationSetion(response.q_id_appstep);
                            $("#ddl_Section").data("kendoDropDownList").value(response.q_id_appsection);
                        }
                        ////$("#q_typeId").select
                        //if (response.q_type == 1) {
                        //    $(".minmax").show();
                        //    $("#dd_answer").hide();
                        //    //$("#dd_depend").hide();
                        //}
                        //if (response.q_type == 2) {
                        //    $(".minmax").hide();
                        //    $("#dd_answer").show();
                        //    //$("#dd_depend").show();
                            
                        //}
                        //var i = 1;
                        //if (i == 1) {
                            $("#message").summernote('editor.pasteHTML', response.q_html_lable);
                           
                        //}
                        //i += 1;
                        var id = response.q_type;
                        if (id == 1) {
                            $(".minmax").show();
                            $("#dd_answer").hide();
                            $("#dd_depend").hide();
                            $("#isfuture").hide();
                            $("#YesNoDepend").hide();
                        }
                        if (id == 2) {
                            $(".minmax").show();
                            $("#dd_answer").hide();
                            $("#dd_depend").show();
                            $("#YesNoDepend").show();

                            // app.GetAllDropdownsHeaders();
                            $("#isfuture").hide();
                        }
                        if (id == 3) {
                            $(".minmax").hide();
                            $("#dd_answer").show();
                            $("#dd_depend").show();
                            $("#YesNoDepend").show();

                            app.GetAllDropdownsHeaders();
                            $("#isfuture").hide();
                        }
                        if (id == 4) {
                            $(".minmax").hide();
                            $("#dd_answer").show();
                            $("#dd_depend").show();
                            $("#YesNoDepend").show();

                            app.GetAllDropdownsHeaders();
                            $("#isfuture").hide();
                        }
                        if (id == 5) {
                            $("#isfuture").show();
                            $("#dd_depend").show();
                            $("#YesNoDepend").show();

                            $("#dd_answer").hide();

                        }
                        if (id == 6) {
                            $("#isfuture").hide();
                            $("#dd_depend").hide();
                            $("#dd_answer").hide();
                            $(".minmax").show();
                            $("#YesNoDepend").hide();


                        }
                        if (id == 7) {
                            $(".minmax").show();
                            $("#YesNoDepend").hide();

                            $("#isfuture").hide();
                            $("#dd_depend").hide();
                            $("#dd_answer").hide();

                        }
                        if (id == 8) {
                            $(".minmax").hide();
                            $("#YesNoDepend").hide();

                            $("#isfuture").hide();
                            $("#dd_depend").hide();
                            $("#dd_answer").hide();
                        }
                        if (id == 9) {
                            $(".minmax").hide();
                            $("#YesNoDepend").hide();

                            $("#isfuture").hide();
                            $("#dd_depend").hide();
                            $("#dd_answer").hide();
                        }
                        if (id == 11) {
                            $(".minmax").hide();
                            $("#YesNoDepend").hide();

                            $("#isfuture").hide();
                            $("#dd_depend").hide();
                            $("#dd_answer").show();
                            app.GetAllDropdownsHeaders();

                        }
                        if (id == 12) {
                            $(".minmax").hide();
                            $("#YesNoDepend").hide();

                            $("#isfuture").hide();
                            $("#dd_depend").hide();
                            $("#dd_answer").hide();
                            $("#dd_step").hide();
                            $("#dd_section").hide();
                            $("#htmlediter").show();

                        }
                        if (id == 13) {
                            $(".minmax").hide();
                            $("#YesNoDepend").hide();

                            $("#isfuture").hide();
                            $("#dd_depend").hide();
                            $("#dd_answer").hide();
                            $("#dd_step").hide();
                            $("#dd_section").hide();
                            $("#htmlediter").hide();
                        }

                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });



                }
            }

        },



    }
});
var courseId = 0;
var stepId = 0;
var sectionId =0;
$(".minmax").hide();
$("#dd_answer").hide();
$("#htmlediter").hide();
$("#isfuture").hide();
$("#ddl_CourseLevel").change(function () {
    var id = this.value;
    courseId = id;
})
//$("#dd_depend").hide();

$("#q_typeId").change(function () {
    var id = this.value;
    if (id == 1) {
        $(".minmax").show();
        $("#dd_answer").hide();
        $("#dd_depend").hide();
    }
    if (id == 2) {
        $(".minmax").show();
        $("#dd_answer").hide();
        $("#dd_depend").hide();
      //  app.GetAllDropdownsHeaders();

    }
    if (id == 3) {
        $(".minmax").hide();
        $("#dd_answer").show();
        $("#dd_depend").show();
        app.GetAllDropdownsHeaders();

    }
    if (id == 4) {
        $(".minmax").hide();
        $("#dd_answer").show();
        $("#dd_depend").show();
        app.GetAllDropdownsHeaders();

    }
});

$("#ddl_Step").change(function () {
    var id = this.value;
    stepId = id;
    $.ajax({
        url: "/api/ApplicationQuestionsApi/GetApplicationSection",
        data: { id: id },
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {
        if (response != null) {
            //$("#Lead_CompanyHouseReg").val(response)
            var collection = [];
            var obj = new Object();
            //obj.OptHeader_Id = -1;
            //obj.OptHeader_Title = "Please select header name to edit";
            for (var count = 0; count < response.length; count++) {
                obj = new Object();
                obj.ase_id = response[count].ase_id;
                obj.ase_title = response[count].ase_title;
                collection.push(obj);
            }

            app.Section = collection;
            var ddl = '';

            ddl = $('#ddl_Section').data("kendoDropDownList");
            if (ddl !== undefined) {
                ddl.setDataSource(app.Section);
                ddl.refresh();
            }


        } else {
            toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
        }
    }).fail(function (xhr) {
        toastr.error(xhr.responseText, 'Error!');
    });
});
$("#ddl_Section").change(function () {
    var id = this.value;
    this.sectionId = id;

    if (courseId == 0) {
        toastr.warning("Please Select the Course Level.", "Error!");
        return;
    }


    if (stepId != 0 && this.sectionId != 0) {
        $.ajax({
            url: "/api/ApplicationQuestionsApi/GetApplicationDepend",
            data: {
                stepId: stepId,
                sectionId: this.sectionId,
                courselevelId: courseId
            },
            type: "GET",
            contentType: "application/json",
            dataType: "json"
        }).done(function (response) {
            if (response != null) {
                //$("#Lead_CompanyHouseReg").val(response)
                var collection = [];
                var obj = new Object();
                //obj.OptHeader_Id = -1;
                //obj.OptHeader_Title = "Please select header name to edit";
             ///   collection.push(obj);
                for (var count = 0; count < response.length; count++) {
                    obj = new Object();
                    obj.q_id = response[count].q_id;
                    obj.q_title = response[count].q_title;
                    collection.push(obj);
                }

                app.Section = collection;
                var ddl = '';

                ddl = $('#ddl_depend').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.Section);
                    ddl.refresh();
                }


            } else {
                toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
            }
        }).fail(function (xhr) {
            toastr.error(xhr.responseText, 'Error!');
        });
    }

});