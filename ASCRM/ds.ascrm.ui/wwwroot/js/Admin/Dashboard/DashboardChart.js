"use strict";
var _statusCollection = [];
var _statusCountCollection = [];
var _backgroundColorCollection = [];



var _leadTotalCollection = [];
var _leadDatesCollection = [];

var profitShare = function () {
    if (!KTUtil.getByID('kt_chart_profit_share_status')) {
        return;
    }

    var randomScalingFactor = function () {
        return Math.round(Math.random() * 100);
    };

   

    var config = {
        type: 'doughnut',
        data: {
            datasets: [{
                data: _statusCountCollection,
                backgroundColor: _backgroundColorCollection
                /*backgroundColor: [
                    KTApp.getStateColor('brand'),
                    KTApp.getStateColor('warning'),
                    KTApp.getStateColor('success'),
                    KTApp.getStateColor('info'),
                    KTApp.getStateColor('notify'),
                    KTApp.getStateColor('danger')
                ]*/
            }],
            labels: _statusCollection
        },
        options: {
            cutoutPercentage: 75,
            responsive: true,
            maintainAspectRatio: false,
            legend: {
                display: false,
                position: 'top'
            },
            title: {
                display: false,
                text: 'Technology'
            },
            animation: {
                animateScale: true,
                animateRotate: true
            },
            tooltips: {
                enabled: true,
                intersect: false,
                mode: 'nearest',
                bodySpacing: 5,
                yPadding: 10,
                xPadding: 10,
                caretPadding: 0,
                displayColors: true,
                backgroundColor: KTApp.getStateColor('info'),
                titleFontColor: '#ffffff',
                cornerRadius: 3,
                footerSpacing: 0,
                titleSpacing: 0
            }
        }
    };

    var ctx = KTUtil.getByID('kt_chart_profit_share_status').getContext('2d');
    var myDoughnut = new Chart(ctx, config);
};

var orderStatistics = function () {
    var container = KTUtil.getByID('kt_chart_order_statistics1');

    if (!container) {
        return;
    }

    var MONTHS = _leadDatesCollection;//['1 Jan', '2 Jan', '3 Jan', '4 Jan', '5 Jan', '6 Jan', '7 Jan'];

    var color = Chart.helpers.color;
    var barChartData = {
        labels: _leadDatesCollection,//['1 Jan', '2 Jan', '3 Jan', '4 Jan', '5 Jan', '6 Jan', '7 Jan'],
        datasets: [
            {
                fill: true,
                //borderWidth: 0,
                backgroundColor: color(KTApp.getStateColor('brand')).alpha(0.6).rgbString(),
                borderColor: color(KTApp.getStateColor('brand')).alpha(0).rgbString(),

                pointHoverRadius: 4,
                pointHoverBorderWidth: 12,
                pointBackgroundColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                pointBorderColor: Chart.helpers.color('#000000').alpha(0).rgbString(),
                pointHoverBackgroundColor: KTApp.getStateColor('brand'),
                pointHoverBorderColor: Chart.helpers.color('#000000').alpha(0.1).rgbString(),

                data: _leadTotalCollection//[20, 30, 20, 40, 30, 60, 30]
            }
        ]
    };

    var ctx = container.getContext('2d');
    var chart = new Chart(ctx, {
        type: 'line',
        data: barChartData,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            legend: false,
            scales: {
                xAxes: [{
                    categoryPercentage: 0.35,
                    barPercentage: 0.70,
                    display: true,
                    scaleLabel: {
                        display: false,
                        labelString: 'Month'
                    },
                    gridLines: false,
                    ticks: {
                        display: true,
                        beginAtZero: true,
                        fontColor: KTApp.getBaseColor('shape', 3),
                        fontSize: 13,
                        padding: 10
                    }
                }],
                yAxes: [{
                    categoryPercentage: 0.35,
                    barPercentage: 0.70,
                    display: true,
                    scaleLabel: {
                        display: false,
                        labelString: 'Value'
                    },
                    gridLines: {
                        color: KTApp.getBaseColor('shape', 2),
                        drawBorder: false,
                        offsetGridLines: false,
                        drawTicks: false,
                        borderDash: [3, 4],
                        zeroLineWidth: 1,
                        zeroLineColor: KTApp.getBaseColor('shape', 2),
                        zeroLineBorderDash: [3, 4]
                    },
                    ticks: {
                        max: _totalLeads,
                        stepSize: 1,
                        display: true,
                        beginAtZero: true,
                        fontColor: KTApp.getBaseColor('shape', 3),
                        fontSize: 13,
                        padding: 10
                    }
                }]
            },
            title: {
                display: false
            },
            hover: {
                mode: 'index'
            },
            tooltips: {
                enabled: true,
                intersect: false,
                mode: 'nearest',
                bodySpacing: 1,
                yPadding: 10,
                xPadding: 10,
                caretPadding: 0,
                displayColors: false,
                backgroundColor: KTApp.getStateColor('brand'),
                titleFontColor: '#ffffff',
                cornerRadius: 4,
                footerSpacing: 0,
                titleSpacing: 0
            },
            layout: {
                padding: {
                    left: 0,
                    right: 0,
                    top: 5,
                    bottom: 5
                }
            }
        }
    });
};


jQuery(document).ready(function () {
    GetLeadChart();
    GetLastSevenLeadChart();
});

function GetLeadChart() {
    $.ajax({
        url: "/api/DashboardApi/GetAllLeadsForDashboard",
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {

        _statusCollection = [];
        _statusCountCollection = [];
        _backgroundColorCollection = [];
        var lbls = '';
        var lbl_empty = '';
        var lbl = '<div class="kt-widget14__legend"><span id="lbl{0}" class="kt-badge {1} kt-badge--inline" style="font-weight: 600; background-color:{4}">{2} {3}</span></div>';
        var _t_leads =0;
        for (var i = 0; i < response.length ; i++) {
            _statusCollection.push(response[i].lead_status_title);
            _statusCountCollection.push(response[i].lead_total);
            _backgroundColorCollection.push(response[i].lead_color);
            lbl_empty = lbl.replace("{0}", i + 1).replace("{1}", response[i].lead_status_class).replace("{2}", response[i].lead_total).replace("{3}", response[i].lead_status_title).replace("{4}", response[i].lead_color);
            lbls += lbl_empty;
            _t_leads += response[i].lead_total;
        }
        $("#cntrls").append(lbls);
        $("#lblLeadTotal").text(_t_leads);
    
        profitShare();

    }).fail(function (xhr) {
        toastr.error(xhr.responseText, 'Error!');
    });
}

var _totalLeads = 0;
function GetLastSevenLeadChart() {
    $.ajax({
        url: "/api/DashboardApi/GetLastSevenDaysLead",
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {

        _leadTotalCollection = [];
        _leadDatesCollection = [];
        _totalLeads = 0;
        var leadstotalCollection = [];
        var _total = '';
        for (var count = 0; count < response.length; count++) {
            _total = "'" + response[count].TotalCount + "'";
            _leadTotalCollection.push(response[count].TotalCount);
            _leadDatesCollection.push(response[count].Lead_DateCreated);
            _totalLeads += response[count].TotalCount;
            leadstotalCollection.push(response[count].TotalCount);
        }
         
        var max = leadstotalCollection.reduce(function (a, b) {
            return Math.max(a, b);
        });

        _totalLeads = max + 5;//TODO _totalLeads + 20;
        console.log(_totalLeads);
        orderStatistics();

    }).fail(function (xhr) {
        toastr.error(xhr.responseText, 'Error!');
    });
}

///-----------------------------------------------------------------------------------------------------------

Vue.use(vuelidate.default)
var _html = '';
var app = new Vue({

    el: '#dv_Dashboard_SalesStatus',
    data: {
        //salesAdvisor: [],
        //salesAdvisorStatus: []
    },
    methods: {
        GetSalesAdvisorStatus: function () {
            $.ajax({
                url: "/api/DashboardApi/GetLeadSalesAdvisor",
                type: "GET",
                contentType: "application/json",
                dataType: "json"
            }).done(function (response) {

                var salesAdvisorStatus = response.salesAdvisors;

                for (var count = 0; count < salesAdvisorStatus.length; count++) {
                    _html += '<div style="margin-top:35px;">';
                    _html += '<div style="display:inline-block;float:left;width:48%">';
                    _html += '<div style="float:left;margin-top: -20px;">';
                    _html += salesAdvisorStatus[count].Users_Username;
                    _html += '   </div>';
                    _html += '   <div class="progress">';

                    _html += '       <div class="progress-bar" title="New" role="progressbar" style="width:' + salesAdvisorStatus[count].NewCalc + '%;background-color:#22b9ff;height: 15px">';
                    _html += salesAdvisorStatus[count].New;
                    _html += '       </div>';
                    _html += '       <div class="progress-bar" title="Deleted" role="progressbar" style="width:' + salesAdvisorStatus[count].DeletedCalc + '%;background-color:rgb(253, 57, 149);height: 15px">';
                    _html += salesAdvisorStatus[count].Deleted;
                    _html += '       </div>';
                    _html += '       <div class="progress-bar" title="Completed" role="progressbar" style="width:' + salesAdvisorStatus[count].CompletedCalc + '%;background-color:green;height: 15px">';
                    _html += salesAdvisorStatus[count].Completed;
                    _html += '      </div>';
                    _html += '       <div class="progress-bar" title="In Progress" role="progressbar" style="width:' + salesAdvisorStatus[count].InProgressCalc + '%;background-color:#ffb822;height: 15px">';
                    _html += salesAdvisorStatus[count].InProgress;
                    _html += '       </div>';

                    _html += '   </div>';
                    _html += '</div>';

                    count++;
                    if (count !== salesAdvisorStatus.length) {
                        _html += '   <div style="display:inline-block;float:right;width:48%">';
                        _html += '       <div style="left:right;margin-top: -20px;">';
                        _html += salesAdvisorStatus[count].Users_Username;
                        _html += '       </div>';
                        _html += '       <div class="progress">';

                        _html += '       <div class="progress-bar" title="New" role="progressbar" style="width:' + salesAdvisorStatus[count].NewCalc + '%;background-color:#22b9ff;height: 15px">';
                        _html += salesAdvisorStatus[count].New;
                        _html += '       </div>';
                        _html += '       <div class="progress-bar" title="Deleted" role="progressbar" style="width:' + salesAdvisorStatus[count].DeletedCalc + '%;background-color:rgb(253, 57, 149);height: 15px">';
                        _html += salesAdvisorStatus[count].Deleted;
                        _html += '       </div>';
                        _html += '       <div class="progress-bar" title="Completed" role="progressbar" style="width:' + salesAdvisorStatus[count].CompletedCalc + '%;background-color:green;height: 15px">';
                        _html += salesAdvisorStatus[count].Completed;
                        _html += '      </div>';
                        _html += '       <div class="progress-bar" title="In Progress" role="progressbar" style="width:' + salesAdvisorStatus[count].InProgressCalc + '%;background-color:#ffb822;height: 15px">';
                        _html += salesAdvisorStatus[count].InProgress;
                        _html += '       </div>';

                        _html += '      </div>';
                        _html += '  </div>';
                        _html += '</div >';
                    }
                }

                $("#dv_Dashboard_SalesStatus").append(_html);

            }).fail(function (xhr) {
                toastr.error(xhr.responseText, 'Error!');
            });
        }
    }
});


app.GetSalesAdvisorStatus();

