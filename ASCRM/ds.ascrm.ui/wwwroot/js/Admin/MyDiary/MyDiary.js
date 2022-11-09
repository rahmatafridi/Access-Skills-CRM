"use strict";
var KTCalendarBasic =
{
    init: function () {
        var e = moment().startOf("day"),
            t = e.format("YYYY-MM"),
            i = e.clone().subtract(1, "day").format("YYYY-MM-DD"),
            n = e.format("YYYY-MM-DD"),
            r = e.clone().add(1, "day").format("YYYY-MM-DD"),
            o = document.getElementById("kt_calendar");
        new FullCalendar.Calendar(o, {
            plugins: ["interaction", "dayGrid", "timeGrid", "list"], isRTL: KTUtil.isRTL(), header: {
                left: "prev,next today", center: "title", right: "dayGridMonth,timeGridWeek,timeGridDay,listWeek"
            }
            , height: 800, contentHeight: 780, aspectRatio: 3, nowIndicator: !0, now: n + "T09:25:00", views:
            {
                dayGridMonth: {
                    buttonText: "month"
                }
                , timeGridWeek: {
                    buttonText: "week"
                }
                , timeGridDay: {
                    buttonText: "day"
                }, listWeek: {
                    buttonText: "list"
                }
            }
            , defaultView: "dayGridMonth", defaultDate: n, editable: !0, eventLimit: !0, navLinks: !0,
            events: _dataCollection
            , eventRender: function (e) {
                var t = $(e.el);
                e.event.extendedProps &&
                    e.event.extendedProps.description &&
                    (t.hasClass("fc-day-grid-event") ? (t.data("content", ), t.data("placement", "top"), KTApp.initPopover(t)) : t.hasClass("fc-time-grid-event") ? t.find(".fc-title").append('<div class="fc-description">' + e.event.extendedProps.description + "</div>") : 0 !== t.find(".fc-list-item-title").lenght && t.find(".fc-list-item-title").append('<div class="fc-description">' + e.event.extendedProps.description + "</div>").find("#task_id").text(e.event.id));
            }
        }
        ).render();
    }
};
var _dataCollection = [];

jQuery(document).ready(function () {
    GetAllDiaries();
});

function GetAllDiaries() {
    $.ajax({
        url: "/api/DiaryApi/GetAll",
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {

        //Date/ Time/ Name/ Note (reason for the follow up)

        _dataCollection = [];
        var item = '';
        for (var count = 0; count < response.length; count++) {
            var _description = '';
            _description += ' <b>Date:</b> ' + moment(response[count].activity_date).format("DD/MM/YYYY");
            _description += ' <b>| Time:</b> ' + moment(response[count].activity_date).format("HH:mm");
            _description += ' <b>| Name:</b> ' + response[count].Lead_ContactName;
            _description += ' <b>| Note:</b> ' + response[count].activity_Note;

            item =
                {
                    title: response[count].Opt_Title
                    , start: moment(response[count].activity_date)._i
                    , description: _description
                    , url: 'Lead/Edit?id=' + response[count].encodeLeadId + '&Act=Y'
                    , id: response[count].Activity_Id
                    , groupId: response[count].is_done
                };
            _dataCollection.push(item);
        }

        KTCalendarBasic.init();

    }).fail(function (xhr) {
        toastr.error(xhr.responseText, 'Error!');
    });
}

function CreateTask() {
    $("#task_id").val(0);
    $("#btnSave").text("Save");
    $("#btnDelete").hide();
    $("#div_lead").hide();
    clear();
    $("#kt_modal_duplicate").modal("show");
}

function SaveTask() {
    if ($("#txtTitle").val().trim() === "" || $("#txtTitle").val().trim() === '' || $("#txtTitle").val().trim() === null || $("#txtTitle").val().trim() === undefined) {
        toastr.error("Title is required filed.", "Error!");
        $("#txtTitle").addClass("is-invalid");
        return;
    }
    else { $("#txtTitle").removeClass("is-invalid"); }

    if ($("#txtNotes").val().trim() === "" || $("#txtNotes").val().trim() === '' || $("#txtNotes").val().trim() === null || $("#txtNotes").val().trim() === undefined) {
        toastr.error("Notes is required filed.", "Error!");
        $("#txtNotes").addClass("is-invalid");
        return;
    }
    else { $("#txtNotes").removeClass("is-invalid"); }

    if ($("#task_datetime").val().trim() === "" || $("#task_datetime").val().trim() === '' || $("#task_datetime").val().trim() === null || $("#task_datetime").val().trim() === undefined) {
        toastr.error("Date is required filed.", "Error!");
        $("#task_datetime").addClass("is-invalid");
        return;
    }
    else { $("#task_datetime").removeClass("is-invalid"); }

    var data =
    {
        title: $("#txtTitle").val(),
        note: $("#txtNotes").val(),
        date_time: $("#task_datetime").val(),
        is_done: $("#chkIsdone").prop('checked'),
        task_id: $("#task_id").val(),
        task_action_id: ($("#ddlLead").val() === "") ? 0 : $("#ddlLead").val()
    };

    $.ajax({
        url: "/api/TaskApi/InsertTask",
        data: JSON.stringify(data),
        type: "Post",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {
        if (response === true) {
            toastr.success("Task inserted successfully.", "Inserted!");
            $("#kt_modal_duplicate").modal("hide");
            location.href = "/MyDiary";
        } else {
            toastr.error("Cannot submitted, please try again.", "Error!");
        }
    }).fail(function (xhr) {
        toastr.error(xhr.responseText, 'Error!');
    });
}

function getTaskById(task_Id) {
    $.ajax({
        url: "/api/TaskApi/GetTaskById",
        data: { taskId: task_Id },
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {
        $("#task_id").val(response.task_id);
        $("#txtTitle").val(response.title);
        $("#txtNotes").val(response.note);
        $("#task_datetime").val(kendo.toString(response.date_time, "dd/MM/yyyy hh:mm"));
        $("#chkIsdone").prop('checked', response.is_done);
        if (response.task_action_id > 0) {
            $("#chkIsLead").prop('checked', true);
            $("#div_lead").show();
        }
        else {
            $("#div_lead").hide();
            $("#chkIsLead").prop('checked', false);
        }
        var dropdownlist = $("#ddlLead").data("kendoDropDownList");
        dropdownlist.value(response.task_action_id);

        $("#btnSave").text("Update");
        $("#btnDelete").show();
        $("#kt_modal_duplicate").modal("show");
    });
}

function deleteTask() {
    $.ajax({
        url: "/api/TaskApi/DeleteTask",
        data: { taskId: $("#task_id").val() },
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {
        toastr.success("Task deleted successfully.", "Deleted!");
        $("#kt_modal_duplicate").modal("hide");
        location.href = "/MyDiary";
    });
}

function lead_change() {
    if ($("#chkIsLead").prop('checked')) {
        $("#div_lead").show();
    }
    else { $("#div_lead").hide(); }
}

function clear() {
    $("#txtTitle").val('');
    $("#txtNotes").val('');
}
