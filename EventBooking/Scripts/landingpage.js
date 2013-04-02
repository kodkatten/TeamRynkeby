(function (intiUpcomingEventsUpdate, $) {

    var previousButton = $('.previous-activities-list-command');
    var nextButton = $('.next-activities-list-command');
    var activityContainer = $('.upcoming-activity-list');
    var upcomingEventsModule = teamrynkebyse.upcomingEvents;

    upcomingEventsModule.init(previousButton, nextButton, activityContainer);
    upcomingEventsModule.setTeamButtons($(".btn-select-team"), (".btn-select-all-teams"));

}(teamrynkebyse.intiUpcomingEventsUpdate, jQuery));