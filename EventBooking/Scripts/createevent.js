
var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;

$(function () {

    function checkValid($elem) {
        if ($elem.val() == "") $elem.addClass('invalid');
        else $elem.removeClass('invalid');
    }

    function enableLiveCheckingPageOne($emptyBoxes) {
        $emptyBoxes.each(function () { checkValid($(this)); });
        $requiredInput.off('keyup', "**");
        $requiredInput.on('keyup', function () { checkValid($(this)); });
    }
    
    function testDate($dateElem) {
        console.log('testDate');
        if ($dateElem.val() == "") {
            $dateElem.addClass('invalid');
        } else {
            $dateElem.removeClass('invalid');
        }
    }
    
    function canNavigateToPageTwo() {
        var emptyTextBoxes = $requiredInput.filter(function () { return this.value == ""; });
        var isValid = emptyTextBoxes.length === 0;
        if (!isValid) { enableLiveCheckingPageOne(emptyTextBoxes); }
        return isValid;
    }

    function canNavigateToPageThree() {
        return teamrynkebyse.sessionbuilder.isValid();
    }

    var wizardOptions = {
        pageTwo: canNavigateToPageTwo,
        pageThree: canNavigateToPageThree 
    };
    
    teamrynkebyse.wizard.init(wizardOptions);
    teamrynkebyse.sessionbuilder.init();
    teamrynkebyse.itemsbuilder.init();
    
    var $requiredInput  = $('[data-wizard-page="pageOne"]').find('input:text, textarea');
});