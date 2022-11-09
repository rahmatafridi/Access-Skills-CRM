Vue.use(vuelidate.default)
var app = new Vue({
    el: '#dv_Portal_UserManage',
    data: {
        Users: []
    },
    methods: {

        GetCategory: function () {
            $.ajax({
                url: "/api/PortalResourceLibraryApi/GetCategory",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.Category = response;
                console.log(app.Category);
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        downlaodFile: function (path) {
            window.location = '/Document/DownloadFile?filePath=' + path;
        }
    }

})