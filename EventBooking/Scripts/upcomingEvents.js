var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;


teamrynkebyse.upcomingEvents = function() {
    var teams = [];
    var prevLink;
    var nextLink;
    var container;
    var baseUri;
    var currentPage = 0;
    
    var activityUrl = function(page) {
           return baseUri + '/?page=' + page;
    };

    function setupLinks() {
        
            nextLink.click(function () {
                var nextPage = currentPage + 1;
                loadPage(nextPage);
               
                return false;
            });
        
            prevLink.click(function () {
                --currentPage;
                container.load(activityUrl(currentPage));
                updateLinks();
                return false;
            });

    };

    function updateLinks() {
        prevLink.toggleClass("disabled", currentPage <= 0);
    };
    
    function loadPage(nextPageNumber) {
        $.get(activityUrl(nextPageNumber), function (result) {
            // Brain br0ken have a hack!
            if ($("li", result).size() !== 0) {
                container.html(result);
                currentPage = nextPageNumber;
            }
            updateLinks();

        });
    };
    
   
    return {
        init: function (prevPage, nextPage, activityContainer) {
            prevLink = prevPage;
            nextLink = nextPage;
            container = activityContainer;
            baseUri = activityContainer.data('update-uri');
            prevLink.toggleClass("disabled", true);
            setupLinks();
        },
        load: function(pageNumber) {
            var selectedTeamIds = [];
            for (var i = 0; i < teams.length; i++) {
                if (teams[i].selected === true) {
                    selectedTeamIds.push(teams[i].id);
                }
            }
            
            if (pageNumber === undefined) {
                alert("Load ALL");
            }
            
        },
        setTeams: function(allTeams) {
            for (var i = 0; i < allTeams.length; i++) {
                teams.push({ id: allTeams[i], selected:false});
            }
        },
        toggleTeam: function(teamId) {
            var team = teams[teamId];

            if (team.selected === false) {
                team.selected = true;
            }
            else {
                team.selected = false;
            }
            
            this.load();
        }
            
    };

}();


//teamrynkebyse.intiUpcomingEventsUpdate = function (previousLink, nextLink, activityContainer, teams) {
//    var currentPage = 0;
//    var baseUri = activityContainer.data('update-uri');
//    alert(teams);
//    previousLink.addClass("disabled");

//    var activityUrl = function(page) {
//        return baseUri + '/?page=' + page;
//    };

//    var updateLinks = function() {
//        previousLink.toggleClass("disabled", currentPage <= 0);
//    };

//    nextLink.click(function () {
//        var nextPage = currentPage + 1;
        
//        $.get(activityUrl(nextPage), function (result) {
//            // Brain br0ken have a hack!
//            if ($("li", result).size() !== 0) {
//                activityContainer.html(result);
//                currentPage = nextPage;
//            }
//        });

//        updateLinks();

//        return false;
//    });
    
//    previousLink.click(function () {
//        --currentPage;
//        activityContainer.load(activityUrl(currentPage));
//        updateLinks();
        
//        return false;
//    });
    
    
//};
