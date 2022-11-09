Vue.use(vuelidate.default)
var app = new Vue({
    el: '#dv_Portal_Contact',
    data: {
        subject: '',
        massage:''

    },
    methods: {
        SubmitContact: function () {
            $.ajax({
                url: "/api/PortalContactApi/SubmitContact",
                type: "Post",
                data: {
                    subject: app.subject,
                    massage: app.massage
                 
                },
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                //app.FullSummary = response;
                //console.log(app.FullSummary);
                app.subject = '';
                app.massage = '';
                toastr.success("Massage Send successfully.", "Success!");

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }
    }
})