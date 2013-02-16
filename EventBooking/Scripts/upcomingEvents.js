var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;

teamrynkebyse.intiUpcomingEventsUpdate = function (previousLink, nextLink, activityContainer) {
    var currentPage = 0;
    var baseUri = activityContainer.data('update-uri');

    previousLink.addClass("disabled");

    var activityUrl = function(page) {
        return baseUri + '/?page=' + page;
    };

    var updateLinks = function() {
        previousLink.toggleClass("disabled", currentPage <= 0);
    };

    nextLink.click(function () {
        var nextPage = currentPage + 1;
        
        $.get(activityUrl(nextPage), function (result) {
            // Brain br0ken have a hack!
            if ($("li", result).size() !== 0) {
                activityContainer.html(result);
                currentPage = nextPage;
            }
        });

        updateLinks();

        return false;
    });
    
    previousLink.click(function () {
        --currentPage;
        activityContainer.load(activityUrl(currentPage));
        updateLinks();
        
        return false;
    });
    
    
};