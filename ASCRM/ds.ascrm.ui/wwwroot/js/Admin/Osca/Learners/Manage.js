var app = new Vue({

    el: '#dv_Manage_Learner',
    data: {
        isSubmitted: false,
        Companies: [],
        SalesAdvisors: []
    },
})

function PerformLearnerView(learner_Id) {
    
    window.location.href = 'Learners/Detail?id=' + learner_Id;

   
}