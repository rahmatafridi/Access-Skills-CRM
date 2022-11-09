Vue.use(vuelidate.default);

var router = new VueRouter({
    mode: 'history',
    routes: []
});

var app = new Vue({
    router,
    el: '#dv_Create_QuestionValidation',
    data: {
        question: {
            v_id: 0,
            q_max: 0,
            q_min: 0,
            q_type_regex: '',
            q_type_name: '',
            q_validation_massage:''

        },
     

    
        isSubmitted: false,
        
       


    },
    validations: {
        question: {
            q_type_name: {
                required: validators.required
            }

        },
      

    },
    methods: {
        Populate: function () {



                app.mounted();

        },

        submitted: function (type) {
            this.isSubmitted = true;
            var root = this;
           if(this.question.q_max == "")
           {
               this.question.q_max = 0;
           }

            if (this.question.q_min == "") {
                this.question.q_min = 0;
            }
            if (this.question.q_type_name == "") {
                toastr.error('Name is Required ', 'Error!');
                return;
            }
            var data = this.question;

            $.ajax({
                url: "/api/QuestionValidationApi/InsertUpdateValidationRecord",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                if (response !== null) {
                    toastr.success("Validation updated successfully.", "Success!");
                    //// if type is 1 then save and go back.
                    //// if type is 2 then save and sign out.
                    //// if type is 3 then save and Add new.
                 
                        location.href = "/QuestionValidation/Manage";
                    
                } else {
                    toastr.error("Cannot update. Please try again.", "Error!");
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },

        mounted: function () {
            var root = this;

          
            parameters = this.$route.query;
            if (_validationId !== undefined) {

                parameters.id = _validationId;
                if (parameters.id > 0) {
                    if (!_canManagerEditQV) {
                        window.location.href = '/Error/PermissionDenied';
                    }

                    //app.parameters.id = parameters.id;
                
                    $.ajax({
                        url: "/api/QuestionValidationApi/GetValidatinById",
                        data: { id: parameters.id },
                        type: "GET",
                        contentType: "application/json",
                        dataType: "json"
                    }).done(function (response) {
                        app.question = response;
                     

                        $("#lblEditHeader").text(response.q_type_name + ' - ' + response.v_id);


                    }).fail(function (xhr) {
                        toastr.error(xhr.responseText, 'Error!');
                    });
                 

                }
            }
        },

        isNumber: function (evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
                evt.preventDefault();;
            } else {
                return true;
            }
        }

    }
});


