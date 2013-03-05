
$(function() {
    $('.exclude').on('click', function() {
        var that = $(this);
        var container = that.parents('.volonteer');
        var id = container.data('volonteer-id');
        that.addClass('disabled');

        $.ajax({ url: "/admin/excludefromteam/" + id })
            .done(function () {
                container.addClass('hide-animation');
                container.fadeOut('slow', function () { that.remove(); })
            })
            .fail(function () {
                $('#errorModal').modal('show');
            })
            .always(function () { that.removeClass('disabled'); });
    });
    

    $('.toggle-admin').on('click', function () {
        var that = $(this);
        var userId = that.parents('.volonteer').data('volonteer-id');
        var teamId = that.parents('.team').data('teamid');
        var targetUrl = "/admin/toogleadmin?userId=" + userId + "&teamId=" + teamId;
        that.addClass('disabled');
        
        $.ajax({ url: targetUrl })
            .done(function(data) {
                if (data.isTeamAdmin) {
                    that.addClass('btn-success');
                    $('.icon', that).addClass('icon-white');
                } else {
                    that.removeClass('btn-success');
                    $('.icon', that).removeClass('icon-white');
                }
            })
            .fail(function() {
                $('#errorModal').modal('show');
            })
            .always(function () { that.removeClass('disabled'); });
    });
});
