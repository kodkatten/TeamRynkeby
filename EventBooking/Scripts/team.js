(function ($, undefined) {
    var initRedirect = function () {
        var redirectUrl = $('a.redirect:first').attr("href");

        if (redirectUrl !== undefined) {
            setTimeout(function () {
                window.location.href = redirectUrl;
            }, 3000);
        }
    };

    $(function () {
        $('[rel=tooltip]').tooltip({
            placement: "right"
        });

        initRedirect();
    });
}(jQuery));