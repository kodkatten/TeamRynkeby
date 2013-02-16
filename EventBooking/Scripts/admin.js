
$(function() {
    $('.exclude').on('click', function() {
        alert('hello');
    });
    

    $('.toggle-admin').on('click', function () {
        var that = $(this);
        var id = that.parents('.volonteer').data('volonteer-id');

        that.addClass('disabled');

        $.ajax({ url: "/admin/toogleadmin/" + id })
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
