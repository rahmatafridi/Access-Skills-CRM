"use strict";

// Class definition
var KTToastrDemoCustom = function() {

    var customToastr = function () {
        var i = -1;
        var toastCount = 0;
        var $toastlast;

        var getMessage = function () {
            var msgs = [
                'New order has been placed!',
                'Are you the six fingered man?',
                'Inconceivable!',
                'I do not think that means what you think it means.',
                'Have fun storming the castle!'
            ];
            i++;
            if (i === msgs.length) {
                i = 0;
            }

            return msgs[i];
        };

        var getMessageWithClearButton = function (msg) {
            msg = msg ? msg : 'Clear itself?';
            msg += '<br /><br /><button type="button" class="btn btn-outline-light btn-sm--air--wide clear">Yes</button>';
            return msg;
        };

        $('#showtoast').click(function () {
            var shortCutFunction = $("#toastTypeGroup input:radio:checked").val();
            var msg = $('#message').val();
            var title = $('#title').val() || '';
            var $showDuration = $('#showDuration');
            var $hideDuration = $('#hideDuration');
            var $timeOut = $('#timeOut');
            var $extendedTimeOut = $('#extendedTimeOut');
            var $showEasing = $('#showEasing');
            var $hideEasing = $('#hideEasing');
            var $showMethod = $('#showMethod');
            var $hideMethod = $('#hideMethod');
            var toastIndex = toastCount++;
            var addClear = $('#addClear').prop('checked');

            custoastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": false,
                "progressBar": true,
                "positionClass": "toast-top-center",
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            };

            //custoastr.options = {
            //    closeButton: $('#closeButton').prop('checked'),
            //    debug: $('#debugInfo').prop('checked'),
            //    newestOnTop: $('#newestOnTop').prop('checked'),
            //    progressBar: $('#progressBar').prop('checked'),
            //    positionClass: $('#positionGroup input:radio:checked').val() || 'toast-top-right',
            //    preventDuplicates: $('#preventDuplicates').prop('checked'),
            //    onclick: null
            //};

            if ($('#addBehaviorOnToastClick').prop('checked')) {
                custoastr.options.onclick = function () {
                    alert('You can perform some custom action after a toast goes away');
                };
            }

            if ($showDuration.val().length) {
                custoastr.options.showDuration = $showDuration.val();
            }

            if ($hideDuration.val().length) {
                custoastr.options.hideDuration = $hideDuration.val();
            }

            if ($timeOut.val().length) {
                custoastr.options.timeOut = addClear ? 0 : $timeOut.val();
            }

            if ($extendedTimeOut.val().length) {
                custoastr.options.extendedTimeOut = addClear ? 0 : $extendedTimeOut.val();
            }

            if ($showEasing.val().length) {
                custoastr.options.showEasing = $showEasing.val();
            }

            if ($hideEasing.val().length) {
                custoastr.options.hideEasing = $hideEasing.val();
            }

            if ($showMethod.val().length) {
                custoastr.options.showMethod = $showMethod.val();
            }

            if ($hideMethod.val().length) {
                custoastr.options.hideMethod = $hideMethod.val();
            }

            if (addClear) {
                msg = getMessageWithClearButton(msg);
                custoastr.options.tapToDismiss = false;
            }
            if (!msg) {
                msg = getMessage();
            }

            $('#toastrOptions').text(
                'custoastr.options = '
                + JSON.stringify(custoastr.options, null, 2)
                + ';'
                + '\n\custoastr.'
                + shortCutFunction
                + '("'
                + msg
                + (title ? '", "' + title : '')
                + '");'
            );

            var $toast = custoastr[shortCutFunction](msg, title); // Wire up an event handler to a button in the toast, if it exists
            $toastlast = $toast;

            if (typeof $toast === 'undefined') {
                return;
            }

            if ($toast.find('#okBtn').length) {
                $toast.delegate('#okBtn', 'click', function () {
                    alert('you clicked me. i was toast #' + toastIndex + '. goodbye!');
                    $toast.remove();
                });
            }
            if ($toast.find('#surpriseBtn').length) {
                $toast.delegate('#surpriseBtn', 'click', function () {
                    alert('Surprise! you clicked me. i was toast #' + toastIndex + '. You could perform an action here.');
                });
            }
            if ($toast.find('.clear').length) {
                $toast.delegate('.clear', 'click', function () {
                    custoastr.clear($toast, { force: true });
                });
            }
        });

        function getLastToast() {
            return $toastlast;
        }
        $('#clearlasttoast').click(function () {
            custoastr.clear(getLastToast());
        });
        $('#cleartoasts').click(function () {
            custoastr.clear();
        });
    };

    return {
        // public functions
        init: function() {
            customToastr();
        }
    };
}();

$(document).ready(function() {
    KTToastrDemoCustom.init();
});
