var router = new VueRouter({
    mode: 'history',
    routes: []
});
var app = new Vue({
    router,
    el: '#dv_Create_Massage',
    data: {
        massage: {
            message: '',
            subject: '',
            topic_id: 0,
            topic: '',
            id:0
        },
        Topices: [],
        messageDetails: [],
        firstMassage: {
            message: '',
            user_name: '',
            created_date: '',
            subject: '',
            topic: '',
        }
    },
    methods: {
        getTopic: function (id) {
            $.ajax({
                url: "/api/PortalMassagesApi/GetTopic",
                type: "GET",
                data: {learnerId:id},
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response !== null) {
                    app.Topices = response;
                } 
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        submitted: function () {
            
            var topics = app.Topices.filter(function (a) {
                if (a.SSJLP_Id === app.massage.topic_id) return a.SSJLP_Id;
            });
            var topic = topics[0].SSJLP_Topic;

            var massage = $("#message").val();
            app.massage.message = massage;
            app.massage.topic = topic;
            var data = app.massage;
            var role = $("#role").val();
            $.ajax({
                url: "/api/PortalMassagesApi/InsertMassage",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response === true) {
                    if (role !== "100") {
                        window.location.href = "/Portal/Messages/Manage";
                    } else {
                        window.location.href = "/Portal/ManageMessages";
                    }
                }
            })
        },
      

        getMessageDetail: function (id) {
            $.ajax({
                url: "/api/PortalMassagesApi/GetMessageById?id=" + id,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response !== null) {
                    console.log(response);

                    app.massage = response;
                    app.firstMassage = response;
                    app.getTopic(response.learner_id);
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
            $.ajax({
                url: "/api/PortalMassagesApi/GetMessageDetail?id="+ id,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                if (response !== null) {
                    app.messageDetails = response;
                    console.log(app.messageDetails);
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        formatDateEN(date) {
            const options = { year: 'numeric', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' }
            return new Date(date).toLocaleDateString('en-GB', options)
        },
        messageTitle: function (char) {
            var str = char.substring(2);

            return char.slice(0, 100);
        },
        GotoList: function () {
            window.location.href = "/Portal/Messages/Manage";

        },
         GotoList1: function () {
             window.location.href = "/Portal/ManageMessages";

        }

    }

})
//app.getTopic();

