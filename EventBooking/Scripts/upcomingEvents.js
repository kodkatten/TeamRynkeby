﻿var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;


teamrynkebyse.upcomingEvents = function() {
    var teams = [];
    var prevLink;
    var nextLink;
    var container;
    var baseUri;
    var currentPage = 0;
    var showAllTeamsBtn;
    var buttonSelectedClass = "btn-primary";
    
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
                nextLink.toggleClass("disabled", true);
            }
            updateLinks();
        });
    };

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
        });
        
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
        load();
    }
    
    function load() {
        var selectedTeamIds = [];
        for (var i = 0; i < teams.length; i++) {
            if (teams[i].selected === true) {
                selectedTeamIds.push(teams[i].id);
            }
        }

        currentPage = 0;
        var uri = activityUrl(0) + '&teamIds=' + selectedTeamIds.join(",");
            
        $.get(uri, function (result) {
            container.html(result);
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

