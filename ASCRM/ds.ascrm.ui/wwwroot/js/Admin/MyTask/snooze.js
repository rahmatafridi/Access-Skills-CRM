
    function GetTasks() {
        $.ajax({
            url: "/api/TaskApi/GetTasks",
            type: "GET",
            contentType: "application/json",
            dataType: "json"
        }).done(function (response) {

            for (var count = response.length - 1; count >= 0; count--) {

                //toastr.options = {
                //    timeOut: 0,
                //    extendedTimeOut: 0
                //};
                var btn_snooze10 = "<button style='height: 23px;padding: 1px;width:80px;margin: 3px' onclick='task_snooze_click(" + response[count].task_id + ",10)' type='button' class='btn btn-outline-light btn-sm--air--wide clear'><i class='fa fa-bell'></i> 10mn</button>";
                var btn_snooze15 = "<button style='height: 23px;padding: 1px;width:80px;margin: 3px' onclick='task_snooze_click(" + response[count].task_id + ",15)' type='button' class='btn btn-outline-light btn-sm--air--wide clear'><i class='fa fa-bell'></i> 15mn</button>";
                var btn_go = "<button style='height: 23px;padding: 1px;width: 80px;margin: 3px' onclick=task_ok_click('" + response[count].encodeLeadId + "') type = 'button' class= 'btn btn-outline-light btn-sm--air--wide clear'><i class='fa fa-location-arrow'></i> Check</button> ";
                var btn_done = "<button style='height: 23px;padding: 1px;width: 80px;margin: 3px' onclick=task_done_click('" + response[count].task_id + "'," + response[count].lead_id + ") type = 'button' class= 'btn btn-outline-light btn-sm--air--wide clear'><i class='fa fa-check'></i> Done</button> ";

                if (response[count].Lead_ContactName !== null) {                                   
                   
                    toastr.info
                        (
                            response[count].title + " - "
                            + response[count].Lead_ContactName
                            + " - " + response[count].note
                            + " - " + response[count].task_date
                            + "<br /><br />"                           
                            + btn_snooze10  
                            + btn_snooze15  
                            + "<br />"
                            + btn_go 
                            + btn_done
                            , '', { positionClass: 'toast-bottom-right', timeOut: 100000 });
                }
                else {         
                   
                    toastr.info(
                        response[count].title + " - "
                        + response[count].note + " - "
                        + response[count].task_date
                        + "<br /><br />"                                            
                        + btn_snooze10 
                        + btn_snooze15 
                        + "<br />"
                        + btn_go
                        + btn_done
                        , { timeOut: 100000 });
                }
            }

        }).fail(function (xhr) {
            custoastr.error(xhr.responseText, 'Error!');
        });
}

    function task_ok_click(leadId) {
        window.location.href = 'Lead/Edit?id=' + leadId + '&Act=Y';
    }

    function task_done_click(task_id, lead_id) {
        $.ajax({
            url: "/api/TaskApi/TaskDone",
            data: { task_id: task_id, lead_id: lead_id },
            type: "GET",
            contentType: "application/json",
            dataType: "json"
        }).done(function () {
            toastr.clear();
            $("#toast-container").html('');
            GetTasks();
        });
    }
function task_snooze_click(task_id, snooze_delay) {
        $.ajax({
            url: "/api/TaskApi/SnoozeTask",
            data: { task_id: task_id, task_snooze_delay: snooze_delay},
            type: "GET",
            contentType: "application/json",
            dataType: "json"
        }).done(function () {
            toastr.clear();
            $("#toast-container").html('');
            GetTasks();
        });
}

