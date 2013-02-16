
$(document).ready(function () {
    $('[rel=tooltip]').tooltip({
        placement: "right"
    });
});

$(function () {
    //TODO: drag in something that works good instead of this hack
    $('.rotater>div').hide().slice(0, 2).show();
    $('.rotater').data('index', 0);
    $('.prev').click(function (e) {
        e.preventDefault();
        var rotater = $('.rotater');
        var currentIndex = rotater.data('index') - 1;
        if (currentIndex < 0) return false;

        $('.rotater').data('index', currentIndex);

        rotater.children().hide().slice(currentIndex, currentIndex + 2).show();
        return false;
    });

    $('.next').click(function (e) {
        e.preventDefault();
        var rotater = $('.rotater');
        var currentIndex = rotater.data('index') + 1;
        if (currentIndex >= rotater.children().length - 1) return false;
        rotater.data('index', currentIndex);

        rotater.children().hide().slice(currentIndex, currentIndex + 2).show();
        return false;
    });

    $('.datepicker').pickadate();

});