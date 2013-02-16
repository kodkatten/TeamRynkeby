var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;

teamrynkebyse.intiUpcomingEventsUpdate = function (updateLinks, activityContainer) {
    updateLinks.click(function () {
        activityContainer.load($(this).attr("href"));
        return false;
    });
};