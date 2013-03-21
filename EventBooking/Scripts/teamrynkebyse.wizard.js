
var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;
teamrynkebyse.wizard = (function() {
    var options;

    function gotoPage(pageName, event) {
        
        if (options[pageName] && options[pageName]() !== true) {
            return;
        }

        $('.active').removeClass('active');
        $('[data-wizard-header="' + pageName + '"]').addClass('active');

        $('[data-wizard-page]').hide();
        $('[data-wizard-page="' + pageName + '"]').show();
    }

    return {
        
        init: function (opt) {
            options = opt;
            $('[data-wizard-link]').on('click', function(e) { gotoPage($(this).data('wizard-link'), e); });
            $('[data-wizard-page]').next().hide();
        }
    };
})();