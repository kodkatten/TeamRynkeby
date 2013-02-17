var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;

teamrynkebyse.upcomingEvents = function() {
    var teams = [];
    var prevLink;
    var nextLink;
    var container;
    var baseUri;
    var currentPage = 0;
    var showAllTeamsBtn;
    var buttonSelectedClass = "btn-inverse";
    
    var activityUrl = function (page) {
        var selectedTeamIds = [];
        for (var i = 0; i < teams.length; i++) {
            if (teams[i].selected === true) {
                selectedTeamIds.push(teams[i].id);
            }
        }
        return baseUri + '/?page=' + page +'&teamIds=' + selectedTeamIds.join(",");
    };

    function setupLinks() {
            nextLink.click(function () {
                var nextPage = currentPage + 1;
                loadNextPage(nextPage);
                return false;
            });
            prevLink.click(function () {
                var prevPage = currentPage - 1;
                loadNextPage(prevPage);
                return false;
            });
    };

    function loadNextPage(nextPageNumber) {
        var uri = activityUrl(nextPageNumber);
        $.get(uri, function (result) {
            updateLinksAfterLoad(result, nextPageNumber);
        });
    };

    function updateLinksAfterLoad(result, nextPageNumber) {
        // Brain br0ken have a hack!
        if ($("li", result).size() !== 0) {
            container.html(result);
            currentPage = nextPageNumber;
        } 

        var isThereMorePages = ($("li", result).size() >= 6);
        if (isThereMorePages) {
            nextLink.removeClass("disabled");
        } else {
            nextLink.addClass("disabled");
        }
        prevLink.toggleClass("disabled", currentPage <= 0);
    }

    function findTeam(teamId) {
        var team;
        $.each(teams, function (i, item) {
            if (item.id === teamId) {
                team = item;
            }
        });
        return team;
    }
    
    function showAllTeams() {
        showAllTeamsBtn.toggleClass(buttonSelectedClass, true);
        
        $.each(teams, function (i, team) {
            team.button.removeClass(buttonSelectedClass);
            team.selected = false;
        });
        load();
    }

    function isATeamSelected() {
        var isSelected = false;
        $.each(teams, function (i, team) {
            if (team.selected === true) {
                isSelected = true;
            }
        });
        return isSelected;
    }
    
    function toggleTeam(teamId) {
        var team = findTeam(teamId);
   
        showAllTeamsBtn.removeClass(buttonSelectedClass);
        
        if (team.selected === false) {
            team.button.addClass(buttonSelectedClass);
            team.selected = true;
        }
        else {
            team.button.removeClass(buttonSelectedClass);
            team.selected = false;
        }
        if (!isATeamSelected()) {
            showAllTeams();
            return;
        }
        load();
    }
    
    function load() {
        currentPage = 0;
        var uri = activityUrl(0);
            
        $.get(uri, function (result) {
            container.html(result);
            updateLinksAfterLoad(result, 0);
        });
    }

    return {
        init: function (prevPage, nextPage, activityContainer) {
            prevLink = prevPage;
            nextLink = nextPage;
            container = activityContainer;
            baseUri = activityContainer.data('update-uri');
            prevLink.toggleClass("disabled", true);
            setupLinks();
        },
    
        setTeamButtons: function (buttons, showAllTeamButton) {
            showAllTeamsBtn = $(showAllTeamButton);
            showAllTeamsBtn.click(function () {
                showAllTeams();
            });
            for (var i = 0; i < buttons.length; i++) {
                var teamId = $(buttons[i]).data("team-id");
                var btn = $(buttons[i]);
                teams.push({ id: teamId, selected: false, button: btn });
                
                $(buttons[i]).click(function () {
                    toggleTeam($(this).data("team-id"));
                });
            }
        }
            
    };

}();

