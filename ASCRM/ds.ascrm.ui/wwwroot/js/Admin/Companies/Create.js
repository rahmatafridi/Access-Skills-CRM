Vue.use(vuelidate.default);

var router = new VueRouter({
    mode: 'history',
    routes: []
});

var app = new Vue({
    router,
    el: '#dv_Create_Company',
    data: {
        company: {
            company_number: '',
            name_desision_maker: '',
            name: '',
            reference: '',
            contact_name1: '',
            contact_name2: '',
            address1: '',
            address2: '',
            address3: '',
            town: '',
            county: '',
            postcode: '',
            country_id: 0,
            tel1: '',
            tel2: '',
            mobile1: '',
            mobile2: '',
            fax1: '',
            fax2: '',
            email1: '',
            email2: '',
            website: '',
            rating: 0,
            number_of_beds: 0,
            number_of_employees: 0,
            edrs_number: '',
            is_active: false,
            is_sponsor: false,
            is_levy_paying: false,
            company_group_id: 0,
            ratings_id: 0,
            sales_advisor_id: 0,
            cqc_rating: '',
            cqc_capacity: null,
            cqc_inspection_date: '',
            cqc_last_update_date: '',
            cqc_standard_1: null,
            cqc_standard_2: null,
            cqc_standard_3: null,
            cqc_standard_5: null,
            cqc_standard_4: null,
            cqc_registration_number: null,
            business_type: null,
            levy_employer: null,
            no_of_employees: null,
            company_house_registration_number: null,
            employer_email_address: null,
            job_title: null,
            insurance_no: '',
            expiry_date:''

        },
        companyContacts: {
            company_id: '',
            wp_id: '',
            contact_name: '',
            title: '',
            job_title: '',
            email1: '',
            email2: '',
            mobile1: '',
            website: '',
            contact_id: 0

        },
        companyWorkPlaces: {
            wp_id: '',
            wp_name: '',
            address1: '',
            address2: '',
            post_code: '',
            company_id: '',
            town: '',
            county: '',
            employee_email: '',
            phone_number: '',
            job_title: 0,
            business_type: '',
            company_name:''
        },
        isCompanyContactSubmitted: false,
        isWorkplaceContactSubmitted: false,
        isCompanyWorkPlaceSubmitted: false,
        CompanyContact: [],
        isSubmitted: false,
        isNotesSubmitted: false,
        isDocumentSubmitted: false,
        isActivitySubmitted: false,
        isCSLSubmitted: false,
        isTaskSubmitted: false,
        NotesCategories: {},
        parameters: {},
        SalesAdvisors: [],
        Countries: [],
        Note: { Note_Id: 0, Note_Id_Company: 0, Note_Description: '', Note_Id_Category: null },
        Notes: [],
        CompanyGroup: [],
        wp_id: '',
        JobTitles: [],
        Titles: [],
        companyContactJobTitle: '',
        companyWorkPlaceJobTitle: '',
        companyWorkPlaceBusinessType: '',
        website: '',
        companyDuplicateList: [],
        assignLearnnerList: [],
        companyNameForLearner: '',
        CompanyAfterDuplicate: [],
        oldCompanyName: '',
        oldPostcode:'',
        Validations:[]

    },
    validations: {
        company: {
            name: {
                required: validators.required
            }
            
        },
        Note: {
            Note_Description: {
                required: validators.required
            },
            Note_Id_Category: {
                required: validators.required
            }
        },
      
    },
    methods: {
        ViewLearner: function (id) {
            //window.location.href = "http://dmss.uk/LearnerTrack/LearnersDetails.aspx?id=" + id;
            window.open('http://dmss.uk/LearnerTrack/LearnersDetails.aspx?id='+id ,'_blank')
        },
        EditLead: function (leadId, activityTab) {
            //alert("Edit Id : " + leadId);
            if (!activityTab) {
                window.location.href = 'Lead/Edit?id=' + leadId;
            } else {
                window.location.href = 'Lead/Edit?id=' + leadId + '&Act=Y';
            }
        },

        ChangeContact: function (event) {
        },
        refreshData: function () {
            this.isSubmitted = true;
        },

        RefreshWorkPlaces: function () {
            $('#grid').data('kendoGrid').dataSource.read();
            $('#grid').data('kendoGrid').refresh();
            //  refreshHistory();
        },
        RefreshWorkPlaceContact: function () {
            $('#grid').data('kendoGrid').dataSource.read();
            $('#grid').data('kendoGrid').refresh();
            //  refreshHistory();
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
               
                app.SalesAdvisors = response.SalesAdvisors;
                app.Countries = response.Countries;
                app.NotesCategories = response.CompanyNoteCategory;
                app.CompanyGroup = response.CompanyGroup;
                app.JobTitles = response.JobTitles;
                app.Titles = response.Title;
                console.log(app.Titles);
                var ddl = '';

                ddl = $('#ddl_SalesAdvisor').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.SalesAdvisors);
                    ddl.refresh();
                }
                ddl = $('#ddl_Country').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.Countries);
                    ddl.refresh();
                }
                ddl = $('#ddl_CompanyGroup').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.CompanyGroup);
                    ddl.refresh();
                }
                ddl = $('#ddl_JobTitle').data("kendoDropDownList");
                if (ddl !== undefined) {
                    ddl.setDataSource(app.JobTitles);
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

        NewNote: function () {
            app.isNotesSubmitted = false;
            app.Note = {};
            // app.RefreshNotes();
            if (!_canRoleManagerCreateNotes) {
                toastr.error('You cannot add new note please contact administrator', 'Permission denied!');
                return false;
            }
            $('.nav-tabs a[href="#kt_tabs_1_4"]').tab('show');
            $("#kt_modal_notes1").modal("show");
        },
        RefreshNotes: function () {
            $('#companynotesGrid').data('kendoGrid').dataSource.read();
            $('#companynotesGrid').data('kendoGrid').refresh();
          //  refreshHistory();
        },
        submitNotes: function () {
            this.isNotesSubmitted = true;
            if (this.$v.Note.$invalid) {
                toastr.error('Error in validation.', 'Error!');
                return;
            }
           
            this.Note.Note_Id_Company = parameters.id;
            var data = this.Note;
            $.ajax({
                url: "/api/CompanyApi/InsertCompanyNotes",
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
                    $("#kt_modal_notes1").modal("hide");
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
                url: "/api/CompanyApi/GetNoteById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.Note = response;
                $("#kt_modal_notes1").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformNoteDelete: function (id) {
            $.ajax({
                url: "/api/CompanyApi/DeleteNoteById",
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

        NewCompanyContact: function () {
            app.isCompanyContactSubmitted = false;
            app.companyContacts = {};
            // app.RefreshNotes();
            if (!_canRoleManagerCreateCompanyContact) {
                toastr.error('You cannot add new note please contact administrator', 'Permission denied!');
                return false;
            }
            $('.nav-tabs a[href="#kt_tabs_1_3"]').tab('show');
            $("#kt_modal_companycontact").modal("show");
        },
        RefreshCompanyContact: function () {
            $('#companycontactGrid').data('kendoGrid').dataSource.read();
            $('#companycontactGrid').data('kendoGrid').refresh();
            //  refreshHistory();
        },
        submitCompanyContact: function () {
            this.isCompanyContactSubmitted = true;
            //if (this.$v.companyContacts.$invalid) {
            //    toastr.error('Error in validation.', 'Error!');
            //    return;
            //}
            debugger; // eslint-disable-line

           // var jobId = $("#jobTitle").val();
            this.companyContacts.job_title = app.companyContactJobTitle;

            this.companyContacts.company_id = parameters.id;
            this.companyContacts.wp_id = this.wp_id;
            var data = this.companyContacts;
            $.ajax({
                url: "/api/CompanyApi/InsertCompanyContact",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    if (app.compayContacts > 0) {
                        toastr.success("Contact updated successfully.", "Updated!");
                    } else {
                        toastr.success("Contact inserted successfully.", "Inserted!");
                    }
                    $("#kt_modal_companycontact").modal("hide");
                    $("#kt_modal_workplacecontact").modal("hide");

                    app.isCompanyContactSubmitted = false;
                    app.RefreshCompanyContact();
                    app.RefreshWorkPlaceContact();
                    app.companyContacts = {};
                } else {
                    toastr.error("Cannot submitted, please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformCompanyContactEdit: function (id) {
            $.ajax({
                url: "/api/CompanyApi/GetCompanyContactById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.companyContacts = response;
                $("#kt_modal_companycontact").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformCompanyContactDelete: function (id) {
            $.ajax({
                url: "/api/CompanyApi/DeleteCompanyContactById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                if (response === true) {
                    app.RefreshCompanyContact();
                    toastr.success("Contact deleted successfully.", "Success!");
                } else {
                    toastr.error("Contact cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        NewWorkPlace: function () {
            app.isCompanyWorkPlaceSubmitted = false;
            app.companyWorkPlaces = {};
            // app.RefreshNotes();
            if (!_canRoleManagerCreateNotes) {
                toastr.error('You cannot add new note please contact administrator', 'Permission denied!');
                return false;
            }
            $('.nav-tabs a[href="#kt_tabs_1_5"]').tab('show');
            $("#kt_modal_companyworkplace").modal("show");
        },
        submitWorkPlace: function () {
            this.isCompanyWorkPlaceSubmitted = true;
            //if (this.$v.companyWorkPlaces.$invalid) {
            //    toastr.error('Error in validation.', 'Error!');
            //    return;
            //}
            //var jobId = $("#jobTitleId").val();
            //var businessId = $("#bussTypeId").val();
            debugger; // eslint-disable-line

            this.companyWorkPlaces.job_title = this.companyWorkPlaceJobTitle;
            this.companyWorkPlaces.business_type = this.companyWorkPlaceBusinessType;
            this.companyWorkPlaces.company_id = parameters.id;
            var data = this.companyWorkPlaces;
            $.ajax({
                url: "/api/CompanyApi/InsertCompanyWorkPlace",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    if (app.Note.Note_Id > 0) {
                        toastr.success("Work Place updated successfully.", "Updated!");
                    } else {
                        toastr.success("Work Place inserted successfully.", "Inserted!");
                    }
                    $("#kt_modal_companyworkplace").modal("hide");
                    app.isCompanyWorkPlaceSubmitted = false;
                    app.companyWorkPlaces = {};
                    app.RefreshWorkPlaces();
                } else {
                    toastr.error("Cannot submitted, please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
            $("#jobTitleId").val("").trigger("chosen:updated");
            $("#bussTypeId").val("").trigger("chosen:updated");
        },
        PerformWorkPlaceEdit: function (wp_id) {
            $.ajax({
                url: "/api/CompanyApi/GetCompanyWorkPlaceById",
                data: { id: wp_id },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.companyWorkPlaces = response;
                $("#kt_modal_companyworkplace").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformWorkPlaceDelete: function (wp_id) {
            $.ajax({
                url: "/api/CompanyApi/DeleteCompanyWorkPlaceById",
                data: { id: wp_id },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                if (response === true) {
                    app.RefreshWorkPlaces();
                    toastr.success("WorkPlace deleted successfully.", "Success!");
                } else {
                    toastr.error("WorkPlace cannot be deleted. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        PerformWorkPlaceContactAdd: function (wp_id) {
            this.wp_id = wp_id;
            app.isWorkplaceContactSubmitted = false;
            app.companyContacts = {};
            if (!_canRoleManagerCreateNotes) {
                toastr.error('You cannot add new note please contact administrator', 'Permission denied!');
                return false;
            }
            $('.nav-tabs a[href="#kt_tabs_1_5"]').tab('show');
            $("#kt_modal_workplacecontact").modal("show");
        },

        PerformWorkPlaceContactEdit: function (id) {
            $.ajax({
                url: "/api/CompanyApi/GetCompanyContactById",
                data: { id: id },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                app.companyContacts = response;
                $("#kt_modal_workplacecontact").modal("show");
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },

        submitCompanyAfterDulicate: function () {
            this.isSubmitted = true;
            //if (this.$v.companyContacts.$invalid) {
            //    toastr.error('Error in validation.', 'Error!');
            //    return;
            //}
            Swal.fire({
                title: 'Are you sure?',
                text: "",
                type: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                confirmButtonText: 'Yes, Save it!',
                cancelButtonColor: '#d33',
                customClass: {
                    actions: 'my-actions',
                    cancelButton: 'order-1 right-gap',
                    confirmButton: 'order-2',
                    denyButton: 'order-3',
                }
              
            }).then((result) => {
                if (result.value) {
                    var data = this.CompanyAfterDuplicate;
                    $.ajax({
                        url: "/api/CompanyApi/InsertCompanyRecord",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {

                        if (response !== null) {
                            toastr.success("Company Save successfully.", "Success!");
                            $("#kt_modal_Duplicate").modal("hide");


                            location.href = '/Company/Edit?id=' + response.encodedId;


                        } else {
                            toastr.error("Cannot update. Please try again.", "Error!");
                        }
                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });


          
        },

        submitted: function (type) {
            this.isSubmitted = true;
            var root = this;
            if (this.company.name == "") {
                //$("#companyMobilemsg").hide();
                //$("#companyPostcodemsg").hide();
                //$("#companyAddressmsg").hide();
                $("#companyNamemsg").show();
                return;
            }
            if (this.company.address1 == "") {
                $("#companyAddressmsg").show();
                return;
            }
            if (this.company.postcode == "") {
                $("#companyPostcodemsg").show();
                return;
            }
            if (this.company.tel1 == "") {
                $("#companyMobilemsg").show();
                return;
            }
            //  this.Company.Company_sales_advisor_id = parseInt($('#ddl_SalesAdvisor').val());
            this.company.sales_advisor_id = parseInt($('#ddl_SalesAdvisor').val());
            this.company.country_id = parseInt($('#ddl_Country').val());
            this.company.company_group_id = parseInt($('#ddl_CompanyGroup').val());
            this.company.business_type = $('#ddl_BusinessType').val();
            this.company.levy_employer = $('#ddl_LevyEmployer').val();
            this.company.no_of_employees = $('#ddl_NoEmployees').val();
            var job = $('#ddl_JobTitle').val();
            if (job != "") {
                this.company.job_title = parseInt($('#ddl_JobTitle').val());
            }
            
            if (this.company.job_title == NaN) {
                this.company.job_title = 0;
            }
            this.company.cqc_rating = $("#ddl_CQCRating").val();
            if (this.$v.company.$invalid) {
                toastr.error('Error in validation', 'Error!');
                return;
            }
            var data = this.company;

            $.ajax({
                url: "/api/CompanyApi/CheckDuplicateCompany",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                console.log(response);

                if (response.length > 0) {
                    root.companyDuplicateList = response;
                    root.CompanyAfterDuplicate = root.company;
                    if (root.oldCompanyName != "") {
                        if (root.oldCompanyName != root.company.name || root.oldPostcode != root.company.postcode) {
                            $("#kt_modal_Duplicate").modal("show");

                            toastr.error("Duplicate. Those Company Already Exist. With Postcode", "Error!");
                            return false;
                        }
                        else {
                            $.ajax({
                                url: "/api/CompanyApi/InsertCompanyRecord",
                                data: JSON.stringify(data),
                                type: "Post",
                                contentType: "application/json",
                                dataType: "json"
                            }).done(function (response) {

                                if (response !== null) {
                                    toastr.success("Company updated successfully.", "Success!");
                                    //// if type is 1 then save and go back.
                                    //// if type is 2 then save and sign out.
                                    //// if type is 3 then save and Add new.
                                    if (type === 1) {
                                        location.href = "/Company/Manage";
                                    } else if (type === 2) {
                                        location.href = "/Account/Logout";
                                    } else if (type === 3) {
                                        location.href = "/Company/Create";
                                    } else {
                                        location.href = '/Company/Edit?id=' + response.encodedId;
                                    }
                                } else {
                                    toastr.error("Cannot update. Please try again.", "Error!");
                                }
                            }).fail(function (xhr) {
                                toastr.error(xhr.responseText, 'Error!');
                            });
                        }
                    } else {
                        $("#kt_modal_Duplicate").modal("show");

                        toastr.error("Duplicate. Those Company Already Exist. With Postcode", "Error!");
                        return false;
                    }
                } else {
                    $.ajax({
                        url: "/api/CompanyApi/InsertCompanyRecord",
                        data: JSON.stringify(data),
                        type: "Post",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function(response) {

                        if (response !== null) {
                            toastr.success("Company updated successfully.", "Success!");
                            //// if type is 1 then save and go back.
                            //// if type is 2 then save and sign out.
                            //// if type is 3 then save and Add new.
                            if (type === 1) {
                                location.href = "/Company/Manage";
                            } else if (type === 2) {
                                location.href = "/Account/Logout";
                            } else if (type === 3) {
                                location.href = "/Company/Create";
                            } else {
                                location.href = '/Company/Edit?id=' + response.encodedId;
                            }
                        } else {
                            toastr.error("Cannot update. Please try again.", "Error!");
                        }
                    }).fail(function(xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                }
            });
        },

        mounted: function () {
            var root = this;

          


            $("#companyMobilemsg").hide();
            $("#companyPostcodemsg").hide();
            $("#companyAddressmsg").hide();
            $("#companyNamemsg").hide();

            $('#txt_inspenction_Date').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                todayHighlight: true
            }).on('changeDate', () => { this.company.cqc_inspection_date = $('#txt_inspenction_Date').val(); });

            $("#txt_inspenction_Date").datepicker().datepicker("setDate", new Date());

            $('#txt_ExpiryDate').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                todayHighlight: true
            }).on('changeDate', () => { this.company.expiry_date = $('#txt_ExpiryDate').val(); });

            $("#txt_ExpiryDate").datepicker().datepicker("setDate", new Date());

            $('#txt_last_inspenction_Date').datepicker({
                format: 'dd/mm/yyyy',
                autoclose: true,
                todayHighlight: true
            }).on('changeDate', () => { this.company.cqc_last_update_date = $('#txt_last_inspenction_Date').val(); });

            $("#txt_last_inspenction_Date").datepicker().datepicker("setDate", new Date());
            parameters = this.$route.query;
            if (_companyId !== undefined) {

                parameters.id = _companyId;
                if (parameters.id > 0) {
                    if (!_canRoleManagerEditCompany) {
                        window.location.href = '/Error/PermissionDenied';
                    }
                  
                    app.parameters.id = parameters.id;
                    app.parameters.Act = parameters.Act;
                    $.ajax({
                        url: "/api/CompanyApi/GetCompanyById",
                        data: { id: parameters.id },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        app.company = response;
                        app.oldCompanyName = response.name;
                        app.oldPostcode = response.postcode;
                        //if (response.website != "") {
                        //    var regex = /https?:\/\/[\-A-Za-z0-9+&@#\/%?=~_|$!:,.;]*/g;
                        //    var regex2 = /http?:\/\/[\-A-Za-z0-9+&@#\/%?=~_|$!:,.;]*/g;
                        //    var regex3 = /www:\/\/[\-A-Za-z0-9+&@#\/%?=~_|$!:,.;]*/g;

                        //    if (response.website.match(regex) || response.website.match(regex2)) {
                        //        $("#btnIcon").attr('disabled', false);
                        //        app.website = response.website;
                        //        //$("#link_label").html("Your Text Contains Link");
                        //    }
                        //    else {
                        //        var url = response.website;
                        //        var web = 'https://' + url;
                        //        if (web.match(regex)) {
                        //            app.website = web;
                        //            $("#btnIcon").attr('disabled', false);

                        //        }
                        //        else {
                        //            $("#btnIcon").attr('disabled', true);
                        //        }

                        //    }


                        //}
                        $("#lblEditHeader").text(response.name + ' - ' + response.id);
                        root.companyNameForLearner = response.name;
                        if (response.cqc_inspection_date !== null && response.cqc_inspection_date !== "") {
                            $('#txt_inspenction_Date').datepicker('update', response.cqc_inspection_date);

                        }
                        $("#ddl_SalesAdvisor").data("kendoDropDownList").value(response.sales_advisor_id);
                        //$("#ddl_Country").data("kendoDropDownList").value(response.country_id);
                        $("#ddl_CompanyGroup").data("kendoDropDownList").value(response.company_group_id);

                        if (response.cqc_last_update_date !== null && response.cqc_last_update_date !== "") {
                            $('#txt_last_inspenction_Date').datepicker('dd/mm/yyyy', response.cqc_last_update_date);
                           
                        }
                        if (response.expiry_date !== null && response.expiry_date !== "") {
                            $('#txt_ExpiryDate').datepicker('dd/mm/yyyy', response.expiry_date);

                        }

                        $("#ddl_BusinessType").data("kendoDropDownList").value(response.business_type);
                        $("#ddl_LevyEmployer").data("kendoDropDownList").value(response.levy_employer);
                        $("#ddl_CompanyGroup").data("kendoDropDownList").value(response.company_group_id);
                        $("#ddl_CQCRating").data("kendoDropDownList").value(response.cqc_rating);
                        $("#ddl_JobTitle").data("kendoDropDownList").value(response.job_title);
                        $("#ddl_NoEmployees").data("kendoDropDownList").value(response.no_of_employees);


                        if ($("#hfd_roleName").val() !== "Admin" && $("#hfd_roleName").val() !== "Administrator") {
                            //  $("#ddl_SalesAdvisor").data("kendoDropDownList").value($("#hfd_userId").val());
                        }

                        //viewlead(owner);
                        $('#txt_inspenction_Date').datepicker({ format: 'dd/mm/yyyy' });
                        $('#txt_last_inspenction_Date').datepicker({ format: 'dd/mm/yyyy' });

                        if ($("#hfd_owner").val() == "0") {
                            //for viewlead only
                            $("#ddl_SalesAdvisor").attr("disabled", "disabled");

                            $("k-dropdown").attr("disabled", "disabled");
                            $('select').attr("disabled", "disabled");
                            $('span').attr("disabled", "disabled");
                            $('input').attr("disabled", "disabled");
                            $('textarea').attr("disabled", "disabled");
                            $('#companynotesGrid').attr("disabled", "disabled");
                            $(".k-multiselect").attr("disabled", "disabled");

                            $('.btnhide').hide();
                           // $('.nav-tabs a[href="#kt_tabs_1_5"]').hide();
                           // $('.nav-tabs a[href="#kt_tabs_1_6"]').hide();

                            $('#kt_modal_notes1').find('select').removeAttr("disabled");
                            $('#kt_modal_notes1').find('textarea').removeAttr("disabled");

                            $('#txt_Search_Query').removeAttr("disabled");

                        }

                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                    debugger; // eslint-disable-line
                  
                    $.ajax({
                        url: "/api/CompanyApi/LoadLeaner",
                        data: { id: parameters.id },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                     
                        root.assignLearnnerList = response.SPs;
                        console.log(root.assignLearnnerList);
                    }).fail(function (xhr) {
                    });

                } 
            }
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

        $('#companynotesGrid').data('kendoGrid').dataSource.read();
        $('#companynotesGrid').data('kendoGrid').refresh();
    }
}

function PerformCompanyContactEdit(company_Id) {
    if (!_canRoleManagerEditCompanyContact) {
        toastr.error('You cannot edit please contact administrator', 'Permission denied!');
        return false;
    }
    app.PerformCompanyContactEdit(company_Id);
}
function PerformCompanyContactDelete(company_Id) {
    if (!_canRoleManagerDeleteCompanyContact) {
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
            app.PerformCompanyContactDelete(company_Id);
        }
    });
}
function PerformWorkPlaceEdit(wp_id) {
    if (!_canRoleManagerEditCompanyWorkplace) {
        toastr.error('You cannot edit please contact administrator', 'Permission denied!');
        return false;
    }
    app.PerformWorkPlaceEdit(wp_id);
}
function PerformWorkPlaceDelete(wp_id) {
    if (!_canRoleManagerDeleteCompanyWorkplace) {
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
            app.PerformWorkPlaceDelete(wp_id);
        }
    });
}
function PerformWorkPlaceContactAdd(wp_id) {
    if (!_canRoleManagerCreateCompanyWorkplace) {
        toastr.error('You cannot delete please contact administrator', 'Permission denied!');
        return false;
    }
    app.PerformWorkPlaceContactAdd(wp_id);

}

function PerformWorkPlaceContactEdit(contact_id) {
    if (!_canRoleManagerCreateCompanyWorkplace) {
        toastr.error('You cannot delete please contact administrator', 'Permission denied!');
        return false;
    }
    app.PerformWorkPlaceContactEdit(contact_id);

}

function PerformLeadEdit(lead_Id, activityTab) {
    //if (_canRoleManagerEditLead) {
        app.EditLead(lead_Id, activityTab);
    //} else {
    //    toastr.error('You cannot edit please contact administrator', 'Permission denied!');
    //}
}
//function validURL(str) {
//    var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
//        '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
//        '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
//        '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
//        '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
//        '(\\#[-a-z\\d_]*)?$', 'i'); // fragment locator
    
//    return !!pattern.test(str);
//}
var url = "";
function validURL(str) {
    var regex = /(http|https):\/\/(\w+:{0,1}\w*)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%!\-\/]))?/;
    if (!regex.test(str)) {
        alert("Please enter valid URL.");
        return false;
    } else {
        return true;
    }

};

function myFunction() {
    var x = document.getElementById("webiste");
    debugger; // eslint-disable-line

  //  var text = document.getElementById("webiste").value;
  //  var exp = /(\b(https?|ftp|file):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig;
  //  var text1 = text.replace(exp, "<a href='$1'>$1</a>");
  //  var exp2 = /(^|[^\/])(www\.[\S]+(\b|$))/gim;
  ////  document.getElementById("converted_url").innerHTML = text1.replace(exp2, '$1<a target="_blank" href="http://$2">$2</a>');
  //  x.value = x.value.toUpperCase();
    var regex = /https?:\/\/[\-A-Za-z0-9+&@#\/%?=~_|$!:,.;]*/g;
    var regex2 = /http?:\/\/[\-A-Za-z0-9+&@#\/%?=~_|$!:,.;]*/g;
    var regex3 = /www:\/\/[\-A-Za-z0-9+&@#\/%?=~_|$!:,.;]*/g;

    if ($('#webiste').val().match(regex) || $('#webiste').val().match(regex2)) {
        $("#btnIcon").attr('disabled', false);
        app.website = $('#webiste').val();
        //$("#link_label").html("Your Text Contains Link");
    }
    else {
        var url = $('#webiste').val();
        var web = 'https://' + url;
        if (web.match(regex)) {
            app.website = web;
            $("#btnIcon").attr('disabled', false);

        }
        else {
            $("#btnIcon").attr('disabled', true);
        }
    }
}

///var testCase1 = "http://en.wikipedia.org/wiki/Procter_&_Gamble";

//alert(validURL(url)); 

$("#jobTitle").on('change', function () {
    app.companyContactJobTitle = this.value;
 
});

$("#jobTitleId").on('change', function () {
    app.companyWorkPlaceJobTitle = this.value;

});

$("#bussTypeId").on('change', function () {
    app.companyWorkPlaceBusinessType = this.value;

});
$("#btnIcon").on('click', function () {
    window.open(app.website, "_blank");
})
