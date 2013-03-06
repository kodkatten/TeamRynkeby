
$(function() {
    $('.exclude').on('click', function() {
        var that = $(this);
        var container = that.parents('.volonteer');
        var userId = container.data('volonteer-id');
        var teamId = that.parents('.team').data('team-id');
        var targetUrl = "/admin/excludefromteam/?userId=" + userId + "&teamId=" + teamId;

        that.addClass('disabled');

        $.ajax({ url: targetUrl })
            .done(function () {
                container.addClass('hide-animation');
                container.fadeOut('slow', function() { that.remove(); });
            })
            .fail(function () {
                $('#errorModal').modal('show');
            })
            .always(function () { that.removeClass('disabled'); });
    });
    
    function togglePrivilege(that, url) {
        var userId = that.parents('.volonteer').data('volonteer-id');
        var teamId = that.parents('.team').data('team-id');
        var targetUrl = url + "?userId=" + userId + "&teamId=" + teamId;

        that.addClass('disabled');

        $.ajax({ url: targetUrl })
            .done(function (data) {
                if (data.newState) {
                    that.addClass('btn-success');
                    $('.icon', that).addClass('icon-white');
                } else {
                    that.removeClass('btn-success');
                    $('.icon', that).removeClass('icon-white');
                }
            })
            .fail(function () {
                $('#errorModal').modal('show');
            })
            .always(function () { that.removeClass('disabled'); });
    }

    $('.toggle-poweruser').on('click', function () {
        togglePrivilege($(this), '/admin/toogleTeamPowerUser');
    });

    $('.toggle-admin').on('click', function () {
        togglePrivilege($(this), '/admin/ToogleAdministrator');
    });

    $('.teamdelete').on('click', function () {
        var that = $(this);
        var teamId = that.parents('[data-team-id]').data('team-id');
        var deleteModal = $('#teamDeleteWarning');
        deleteModal.data('team-id', teamId);
        deleteModal.modal('show', { backdrop: 'static' });
    });

    $('#deleteTeam').on('click', function () {
        var that = $(this);
        var modal = $('#teamDeleteWarning');
        var failMessage = modal.find('.alert');
        var teamId = modal.data('team-id');
        var buttons = modal.find('button');
        
        buttons.attr('disabled', 'disabled');
        failMessage.addClass('hidden');

        $.ajax({ url: '/admin/deleteTeam/' + teamId })
            .done(function () {
                modal.modal('hide');
                var container = $('[data-team-id=' + teamId + ']');
                container.addClass('hide-animation');
                container.delay(800).fadeOut('slow', function () { container.remove(); });
            })
            .fail(function () {
                failMessage.removeClass('hidden');
            })
            .always(function () { buttons.attr('disabled', false); });
    });

    $(".card a[rel=tooltip]").tooltip({ container: 'body', placement: 'top' });
});
