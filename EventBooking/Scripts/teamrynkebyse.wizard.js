
var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;
teamrynkebyse.wizard = (function() {

    function gotoPage(pageName) {
        $('.active').removeClass('active');
        $('[data-wizard-header="' + pageName + '"]').addClass('active');

        $('[data-wizard-page]').hide();
        $('[data-wizard-page="' + pageName + '"]').show();
    }

    return {
        init: function() {
            $('[data-wizard-link]').on('click', function() { gotoPage($(this).data('wizard-link')); });
            $('[data-wizard-page]').next().hide();
        }
    };
})();