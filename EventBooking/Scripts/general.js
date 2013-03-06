
(function($, undefined) {
    var initRedirect = function () {
        var redirectUrl = $('a.redirect:first').attr("href");

        if (redirectUrl !== undefined) {
            setTimeout(function () {
                window.location.href = redirectUrl;
            }, 3000);
        }
    };

    $(document).ready(function () {
        $('[rel=tooltip]').tooltip({
            placement: "right"
        });

        initRedirect();
    });

    $(function () {
        $('.datepicker').pickadate({
            format: 'Den dd mmmm, yyyy',
            formatSubmit: 'yyyy-mm-dd'
        });
        
        $('.timepicker').timepicker({
            minuteStep: 15,
            showSeconds: false,
            showMeridian: false
        });
    });
}(jQuery));

