

var app = new Vue({
    el: '#dv_company_search',
    data: {
        SalesAdvisors: [],
        name: '',
        id: '',
        company: [],
        companyLoad: {
            name: '',
            let: '',
            sa:'',
            list: []
        },
        companyLoad2: {
            list: [],
            name: '',
            let: '',
            sa: ''
        },
        companyNumber:[],
        oldValue: '',
        count: 0,
        loading: false,
        LoadCompanyOneList: [],
        LoadCompanyTwoList:[]
    },
    methods: {
        Populate: function () {
            $.ajax({
                url: "/api/LeadApi/GetAllDropdowns",
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                //  app.Countries = response.Countries;

                app.SalesAdvisors = response.NewSalesAdvisors;

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });

        },
        searchOn: function () {
            var root = this;
            var val = event.target.value
            var searchtype = '';
            if (val == '') {
                searchtype = "All";
            }
            if (searchtype == "All") {
                root.loading = false;

                $.ajax({
                    url: "/api/CompanyApi/CompanySearchAtZ",
                    data: { name: '', searchtype: 'All' },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json",
                    async: false
                }).done(function (response) {

                    root.companyLoad.list = [];
                    root.companyLoad2.list = [];
                    root.companyNumber = [];

                    //to split number 
                    function isNumber(n) { return /^-?[\d.]+(?:e-?\d+)?$/.test(n); }

                    var array = [];
                    var arrNum = [];
                    var arrStr = [];
                    for (var i = 0; i < response.Data.length; i++) {
                        if (isNumber(response.Data[i].let)) {
                            arrNum.push({
                                let: response.Data[i].let
                            });

                        } else {
                            if (response.Data[i].let != " ") {
                                arrStr.push({
                                    let: response.Data[i].let
                                });
                            }
                        }




                    }
                    root.companyNumber = arrNum;
                    root.count = arrStr.length / 2;
                    for (var i = 0; i < arrStr.length; i++) {
                        if (i < root.count) {
                            root.companyLoad.list.push({

                                let: arrStr[i].let,

                            })
                        }
                        else {
                            root.companyLoad2.list.push({
                                let: arrStr[i].let,


                            })
                        }
                    }


                });
            }
            else {
                root.loading = true;

                $.ajax({
                    url: "/api/CompanyApi/CompanySearchAtZ",
                    data: { name: val, searchtype: searchtype },
                    type: "GET",
                    contentType: "application/json",
                    dataType: "json",
                    async: false
                }).done(function (response) {

                    root.companyLoad.list = [];
                    root.companyLoad2.list = [];
                    root.companyNumber = [];

                    var count1 = 0;
                    // root.companyLoad.list = response.Data;
                    for (var i = 0; i < response.Data.length; i++) {
                        count1++;
                        if (count1 < 49) {
                            root.companyLoad.list.push({
                                name: response.Data[i].name,
                                let: response.Data[i].let,
                                sa: response.Data[i].sa,
                                encodedId: response.Data[i].encodedId
                            })
                        }
                        else {
                            root.companyLoad2.list.push({
                                name: response.Data[i].name,
                                let: response.Data[i].let,
                                sa: response.Data[i].sa,
                                encodedId: response.Data[i].encodedId

                            })
                        }


                    }
                    console.log(root.companyLoad.list);

                });
            }
          
        },
        onLoad: function () {
            var root = this;
            $.ajax({
                url: "/api/CompanyApi/CompanySearchAtZ",
                data: { name: '',searchtype:'All' },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
                async: false
            }).done(function (response) {

                root.companyLoad.list = [];
                root.companyLoad2.list = [];
                root.companyNumber = [];
                //to split number 
                function isNumber(n) { return /^-?[\d.]+(?:e-?\d+)?$/.test(n); }

                var array = [];
                var arrNum = [];
                var arrStr = [];
                for (var i = 0; i < response.Data.length; i++) {
                    if (isNumber(response.Data[i].let)) {
                        arrNum.push({
                            let: response.Data[i].let
                        });

                    } else {
                        if (response.Data[i].let != " ") {
                            arrStr.push({
                                let: response.Data[i].let
                            });
                        }
                    }
                }
                root.companyNumber = arrNum;
                root.count = arrStr.length / 2;
                for (var i = 0; i < arrStr.length; i++) {
                    if (i < root.count) {
                        root.companyLoad.list.push({

                            let: arrStr[i].let,

                        })
                    }
                    else {
                        root.companyLoad2.list.push({
                            let: arrStr[i].let,


                        })
                    }
                }
                
      

            });
        },
        GotoCompany: function (company_id) {
            console.log(company_id);
            window.location.href = 'Company/Edit?id=' + company_id;
        },
        getCompany: function (text) {
            var root = this;
            
            $.ajax({
                url: "/api/CompanyApi/LoadCompanyByName",
                data: { name: text },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
            }).done(function (response) {
                //  app.Countries = response.Countries;
                console.log(response);
                root.LoadCompanyOneList = [];
                root.LoadCompanyTwoList = [];

                var total = response.Data.length / 2;
                for (var i = 0; i < response.Data.length; i++) {
                    if (i <= total) {
                        root.LoadCompanyOneList.push({
                            name: response.Data[i].name,
                            sa: response.Data[i].sa,
                            encodedId: response.Data[i].encodedId

                        })
                    } else {
                        root.LoadCompanyTwoList.push({
                            name: response.Data[i].name,
                            sa: response.Data[i].sa,
                            encodedId: response.Data[i].encodedId

                        })
                    }
                }
                console.log(total);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
            $("#kt_modal_company_Load").modal("show");

        }
    },
    mounted: function () {
       // this.Populate();
        this.onLoad();
    }
})
function companySearch() {

   // search();
    $("#kt_modal_company_search").modal("show");

}


function search() {
    debugger; // eslint-disable-line

    $.ajax({
        url: "/api/CompanyApi/CompanySearch",
        data: { name: 'All' },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        async: false
    }).done(function (response) {

        let grid = $("#companyGrid").data("kendoGrid");
        console.log(response);

        for (var i = 0; i < response.Data.length; i++) {

            grid.dataSource.add({
                encodedId: response.Data[i].encodedId, id: response.Data[i].id, name: response.Data[i].name, sales_advisor: response.Data[i].sales_advisor
            });

        }

    });


}
