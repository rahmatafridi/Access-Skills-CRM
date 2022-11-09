
Vue.use(vuelidate.default)

var router = new VueRouter({
    mode: 'history',
    routes: []
});

var app = new Vue({
    router,
    el: '#dv_Create_Lead',
    data: {
        lead: {
            Lead_ContactName: '',
            ContactName: '',
            Lead_Contact_FirstName: '',
            Lead_Contact_LastName: '',
            Lead_Contact_Company_Name: '',
            Lead_Contact_Id_JobTitle: null,
            Lead_Contact_Id_Department: 0,
            Lead_Contact_Salutation: null,
            Lead_Contact_Phone: '',
            Lead_Contact_Mobile: '',
            Lead_Contact_Email: '',
            Lead_Contact_Email2: '',
            Lead_Id_LastResult: 0,
            Lead_Contact_Address1: '',
            Lead_Contact_Id_Pathway: 0,
            Lead_Contact_Id_Registration:-1,
            Lead_Contact_Postcode: '',
            Lead_Contact_County: '',
            Lead_Contact_Id_Country: 0,
            Lead_Contact_Website: '',
            Lead_Contact_WebSubject: '',
            Lead_Contact_WebMessage: '',
            Lead_Id_CourseLevel: 1,
            Lead_DateOfEnquiry: '',
            Lead_DealClosedDate: '',
            Lead_Id_SourceOfEnquiry: 0,
            Lead_Id_SalesAdvisor: 0,
            Lead_DateClosed: '',
            Lead_IsDealClosed: 0,
            Lead_Id_TrainingProvider: -1,
            Lead_LearnersEnrolled: -1,
            Lead_PreferredTimeToContact: '',
            Lead_DateCancellation: '',
            Lead_Id_Client: null,
            Lead_Email: '',
            Lead_IsCallAttempt: false,
            Lead_IsCallReach: false,
            Lead_IsMeeting: false,
            Lead_IsLetterSent: false,
            Lead_IsLevyPayingEmployer: false,
            Lead_IsEnrolmentCancelled: false,
            Lead_Id_Status: 1,
            Lead_Contact_Company_Postcode: '',
            Lead_is_validated_duplicate: false,
            Lead_IsDuplicate: false,
            Lead_Id_CourseLevel_Enquiry: '',
            courseLevelEnquiryList: '',
            Lead_Learner_Enrolment_Date: null,
            Lead_ERN_Number: '',
            Lead_Enrolment_Cancelled: -1,
            Lead_Enrolment_Cancelled_Date: null,
            Lead_CompanyHouseReg: '',
            Lead_CompanyLineManagerContactName: '',
            Lead_CompanyDecisionMakerName: '',
            Lead_CompanyEmail: '',
            Company_Id: '',
            Company_WorkPlace_Id: '',
            lastResultSelect: '',
            oldLastResult: '',
            newLastResult: ''
        },
        leadWorkPlace: {
            WorkPlace_Contact_Address1: '',
            WorkPlace_Contact_Address2: '',
            WorkPlace_Contact_Twon: '',
            WorkPlace_Contact_Email: '',
            WorkPlace_Contact_County: '',
            WorkPlace_Contact_Postcode: '',



        },
        duplicateLead: {
            Lead_Id: 0,
            LD_Id_Lead: 0,
            LD_Id: 0,
            Duplicate_Lead_Id: 0,
            ContactName: '',
            Duplicate_ContactName: '',
            Mobile: '',
            Duplicate_Mobile: '',
            CourseLevel: '',
            Duplicate_CourseLevel: '',
            LastResult: '',
            Duplicate_LastResult: '',
            Status: '',
            Duplicate_Status: '',
            EnquiryDate: '',
            Duplicate_EnquiryDate: '',
            Email: '',
            Duplicate_Email: '',
            Sales_Advisor_Id: '',
            Duplicate_Sales_Advisor_Id: '',
            Duplicate_Lead_Contact_Company_Postcode: '',
            Duplicate_Lead_Contact_Company_Name: '',
            Lead_Contact_Company_Postcode: '',
            Lead_Contact_Company_Name: '',
            Lead_is_validated_duplicate: 0,
            Duplicate_is_validated_duplicate: 0
        },
        isSubmitted: false,
        isNotesSubmitted: false,
        isDocumentSubmitted: false,
        isActivitySubmitted: false,
        isCSLSubmitted: false,
        isTaskSubmitted: false,
        JobTitles: [],
        Departments: [],
        Company:[],
        Salutations: [],
        LastResults: [],
        Pathways: [],
        Registrations: [],
        Countries: [],
        Courses: [],
        CourseLevels: [],
        CourseLevelEnquiries: [],
        LearnerEnrolleds: [],
        Sources: [],
        SalesAdvisors: [],
        DealClosedOption: [],
        TrainingProviders: [],
        Contacts: [],
        Note: { Note_Id: 0, Note_Id_Lead: 0, Note_Description: '', Note_Id_Category: null },
        Notes: [],
        Document: {
            Document_Id: 0,
            Document_Id_Lead: 0,
            Document_Name: '',
            Document_Description: '',
            Document_FilePath: '',
            Document_Id_Category: ''
        },
        parameters: {},
        NotesCategories: {},
        Activity: { Activity_Id: 0, Activity_Id_Lead: 0, Activity_Id_Type: 0, Activity_Id_Action: 0, Activity_Reminder_Date: '', Activity_Note: '', Activity_EmailBody: '', Activity_Date: null, Activity_Location: null },
        Activity_Id_DocCategory: 0,
        ActivityTypes: [],
        EmailTemplates: [],
        LetterTemplates: [],
        ActivityActions: [],
        DocumentCategories: [],
        DefaultDocuments: [],
        DefaultAttachments: [],
        Activity_DefaultDocument: null,
        LeadStatuses: [],
        Task: {
            task_id: 0,
            task_id_lead: 0,
            task_id_user: 0,
            task_id_optheader: 0,
            task_id_activity: null,
            task_scheduled_with: '',
            task_note: '',
            task_is_done: false,
            task_start: null,
            task_end: null
        },
        TaskActivities: [],
        CSL: {
            CSL_Id: 0,
            CSL_Id_Lead: 0,
            CSL_Id_OptHeader: 0,
            CSL_Id_Outcome: 0,
            CSL_Note: '',
            CSL_Date: null
        },
        CSLOutcomes: [],
        AppInvite: { lead_id:0,api_id: 0, api_id_courselevel: null, api_courseleveltitle: null, api_email: null, api_firstname: null, api_lastname: null, api_emailbody: null, api_password: null },
        //CourseLevels: [],
        isInviteSubmitted: false,
        htmlEmail: '',
        WorkPlace: [],
        compId: '',
        workplaceId: '',
        roleName: '',
        lastResultSelect: '',
        OldLead: [],
        oldLastResult: '',
        newLastResult: '',
        lastResultForSa: '',
        CourseLevel: [],
        CourseLevel2: []


    
    },
    validations: {
        lead: {
            Lead_ContactName: {
                required: validators.required
            }
            //,Lead_Id_CourseLevel: {
            //    required: validators.required
            //}
        },
        Note: {
            Note_Description: {
                required: validators.required
            },
            Note_Id_Category: {
                required: validators.required
            }
        },
        Document: {
            Document_Name: {
                required: validators.required
            },
            //Document_Id_Category: {
            //    required: validators.required
            //},
            Document_Description: {
                required: validators.required
            }
        },
        Activity: {
            Activity_Id_Type: {
                required: validators.required
            },
            Activity_Id_Action: {
                required: validators.required
            },
            //Activity_Reminder_Date: '',
            //Activity_Note: {
            //    required: validators.required
            //},
            //Activity_EmailBody: '',
            //Activity_Date: null,
            //Activity_Location: null
            //Activity_Note: {
            //    required: requiredIf(function (abc) {
            //        return abc > 10 && abc < 20
            //    })
            //},
        },
        CSL: {
            CSL_Id_Outcome: {
                required: validators.required
            },
        },
        Task: {
            task_id_activity: {
                required: validators.required
            },
            task_scheduled_with: {
                required: validators.required
            },
            task_start: {
                required: validators.required
            },
        },
        AppInvite: {
            api_firstname: {
                required: validators.required
            },
            api_lastname: {
                required: validators.required
            },
            api_email: {
                required: validators.required
            },
            api_password: {
                required: validators.required
            },
            api_id_courselevel: {
                required: validators.required
            }
        }

    },
    methods: {
        GetRandom8DigitPassword: function () {
            $.ajax({
                url: "/api/ApplicationInviteApi/GetRandom8DigitPassword",
                type: "GET",
                contentType: "application/json",
                //dataType: "json"
            }).done(function (response) {
                app.AppInvite.api_password = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText);
            });
        },
        NewInvites: function () {

            app.isInviteSubmitted = false;
            //app.AppInvite = { api_id: 0, api_id_courselevel: null, api_courseleveltitle: null, api_email: null, api_firstname: null, api_lastname: null, api_emailbody: null, api_password: null };
            //if (!_canRoleManagerCreateAppInvite) {
            //    toastr.error('You cannot add new invite please contact administrator', "Permission denied!");
            //    return false;
            //}
            //app.GetRandom8DigitPassword();
            $("#kt_modal_template").modal("show");
        },
        ChangeContact: function (event) {
        },
        refreshData: function () {
            this.isSubmitted = true;
        },
        Populate: function () {
            $.ajax({
                url: "/api/LeadApi/GetAllDropdowns",
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.JobTitles = response.JobTitles;
                app.Departments = response.Departments;
                app.Company = response.Company;
                app.LastResults = response.LastResults;
                app.Pathways = response.Pathways;
                app.Registrations = response.Registrations;
                app.Countries = response.Countries;
                app.Sources = response.Sources;
                app.DealClosedOption = response.DealClosedOption;
                app.NotesCategories = response.NotesCategories;
                app.DocumentCategories = response.DocumentCategories;
                app.LeadStatuses = response.LeadStatuses;
                app.CourseLevels = response.CourseLevels;
                app.CourseLevelInquiries = response.CourseLevelInquiries;
                app.TrainingProviders = response.TrainingProviders;
                app.Salutations = response.Salutations;
                app.CourseLevelEnquiries = response.CourseLevelEnquiries;
                app.LearnerEnrolleds = response.LearnerEnrolleds;
                app.SalesAdvisors = response.SalesAdvisors;

                $('#ddlLearnerEnrolled').append("<option value='-1'>Please Select</option>");
                $('#ddlRegistration').append("<option value='-1'>Please Select</option>");
                $('#ddlAgreedPaymentScheme').append("<option value='-1'>Please Select</option>");
                $('#ddlEnrolmentCancelled').append("<option value='-1'>Please Select</option>");
                 app.CourseLevels = response.CourseLevels;

                var ddl = '';

                ddl = $('#ddlSourceOfEnquiry').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.Sources);
                    ddl.refresh();
                }

                ddl = $('#ddlDepartment').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.Departments);
                    ddl.refresh();
                }
                ddl = $('#ddl_Company').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.Company);
                    ddl.refresh();
                }
                ddl = $('#ddl_CourseLevel').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.CourseLevels);
                    ddl.refresh();
                }

                ddl = $('#ddl_CourseLevelEnquiry').data("kendoMultiSelect");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.CourseLevelEnquiries);
                    ddl.refresh();
                }

                ddl = $('#ddlJobTitle').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.JobTitles);
                    ddl.refresh();
                }

                ddl = $('#ddl_LastResults').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.LastResults);
                    ddl.refresh();
                }

                ddl = $('#ddl_SalesAdvisor').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.SalesAdvisors);
                    ddl.refresh();
                }

                if ($("#hfd_roleName").val() !== "Admin" && $("#hfd_roleName").val() !== "Administrator") {
                    $("#ddl_SalesAdvisor").data("kendoDropDownList").value($("#hfd_userId").val());
                }

                app.mounted();

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });


          
        },
        PerformInviteEdit: function (aPI_Id) {
            //if (!_canRoleManagerEditAppInvite) {
            //    toastr.error('You cannot edit please contact administrator', 'Permission denied!');
            //    return false;
            //}
            //app.CancelEdit();

            $.ajax({
                url: "/api/ApplicationInviteApi/GetApplicationInviteById",
                data: { id: aPI_Id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                //console.log(response);
                app.AppInvite.api_id = response.api_id;
                app.AppInvite.api_id_courselevel = response.api_id_courselevel;
                $("#kt_modal_edit_course_level").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        EditAppInvite: function () {

            this.isInviteSubmitted = true;

            //if (this.$v.AppInvite.api_id_courselevel.$invalid) {
            //    toastr.error('Error in validation.', "Error!");
            //    return;
            //}
            var data = this.AppInvite;
            data.api_courseleveltitle = $("#ddl_CourseLevel option:selected").text();
            data.lead_id = parameters.id;

          //  app.isExists = false;
            $.ajax({
                url: "/api/ApplicationInviteApi/InsertUpdateApplicationInvite",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    app.isSubmitted = false;
                    $("#kt_modal_edit_course_level").modal("hide");
                    toastr.success("Invite updated successfully.", "Updated!");
                    app.CancelEdit();
                    $('#templateGrid').data('kendoGrid').dataSource.read();
                    $('#templateGrid').data('kendoGrid').refresh();
                } else {
                    toastr.error("Record have not been saved. Please try again.", 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        AddEditAppInvite: function () {

            this.isInviteSubmitted = true;
            debugger; // eslint-disable-line
            if (this.$v.AppInvite.$invalid) {
                toastr.error('Error in validation.', "Error!");
                return;
            }
            var data = this.AppInvite;
            data.api_courseleveltitle = $("#ddl_CourseLevel2 option:selected").text();
            data.lead_id = parameters.id;
            $.ajax({
                url: "/api/UserAdminApi/CheckEmailAlreadyExists",
                data: JSON.stringify({ Users_ID: 0, Users_UserName: data.api_email}),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === false) {
                    app.isExists = false;
                    $.ajax({
                        url: "/api/ApplicationInviteApi/InsertUpdateApplicationInvite",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        if (response === true) {
                            app.isSubmitted = false;
                            $("#kt_modal_template").modal("hide");
                            if (app.AppInvite.api_id > 0) {
                                toastr.success("Invite updated successfully.", "Updated!");
                            } else {
                                toastr.success("Invite added successfully.", "Inserted!");
                            }
                            app.CancelEdit();
                            $('#templateGrid').data('kendoGrid').dataSource.read();
                            $('#templateGrid').data('kendoGrid').refresh();
                            app.GetRandom8DigitPassword();
                        } else {
                            toastr.error("Record have not been saved. Please try again.", 'Error!');
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
        PerformInviteView: function (aPI_Id) {

            $.ajax({
                url: "/api/ApplicationInviteApi/GetApplicationInviteById",
                data: { id: aPI_Id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.htmlEmail = response.api_emailbody;
                $("#kt_modal_invite_view").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformInviteDelete: function (aPI_Id) {

            //if (!_canRoleManagerDeleteAppInvite) {
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
                if (!result.value) {
                    return;
                }
                $.ajax({
                    url: "/api/ApplicationInviteApi/DeleteApplicationInviteById",
                    data: { id: aPI_Id },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json"
                }).done(function (response) {
                    if (response === true) {
                        $('#templateGrid').data('kendoGrid').dataSource.read();
                        $('#templateGrid').data('kendoGrid').refresh();
                        toastr.success('Application invite deleted successfully.', 'Deleted');
                    } else {
                        toastr.error('Application invite cannot be deleted. Please try again.', 'Deleted');
                    }
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Deleted');
                });
            });
        },
        CancelEdit: function () {
            app.isInviteSubmitted = false;
            app.AppInvite = { api_id: 0, api_id_courselevel: null, api_courseleveltitle: null, api_email: null, api_firstname: null, api_lastname: null, api_emailbody: null, api_password: null };
        },

        NewNote: function () {
            app.isNotesSubmitted = false;
            app.Note = {};
           // app.RefreshNotes();
            if (!_canRoleManagerCreateNotes) {
                toastr.error('You cannot add new note please contact administrator', 'Permission denied!');
                return false;
            }
            $('.nav-tabs a[href="#kt_tabs_1_4"]').tab('show');
            $("#kt_modal_notes").modal("show");
        },
        RefreshNotes: function () {
            $('#notesGrid').data('kendoGrid').dataSource.read();
            $('#notesGrid').data('kendoGrid').refresh();
            refreshHistory();
        },
        submitNotes: function () {
            this.isNotesSubmitted = true;
            if (this.$v.Note.$invalid) {
                toastr.error('Error in validation.', 'Error!');
                return;
            }
            this.Note.Note_Id_Lead = parameters.id;
            var data = this.Note;
            $.ajax({
                url: "/api/LeadApi/InsertLeadNotes",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    if (app.Note.Note_Id > 0) {
                        toastr.success("Note updated successfully.", "Updated!");
                    } else {
                        toastr.success("Note inserted successfully.", "Inserted!");
                    }
                    $("#kt_modal_notes").modal("hide");
                    app.isNotesSubmitted = false;
                    app.Note = {};
                    app.RefreshNotes();
                } else {
                    toastr.error("Cannot submitted, please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformNoteEdit: function (id) {
            $.ajax({
                url: "/api/LeadApi/GetNoteById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.Note = response;
                $("#kt_modal_notes").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformNoteDelete: function (id) {
            $.ajax({
                url: "/api/LeadApi/DeleteNoteById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                if (response === true) {
                    app.RefreshNotes();
                    toastr.success("Note deleted successfully.", "Success!");
                } else {
                    toastr.error("Note cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GetLastResultForSa: function (value, title) {
            var root = this;
            $.ajax({
                url: "/api/LeadApi/GetLastResultForSa?value="+value +"&title="+title,
            
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);
                root.lastResultForSa = response.Opt_Title;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        submitted: function (type) {
            var root = this;
            //console.log(this.lead.Lead_Id_TrainingProvider); 
            this.isSubmitted = true;
            debugger; // eslint-disable-line
            this.lead.Lead_Id_SalesAdvisor = parseInt($('#ddl_SalesAdvisor').val());
            this.lead.Lead_Id_SourceOfEnquiry = parseInt($('#ddlSourceOfEnquiry').val());
            this.lead.Lead_Contact_Id_Department = parseInt($('#ddlDepartment').val());
            this.lead.Lead_Contact_Id_JobTitle = parseInt($('#ddlJobTitle').val());
            this.lead.Lead_Id_LastResult = parseInt($('#ddl_LastResults').val());
            this.lead.courseLevelEnquiryList = $("#ddl_CourseLevelEnquiry").data("kendoMultiSelect").value().toString();
            this.lead.Lead_Id_CourseLevel = parseInt($('#ddl_CourseLevel').val());
            this.lastResultSelect = $('#ddl_LastResults').data("kendoDropDownList").text();

            root.newLastResult = this.lead.Lead_Id_LastResult;
            var companyId = $('#ddl_Company').val();
            if (companyId != "") {
                this.lead.Company_Id = parseInt($('#ddl_Company').val());
            }
            else {
                this.lead.Company_Id = 0;
            }
          


            var job = $('#ddl_WorkPlace').val();
            if (job != "") {
                this.lead.Company_WorkPlace_Id = parseInt($('#ddl_WorkPlace').val());
            }
            else {
                this.lead.Company_WorkPlace_Id = 0;
            }
            if (this.lead.Company_WorkPlace_Id == NaN) {
                this.lead.Company_WorkPlace_Id = 0;
            }

           // this.lead.Company_WorkPlace_Id = parseInt($('#ddl_WorkPlace').val());

            if (this.lastResultSelect == "Eligible - sent information") {
                if (this.lead.Lead_Id_TrainingProvider == -1 || this.lead.Lead_Id_TrainingProvider == null) {
                    toastr.error('Agreed Payment Scheme  is Required', 'Error!');
                    return;
                }
              }


            if (this.lead.Lead_Id != "") {

                if (app.roleName == "Sales Advisor") {
                    
                    if (this.lastResultSelect == "Enrolled on course" || this.lastResultSelect == "Registered to CPD/ Short courses")
                    {

                        if (this.lastResultForSa == "Enrolled on course" && this.lastResultSelect == "Enrolled on course") {

                        }
                        else if (this.lastResultForSa == "Registered to CPD/ Short courses" && this.lastResultSelect == "Registered to CPD/ Short courses") { }
                        else {
                            toastr.error('You are not allowed to change to this Last Results status. Please contact your supervisor.', 'Error!');
                            return;
                        }
                    }
                }

            }


            if (this.$v.lead.$invalid)
            {
                toastr.error('Error in validation', 'Error!');
                return;
            }

            this.lead.Lead_CompanyLineManagerContactName = $('#Lead_CompanyLineManagerContactName').val();
            this.lead.Lead_CompanyDecisionMakerName = $('#Lead_CompanyDecisionMakerName').val();
            this.lead.Lead_CompanyEmail = $('#Lead_CompanyEmail').val();
            this.lead.Lead_Contact_Address1 = $('#Lead_Contact_Address1').val();
            this.lead.Lead_Contact_Address2 = $('#Lead_Contact_Address2').val();
            this.lead.Lead_Contact_Address3 = $('#Lead_Contact_Address3').val();
            this.lead.Lead_Contact_Company_Postcode = $('#Lead_Contact_Company_Postcode').val();
            this.lead.Lead_Contact_County = $('#Lead_Contact_County').val();
            this.lead.Lead_Contact_TownCity = $('#Lead_Contact_TownCity').val();
            this.lead.Lead_CompanyHouseReg = $('#Lead_CompanyHouseReg').val();

            this.lead.Lead_Id_SalesAdvisor = this.lead.Lead_Id_SalesAdvisor || 0;
            this.lead.Lead_Id_SourceOfEnquiry = this.lead.Lead_Id_SourceOfEnquiry || 0;
            this.lead.Lead_Contact_Id_Department = this.lead.Lead_Contact_Id_Department || 0;
            this.lead.Lead_Contact_Id_JobTitle = this.lead.Lead_Contact_Id_JobTitle || 0;
            this.lead.Lead_Id_LastResult = this.lead.Lead_Id_LastResult || 0;
            this.lead.Lead_Id_CourseLevel = this.lead.Lead_Id_CourseLevel || 0;

            // if (this.lead.Lead_Id_CourseLevel === 0) {
            //    toastr.error('Course is required field', 'Error!');
            //    return;

            //}
            this.lead.lastResultSelect = this.lastResultSelect;
            console.log(root.newLastResult);
            if (isNaN(root.newLastResult)) {
                this.lead.newLastResult = 0;
            }
            else {
                this.lead.newLastResult = root.newLastResult;
            }
           
            this.lead.oldLastResult = this.oldLastResult;
            var data = this.lead;

            $.ajax({
                url: "/api/LeadApi/CheckDuplicateLead",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                if (response.Duplicate_Lead_Id > 0)
                {
                    if (data.Lead_Id > 0) {
                        app.GetDuplicateLead(data.Lead_Id);
                    }
                    else {
                        app.duplicateLead = response;

                        if (response.Lead_Id > 0)
                        {
                            if (response.Lead_Id === response.Duplicate_Lead_Id)
                            {
                                $("#txtLead_Id").addClass("text-danger");
                                $("#txtDuplicate_Lead_Id").addClass("text-danger");
                            }
                            else
                            {
                                $("#txtLead_Id").removeClass("text-danger");
                                $("#txtDuplicate_Lead_Id").removeClass("text-danger");
                            }
                            $("#txtLead_Id").val(response.Lead_Id);
                        }

                        $("#txtDuplicate_Lead_Id").val(response.Duplicate_Lead_Id);

                        $("#txtContactName1").val(response.ContactName);
                        $("#txtDuplicate_ContactName").val(response.Duplicate_ContactName);

                        if (response.ContactName !== "" &&  response.Duplicate_ContactName !== "")
                        {
                            if (response.ContactName === response.Duplicate_ContactName) {
                                $("#txtContactName1").addClass("text-danger");
                                $("#txtDuplicate_ContactName").addClass("text-danger");
                            }
                            else {
                                $("#txtContactName1").removeClass("text-danger");
                                $("#txtDuplicate_ContactName").removeClass("text-danger");
                            }
                        }

                        $("#txtMobile").val(response.Mobile);
                        $("#txtduplicateLead_Mobile").val(response.Duplicate_Mobile);

                        if (response.Mobile !== "" && response.Duplicate_Mobile !== "") {
                            if (response.Mobile === response.Duplicate_Mobile) {
                                $("#txtMobile").addClass("text-danger");
                                $("#txtduplicateLead_Mobile").addClass("text-danger");
                            }
                            else {
                                $("#txtMobile").removeClass("text-danger");
                                $("#txtduplicateLead_Mobile").removeClass("text-danger");
                            }
                        }


                        $("#txtEmail").val(response.Email);
                        $("#txtDuplicate_Email").val(response.Duplicate_Email);

                        if (response.Email !== "" && response.Duplicate_Email !== "") {
                            if (response.Email === response.Duplicate_Email) {
                                $("#txtEmail").addClass("text-danger");
                                $("#txtDuplicate_Email").addClass("text-danger");
                            }
                            else {
                                $("#txtEmail").removeClass("text-danger");
                                $("#txtDuplicate_Email").removeClass("text-danger");
                            }
                        }

                        $("#txtCourseLevel").val(response.CourseLevel);
                        $("#txtDuplicate_CourseLevel").val(response.Duplicate_CourseLevel);

                        if (response.CourseLevel !== "" && response.Duplicate_CourseLevel !== "") {
                            if (response.CourseLevel === response.Duplicate_CourseLevel) {
                                $("#txtCourseLevel").addClass("text-danger");
                                $("#txtDuplicate_CourseLevel").addClass("text-danger");
                            }
                            else {
                                $("#txtCourseLevel").removeClass("text-danger");
                                $("#txtDuplicate_CourseLevel").removeClass("text-danger");
                            }
                        }

                        $("#txtLastResult").val(response.LastResult);
                        $("#txtDuplicate_LastResult").val(response.Duplicate_LastResult);

                        if (response.LastResult !== "" && response.Duplicate_LastResult !== "") {
                            if (response.LastResult === response.Duplicate_LastResult) {
                                $("#txtLastResult").addClass("text-danger");
                                $("#txtDuplicate_LastResult").addClass("text-danger");
                            }
                            else {
                                $("#txtLastResult").removeClass("text-danger");
                                $("#txtDuplicate_LastResult").removeClass("text-danger");
                            }
                        }

                        $("#txtStatus").val(response.Status);
                        $("#txtDuplicate_Status").val(response.Duplicate_Status);

                        if (response.Status !== "" && response.Duplicate_Status !== "") {
                            if (response.Status === response.Duplicate_Status) {
                                $("#txtStatus").addClass("text-danger");
                                $("#txtDuplicate_Status").addClass("text-danger");
                            }
                            else {
                                $("#txtStatus").removeClass("text-danger");
                                $("#txtDuplicate_Status").removeClass("text-danger");
                            }
                        }

                        app.duplicateLead.CourseLevel = $("#ddl_CourseLevel option:selected").text();
                        app.duplicateLead.LastResult = $("#ddl_LastResults option:selected").text();
                        app.duplicateLead.Status = "New";
                        $("#kt_modal_duplicate").modal("show");
                    }
                }
                else {
                    $.ajax({
                        url: "/api/LeadApi/InsertLeadRecord",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        if (response !== null) {
                           
                                toastr.success("Lead updated successfully.", "Success!");
                                if (type === 1) {
                                    location.href = "/Lead/Manage";
                                }
                                else if (type === 2) {
                                    location.href = "/Account/Logout";
                                }
                                else if (type === 3) {
                                    location.href = "/Lead/Create";
                                }
                                else {
                                    location.href = '/Lead/Edit?id=' + response.encodeLeadId;
                                }

                          
                        }
                        else {
                            toastr.error("Cannot update. Please try again.", "Error!");
                        }
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                    
                }
                }).fail(function (xhr) {
                    toastr.error(xhr.responseText, 'Error!');
                });
        },
        GetDuplicateLead: function (leadId) {
            $.ajax({
                url: "/api/LeadApi/GetDuplicateLead",
                data: { id: leadId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response.ContactName !== null) {
                    $("#kt_modal_duplicate").modal("show");
                    app.duplicateLead = response;
                }
                else {
                    app.submittedDuplicate(_saveType);
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        submittedDuplicate: function (type) {

            var data = this.lead;

            $.ajax({
                url: "/api/LeadApi/InsertLeadRecord",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response !== null) {
                    toastr.success("Lead updated successfully.", "Success!");
                    //// if type is 1 then save and go back.
                    //// if type is 2 then save and sign out.
                    //// if type is 3 then save and Add new.
                    if (type === 1) {
                        location.href = "/Lead/Manage";
                    }
                    else if (type === 2) {
                        location.href = "/Account/Logout";
                    }
                    else if (type === 3) {
                        location.href = "/Lead/Create";
                    } else {
                        location.href = '/Lead/Edit?id=' + response.encodeLeadId;
                    }

                    $("#kt_modal_duplicate").modal("hide");
                }
                else {
                    toastr.error("Cannot update. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GetWorkPlaceOnEdit: function (companyId) {
            $.ajax({
                url: "/api/LeadApi/GetCompanyWorkPlace",
                data: { id: companyId },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                debugger;
                if (response.length > 0) {
                    var collection = [];
                    var obj = new Object();
                    //obj.OptHeader_Id = -1;
                    //obj.OptHeader_Title = "Please select header name to edit";
                    for (var count = 0; count < response.length; count++) {
                        obj = new Object();
                        obj.wp_id = response[count].wp_id;
                        obj.wp_name = response[count].wp_name;
                        collection.push(obj);
                    }

                    app.WorkPlace = collection;
                    var ddl = '';

                    ddl = $('#ddl_WorkPlace').data("kendoDropDownList");
                    if (ddl !== undefined) {
                        ddl.setDataSource(app.WorkPlace);
                        ddl.refresh();
                    }
                    app.GetWorkplaceDetail(app.workplaceId);

                }
                //} else {
                //    toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
                //}
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GetWorkplaceDetail: function (id) {
            $.ajax({
                url: "/api/CompanyApi/GetCompanyWorkPlaceById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                debugger;
                if (response.length > 0) {
                    //$("#Lead_CompanyHouseReg").val(response)

                    $("#WorkPlace_Contact_Address1").val(response.address1);
                    $("#WorkPlace_Contact_Address2").val(response.address2);
                    $("#WorkPlace_Contact_Twon").val(response.town);
                    $("#WorkPlace_Contact_County").val(response.county);
                    $("#WorkPlace_Contact_Postcode").val(response.postcode);
                    $("#WorkPlace_Contact_Email").val(response.employee_email);



                }
                  //  else {
                //    toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
                //}
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        mounted: function () {

            $.ajax({
                url: "/api/ApplicationApi/GetAllCourseLevel",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                debugger;
                app.CourseLevel2 = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
          
            $('#txtLearnerEnrolmentDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                todayHighlight: true
            }).on('changeDate', () => { this.lead.Lead_Learner_Enrolment_Date = $('#txtLearnerEnrolmentDate').val(); });

            $('#txt_DateEnquiry').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                todayHighlight: true
            }).on('changeDate', () => { this.lead.Lead_DateOfEnquiry = $('#txt_DateEnquiry').val(); });

            $("#txt_DateEnquiry").datepicker().datepicker("setDate", new Date());

            $('#txt_EnrolmentCancelledDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                todayHighlight: true
            }).on('changeDate', () => { this.lead.Lead_Enrolment_Cancelled_Date = $('#txt_EnrolmentCancelledDate').val(); });

            $('#txt_PreferredTimeToContact').timepicker({
                minuteStep: 1,
                defaultTime: '',
                showSeconds: false,
                showMeridian: false,
                snapToStep: true
            }).on('changeTime.timepicker', () => { this.lead.Lead_PreferredTimeToContact = $('#txt_PreferredTimeToContact').val() });
            $.ajax({
                url: "/api/ListApi/GetDropdownOptionsByHeaderName",
                type: "GET",
                contentType: "application/json",
                data: { headerName: "DefaultDocCategories" },
                dataType: "json",
            }).done(function (response) {
                app.DocumentCategories = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
            parameters = this.$route.query;
            if (_leadId !== undefined)
            {
                debugger;
                parameters.id = _leadId;
                if (parameters.id > 0) {
                    if (!_canRoleManagerEditLead) {
                        window.location.href = '/Error/PermissionDenied';
                    }

                    app.roleName = _roleName;
                    $('#txt_Activity_Reminder_Date').datetimepicker({
                        todayHighlight: true,
                        autoclose: true,
                        pickerPosition: 'bottom-right',
                        //format: 'dd/mm/yyyy HH:ii:ss P'
                        format: 'dd/mm/yyyy hh:ii'
                    }).on('changeDate', () => { this.Activity.Activity_Reminder_Date = $('#txt_Activity_Reminder_Date').val() });

                    $('#txt_Activity_Date').datetimepicker({
                        todayHighlight: true,
                        autoclose: true,
                        pickerPosition: 'bottom-right',
                        //format: 'dd/mm/yyyy HH:ii:ss P'
                        format: 'dd/mm/yyyy hh:ii'
                    }).on('changeDate', () => { this.Activity.Activity_Date = $('#txt_Activity_Date').val() });

                    app.parameters.id = parameters.id;
                    app.parameters.Act = parameters.Act;
                    $.ajax({
                        url: "/api/LeadApi/GetLeadById",
                        data: { id: parameters.id },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        app.lead = response;
                        app.OldLead = response;
                        app.oldLastResult = response.Lead_Id_LastResult;
                        if (app.oldLastResult != 0) {
                            app.GetLastResultForSa(response.Lead_Id_LastResult, "LastResults");
                        }
                        $("#lblEditHeader").text(response.Lead_ContactName + ' - ' + response.Lead_Id);
                        if (response.Lead_IsDealClosed) {
                            app.lead.Lead_IsDealClosed = 1;
                        } else {
                            app.lead.Lead_IsDealClosed = 0;
                        }

                        if (response.Lead_DateOfEnquiry !== null && response.Lead_DateOfEnquiry !== "") {
                            $('#txt_DateEnquiry').datepicker('update', response.Lead_DateOfEnquiry);
                        }
                        if (response.Lead_Enrolment_Cancelled_Date !== null && response.Lead_Enrolment_Cancelled_Date !== "") {
                            $('#txt_EnrolmentCancelledDate').datepicker('update', response.Lead_Enrolment_Cancelled_Date);
                        } 
                        if (response.Lead_Learner_Enrolment_Date !== null && response.Lead_Learner_Enrolment_Date !== "") {
                            $('#txtLearnerEnrolmentDate').datepicker('update', response.Lead_Learner_Enrolment_Date);
                        }
                        if (response.Lead_PreferredTimeToContact !== null && response.Lead_PreferredTimeToContact !== "") {
                            $('#txt_PreferredTimeToContact').timepicker('setTime', response.Lead_PreferredTimeToContact);
                        }

                        $("#ddl_SalesAdvisor").data("kendoDropDownList").value(response.Lead_Id_SalesAdvisor);
                        $("#ddlSourceOfEnquiry").data("kendoDropDownList").value(response.Lead_Id_SourceOfEnquiry);
                        $("#ddlDepartment").data("kendoDropDownList").value(response.Lead_Contact_Id_Department);
                        $("#ddlJobTitle").data("kendoDropDownList").value(response.Lead_Contact_Id_JobTitle);
                        $("#ddl_LastResults").data("kendoDropDownList").value(response.Lead_Id_LastResult);
                        $("#ddl_CourseLevel").data("kendoDropDownList").value(response.Lead_Id_CourseLevel);
                        $("#ddl_Company").data("kendoDropDownList").value(response.Company_Id);
                        app.compId = response.Company_Id;
                        app.GetWorkPlaceOnEdit(response.Company_Id);
                        $("#ddl_WorkPlace").data("kendoDropDownList").value(response.Company_WorkPlace_Id);
                        app.workplaceId = response.Company_WorkPlace_Id;
                        if (response.courseLevelEnquiryList !== null) {
                            $("#ddl_CourseLevelEnquiry").data("kendoMultiSelect").value(response.courseLevelEnquiryList.split(','));
                        }

                       
                        if ($("#hfd_roleName").val() !== "Admin" && $("#hfd_roleName").val() !== "Administrator") {
                          //  $("#ddl_SalesAdvisor").data("kendoDropDownList").value($("#hfd_userId").val());
                        }
                        //viewlead(owner);
                        if ($("#hfd_owner").val() == "0") {
                            //for viewlead only
                            $("#ddl_SalesAdvisor").attr("disabled", "disabled");

                            $("k-dropdown").attr("disabled", "disabled");
                            $('select').attr("disabled", "disabled");
                            $('span').attr("disabled", "disabled");
                            $('input').attr("disabled", "disabled");
                            $('textarea').attr("disabled", "disabled");
                            $('#notesGrid').attr("disabled", "disabled");
                            $(".k-multiselect").attr("disabled", "disabled");

                            $('#btn_add_activity').hide();
                            $('#btn_add_document').hide();

                            $('.btn-group').hide();
                            $('.nav-tabs a[href="#kt_tabs_1_5"]').hide();
                            $('.nav-tabs a[href="#kt_tabs_1_6"]').hide();

                            $('#kt_modal_notes').find('select').removeAttr("disabled");
                            $('#kt_modal_notes').find('textarea').removeAttr("disabled");
                            
                            $('#txt_Search_Query').removeAttr("disabled");
                           
                        }

                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });

                    $.ajax({
                        url: "/api/LeadApi/GetAllActivityTypes",
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json",
                    }).done(function (response) {
                        app.ActivityTypes = response;
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });

                    $.ajax({
                        url: "/api/Core_EmailTemplateApi/GetTemplatesForDropdownByType",
                        data: { id: 1 }, //1 = Email, 
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json",
                    }).done(function (response) {
                        app.EmailTemplates = response;
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });

                    $.ajax({
                        url: "/api/Core_EmailTemplateApi/GetTemplatesForDropdownByType",
                        data: { id: 2 }, //1 = Letter, 
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json",
                    }).done(function (response) {
                        app.LetterTemplates = response;
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                    if (parameters.Act === "Y") {
                        $('.nav-tabs a[href="#kt_tabs_1_6"]').tab('show');
                    }

                } else {
                    if (!_canRoleManagerCreateLead) {
                        window.location.href = '/Error/PermissionDenied';
                    }

                    if ($("#hfd_roleName").val() === "Admin" || $("#hfd_roleName").val() === "Administrator") {
                        $.ajax({
                            url: "/api/LeadApi/GetAvailableSalesAdvisorIdRota",
                            type: "GET",
                            contentType: "application/json",
                            dataType: "json",
                        }).done(function (response) {
                            app.lead.Lead_Id_SalesAdvisor = response;
                        }).fail(function (xhr) {
                            toastr.error(xhr.responseText, 'Error!');
                        });
                    }
                }
            }

            if (_companyId !== undefined) {
                //debugger; // eslint-disable-line
                if (app.compId !== "") {
                    app.GetWorkPlaceOnEdit(app.compId);
                    //  $("#ddl_WorkPlace").data("kendoDropDownList").value(response.Company_WorkPlace_Id);
                    $.ajax({
                        url: "/api/LeadApi/GetCompanyContact",
                        data: { id: app.compId },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        debugger;
                        if (response != null) {
                            //$("#Lead_CompanyHouseReg").val(response)


                            $("#Lead_CompanyHouseReg").val(response.company_number);
                            $("#Lead_CompanyLineManagerContactName").val(response.contact_name1);
                            $("#Lead_CompanyDecisionMakerName").val(response.name_desision_maker);
                            $("#Lead_CompanyEmail").val(response.email1);
                            $("#Lead_Contact_Address1").val(response.address1);
                            $("#Lead_Contact_Address2").val(response.address2);
                            $("#Lead_Contact_Address3").val(response.address3);
                            $("#Lead_Contact_Company_Postcode").val(response.postcode);
                            $("#Lead_Contact_County").val(response.county);
                            $("#Lead_Contact_TownCity").val(response.town);

                        } else {
                            toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
                        }
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
                // parameters.id = _companyId;
                if (_companyId != "0" ) {

                    $("#ddl_Company").data("kendoDropDownList").value(_companyId);
                    app.GetWorkPlaceOnEdit(_companyId);
                    //  $("#ddl_WorkPlace").data("kendoDropDownList").value(response.Company_WorkPlace_Id);
                    $.ajax({
                        url: "/api/LeadApi/GetCompanyContact",
                        data: { id: _companyId },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        debugger;
                        if (response != null) {
                            //$("#Lead_CompanyHouseReg").val(response)


                            $("#Lead_CompanyHouseReg").val(response.company_number);
                            $("#Lead_CompanyLineManagerContactName").val(response.contact_name1);
                            $("#Lead_CompanyDecisionMakerName").val(response.name_desision_maker);
                            $("#Lead_CompanyEmail").val(response.email1);
                            $("#Lead_Contact_Address1").val(response.address1);
                            $("#Lead_Contact_Address2").val(response.address2);
                            $("#Lead_Contact_Address3").val(response.address3);
                            $("#Lead_Contact_Company_Postcode").val(response.postcode);
                            $("#Lead_Contact_County").val(response.county);
                            $("#Lead_Contact_TownCity").val(response.town);

                        } else {
                            toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
                        }
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            }
        },
        NewDocument: function () {
            app.isDocumentSubmitted = false;
            app.Document = { Document_Id: 0, Document_Id_Lead: 0, Document_Name: '', Document_Description: '', Document_FilePath: '' };
            $('label[id*="lbl_docName"]').text('');
            $("#inputDocument").val("");
            $('#DocumentsGrid').data('kendoGrid').dataSource.read();
            $('#DocumentsGrid').data('kendoGrid').refresh();
            if (!_canRoleManagerCreateDocument) {
                toastr.error('You cannot add new document, please contact administrator', 'Permission denied!');
                return false;
            }
            /// reset file upload.
            var upload = $("#files").data("kendoUpload");
            upload.clearAllFiles();
            $('.nav-tabs a[href="#kt_tabs_1_5"]').tab('show');
            $("#kt_modal_document").modal("show");
        },
        submitDocument: function () {
            debugger; // eslint-disable-line

            this.isDocumentSubmitted = true;
            if (this.$v.Document.Document_Name.$invalid) {
                toastr.error('Error in validation', 'Error!');
                return;
            }

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

            this.Document.Document_Id_Lead = parameters.id;
            var data = this.Document;

            formData.append("documentData", JSON.stringify(data));

            $.ajax({
                url: "/api/LeadApi/UploadDocumentsAjax",
                data: formData,
                type: "Post",
                contentType: false,
                processData: false
            }).done(function (response) {
                if (response === true) {
                    app.isDocumentSubmitted = false;
                    app.Document = { Document_Id: 0, Document_Id_Lead: 0, Document_Name: '', Document_Description: '', Document_FilePath: '' };
                    $("#kt_modal_document").modal("hide");
                    $('label[id*="lbl_docName"]').text('');
                    $("#inputDocument").val("");
                    $('#DocumentsGrid').data('kendoGrid').dataSource.read();
                    $('#DocumentsGrid').data('kendoGrid').refresh();
                    toastr.success("Document uploaded successfully.", "Success!");
                    refreshHistory();
                } else {
                    toastr.error("Document cannot uploaded. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformDocumentDelete: function (id) {
            $.ajax({
                url: "/api/LeadApi/DeleteDocumentById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#DocumentsGrid').data('kendoGrid').dataSource.read();
                    $('#DocumentsGrid').data('kendoGrid').refresh();
                    refreshHistory();
                    toastr.success("Document deleted successfully.", "Success!");
                } else {
                    toastr.error("Document cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        ChangeActivityType: function (event) {
            var selectedType = event.target.value;
            app.PerformActivityTypeChange(selectedType);
            $("#chkAttachDocuments").prop('checked', false);
            $(".attachDocuments").attr("style", "display:none");
        },
        PerformActivityTypeChange: function (selectedType) {
            var type;
            switch (selectedType) {
                case "1":
                    type = "ActivityPhoneActions";
                    break;
                case "2":
                    type = "ActivityEmailActions";
                    break;
                case "3":
                    type = "ActivityAppointmentMeetingActions";
                    break;
                case "4":
                    type = "ActivityAppointmentMeetingActions";
                    break;
                case "5":
                    type = "ActivityAppointmentMeetingActions";

            }
            $.ajax({
                url: "/api/ListApi/GetDropdownOptionsByHeaderName",
                type: "GET",
                contentType: "application/json",
                data: { headerName: type },
                dataType: "json"
            }).done(function (response) {
                app.ActivityActions = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        submitActivity: function () {
            this.isActivitySubmitted = true;
            if (this.$v.Activity.$invalid) {
                toastr.error("Error in validation.", 'Error!');
                return;
            }
            this.Activity.Activity_Id_Lead = parameters.id;
            var activity = this.Activity;
            var email = this.lead.Lead_Contact_Email;

            activity.Activity_EmailBody = $('#editor').summernote('code');
            activity.Activity_EmailTo = email;

            $.ajax({
                url: "/api/LeadApi/InsertUpdateLeadActivityRecord",
                data: JSON.stringify(activity),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    if (app.Activity.Activity_Id > 0) {
                        toastr.success("Activity updated successfully.", "Success!");
                    } else {
                        toastr.success("Activity inserted successfully.", "Success!");
                    }
                    $('#editor').summernote('code', "");
                    app.isActivitySubmitted = false;
                    //app.Activity = { Activity_Id: 0, Activity_Id_Lead: 0, Activity_Id_Type: 0, Activity_Id_Action: 0, Activity_Reminder_Date: '', Activity_Note: '', Activity_EmailBody: '', Activity_Date: null, Activity_Location: null, Activity_EmailSubject: '' };
                    app.Activity = { Activity_Id: 0, Activity_Id_Lead: 0, Activity_Id_Type: 0, Activity_Id_Action: 0, Activity_Reminder_Date: '', Activity_Note: '', Activity_EmailBody: '', Activity_Date: null, Activity_Location: null };
                    app.Activity_Id_DocCategory = 0;
                    $("#kt_modal_activity").modal("hide");
                    $('#ActivitiesGrid').data('kendoGrid').dataSource.read();
                    $('#ActivitiesGrid').data('kendoGrid').refresh();
                    refreshHistory();
                } else {

                    toastr.error('Activity cannot added. Please try again.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformActivityEdit: function (id) {
            app.ActivityActions = [];
            app.Activity = { Activity_Id: 0, Activity_Id_Lead: 0, Activity_Id_Type: 0, Activity_Id_Action: 0, Activity_Reminder_Date: '', Activity_Note: '', Activity_EmailBody: '', Activity_Date: null, Activity_Location: null };
            $.ajax({
                url: "/api/LeadApi/GetActivityById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.PerformActivityTypeChange(response.Activity_Id_Type.toString());
                app.Activity = response;
                if (response.Activity_EmailBody !== null && response.Activity_EmailBody !== "") {
                    $('#editor').summernote('code', response.Activity_EmailBody);
                } else {
                    $('#editor').summernote('code', "");
                }
                if (response.Activity_Reminder_Date !== null && response.Activity_Reminder_Date !== "") {
                    var d = new Date(response.Activity_Reminder_Date);
                    console.log(d);
                    //format: 'yyyy/mm/dd hh:ii'
                    $('#txt_Activity_Reminder_Date').datetimepicker('update', response.Activity_Reminder_Date);
                    $('#txt_Activity_Reminder_Date').datetimepicker('format', 'yyyy/mm/dd hh:ii');
                }
                $("#kt_modal_view_activity").modal("show");

                $("#chkAttachDocuments").prop('checked', false);
                if (app.Activity.Activity_Id_Type === 2) {
                    $("#chkAttachDocuments").prop('checked', true);
                    $(".attachDocuments").attr("style", "display:inline");
                }
                else {
                    $(".attachDocuments").attr("style", "display:none");
                }

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformActivityDelete: function (id) {
            $.ajax({
                url: "/api/LeadApi/DeleteActivityById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#ActivitiesGrid').data('kendoGrid').dataSource.read();
                    $('#ActivitiesGrid').data('kendoGrid').refresh();
                    refreshHistory();
                    toastr.success("Activity deleted successfully.", "Success!");
                } else {
                    toastr.success("Activity cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        NewActivity: function () {
            app.isActivitySubmitted = false;
            $('#editor').summernote('code', "");
            app.ActivityActions = [];
            app.Activity = { Activity_Id: 0, Activity_Id_Lead: 0, Activity_Id_Type: 0, Activity_Id_Action: 0, Activity_Reminder_Date: '', Activity_Note: '', Activity_EmailBody: '', Activity_Date: null, Activity_Location: null };
            if (!_canRoleManagerCreateActivity) {
                toastr.error('You cannot add new activity please contact administrator', "Permission denied!");
                return false;
            }
            $('.nav-tabs a[href="#kt_tabs_1_6"]').tab('show');
            $("#kt_modal_activity").modal("show");
            $(".attachDocuments").attr("style", "display:none");
        },
        ChangeEmailBody: function (event) {
            if (event.target.value === "0") {
                $('#editor').summernote('code', "");
                return;
            }
            $.ajax({
                url: "/api/Core_EmailTemplateApi/GetEmailTemplateById",
                data: { id: event.target.value },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                $('#editor').summernote('code', response.ET_Body);
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        ChangeDocumentCategory: function (event) {
            if (event.target.value === "0") {
                return;
            }
            $.ajax({
                url: "/api/DocumentApi/GetDocumentsByCategoryId",
                data: { id: event.target.value },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.DefaultDocuments = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        AttachDefaultDocument: function () {
            app.DefaultAttachments.push(app.Activity_DefaultDocument);

            var file = { name: $("#ddl_DefaultDocument option:selected").text(), path: app.Activity_DefaultDocument };
            myDropZone.options.addedfile.call(myDropZone, file);
        },

        NewCSL: function () {
            app.isCSLSubmitted = false;
            $('#editor').summernote('code', "");
            app.CSLOutcomes = [];
            app.CSL = {
                CSL_Id: 0,
                CSL_Id_Lead: 0,
                CSL_Id_OptHeader: 0,
                CSL_Id_Outcome: 0,
                CSL_Note: '',
                CSL_Date: null,
                //CSL_CreatedByUserId: null,
                //CSL_CreatedByUserName: '',
                //CSL_DateCreated: null,
                //CSL_UpdatedByUserId: null,
                //CSL_UpdatedByUserName: null,
                //CSL_DateUpdated: null,
                //CSL_IsDeleted: null,
                //CSL_DeletedByUserId: null,
                //CSL_DeletedByUserName: null,
                //CSL_DateDeleted: null

            };
            if (!_canRoleManagerCreateCommSummaryLog) {
                toastr.error('You cannot add Call Summary Log, please contact administrator', "Permission denied!");
                return false;
            }


            $.ajax({
                url: "/api/LeadApi/GetAllOutcomesTypes",
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.CSLOutcomes = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

               
           $('#txt_CSL_Date').datetimepicker({
                todayHighlight: true,
                autoclose: true,
                pickerPosition: 'bottom-right',
                //format: 'dd/mm/yyyy HH:ii:ss P'
                format: 'dd/mm/yyyy hh:ii'
            }).on('changeDate', () => { this.CSL.CSL_Date = $('#txt_CSL_Date').val() });
             
            $('.nav-tabs a[href="#kt_tabs_1_7"]').tab('show');
            $("#kt_modal_view_comm_summary_log").modal("show");
           
        },
        PerformCSLDelete: function (id) {
            $.ajax({
                url: "/api/LeadApi/DeleteCSLById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                debugger;
                if (response === true) {
                    $('#CSLGrid').data('kendoGrid').dataSource.read();
                    $('#CSLGrid').data('kendoGrid').refresh();
                    refreshHistory();
                    toastr.success("Summary Log deleted successfully.", "Success!");
                } else {
                    toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformCSLEdit: function (id) {
            app.CSLOutcomes = [];
            app.CSL = {
                CSL_Id: 0,
                CSL_Id_Lead: 0,
                CSL_Id_OptHeader: 0,
                CSL_Id_Outcome: 0,
                CSL_Note: '',
                CSL_Date: null,                
                //CSL_CreatedByUserId: null,
                //CSL_CreatedByUserName: '',
                //CSL_DateCreated: null,
                //CSL_UpdatedByUserId: null,
                //CSL_UpdatedByUserName: null,
                //CSL_DateUpdated: null,
                //CSL_IsDeleted: null,
                //CSL_DeletedByUserId: null,
                //CSL_DeletedByUserName: null,
                //CSL_DateDeleted: null

            };
            $.ajax({
                url: "/api/LeadApi/GetActivityById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.PerformActivityTypeChange(response.Activity_Id_Type.toString());
                app.Activity = response;
                if (response.Activity_EmailBody !== null && response.Activity_EmailBody !== "") {
                    $('#editor').summernote('code', response.Activity_EmailBody);
                } else {
                    $('#editor').summernote('code', "");
                }
                if (response.Activity_Reminder_Date !== null && response.Activity_Reminder_Date !== "") {
                    var d = new Date(response.Activity_Reminder_Date);
                    console.log(d);
                    //format: 'yyyy/mm/dd hh:ii'
                    $('#txt_Activity_Reminder_Date').datetimepicker('update', response.Activity_Reminder_Date);
                    $('#txt_Activity_Reminder_Date').datetimepicker('format', 'yyyy/mm/dd hh:ii');
                }
                $("#kt_modal_view_activity").modal("show");

                $("#chkAttachDocuments").prop('checked', false);
                if (app.Activity.Activity_Id_Type === 2) {
                    $("#chkAttachDocuments").prop('checked', true);
                    $(".attachDocuments").attr("style", "display:inline");
                }
                else {
                    $(".attachDocuments").attr("style", "display:none");
                }

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        submitCSL: function () {


            this.isCSLSubmitted = true;
            if (this.$v.CSL.$invalid) {
                toastr.error("Error in validation.", 'Error!');
                return;
            }

            if (this.CSL.CSL_Id_Outcome == 0) {
                toastr.error("Please select the outcome", 'Error!');
                return;
            }

            this.CSL.CSL_Id_Lead = parameters.id;
            var dateMomentObject = moment(new Date(), "DD/MM/YYYY HH:mm"); // 1st argument - string, 2nd argument - format

            //var dateMomentObject = moment(this.CSL.CSL_Date, "DD/MM/YYYY HH:mm"); // 1st argument - string, 2nd argument - format
            var dateObject = dateMomentObject.toDate(); // convert moment.js object to Date object

            this.CSL.CSL_Date = dateObject;
            var csl = this.CSL;

            //xxxvar email = this.lead.Lead_Contact_Email;
            //CSL_Id_Outcome
          
            $.ajax({
                url: "/api/LeadApi/AddUpdateCSL",
                data: JSON.stringify(csl),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    if (app.CSL.CSL_Id > 0) {
                        toastr.success("Summary log updated successfully.", "Success!");
                    } else {
                        toastr.success("Summary log added successfully.", "Success!");
                    }
                    $('#editor').summernote('code', "");
                    app.isCSLSubmitted = false;
                    
                   // CSLOutcomes: []
                    //app.CSL = {
                    //    CSL_Id: 0, CSL_Id_Lead: 0, CSL_Id_OptHeader: 0, CSL_Id_Outcome: '', CSL_Note: '', CSL_Date: null, CSL_Location: null, CSL_CreatedByUserId: null, CSL_CreatedByUserName: '', CSL_DateCreated: null, CSL_UpdatedByUserId: null,
                    //    CSL_UpdatedByUserName: null, CSL_DateUpdated: null, CSL_IsDeleted: null, CSL_DeletedByUserId: null, CSL_DeletedByUserName: null, CSL_DateDeleted: null
                    //};
                    app.CSL = {
                        CSL_Id: 0,
                        CSL_Id_Lead: 0,
                        CSL_Id_OptHeader: 0,
                        CSL_Id_Outcome: 0,
                        CSL_Note: '',
                        CSL_Date: null
                    };
                    //app.Activity_Id_DocCategory = 0;
                    $("#kt_modal_view_comm_summary_log").modal("hide");
                    $('#CSLGrid').data('kendoGrid').dataSource.read();
                    $('#CSLGrid').data('kendoGrid').refresh();
                    refreshHistory();
                } else {

                    toastr.error('Summary log cannot be added. Please try again.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        NewTask: function () {
            app.isTaskSubmitted = false;
            //$('#editor').summernote('code', "");
            app.TaskActivities = [];
            app.Task = {
                task_id: 0,
                task_id_lead: 0,
                task_id_user: 0,
                task_id_optheader: 0,
                task_id_activity: 0,
                task_scheduled_with: '',
                task_note: '',
                task_is_done: false,
                task_start: null,
                task_end: null
            };
            if (!_canRoleManagerCreateTask) {
                toastr.error('You cannot create task, please contact administrator', "Permission denied!");
                return false;
            }

            app.Task.task_scheduled_with = $('#txtContactName').val();

            $.ajax({
                url: "/api/LeadApi/GetAllActivityAppointmentMeetingActions",
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.TaskActivities = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });


            $('#txt_Task_Date').datetimepicker({
                todayHighlight: true,
                autoclose: true,
                pickerPosition: 'bottom-right',
                //format: 'dd/mm/yyyy HH:ii:ss P'
                format: 'dd/mm/yyyy hh:ii'
            }).on('changeDate', () => { this.Task.task_start= $('#txt_Task_Date').val() });

            $('.nav-tabs a[href="#kt_tabs_1_8"]').tab('show');
            $("#kt_modal_form_task").modal("show");

        },
        PerformTaskDone: function (task_id, user_id) {
            
            $.ajax({
                url: "/api/LeadApi/TaskDone",
                data: { id: task_id, user_id: user_id,  lead_id: parameters.id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#TaskGrid').data('kendoGrid').dataSource.read();
                    $('#TaskGrid').data('kendoGrid').refresh();
                    //TODO refreshHistory();
                    toastr.success("Task is marked as done.", "Success!");
                    GetTasks();
                } else {
                    toastr.success("Task cannot be marked as done. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformTaskDelete: function (id) {
            $.ajax({
                url: "/api/LeadApi/DeleteTaskById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    $('#TaskGrid').data('kendoGrid').dataSource.read();
                    $('#TaskGrid').data('kendoGrid').refresh();
                    refreshHistory();
                    toastr.success("Task deleted successfully.", "Success!");
                } else {
                    toastr.success("Task cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformTaskEdit: function (id) {
            app.TaskActivities = [];
            app.Task = {
                task_id: 0,
                task_id_lead: 0,
                task_id_user: 0,
                task_id_optheader: 0,
                task_id_activity: 0,
                task_scheduled_with: '',
                task_note: '',
                task_is_done: false,                
                task_start: null,
                task_end: null
            };

            //$.ajax({
            //    url: "/api/LeadApi/GetAllOutcomesTypes",
            //    type: "GET",
            //    contentType: "application/json",
            //    dataType: "json",
            //}).done(function (response) {
            //    app.TaskActivities = response;
            //}).fail(function (xhr) {
            //    toastr.error(xhr.responseText, 'Error!');
            //});

            $.ajax({
                url: "/api/LeadApi/GetTaskById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
               // app.PerformTaskActivityChange(response.Activity_Id_Type.toString());
                app.Task = response;
                //if (response.Activity_EmailBody !== null && response.Activity_EmailBody !== "") {
                //    $('#editor').summernote('code', response.Activity_EmailBody);
                //} else {
                //    $('#editor').summernote('code', "");
                //}
                if (response.task_start !== null && response.task_start !== "") {
                    var d = new Date(response.task_start);
                    console.log(d);
                    //format: 'yyyy/mm/dd hh:ii'
                    $('#txt_Task_Date').datetimepicker('update', response.task_start);
                    $('#txt_Task_Date').datetimepicker('format', 'yyyy/mm/dd hh:ii');
                }
                $("#kt_modal_form_task").modal("show");

                //$("#chkAttachDocuments").prop('checked', false);
                //if (app.Activity.Activity_Id_Type === 2) {
                //    $("#chkAttachDocuments").prop('checked', true);
                //    $(".attachDocuments").attr("style", "display:inline");
                //}
                //else {
                //    $(".attachDocuments").attr("style", "display:none");
                //}

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformTaskActivityChange: function (selectedType) {
            var type;
            switch (selectedType) {
                case "1":
                    type = "ActivityPhoneActions";
                    break;
                case "2":
                    type = "ActivityEmailActions";
                    break;
                case "3":
                    type = "ActivityAppointmentMeetingActions";
                    break;
                case "4":
                    type = "ActivityAppointmentMeetingActions";
                    break;
                case "5":
                    type = "ActivityAppointmentMeetingActions";

            }
            $.ajax({
                url: "/api/ListApi/GetDropdownOptionsByHeaderName",
                type: "GET",
                contentType: "application/json",
                data: { headerName: type },
                dataType: "json"
            }).done(function (response) {
                app.ActivityActions = response;
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        submitTask: function () {

            this.isTaskSubmitted = true;
            if (this.$v.Task.$invalid) {
                toastr.error("Error in validation. Please fill required fields.", 'Error!');
                return;
            }

            if (this.Task.task_id_activity == 0 || this.Task.task_id_activity =='') {
                toastr.error("Activity is required.", 'Error!');
                return;
            }
            this.Task.task_id_lead = parameters.id;

            var oldDate = this.Task.task_start;
            var dateMomentObject = moment(this.Task.task_start, "DD/MM/YYYY HH:mm"); // 1st argument - string, 2nd argument - format
            var dateObject = dateMomentObject.toDate(); // convert moment.js object to Date object

            this.Task.task_start = dateObject;
            var task = this.Task;

           // var bExisting = false;
            //check if existing task already

            //$.ajax({
            //    url: "/api/LeadApi/CheckExistingTask",
            //    data: JSON.stringify(task),
            //    type: "Post",
            //    contentType: "application/json",
            //    dataType: "json"
            //}).done(function (response) {
            //    if (response === true) {
            //        bExisting = true;
            //    }
            //}).fail(function (xhr) {
            //    toastr.error(xhr.responseText, 'Error!');
            //});

            //if (bExisting == true) {
            //    toastr.error("You have already a task with this Date and Time.", 'Error!');
            //    return;
            //}


            ///all good to save

            //xxxvar email = this.lead.Lead_Contact_Email;
            //Task_Id_Outcome

            $.ajax({
                url: "/api/LeadApi/AddUpdateTask",
                data: JSON.stringify(task),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    if (app.Task.task_id > 0) {
                        toastr.success("Task updated successfully.", "Success!");
                    } else {
                        toastr.success("Task added successfully.", "Success!");
                    }
                   // $('#editor').summernote('code', "");
                    app.isTaskSubmitted = false;
                     
                    app.Task = {
                        task_id: 0,
                        task_id_lead: 0,
                        task_id_user: 0,
                        task_id_optheader: 0,
                        task_id_activity: 0,
                        task_scheduled_with: '',
                        task_note: '',
                        task_is_done: false,
                        task_start: null,
                        task_end: null
                    };
                    //app.Activity_Id_DocCategory = 0;
                    $("#kt_modal_form_task").modal("hide");
                    $('#TaskGrid').data('kendoGrid').dataSource.read();
                    $('#TaskGrid').data('kendoGrid').refresh();
                  //  refreshHistory();
                } else {

                    app.Task.task_start = oldDate;

                    toastr.error("You have already a task with this Date and Time.", 'Error!');
                    return;



                    //toastr.error('Task cannot be added. Please try again.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

    }
});

function PerformNoteEdit(note_Id) {
    if (!_canRoleManagerEditNotes) {
        toastr.error('You cannot edit please contact administrator', 'Permission denied!');
        return false;
    }
    app.PerformNoteEdit(note_Id);
}
function PerformNoteDelete(note_Id) {
    if (!_canRoleManagerDeleteNotes) {
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
        if (result.value) {
            app.PerformNoteDelete(note_Id);
        }
    });
}

function PerformDocumentDelete(document_Id) {
    if (!_canRoleManagerDeleteDocument) {
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
        if (result.value) {
            app.PerformDocumentDelete(document_Id);
        }
    });
}

function PerformActivityEdit(note_Id) {

    if (!_canRoleManagerEditActivity) {
        toastr.error('You cannot edit please contact administrator', 'Permission denied!');
        return false;
    }
    app.PerformActivityEdit(note_Id);
}
function PerformActivityDelete(note_Id) {
    if (!_canRoleManagerDeleteActivity) {
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
        if (result.value) {
            app.PerformActivityDelete(note_Id);
        }
    })
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

        $('#notesGrid').data('kendoGrid').dataSource.read();
        $('#notesGrid').data('kendoGrid').refresh();
    }
}

var _saveType = 0;
function save_stay(type) {
    _saveType = 0;
    _saveType = type;
    app.submitted(type);
}
function duplicateOnclick() {
    app.submittedDuplicate(_saveType);
}
function refreshHistory() {
   // $('#historyGrid').data('kendoGrid').dataSource.read();
   // $('#historyGrid').data('kendoGrid').refresh();
}


function PerformCSLDelete(id) {
    if (!_canRoleManagerDeleteCommSummaryLog) {
        toastr.error('You cannot delete, please contact administrator.', 'Permission denied!');
        return false;
    }
    app.PerformCSLDelete(id);
} 

function PerformTaskDelete(id) {
    if (!_canRoleManagerDeleteTask) {
        toastr.error('You cannot delete, please contact administrator.', 'Permission denied!');
        return false;
    }
    app.PerformTaskDelete(id);
} 

function PerformTaskDone(id) { 
    app.PerformTaskDone(id);
} 

function PerformInviteView(aPI_Id) {
    //if (!_canRoleManagerViewAppInvite) {
    //    toastr.error('You cannot view please contact administrator', 'Permission denied!');
    //    return false;
    //}
    app.PerformInviteView(aPI_Id);
}
function PerformInviteEdit(aPI_Id) {
    app.PerformInviteEdit(aPI_Id);
}
function PerformInviteDelete(aPI_Id) {
    app.PerformInviteDelete(aPI_Id);
}


$("#ddl_Company").change(function () {
    var id = this.value;
    if (id != "" || id != null) {
        $.ajax({
            url: "/api/LeadApi/GetCompanyContact",
            data: { id: id },
            type: "GET",
            contentType: "application/json",
            dataType: "json"
        }).done(function (response) {
            if (response != null) {
                //$("#Lead_CompanyHouseReg").val(response)


                $("#Lead_CompanyHouseReg").val(response.company_number);
                $("#Lead_CompanyLineManagerContactName").val(response.contact_name1);
                $("#Lead_CompanyDecisionMakerName").val(response.name_desision_maker);
                $("#Lead_CompanyEmail").val(response.email1);
                $("#Lead_Contact_Address1").val(response.address1);
                $("#Lead_Contact_Address2").val(response.address2);
                $("#Lead_Contact_Address3").val(response.address3);
                $("#Lead_Contact_Company_Postcode").val(response.postcode);
                $("#Lead_Contact_County").val(response.county);
                $("#Lead_Contact_TownCity").val(response.town);

            } else {
                toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
            }
        }).fail(function (xhr) {
            toastr.error(xhr.responseText, 'Error!');
        });

        $.ajax({
            url: "/api/LeadApi/GetCompanyWorkPlace",
            data: { id: id },
            type: "GET",
            contentType: "application/json",
            dataType: "json"
        }).done(function (response) {
            if (response != null) {
                var collection = [];
                var obj = new Object();
                //obj.OptHeader_Id = -1;
                //obj.OptHeader_Title = "Please select header name to edit";
                for (var count = 0; count < response.length; count++) {
                    obj = new Object();
                    obj.wp_id = response[count].wp_id;
                    obj.wp_name = response[count].wp_name;
                    collection.push(obj);
                }

                app.WorkPlace = collection;
                var ddl = '';
                console.log(app.WorkPlace);

                ddl = $('#ddl_WorkPlace').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.WorkPlace);
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
$("#ddl_WorkPlace").change(function () {
    var id = this.value;
    if (id != "") {
        $.ajax({
            url: "/api/CompanyApi/GetCompanyWorkPlaceById",
            data: { id: id },
            type: "GET",
            contentType: "application/json",
            dataType: "json"
        }).done(function (response) {
            if (response != null) {
                //$("#Lead_CompanyHouseReg").val(response)

                $("#WorkPlace_Contact_Address1").val(response.address1);
                $("#WorkPlace_Contact_Address2").val(response.address2);
                $("#WorkPlace_Contact_Twon").val(response.town);
                $("#WorkPlace_Contact_County").val(response.county);
                $("#WorkPlace_Contact_Postcode").val(response.postcode);
                $("#WorkPlace_Contact_Email").val(response.employee_email);

              

            } else {
                toastr.success("Summary Log cannot be deleted. Please try again.", "Error!");
            }
        }).fail(function (xhr) {
            toastr.error(xhr.responseText, 'Error!');
        });

        
    }
});

