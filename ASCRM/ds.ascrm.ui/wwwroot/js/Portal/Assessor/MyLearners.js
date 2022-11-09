Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_MyLearner',
    data: {
        Learners: [],
        List:[]
    },
    methods: {
        GetMyLearner: function () {
            $.ajax({
                url: "/api/PortalApi/GetMyLearner",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Learners = response;
                console.log(app.Learners);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        GetLearnerData: function (id) {
            $.ajax({
                url: "/api/PortalCourseSummaryApi/GetCourseTopicsById",
                type: "GET",
                data: {
                    id:id
                },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.List = response;
                console.log(app.List);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
            $("#kt_modal_view").modal("show");

        }
    }
})
//app.GetMyLearner();
function PerformView(id) {
    app.GetLearnerData(id);
}