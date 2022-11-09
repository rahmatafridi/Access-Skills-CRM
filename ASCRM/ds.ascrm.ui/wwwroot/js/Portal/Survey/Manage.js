Vue.use(vuelidate.default)
var app = new Vue({

    el: '#dv_Portal_Survey',
    data: {
        documents: [],
        IsSurveyList: true,
        IsSurveyDetail: false,
        survayQuestions: [],
        questionOptions: [],
        SubmitSurvey: [],
        SubmitingArr: [],
        survyId: 0,
        SelectedQuestion:[]

    },
    validations: {

    },
    methods: {
        GetMyTab2: function () {


            $.ajax({
                url: "/api/PortalSurveyApi/GetSurveyLinks",
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
    
       
        surveyDetail: function (id) {
            app.survyId = id;
            $.ajax({
                url: "/api/PortalSurveyApi/GetSurveyLinkOne?surveyId=" + id,
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {
                app.survayQuestions = response.surveyQuestions;
                app.questionOptions = response.options;
                app.IsSurveyList = false;
                app.IsSurveyDetail = true;
                // $("#IsSurveyDetail").show();
                console.log(response);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        },
        SaveSurvey: function () {
            if (this.SubmitSurvey.length > this.survayQuestions.length) {
                toastr.error("Please Refresh The Page");
                return;
            }
            if (this.SubmitSurvey.length < this.survayQuestions.length) {
                
            }

            this.SubmitSurvey.push({ SurveyId: app.survyId});

            var data = this.SubmitSurvey;

            $.ajax({
                url: "/api/PortalSurveyApi/SubmitSurveyLearner",
                data: JSON.stringify(data),
                type: "Post",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                if(response === true) {
                    app.isSubmitted = false;
                    
                }
                else {
                    toastr.error('Record have not been saved. Please try again.', 'Error!');
                }
            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });


            //app.IsSurveyDetail = false;
            //app.IsSurveyList = true;
        },
        handleBlur(e) {

            if (e.target.id != "") {
                //var validationId = this.questionOptions.filter(function (a) {
                //    if (a.id == e.target.id) return a.id;
                //});
                //if (validationId.length > 0) {
                if (e.target.type == 'radio' || e.target.type =='dropdown') {
                    var valObj = this.SubmitSurvey.filter(function (elem) {
                        if (elem.QuestionID == e.target.value) return elem.QuestionID;
                    });
                    if (valObj.length > 0) {
                        this.SubmitSurvey.splice(valObj, 1);
                        this.SubmitSurvey.push({ QuestionID: e.target.value, AnswerOptionScore: e.target.id, AnswerType: e.target.type });

                    } else {
                        this.SubmitSurvey.push({ QuestionID: e.target.value, AnswerOptionScore: e.target.id, AnswerType: e.target.type });
                    }
                }
                if (e.target.type == 'text') {
                    var ques = this.survayQuestions.filter(function (elem) {
                        if (elem.QuestionId == parseInt(e.target.id)) return elem.QuestionId;

                    });

                    var valObj = this.SubmitSurvey.filter(function (elem) {
                        if (elem.QuestionID ==e.target.id) return elem.QuestionID;
                    });
                    if (valObj.length > 0) {
                        this.SubmitSurvey.splice(valObj, 1);
                        this.SubmitSurvey.push({ QuestionID: e.target.id, TextQuestion: ques[0].QuestionText, TextQuestionAnswer: e.target.value, AnswerType: e.target.type });

                    } else {
                        this.SubmitSurvey.push({ QuestionID: e.target.id, TextQuestion: ques[0].QuestionText, TextQuestionAnswer: e.target.value, AnswerType: e.target.type });
                    }
                }
                

                //}
            }
        }
    }
})

app.GetMyTab2();
/*$("#IsSurveyDetail").hide();*/