$(function () {
    $('.datepicker').pickadate({
        format: 'd mmmm, yyyy',
        formatSubmit: 'yyyy-mm-dd'
    });

    console.log($('.timepicker').get(0));
    $('.timepicker').timepicker({
        minuteStep: 15,
        showSeconds: false,
        showMeridian: false
    });
});