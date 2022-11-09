var subscriberListComponent = new Vue({
    el: '#subscriber-list-component',
    data: {
        subscribers: [],
        isViewReady: false
    },
    methods: {
        refreshData: function () {
            //this.isViewReady = false;

            ////dummy data for now, will update this later
            //var subscribers = [
            //    { name: 'jic', email: 'paytercode@waykurat.com' },
            //    { name: 'jic', email: 'paytercode@waykurat.com' },
            //    { name: 'kin', email: 'monsterart@waykurat.com' }
            //];
            //this.subscribers = subscribers;
            //this.isViewReady = true;


            var self = this;
            this.isViewReady = false;

            ////UPDATED TO GET DATA FROM WEB API
            //axios.get('/api/ACTApi/GetAll/')
            //    .then(function (response) {
            //        self.subscribers = response.data;
            //        self.isViewReady = true;
            //    })
            //    .catch(function (error) {
            //        alert("ERROR: " + (error.message | error));
            //    });
            $.ajax({
                url: "/api/ACTApi/GetAll",
                //data: { userId: userId },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                self.subscribers = response;
                self.isViewReady = true;
            }).fail(function (xhr) {
                alert("ERROR: " + (xhr.responseText));
            });
        }
    },
    created: function () {
        this.refreshData();
    }
});