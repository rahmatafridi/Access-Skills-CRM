Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_MyTap2',
    data: {
        documents: [],
        learnerComment: '',
        tabs: {
            TAP_Id: 0,
            TAP_FinalFile: '',
            TAP_LearnerSignatureText: '',
            TAP_LearnerComments: '',
            TAP_FinalFileStr:''
        }
    },
    validations: {

    },
    methods: {
        GetMyTab2: function () {


            $.ajax({
                url: "/api/PortalTab2Api/GetPortalTab2",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.documents = response;
                console.log(app.documents);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        DownladPdf: function (id) {
            window.location = '/api/PortalTab2Api/DownlaodPDF?tabId=' + id;
        },
        DownladDoc: function (id) {
            window.location = '/api/PortalTab2Api/DownlaodDoc?tabId=' + id;
        },
        Sign: function (id) {
            app.tabs.TAP_Id = id;
            app.tabs.learnerComment = '';
            $("#printName").val('');
            $("#myModal").modal("show");

        },
        txtPrintName: function (e) {
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
        SaveSign: function () {
            var sing = $("#printName").val();
            
            var canvas = document.getElementById('signaturepad');
            var context = canvas.getContext('2d');
            var dataUri = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");  // here is the most important part because if you dont replace you will get a DOM 18 exception.

            $("#sketch_data").val(dataUri);
            $("#sketch_data").val(dataUri);
            app.tabs.TAP_FinalFileStr = dataUri;
            app.tabs.TAP_LearnerSignatureText = sing;
            app.tabs.TAP_LearnerComments = app.learnerComment;

            $.ajax({
                url: "/api/PortalTab2Api/SaveSign",
                data: JSON.stringify(app.tabs),
                type: "Post",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                if (response === true) {
                   

                   
                    toastr.success("Record updated successfully.", "Updated!");
                    $("#myModal").modal("hide");
                    
                   
                } else {
                    toastr.error('Record have not been saved. Please try again.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        }
    }
})

app.GetMyTab2();