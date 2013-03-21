
var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;
teamrynkebyse.sessionbuilder = (function () {
    var sessioncontainerId = '#sessionContainer';
    var addMoreSessionsId = '#addMoreSessions';
    var datePickerClass = '.datepicker';
    var sessionRowClass = '.sessionRow';
    var warningId = '#overlappingWarning';
    var timePickerClass = '.timepicker';
    var sessionTemplateId = 'sessionTemplate';
    var undoClass = '.undo';
    
    function hockTimePicker($selection) {
        $selection.timepicker({
            minuteStep: 15,
            showSeconds: false,
            showMeridian: false
        }).on('changeTime.timepicker', function (e) { validate(); });
    }

    function getEndTime($ctx) {
        return parseTime(getEndTimeElement($ctx));
    }

    function getStartTime($ctx) {
        return parseTime(getStartTimeElement($ctx));
    }

    function getEndTimeElement($ctx) {
        return $ctx.find('[name="endTime"]');
    }

    function getStartTimeElement($ctx) {
        return $ctx.find('[name="startTime"]');
    }

    function getLastEndTime() {
        var result = getLastSessionRow();
        return getEndTime(result);
    }

    function getLastSessionRow() {
        return $(sessionRowClass).last();
    }

    function parseTime($input) {
        var time = $input.val();
        return teamrynkebyse.timehelper.getTimeFromString(time);
    }

    function getRange(sessionRow) {
        var endTime = getEndTime(sessionRow);
        var startTime = getStartTime(sessionRow);
        return teamrynkebyse.timehelper.makeRange(startTime, endTime);
    }

    function validate() {
        var isInvalid = false;
        var allSessions = [];

        $(sessionRowClass.each(function () {
            var range = getRange($(this));

            if (range.endTime.isGreaterThan(range.startTime) == false) {
                isInvalid = true;
                return false;
            }
            
            if (isOverlap(range, allSessions)) {
                isInvalid = true;
                return false;
            }

            allSessions.push(range);
            return true;
        }));
        
        var $warning = $(warningId);
        if (isInvalid) $warning.show();
        else $warning.hide();
    }

    function isOverlap(range, allSessions) {

        for (var i = 0; i < allSessions.length; i++) {
            if (allSessions[i].isBetween(range.startTime)
                || allSessions[i].isBetween(range.endTime)) {
                return true;
            }
            else if (range.startTime.isEqualOrLessThan(allSessions[i].startTime)
                && range.endTime.isEqualOrGreaterThan(allSessions[i].endTime)) {
                return true;
            }
        }

        return false;
    }

    function addSession() {
        var lastEndTime = getLastEndTime();

        var viewData = {};
        viewData.startTime = lastEndTime.toString();
        viewData.endTime = lastEndTime.addHours(2).toString();
        var result = $(sessioncontainerId)
                        .mustache(sessionTemplateId, viewData)
                        .children()
                        .last();
        
        result.find(undoClass).on('click', function () {
            result.remove();
            validate();
        });
        
        hockTimePicker(result.find(timePickerClass));
        validate();
    }

    return {
        init: function () {
            
            $(addMoreSessionsId).on('click', addSession);
            hockTimePicker($(timePickerClass));
            
            $(datePickerClass).pickadate({
                format: 'd mmmm, yyyy',
                formatSubmit: 'yyyy-mm-dd'
            });
            $.Mustache.addFromDom();
        }
    };
})();
