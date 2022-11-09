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
            , defaultView: "listWeek", defaultDate: n, editable: !0, eventLimit: !0, navLinks: !0,
            events: _dataCollection
            //    , {
            //    title: "Reporting", start: t + "-14T13:30:00", description: "Lorem ipsum dolor incid idunt ut labore", end: t + "-14", className: "fc-event-success"
            //}
            //    , {
            //    title: "Company Trip", start: t + "-02", description: "Lorem ipsum dolor sit tempor incid", end: t + "-03", className: "fc-event-primary"
            //}
            //    , {
            //    title: "ICT Expo 2017 - Product Release", start: t + "-03", description: "Lorem ipsum dolor sit tempor inci", end: t + "-05", className: "fc-event-light fc-event-solid-primary"
            //}
            //    , {
            //    title: "Dinner", start: t + "-12", description: "Lorem ipsum dolor sit amet, conse ctetur", end: t + "-10"
            //}
            //    , {
            //    id: 999, title: "Repeating Event", start: t + "-09T16:00:00", description: "Lorem ipsum dolor sit ncididunt ut labore", className: "fc-event-danger"
            //}
            //    , {
            //    id: 1e3, title: "Repeating Event", description: "Lorem ipsum dolor sit amet, labore", start: t + "-16T16:00:00"
            //}
            //    , {
            //    title: "Conference", start: i, end: r, description: "Lorem ipsum dolor eius mod tempor labore", className: "fc-event-brand"
            //}
            //    , {
            //    title: "Meeting", start: n + "T10:30:00", end: n + "T12:30:00", description: "Lorem ipsum dolor eiu idunt ut labore"
            //}
            //    , {
            //    title: "Lunch", start: n + "T12:00:00", className: "fc-event-info", description: "Lorem ipsum dolor sit amet, ut labore"
            //}
            //    , {
            //    title: "Meeting", start: n + "T14:30:00", className: "fc-event-warning", description: "Lorem ipsum conse ctetur adipi scing"
            //}
            //    , {
            //    title: "Happy Hour", start: n + "T17:30:00", className: "fc-event-info", description: "Lorem ipsum dolor sit amet, conse ctetur"
            //}
            //    , {
            //    title: "Dinner", start: r + "T05:00:00", className: "fc-event-solid-danger fc-event-light", description: "Lorem ipsum dolor sit ctetur adipi scing"
            //}
            //    , {
            //    title: "Birthday Party", start: r + "T07:00:00", className: "fc-event-primary", description: "Lorem ipsum dolor sit amet, scing"
            //}
            //    , {
            //    title: "Click for Google", url: "http://google.com/", start: t + "-28", className: "fc-event-solid-info fc-event-light", description: "Lorem ipsum dolor sit amet, labore"
            //}
            , eventRender: function (e) {
                var t = $(e.el);
                e.event.extendedProps &&
                    e.event.extendedProps.description &&
                    (t.hasClass("fc-day-grid-event") ? (t.data("content", e.event.extendedProps.description), t.data("placement", "top"), KTApp.initPopover(t)) : t.hasClass("fc-time-grid-event") ? t.find(".fc-title").append('<div class="fc-description">' + e.event.extendedProps.description + "</div>") : 0 !== t.find(".fc-list-item-title").lenght && t.find(".fc-list-item-title").append('<div class="fc-description">' + e.event.extendedProps.description + "</div>").find("#task_id").text(e.event.id));
            }
        }
        ).render();
    }
};

var _dataCollection = [];

jQuery(document).ready(function () {
    GetAllTasks();
});

function GetAllTasks() {
    $.ajax({
        url: "/api/TaskApi/GetAllTasks",
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    }).done(function (response) {
        _dataCollection = [];
        var item = '';
        for (var count = 0; count < response.length; count++) {

            item =
                {
                    title: response[count].title
                    , start: moment(response[count].date_time)._i
                    , description: " Note: " + response[count].note
                    , className: response[count].Class_Name
                , id: response[count].task_id
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
            location.href = "/Task";
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
    }).done(function (response)
    {
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
        location.href = "/Task";
    });
}

function lead_change()
{
    if ($("#chkIsLead").prop('checked')) {
        $("#div_lead").show();
    }
    else { $("#div_lead").hide(); }
}

function clear()
{
    $("#txtTitle").val('');
    $("#txtNotes").val('');
}



