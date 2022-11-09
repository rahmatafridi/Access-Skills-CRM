
Vue.use(vuelidate.default)
var router = new VueRouter({
	mode: 'history',
	routes: []
});
var app = new Vue({
	router,
	el: '#dv_Manage_Application',
	data: {
		isSubmitted: false,
		isOfficeUseSubmitted: false,
		isOfficeUse1Submitted: false,
		buttonText: 'Add',
		ApplicationSection: { ase_id: 0, ase_id_appstep: 0, ase_title: null, ase_sortorder: 0, ase_description: null },
		ApplicationStep: { as_id: 0, as_title: null, as_sortorder: 0, as_description: null, as_icon: null },
		ApplicationSections: [],
		ApplicationSteps: [],
		ApplicationOfficeUseSections: [],
		ApplicationOfficeUseSteps: [],

		//Application: { app_islivedeulast3years: 'howw', app_educ_previouscollege: 'are you', app_nationality: '4' }
		Application: {},
		officeUse: {
			app_id: 0, app_officeuse_uniquelearnerreference: null, app_officeuse_startdate: null, app_officeuse_enddate: null, app_officeuse_apprenticeshipframework: null,
			app_officeuse_learnerid: null, app_officeuse_cqcinspectiondate: null, app_officeuse_ukprn: null, app_officeuse_employerid: null,
			app_officeuse_referenceid: null, app_officeuse_isevidenceseen: null, app_officeuse_referenceidtype: null, app_officeuse_referencedate: null,
			app_officeuse_fundingid: 0, app_officeuse_fundingtitle: null, app_officeuse1_courselevel: 0
		},
		officeUse1: {
			app_id: 0, app_officeuse1_courseid: 0, app_officeuse1_coursetitle: '', app_officeuse1_isliteracynumeracydone: false,
			app_officeuse1_isalldocumentssigned: false, app_officeuse1_isconfirmenrolment: false
		},
		CourseLevels: [],
		Courses: [],
		TempList: [],
		SubmitingArr: [],
		appId: 0,
		adminQuestion: [],
		SubmitingAdmin: [],
		CourseLevel: [],
		fileData: '',
		file_extension: '',
		extensionType: [],
		fileName: '',
		isFile: false,
		maxMinArrAdmin: [],
		adminError: false,
		numberForChecks: 0,
		multiCheckArr: [],
		questionsArr: [],
		Validation: [],
		Dependent: [],
		error: '',
		freeTextError: false,
		dropDownError: false,
		textAreaError: false,
		AppDependentArr: [],
		dependId: '',
		dependId1: '',
		isValid: false,
		isValid2: false,
		q_privacy_notice: '',
		q_privacy_notice_answer: '',
		yescheck: false,
		nocheck: false,

		questionsArrAdmin: [],
		DependentAdmin: [],
		adminIsvalid: false,
		Enrolled: [],
		isEnrolled: false,
		EnrolledSubmit: [],
		ConfirmQuestion: [],
		signaturepad: '',
		printName: '',
		LearnerId: "",
		signature: '',



	},
	validations: {
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
		Populate: function () {
			var root = this;

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

			//app.ApplicationSteps.push({
			//    as_id: 1, as_title: 'About You', as_sortorder: 1, as_description: null, as_icon: 'flaticon-avatar',

			//});
			//app.ApplicationSteps.push({ as_id: 2, as_title: 'About Qualifications', as_sortorder: 2, as_description: null, as_icon: 'flaticon-list' });
			//app.ApplicationSteps.push({ as_id: 3, as_title: 'About Employer', as_sortorder: 3, as_description: null, as_icon: 'flaticon-responsive' });
			//app.ApplicationSteps.push({ as_id: 4, as_title: 'About Job Roles', as_sortorder: 4, as_description: null, as_icon: 'flaticon-truck' });
			//app.ApplicationSteps.push({ as_id: 5, as_title: 'Confirmation and Submit', as_sortorder: 4, as_description: null, as_icon: 'flaticon-globe' });
			this.getExtention();
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

			

			parameters = this.$route.query;
			parameters.id = _app_id;
			this.getIsEnrolled(_app_id);
			//this.loadSignature(_app_id);
			$.ajax({
				url: "/api/ApplicationApi/GetDependentAnswerById",
				type: "GET",
				contentType: "application/json",
				data: { id: parameters.id },
				dataType: "json"
			}).done(function (response) {
				//if (response.length == 0) {
				//	location.href = "/Application/Index";
				//}
				app.Dependent = response;

			}).fail(function (xhr) {
				toastr.error(xhr.responseText);
			});

			$.ajax({
				url: "/api/ApplicationApi/GetDependentAnswerForAdminById",
				type: "GET",
				contentType: "application/json",
				data: { id: parameters.id },
				dataType: "json"
			}).done(function (response) {
				//if (response.length == 0) {
				//	location.href = "/Application/Index";
				//}
				app.DependentAdmin = response;

			}).fail(function (xhr) {
				toastr.error(xhr.responseText);
			});
	


			$.ajax({
				url: "/api/ApplicationApi/GetApplicationForAdmin",
				type: "GET",
				contentType: "application/json",
				data: { id: parameters.id },
				dataType: "json"
			}).done(function (response) {
			
				//if (response.length == 0) {
				//	location.href = "/Application/ManageApplication";
				//}
				app.ApplicationOfficeUseSteps = response;
				app.$nextTick(() => {

					$.each(app.ApplicationOfficeUseSteps, function (index1, appStep) {
						$.each(appStep.applicationsections, function (index2, appSection) {

							$.each(appSection.applicationquestions, function (index3, appQuestion) {
								app.questionsArrAdmin.push(appQuestion);

								if (appQuestion.q_is_mandatory) {
									//"myRules." + appQuestion.q_mapname + "=  { required: true };"
									myRules = addToObject(myRules, appQuestion.q_mapname, { required: true });
								}
							
								if (appQuestion.q_type == 1) {
									//$('#' + appQuestion.q_mapname).datepicker({
									//	format: 'dd/mm/yyyy',
									//	todayHighlight: true
									//});
									app.maxMinArrAdmin.push(appQuestion);
								}
								

							});
						});
					});
				//	KTWizard1.init();
				})
			}).fail(function (xhr) {
				toastr.error(xhr.responseText);
			});

			$.ajax({
				url: "/api/ApplicationApi/GetApplicationStepsForLearner",
				type: "GET",
				contentType: "application/json",
				data: { id: parameters.id },
				dataType: "json"
			}).done(function (response) {
				if (response.length == 0) {
					location.href = "/Application/ManageApplication";
				}
				app.ApplicationSteps = response;
				app.$nextTick(() => {

					$.each(app.ApplicationSteps, function (index1, appStep) {
						$.each(appStep.applicationsections, function (index2, appSection) {

							$.each(appSection.applicationquestions, function (index3, appQuestion) {
								root.questionsArr.push(appQuestion);

								if (appQuestion.q_is_mandatory) {
									//"myRules." + appQuestion.q_mapname + "=  { required: true };"
									myRules = addToObject(myRules, appQuestion.q_mapname, { required: true });
								}
								if (appQuestion.q_type == 11) {
									appQuestion.q_ddlMultiCheck.forEach(function (e) {
										if (e.Check_Value == 1) {
											root.multiCheckArr.push({ answer: "", name: e.Opt_Value, q_id: appQuestion.q_mapname })
										}
									})
								}
								//if (appQuestion.q_type == 5) {
								//	$('#' + appQuestion.q_mapname).datepicker({
								//		format: 'dd/mm/yyyy',
								//		autoclose: true,
								//		todayHighlight: true
								//	}).on('changeDate', () => { appQuestion.answer = $('#' + appQuestion.answer).val(); });
								//}

							});
						});
					});
					KTWizard1.init();
				})
			}).fail(function (xhr) {
				toastr.error(xhr.responseText);
			});

			$.ajax({
				url: "/api/ListApi/GetDropdownOptionsByHeaderName",
				type: "GET",
				contentType: "application/json",
				data: { headerName: "CourseLevels" },
				dataType: "json",
			}).done(function (response) {
				app.CourseLevels = response;
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});
			$.ajax({
				url: "/api/ApplicationApi/GetAllCourses",
				type: "GET",
				contentType: "application/json",
				//data: { headerName: "CourseLevels" },
				dataType: "json",
			}).done(function (response) {
				app.Courses = response;
				console.log(app.Courses);
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});
		},
		mounted: function () {
			parameters = this.$route.query;
			parameters.id = _app_id;
			this.appId = _app_id;
			this.loadConfirmQuestion(_app_id);
			this.loadSignature(_app_id);
			if (parameters.id > 0) {

				$.ajax({
					url: "/api/ApplicationApi/GetQuestionAnswerById",
					data: { id: parameters.id },
					type: "GET",
					contentType: "application/json",
					dataType: "json"
				}).done(function (response) {
					//app.Application = response.q_mapname;
					var arr = [];
					response.forEach((value, index) => {
						arr.push(value.q_mapname);
					});

					//var temp = [];
					//app.temp = arr;

					$.each(app.ApplicationSteps, function (index1, appStep) {
						$.each(appStep.applicationsections, function (index2, appSection) {
							$.each(appSection.applicationquestions, function (index3, appQuestion) {

								//if (appQuestion.q_type == 5) {
								//	if (appQuestion.q_mapname !== null && appQuestion.q_mapname !== "") {
								//		$('#' + appQuestion.answer).datepicker('update', appQuestion.q_mapname);
								//	}
								//}

							});
						});
					});

					
					app.$nextTick(() => {
						$("#txtPrintName").keyup();
					})
				}).fail(function (xhr) {
					toastr.error(xhr.responseText, 'Error!');
				});
			} else {
				//if (!_canRoleManagerCreateRole) {
				//    window.location.href = '/Error/PermissionDenied';
				//}
			}
		},
		AddEditApplication: function (isSubmit) {
			//if (this.Configration.config_id === 0 && !_canRoleManagerCreateConfig) {
			//	toastr.error('You cannot add new Configuration please contact administrator.', 'Permission denied!');
			//	return false;
			//}
			var root = this;
			//if ($('input[name=radio2]:checked').val() == undefined) {
			//	alert("Please tick yes or no if you wish to be contacted.");
			//	return;
			//}
			var printnamecontrol = $("#txtPrintName");
			if (this.printName.length < 5) {
				alert("Please type your full name.");
				return;
			}

			const makes = new Set();
			this.multiCheckArr.forEach(a => makes.add(a.q_id));
			var make = Array.from(makes)

			make.forEach(function (x) {
				var fileMulitArr = root.multiCheckArr
					.filter(a => a.q_id === x);
				var ans = "";
				var i = 0;
				var id = "";
				fileMulitArr.forEach(function (e) {
					id = e.q_id;
					if (i == 0) {
						ans += e.name
					}
					else {
						ans += "," + e.name
					}
					i += 1;
				})
				root.SubmitingArr.push({ q_mapname: id, q_answer: ans });

			})


			var canvas = document.getElementById('signaturepad');
			var context = canvas.getContext('2d');
			var dataUri = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");  // here is the most important part because if you dont replace you will get a DOM 18 exception.

			var text = this.printName;
			$("#sketch_data").val(dataUri);
			app.Application.dataUri = dataUri;
			//var hiddencontrol = $("#<%=sketch_data_label.ClientID%>");
			//$(hiddencontrol).val(dataUri);

			//$("#hid_printname").val(printnamecontrol.val());
			//var hiddencontrol2 = $("#<%=hid_printname_label.ClientID%>");
			//$(hiddencontrol2).val($("#hid_printname").val());

			//return;

			app.Application.dataUri = dataUri;
			var submitArr = [];
			this.isSubmitted = true;
			this.SubmitingArr.push({ q_mapname: "id", q_answer: this.appId });


			this.SubmitingArr.push({ q_mapname: "sing", q_answer: dataUri, text: text});

			var data = this.SubmitingArr;
			var app_id = app.Application.app_id;

			$.ajax({
				url: "/api/ApplicationApi/InsertUpdateApplication",
				data: JSON.stringify(data),
				type: "Post",
				contentType: "application/json",
				dataType: "json",
			}).done(function (response) {
				if (response === true) {
					app.isSubmitted = false;
					//if (app.Application.app_id > 0) {
					//	if (isSubmit) {
					   app.SubmitApplication(root.appId);
							toastr.success("Record updated successfully.", "Updated!");
						//}
						
					//} 
					
				} else {
					toastr.error('Record have not been saved. Please try again.', 'Error!');
				}
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});


		},
		SubmitApplication: function (app_id) {
			$.ajax({
				url: "/api/ApplicationApi/SubmitApplication",
				type: "GET",
				contentType: "application/json",
				data: { id: this.appId },
				dataType: "json",
			}).done(function (response) {
				location.href = "/Application/ManageApplication";
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});
		},
		UpdateOfficeUseOnly: function () {

			app.isOfficeUseSubmitted = true;
			app.officeUse.app_id = app.Application.app_id;
			//var data = app.officeUse;
			//var isCourseLevel = false;

			//if (app.ApplicationStep.length > 0) {
			//	isCourseLevel = true;
			//}
			this.SubmitingAdmin.push({ q_mapname: "id", q_answer: this.appId });



			var data = this.SubmitingAdmin;


			$.ajax({
				url: "/api/ApplicationApi/InsertUpdateApplication",
				data: JSON.stringify(data),
				type: "Post",
				contentType: "application/json",
				dataType: "json",
			}).done(function (response) {
				if (response === true) {
					//if (!isCourseLevel) {
					//	location.reload();
					//}
					app.isOfficeUseSubmitted = false;
					toastr.success("Record updated successfully.", "Office Use Success!");
				} else {
					toastr.error('Record have not been saved. Please try again.', 'Office Use Error!');
				}
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});
		},
		UpdateOfficeUse1: function () {

			app.isOfficeUse1Submitted = true;
			app.officeUse1.app_id = app.Application.app_id;
			app.officeUse1.app_officeuse1_coursetitle = $("#ddl_course_level option:selected").text();
			var data = app.officeUse1;

			$.ajax({
				url: "/api/ApplicationApi/UpdateOfficeUse1",
				data: JSON.stringify(data),
				type: "Post",
				contentType: "application/json",
				dataType: "json",
			}).done(function (response) {
				if (response.update_success === true && response.portal_user_success === true && response.osca_learner_success === true && response.is_qcs_success === true) {
					app.isOfficeUseSubmitted = false;
					toastr.success("Record updated successfully.", "Office Use1 Success!");
				} else {
					toastr.error(response.error_message, 'Office Use1 Error!');
				}
				location.href = "/Application/ManageApplication";
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});
		},
		UpdateEndrolled: function () {

			app.isOfficeUseSubmitted = true;
			//app.officeUse.app_id = app.Application.app_id;
			//var data = app.officeUse;
			//var isCourseLevel = false;

			//if (app.ApplicationStep.length > 0) {
			//	isCourseLevel = true;
			//}
			this.EnrolledSubmit.push({ q_mapname: "id", q_answer: this.appId });



			var data = this.EnrolledSubmit;


			$.ajax({
				url: "/api/ApplicationApi/InsertUpdateEnrollmentApplication",
				data: JSON.stringify(data),
				type: "Post",
				contentType: "application/json",
				dataType: "json",
			}).done(function (response) {
				app.isEnrolled = false;
				app.getIsEnrolled(_app_id);
				if (response === true) {
					//if (!isCourseLevel) {
					//	location.reload();
					//}
					app.isOfficeUseSubmitted = false;
					toastr.success("Record updated successfully.", "Office Use Success!");
				} else {
					toastr.error('Record have not been saved. Please try again.', 'Office Use Error!');
				}
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});

		},
		handleBlur(e) {
			debugger
			var root = this;

			if (e.target.value != "") {
				// Validation
				var validationId = root.questionsArr.filter(function (a) {
					if (a.q_mapname == e.target.id) return a.q_validation_id;
				});

				if (validationId.length > 0) {
					var validation = root.Validation.filter(function (x) {
						if (x.v_id == validationId[0].q_validation_id) return x.q_type_name;
					});
					var values = e.target.value;
					if (validationId[0].q_type == 5) {
						values = new Date(values).toLocaleDateString('en-GB');
					}
					if (validation[0].q_type_regex != null) {
						root.reg = new RegExp(validation[0].q_type_regex.toString());

						if (root.reg.test(values)) {

							$('#' + e.target.id + 1).text("");
							root.isValid = false;
						} else {
							$('#' + e.target.id + 1).text("");
							$('#' + e.target.id + 1).text(validation[0].q_validation_massage);
							root.isValid = true;

						}
					}
					if (validation[0].q_max > 0) {
						if (e.target.value.length > validation[0].q_mix) {
							$('#' + e.target.id + 2).text("");
							var value = "Enter Char Less Then " + validation[0].q_mix;
							$('#' + e.target.id + 2).text(value);
							root.isValid2 = true;
						}
						else {
							$('#' + e.target.id + 2).text("");
							root.isValid2 = false;
						}
					}
					if (validation[0].q_min > 0) {
						if (e.target.value.length < validation[0].q_min) {
							$('#' + e.target.id + 2).text("");
							var value = "Enter Char More Then " + validation[0].q_min;
							$('#' + e.target.id + 2).text(value);
							root.isValid = true;
						} else {
							$('#' + e.target.id + 2).text("");
							root.isValid2 = false;
						}
					}
				}
				//validation
				//Dependent
				var qus = root.questionsArr.filter(function (x) {
					if (x.q_mapname == e.target.id) return x.q_mapname;
				})
				if (qus.length > 0) {
					var depend = root.Dependent.filter(function (y) {
						if (y.q_id_dependency == qus[0].q_id) return y.q_mapname;
					})
					if (depend.length > 0) {
						root.dependId = depend[0].q_mapname;
						var yesno = depend[0].q_depend_yesno;
						if (yesno == "Yes") {
							if (e.target.value == 1) {
								$('#' + root.dependId).attr('disabled', 'disabled');
							}
							else {
								$('#' + root.dependId).attr('disabled', false);
							}
						}
						if (yesno == "No") {
							if (e.target.value == 0) {
								$('#' + root.dependId).attr('disabled', 'disabled');
							} else {
								$('#' + root.dependId).attr('disabled', false);
							}
						}
					}
				}
				//Dependent
				if (root.isValid == false) {
					if (this.SubmitingArr.length > 0) {



						//var checkExist = this.SubmitingArr.find(x => x.q_mapname == e.target.id);
						var valObj = this.SubmitingArr.filter(function (elem) {
							if (elem.q_mapname == e.target.id) return elem.q_mapname;
						});
						if (valObj.length > 0) {
							this.SubmitingArr.splice(valObj, 1);
							this.SubmitingArr.push({ q_mapname: e.target.id, q_answer: e.target.value });

						}

						else {
							this.SubmitingArr.push({ q_mapname: e.target.id, q_answer: e.target.value });

						}
					}
					else {

						this.SubmitingArr.push({ q_mapname: e.target.id, q_answer: e.target.value });

					}
				}
			}
		},

		handleBlurAdmin(e) {
			debugger;
			var root = this;
			this.adminError = false;

			if (e.target.value != "") {

				// Validation
				var validationId = root.questionsArrAdmin.filter(function (a) {
					if (a.q_mapname == e.target.id) return a.q_validation_id;
				});

				if (validationId.length > 0) {
					var validation = root.Validation.filter(function (x) {
						if (x.v_id == validationId[0].q_validation_id) return x.q_type_name;
					});
					var values = e.target.value;
					if (validationId[0].q_type == 5) {
						values = new Date(values).toLocaleDateString('en-GB');
					}
					if (validation[0].q_type_regex != null) {
						root.reg = new RegExp(validation[0].q_type_regex.toString());

						if (root.reg.test(values)) {

							$('#' + e.target.id + 3).text("");
							root.adminIsvalid = false;
						} else {
							$('#' + e.target.id + 3).text("");
							$('#' + e.target.id + 3).text(validation[0].q_validation_massage);
							root.adminIsvalid = true;

						}
					}
					if (validation[0].q_max > 0) {
						if (e.target.value.length > validation[0].q_mix) {
							$('#' + e.target.id + 4).text("");
							var value = "Enter Char Less Then " + validation[0].q_mix;
							$('#' + e.target.id + 4).text(value);
							root.adminIsvalid = true;
						}
						else {
							$('#' + e.target.id + 4).text("");
							root.adminIsvalid = false;
						}
					}
					if (validation[0].q_min > 0) {
						if (e.target.value.length < validation[0].q_min) {
							$('#' + e.target.id + 4).text("");
							var value = "Enter Char More Then " + validation[0].q_min;
							$('#' + e.target.id + 4).text(value);
							root.adminIsvalid = true;
						} else {
							$('#' + e.target.id + 4).text("");
							root.adminIsvalid = false;
						}
					}
				}
				//validation
				//Dependent
				var qus = root.questionsArrAdmin.filter(function (x) {
					if (x.q_mapname == e.target.id) return x.q_mapname;
				})
				if (qus.length > 0) {
					var depend = root.DependentAdmin.filter(function (y) {
						if (y.q_id_dependency == qus[0].q_id) return y.q_mapname;
					})
					if (depend.length > 0) {
						root.dependId1 = depend[0].q_mapname;
						var yesno = depend[0].q_depend_yesno;
						if (yesno == "Yes") {
							if (e.target.value == 1) {
								$('#' + root.dependId1).attr('disabled', 'disabled');
							}
							else {
								$('#' + root.dependId1).attr('disabled', false);
							}
						}
						if (yesno == "No") {
							if (e.target.value == 0) {
								$('#' + root.dependId1).attr('disabled', 'disabled');
							} else {
								$('#' + root.dependId1).attr('disabled', false);
							}
						}
					}
				}
				//Dependent


				
				if (this.SubmitingAdmin.length > 0) {
					//var checkExist = this.SubmitingArr.find(x => x.q_mapname == e.target.id);
					var valObj = this.SubmitingAdmin.filter(function (elem) {
						if (elem.q_mapname == e.target.id) return elem.q_mapname;
					});
					if (valObj.length > 0) {
						this.SubmitingAdmin.splice(valObj, 1);
						this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: e.target.value });

					}

					else {
						this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: e.target.value });

					}
				}
				else {
					this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: e.target.value });

				}
			}
		},
		dateSelected(e) {
			if (e != undefined) {
				console.log(this.appointment.appointmet_date);
				this.$nextTick(() => console.log(this.appointment.appointmet_date));
			}
		},
		checked(e) {
			if (e != undefined) {
				debugger
				var root = this;
				var enrollcount = this.Enrolled.length;
				var IsEnroll = this.Enrolled.filter(function (x) {
					if (x.q_mapname == e.target.id) return x.q_mapname
				})
				if (IsEnroll.length > 0) {

					if (this.EnrolledSubmit.length > 1) {
						const arr = this.EnrolledSubmit;
						function getUniqueListBy(arr, key) {
							return [...new Map(arr.map(item => [item[key], item])).values()]
						}
						debugger;
						const arr1 = getUniqueListBy(arr, 'q_mapname')
						console.log(JSON.stringify(arr1))
						this.EnrolledSubmit = arr1;
					}
					var ans = 0;
					if (e.target.checked == true) {
						ans = 1;
					}
					if (ans == 0) {
						var valObj1 = this.EnrolledSubmit.filter(function (elem) {
							if (elem.q_mapname == e.target.id) return elem.q_mapname;
						});
						if (valObj1.length > 0) {
							//if (valObj1.length > 1) {
							//	root.EnrolledSubmit.splice(valObj1, 2);
							//}
							//else {
								root.EnrolledSubmit.splice(valObj1, 1);
                            //}
							//valObj1.forEach(function (x) {
							//	root.EnrolledSubmit.splice(x, 1);

							//})

						}
					}
					if (ans == 1) {
						debugger
						var val = this.EnrolledSubmit.filter(function (x) {
							if (x.q_mapname == e.target.id && x.answer == 1) return x.q_mapname;
                        });
						if (val.length == 0) {
							this.EnrolledSubmit.push({ q_mapname: e.target.id, q_answer: ans });
						}
					}
					if (this.EnrolledSubmit.length > 1) {
						
						if (this.EnrolledSubmit.length == enrollcount) {
							var valObj1 = this.questionsArrAdmin.filter(function (elem) {
								if (elem.q_title == "Learner ID") return elem.q_mapname;
							});
							if (valObj1[0].answer != null) {
								this.isEnrolled = false;
							} else {
								this.isEnrolled = true;

                            }
						}
						else {
							this.isEnrolled = false;
						}
					} else {
						this.isEnrolled = false;
                    }
					
				}
				else {
					if (this.SubmitingAdmin.length > 0) {
						//var checkExist = this.SubmitingArr.find(x => x.q_mapname == e.target.id);
						var valObj = this.SubmitingAdmin.filter(function (elem) {
							if (elem.q_mapname == e.target.id) return elem.q_mapname;
						});
						if (valObj.length > 0) {
							this.SubmitingAdmin.splice(valObj, 1);
							var ans = 0;
							if (e.target.checked == true) {
								ans = 1;
							}
							this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: ans });

						}

						else {
							var ans = 0;
							if (e.target.checked == true) {
								ans = 1;
							}
							this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: ans });

						}
					}
					else {
						var ans = 0;
						if (e.target.checked == true) {
							ans = 1;
						}
						this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: ans });

					}
				}
			}
		},
		checkedLearner(e) {
			if (e != undefined) {

				if (this.SubmitingArr.length > 0) {
					//var checkExist = this.SubmitingArr.find(x => x.q_mapname == e.target.id);
					var valObj = this.SubmitingArr.filter(function (elem) {
						if (elem.q_mapname == e.target.id) return elem.q_mapname;
					});
					if (valObj.length > 0) {
						this.SubmitingArr.splice(valObj, 1);
						var ans = 0;
						if (e.target.checked == true) {
							ans = 1;
						}
						this.SubmitingArr.push({ q_mapname: e.target.id, q_answer: ans });

					}

					else {
						var ans = 0;
						if (e.target.checked == true) {
							ans = 1;
						}
						this.SubmitingArr.push({ q_mapname: e.target.id, q_answer: ans });

					}
				}
				else {
					var ans = 0;
					if (e.target.checked == true) {
						ans = 1;
					}
					this.SubmitingArr.push({ q_mapname: e.target.id, q_answer: ans });

				}

			}
		},
		downloadFile(e) {

			var data = this.fileData;
			var sampleArr = this.base64ToArrayBuffer(data);
			this.saveByteArray(this.fileName, sampleArr);
		},
		base64ToArrayBuffer(base64) {
			var binaryString = window.atob(base64);
			var binaryLen = binaryString.length;
			var bytes = new Uint8Array(binaryLen);
			for (var i = 0; i < binaryLen; i++) {
				var ascii = binaryString.charCodeAt(i);
				bytes[i] = ascii;
			}
			return bytes;
		},
		saveByteArray(reportName, byte) {
			var type = "";
			if (this.file_extension == "pdf") {
				type = "application/pdf";
			}
			if (this.file_extension == "docx") {
				type ="application/msword"
			}
			if (this.file_extension == "doc") {
				type = "application/msword"
			}
			if (this.file_extension == "xls") {
				type = "application/vnd.ms-excel"
			}
			if (this.file_extension == "xlsx") {
				type = "application/vnd.ms-excel"
			}

			if (this.file_extension == "ppt") {
				type = "application/vnd.ms-powerpoint"
			}

			if (this.file_extension == "JPEG") {
				type = "image/jpeg"
			}
			if (this.file_extension == "jpeg") {
				type = "image/jpeg"
			}
			if (this.file_extension == "jpg") {
				type = "image/jpeg"
			}
			if (this.file_extension == "png") {
				type = "image/png"
			}

			var blob = new Blob([byte], { type: type });
			var link = document.createElement('a');
			link.href = window.URL.createObjectURL(blob);
			var fileName = reportName;
			link.download = fileName;
			link.click();
		},
		dateChange: function (e) {
			debugger;
			if (e.target.value != "") {
				if (this.SubmitingAdmin.length > 0) {
					//var checkExist = this.SubmitingArr.find(x => x.q_mapname == e.target.id);
					var valObj = this.SubmitingAdmin.filter(function (elem) {
						if (elem.q_mapname == e.target.id) return elem.q_mapname;
					});
					if (valObj.length > 0) {
						this.SubmitingAdmin.splice(valObj, 1);
						this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: e.target.value });

					}

					else {
						this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: e.target.value });

					}
				}
				else {
					this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: e.target.value });

				}
			}
		},
		async previewFiles(e) {
			debugger;
			if (e != undefined) {
				const fileBase64 = await this.getBase64(e.target.files[0])
				var file = e.target.files[0];
				var fileExtension = file.name.split('.').pop();
				var exe = this.extensionType.find(x=>x.name === fileExtension);
	
				if (exe == undefined) {
					swal.fire({
						"title": "",
						"text": "Invalide File ",
						"type": "error",
						"confirmButtonClass": "btn btn-secondary"
					});
					return;
					
                }

				if (this.SubmitingArr.length > 0) {
					//var checkExist = this.SubmitingArr.find(x => x.q_mapname == e.target.id);
					var valObj = this.SubmitingArr.filter(function (elem) {
						if (elem.q_mapname == e.target.id) return elem.q_mapname;
					});
					if (valObj.length > 0) {
						this.SubmitingArr.splice(valObj, 1);
						this.SubmitingArr.push({ q_mapname: e.target.id, q_answer: "", q_file: fileBase64, q_file_extension: fileExtension });
						this.isFile = true;
					}

					else {
						this.SubmitingArr.push({ q_mapname: e.target.id, q_answer: "", q_file: fileBase64, q_file_extension: fileExtension });
						this.isFile = true;

					}
				}
				else {
					this.SubmitingArr.push({ q_mapname: e.target.id, q_answer: "", q_file: fileBase64, q_file_extension: fileExtension });
					this.isFile = true;

				}

			}
		},
		async previewFilesAdmin(e) {
			debugger;
			if (e != undefined) {
				const fileBase64 = await this.getBase64(e.target.files[0])
				var file = e.target.files[0];
				var fileExtension = file.name.split('.').pop();
				var exe = this.extensionType.find(x => x.name === fileExtension);

				if (exe == undefined) {
					swal.fire({
						"title": "",
						"text": "Invalide File ",
						"type": "error",
						"confirmButtonClass": "btn btn-secondary"
					});
					return;

				}


				if (this.SubmitingAdmin.length > 0) {
					//var checkExist = this.SubmitingArr.find(x => x.q_mapname == e.target.id);
					var valObj = this.SubmitingAdmin.filter(function (elem) {
						if (elem.q_mapname == e.target.id) return elem.q_mapname;
					});
					if (valObj.length > 0) {
						this.SubmitingAdmin.splice(valObj, 1);
						this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: "", q_file: fileBase64, q_file_extension: fileExtension });

					}

					else {
						this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: "", q_file: fileBase64, q_file_extension: fileExtension });

					}
				}
				else {
					this.SubmitingAdmin.push({ q_mapname: e.target.id, q_answer: "", q_file: fileBase64, q_file_extension: fileExtension });

				}

			}
		},

		getBase64(file) {
			// Returns a promise which gets resolved or rejected based on the reader events
			return new Promise((resolve, reject) => {
				const reader = new FileReader();
				// Sets up even listeners BEFORE you call reader.readAsDataURL
				reader.onload = function () {
					const result = reader.result
					return resolve(result);
				};

				reader.onerror = function (error) {
					return reject(error);
				};
				// Calls reader function
				reader.readAsDataURL(file);
			})
		},

		getExtention() {
			var root = this;
			$.ajax({
				url: "/api/ApplicationApi/GetExtension",
				type: "GET",
				contentType: "application/json",
				dataType: "json"
			}).done(function (response) {
				/*this.extensionType = response.config_value;*/
				var myResult = response.config_value.split(",");
				myResult.forEach((value, index) => {
					root.extensionType.push({ name: value });
				});
				console.log(root.extensionType);


			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});
		},
		checkedLearnerMulti(e) {
			if (e != undefined) {
				
				if (this.multiCheckArr.length > 0) {
					var ans = 0;
					if (e.target.checked == true) {
						ans = 1;
					}
					var valObj = this.multiCheckArr.filter(function (elem) {
						if (elem.q_id == e.target.id && elem.name == e.target.name) return elem;
					});
					if (valObj.length > 0) {
						this.multiCheckArr.splice(valObj, 1);
						//this.multiCheckArr.push({ answer: ans, name: e.target.name, q_id: e.target.id })
					}

					else {
						this.multiCheckArr.push({ answer: ans, name: e.target.name, q_id: e.target.id })

					}

				} else {
					var ans = 0;
					if (e.target.checked == true) {
						ans = 1;
					}
					this.multiCheckArr.push({ answer: ans, name: e.target.name, q_id: e.target.id })
				}
			}
		},
		getIsEnrolled(id) {
			$.ajax({
				url: "/api/ApplicationApi/GetEnrolledQurestions",
				type: "GET",
				contentType: "application/json",
				data: { id: id },
				dataType: "json"
			}).done(function (response) {
				//if (response.length == 0) {
				//	location.href = "/Application/Index";
				//}
				app.Enrolled = response;
			}).fail(function (xhr) {
				toastr.error(xhr.responseText);
			});
		},
		loadConfirmQuestion: function (id) {
			$.ajax({
				url: "/api/ApplicationApi/GetConfirmQuestions",
				data: { id: id },
				type: "GET",
				contentType: "application/json",
				dataType: "json"
			}).done(function (response) {
				console.log(response);
				app.ConfirmQuestion = response;
			 
				app.ConfirmQuestion.forEach((value, index) => {
					if (value.q_type == 4) {
						//root.q_privacy_notice = response.q_mapname;
						//root.q_privacy_notice_answer = response.answer;
						if (value.answer == 1) {
							app.yescheck = true;
							app.nocheck = false;
						}
						if (value.answer == 0) {
							app.yescheck = false;
							app.nocheck = true;
						}
					}
					//arr.push(value);
					//console.log(value);
					//console.log(index);
				});

			});
		},
		txtPrintName: function (e) {
			debugger;
			var value = e.target.value;
			this.printName = e.target.value;
			console.log(this.printName);
			var stt = value;
			var lng = stt.length;
			var fnt = 30;
			//max 20!
			if (lng < 20) { fnt = 30; }
			else if (lng < 30) { fnt = 20; }
			else { fnt = 15; }

			var canvas = document.getElementById('signaturepad');
			var context = canvas.getContext('2d');
			context.clearRect(0, 0, canvas.width, canvas.height);
			var fntsize = fnt + 'pt';
			context.font = 'italic ' + fntsize + ' Sacramento, cursive ';
			context.fillText(stt, 50, 60);
		},
		loadSignature: function (id) {
			

			$.ajax({
				url: "/api/ApplicationApi/LoadSignatureData",
				type: "GET",
				contentType: "application/json",
				data: { id: id },
				dataType: "json",
			}).done(function (response) {
				debugger;
				if (response != null) {
					app.printName = response.app_sig_text;
					console.log(app.printName);
					var stt = response.app_sig_text;
					var lng = stt.length;
					var fnt = 30;
					//max 20!
					if (lng < 20) { fnt = 30; }
					else if (lng < 30) { fnt = 20; }
					else { fnt = 15; }

					var canvas = document.getElementById('signaturepad');
					var context = canvas.getContext('2d');
					context.clearRect(0, 0, canvas.width, canvas.height);
					var fntsize = fnt + 'pt';
					context.font = 'italic ' + fntsize + ' Sacramento, cursive ';
					context.fillText(stt, 50, 60);
				}
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});

			
		},
		downloadApplication: function () {

			//$.ajax({
			//	type: "POST",
			//	url: "ManageApplications.aspx/GetApplicationForDownload",
			//	data: '{id: "' + sId + '" }',
			//	contentType: "application/json; charset=utf-8",
			//	dataType: "json",
			//	success: function (data) {
			//		var opt = {
			//			margin: [3, 0, 3, 0],

			//			filename: 'Application.pdf',
			//			html2canvas: {
			//				scale: 2

			//			},
			//			pagebreak: { avoid: 'label' },
			//			/*image: { type: 'jpeg', quality: 0.98 },*/

			//			jsPDF: { format: 'a4', orientation: 'portrait' },

			//		};



			//		html2pdf().set(opt).from(data.d).save();



			//	},
			//	failure: function (response) {
			//		alert(response.d);
			//	}
			//});
        }
	}
});

"use strict";

// Class definition
var KTWizard1 = function () {
	// Base elements
	var wizardEl;
	var formEl;
	var validator;
	var wizard;

	// Private functions
	var initWizard = function () {
		// Initialize form wizard
		wizard = new KTWizard('kt_wizard_v1', {
			startStep: 1, // initial active step number
			clickableSteps: true  // allow step clicking
		});

		// Validation before going to next page
		wizard.on('beforeNext', function (wizardObj) {
			if (validator.form() !== true) {
				wizardObj.stop();  // don't go to the next step
			}
		});

		wizard.on('beforePrev', function (wizardObj) {
			/*if (validator.form() !== true) {*/
			//	wizardObj.stop();  // don't go to the next step
			//}
		});

		// Change event
		wizard.on('change', function (wizard) {
			setTimeout(function () {
				KTUtil.scrollTop();
			}, 500);
		});
	}

	var initValidation = function () {
		validator = formEl.validate({
			// Validate only visible fields
			ignore: ":hidden",

			// Validation rules
			rules: myRules,

			// Display error
			invalidHandler: function (event, validator) {
				KTUtil.scrollTop();

				swal.fire({
					"title": "",
					"text": "There are some errors in your submission. Please correct them.",
					"type": "error",
					"confirmButtonClass": "btn btn-secondary"
				});
			},

			// Submit valid form
			submitHandler: function (form) {

			}
		});
	}

	var initSubmit = function () {
		var btn = formEl.find('[data-ktwizard-type="action-submit"]');

		btn.on('click', function (e) {
			e.preventDefault();

			if (validator.form()) {
				// See: src\js\framework\base\app.js
				KTApp.progress(btn);
				//KTApp.block(formEl);

				// See: http://malsup.com/jquery/form/#ajaxSubmit
				//formEl.ajaxSubmit({
				//	success: function () {
				//		KTApp.unprogress(btn);
				//		//KTApp.unblock(formEl);

				//		swal.fire({
				//			"title": "",
				//			"text": "The application has been successfully submitted!",
				//			"type": "success",
				//			"confirmButtonClass": "btn btn-secondary"
				//		});
				//	}
				//});

				app.AddEditApplication(true);
				//	app.SubmitApplication(app.Application.app_id);
			}
		});
	}

	return {
		// public functions
		init: function () {
			wizardEl = KTUtil.get('kt_wizard_v1');
			formEl = $('#kt_form');

			initWizard();
			initValidation();
			initSubmit();
		}

	};
}();
var myRules = {};
var myRules1 = {
	//= Step 1
	app_islivedeulast3years: {
		required: true
	},
	postcode: {
		required: true
	},
	city: {
		required: true
	},
	state: {
		required: true
	},
	country: {
		required: true
	},

	//= Step 2
	package: {
		required: true
	},
	weight: {
		required: true
	},
	width: {
		required: true
	},
	height: {
		required: true
	},
	length: {
		required: true
	},

	//= Step 3
	delivery: {
		required: true
	},
	packaging: {
		required: true
	},
	preferreddelivery: {
		required: true
	},

	//= Step 4
	locaddress1: {
		required: true
	},
	locpostcode: {
		required: true
	},
	loccity: {
		required: true
	},
	locstate: {
		required: true
	},
	loccountry: {
		required: true
	},
};

var addToObject = function (obj, key, value, index) {

	// Create a temp object and index variable
	var temp = {};
	var i = 0;

	// Loop through the original object
	for (var prop in obj) {
		if (obj.hasOwnProperty(prop)) {
			// If the indexes match, add the new item
			if (i === index && key && value) {
				temp[key] = value;
			}
			// Add the current item in the loop to the temp obj
			temp[prop] = obj[prop];
			// Increase the count
			i++;
		}
	}
	// If no index, add to the end
	if (!index && key && value) {
		temp[key] = value;
	}
	return temp;
};

//// Original object
//var lunch = {
//	sandwich: 'turkey',
//	drink: 'soda',
//	chips: true
//};

//// Add to the end of the object
//var lunchWithDessert = addToObject(lunch, 'dessert', 'cookie');

//// Add between sandwich and drink
//var lunchWithTopping = addToObject(lunch, 'topping', 'tomato', 1);

//// Immutable copy of lunch
//var lunchClone = addToObject(lunch);

//console.log(lunch);
//console.log(lunchWithDessert);
//console.log(lunchWithTopping);
//console.log(lunchClone);
