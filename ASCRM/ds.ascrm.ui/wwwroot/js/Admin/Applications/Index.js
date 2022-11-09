Vue.use(vuelidate.default)

var app = new Vue({

	el: '#dv_Manage_Applications',
	data: {
		isSubmitted: false,
		buttonText: 'Add',
		Applications: [],
		isSubmitted:false
	},
	validations: {

	},
	methods: {
		Populate: function () {

			$.ajax({
				url: "/api/ApplicationApi/SubmittedCheck",
				type: "GET",
				contentType: "application/json",
				data: { id: 0 },
				dataType: "json"
			}).done(function (response) {

				app.isSubmitted = response;

			}).fail(function (xhr) {
				toastr.error(xhr.responseText);
			});

			$.ajax({
				url: "/api/ApplicationApi/GetAllApplicationsByUserId",
				type: "GET",
				contentType: "application/json",
				data: { id: 0 },
				dataType: "json"
			}).done(function (response) {

				app.Applications = response;

			}).fail(function (xhr) {
				toastr.error(xhr.responseText);
			});
		},
		AddNewApplication: function () {
			//if (this.Configration.config_id === 0 && !_canRoleManagerCreateConfig) {
			//	toastr.error('You cannot add new Configuration please contact administrator.', 'Permission denied!');
			//	return false;
			//}
			this.isSubmitted = true;

			//var data = this.Application;

			$.ajax({
				url: "/api/ApplicationApi/InsertNewApplication",
				data: {id: 0},
				type: "GET",
				contentType: "application/json",
				dataType: "json",
			}).done(function (response) {
				app.isSubmitted = false;
				if (response.AppUser_App_Id > 0) {
					location.href = '/Application/Progress?id=' + response.encoded_app_id;
				} else {
					toastr.error('Application have not been created. Please try again.', 'Error!');
				}
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});
		}
	}
});

app.Populate();
