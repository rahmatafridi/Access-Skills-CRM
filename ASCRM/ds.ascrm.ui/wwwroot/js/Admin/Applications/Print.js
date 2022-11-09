Vue.use(vuelidate.default)
var router = new VueRouter({
	mode: 'history',
	routes: []
});
var app = new Vue({
	router,
	el: '#dv_Manage_ApplicationPrint',
	data: {
		isSubmitted: false,
		buttonText: 'Add',
		ApplicationSection: { ase_id: 0, ase_id_appstep: 0, ase_title: null, ase_sortorder: 0, ase_description: null },
		ApplicationStep: { as_id: 0, as_title: null, as_sortorder: 0, as_description: null, as_icon: null },
		ApplicationSections: [],
		ApplicationSteps: [],
		//Application: { app_islivedeulast3years: 'howw', app_educ_previouscollege: 'are you', app_nationality: '4' }
		Application: {},
		app_signature: '',
		Course_Level:''
	},
	validations: {
	},
	methods: {
	
		mounted: function () {
			parameters = this.$route.query;
			parameters.id = _app_id;
			var root = this;
			$.ajax({
				url: "/api/ApplicationApi/GetCourseLevelByIdOnPrint",
				type: "GET",
				contentType: "application/json",
				data: { id: parameters.id },
				dataType: "json"
			}).done(function (response) {
				console.log(response);
				//this.Course_Level=
				root.Course_Level = response.CL_Title;
			});

			if (parameters.id > 0) {
				$.ajax({
					url: "/api/ApplicationApi/GetApplicationStepsForLearner",
					type: "GET",
					contentType: "application/json",
					data: { id: parameters.id },
					dataType: "json"
				}).done(function (response) {
					app.ApplicationSteps = response;
					app.$nextTick(() => {

						$.each(app.ApplicationSteps, function (index1, appStep) {
							$.each(appStep.applicationsections, function (index2, appSection) {

								$.each(appSection.applicationquestions, function (index3, appQuestion) {


								});
							});
						});
					//	KTWizard1.init();
					})
				}).fail(function (xhr) {
					toastr.error(xhr.responseText);
				});
			} else {
				//if (!_canRoleManagerCreateRole) {
				//    window.location.href = '/Error/PermissionDenied';
				//}
			}
			$.ajax({
				url: "/api/ApplicationApi/GetSignatureByAppId",
				type: "GET",
				contentType: "application/json",
				data: { id: parameters.id },
				dataType: "json"
			}).done(function (response) {
				app.app_signature = response;				
			}).fail(function (xhr) {
				app.app_signature = xhr.responseText;
				//toastr.error(xhr.responseText);
			});
		},
		AddEditApplication: function () {
			//if (this.Configration.config_id === 0 && !_canRoleManagerCreateConfig) {
			//	toastr.error('You cannot add new Configuration please contact administrator.', 'Permission denied!');
			//	return false;
			//}
			this.isSubmitted = true;

			var data = this.Application;

			$.ajax({
				url: "/api/ApplicationApi/InsertUpdateApplication",
				data: JSON.stringify(data),
				type: "Post",
				contentType: "application/json",
				dataType: "json",
			}).done(function (response) {
				if (response === true) {
					app.isSubmitted = false;
					if (app.Application.app_id > 0) {
						toastr.success("Record updated successfully.", "Updated!");
					} else {
						toastr.success("Record inserted successfully.", "Inserted!");
					}
					location.href = "/Application/Index";
				} else {
					toastr.error('Record have not been saved. Please try again.', 'Error!');
				}
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});


		},
	}
});


