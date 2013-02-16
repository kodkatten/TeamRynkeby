
$(document).ready(function () {
    $('#teams-list a').on("click", onTeamClicked);
});

function onTeamClicked() {

    var $link = $(this);

    $("#team-members-container h2").text("Team medlemmar i " + $link.text());

    $.ajax({
        dataType: "json",
        url: "/admin/ListTeamMembers",
        data: { teamId: 1 },
        success: function(data, textStatus, jqXHR) {
            UpdateTeamMembers(data);
        }
    });
}

function UpdateTeamMembers(data) {
    var $list = $("#team-members-container ul");
    $list.empty();

    $.each(data, function (index, user) {
        var $li = $("<li/>").addClass("team-member");
        $li.text(user.Name);

        $list.append($li);
    });
}
