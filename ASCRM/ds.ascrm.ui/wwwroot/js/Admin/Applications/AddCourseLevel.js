Vue.use(vuelidate.default)
var router = new VueRouter({
	mode: 'history',
	routes: []
});
var app = new Vue({
	router,
	el: '#dv_Create_Course_Level',
	data: {
		isSubmitted: false,
		level: {
			CL_Id: 0,
			CL_Title: '',
			CL_Description: '',
			CL_Code: '',
			CL_Level: '',
			CL_SortOrder: ''

		},
	},
	validations: {
		level: {
			

		}
	},
	methods: {
		Populate: function () {

		},
		mounted: function () {
			debugger;
			parameters = this.$route.query;
			parameters.id = _levelId;
		
			if (parameters.id > 0) {


				$.ajax({
					url: "/api/ApplicationApi/GetCourseLevelById",
					data: { id: parameters.id },
					type: "GET",
					contentType: "application/json",
					dataType: "json"
				}).done(function (response) {
					app.level = response;
					$("#lblEditHeader").text(response.CL_Title + ' - ' + response.CL_Id);


				}).fail(function (xhr) {
					toastr.error(xhr.responseText, 'Error!');
				});
			} else {
				//if (!_canRoleManagerCreateRole) {
				//    window.location.href = '/Error/PermissionDenied';
				//}
			}
		},
		AddEditCourseLevel: function (isSubmit) {
			debugger;

			this.isSubmitted = true;
			this.level.CL_SortOrder = parseInt(this.level.CL_SortOrder);
			var data = this.level;
			var id = app.level.id;

			$.ajax({
				url: "/api/ApplicationApi/InsertUpdateCourseLevel",
				data: JSON.stringify(data),
				type: "Post",
				contentType: "application/json",
				dataType: "json",
			}).done(function (response) {
				if (response === true) {
					location.href = "/Application/CourseLevel";

					app.isSubmitted = false;
					
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
				data: { id: app_id },
				dataType: "json",
			}).done(function (response) {
				location.href = "/Application/Index";
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});
		},
		submitted: function (id) {
			if (this.level.CL_SortOrder == "") {
				this.level.CL_SortOrder = 0;
			}
			if (this.level.CL_Title == "") {
				toastr.error('Title Is Required.', 'Error!');
				return;
			}
			if (this.level.CL_Code == "") {
				toastr.error('Code Is Required', 'Error!');
				return;
            }
			this.isSubmitted = true;
			var data = this.level;
			$.ajax({
				url: "/api/ApplicationApi/InsertUpdateCourseLevel",
				data: JSON.stringify(data),
				type: "Post",
				contentType: "application/json",
				dataType: "json",
			}).done(function (response) {
				if (response === true) {
					location.href = "/Application/CourseLevel";

					app.isSubmitted = false;
					if (app.Application.app_id > 0) {
						//if (isSubmit) {
						//	app.SubmitApplication(app_id);
						//}
						toastr.success("Record updated successfully.", "Updated!");
						location.href = "/Application/CourseLevel";

					} else {
						toastr.success("Record inserted successfully.", "Inserted!");
						location.href = "/Application/CourseLevel";

					}
					if (!isSubmit) {
						location.href = "/Application/CourseLevel";
					}
				} else {
					toastr.error('Record have not been saved. Please try again.', 'Error!');
				}
			}).fail(function (xhr) {
				toastr.error(xhr.responseText, 'Error!');
			});
        }
	}
});
var submitted = function () {
	var btn = formEl.find('[data-ktwizard-type="action-submit"]');

	btn.on('click', function (e) {
		e.preventDefault();

		if (validator.form()) {
		

			app.AddEditCourseLevel(true);
		}
	});
}
