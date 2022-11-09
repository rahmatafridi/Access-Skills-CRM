Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_Massage',
    data: {
        messageslist: [],
      

    },
    methods: {
        viewData: function (id) {
            window.location.href = "/Portal/Messages/ViewMessage?id=" + id
        },
        viewDataAdmin: function (id) {
            window.location.href = "/Portal/ViewMessage?id=" + id
        },
        getMasseges: function () {
            $.ajax({
                url: "/api/PortalMassagesApi/GetMessages",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response != null) {
                    app.messageslist = response;
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        formatDateEN(date) {
            const options = {  month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' }
            return new Date(date).toLocaleDateString('en-GB', options)
        },
        getNumberOfDays(date) {
            const date1 = new Date(date);
            const date2 = new Date();
            const oneDay = 1000 * 60 * 60 * 24;
            const diffInTime = date2.getTime() - date1.getTime();
            const diffInDays = Math.round(diffInTime / oneDay);
            return diffInDays;

        }
        
        
    }
})
app.getMasseges();
//function PerformView(id) {
    
//    app.viewData(id);
//}