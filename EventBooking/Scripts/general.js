
$(document).ready(function () {
    $('[rel=tooltip]').tooltip({
        placement: "right"
    });
});

$(function () {
    $('.datepicker').pickadate();
    $('.timepicker').timepicker({
        minuteStep: 15,
        showSeconds: false,
        showMeridian: false
    });
});
