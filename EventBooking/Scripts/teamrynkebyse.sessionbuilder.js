
var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;
teamrynkebyse.sessionbuilder = (function () {
    var sessioncontainerId = '#sessionContainer';
    var volunteer0Id = '#volunteer0';
    var addMoreSessionsId = '#addMoreSessions';
    var datePickerClass = '.datepicker';
    var sessionRowClass = '.sessionRow';
    var warningId = '#overlappingWarning';
    var timePickerClass = '.timepicker';
    var textboxClass = '.input-mini';
    var sessionTemplateId = 'sessionTemplate';
    var undoClass = '.undo';
    var count = 0;
    
    function hockTimePicker($selection) {
        $selection.timepicker({
            minuteStep: 15,
            showSeconds: false,
            showMeridian: false
        }).on('changeTime.timepicker', validate);
    }
    
    function getToTime($ctx) {
        return parseTime(getToTimeElement($ctx));
    }

    function getFromTime($ctx) {
        return parseTime(getFromTimeElement($ctx));
    }

    function getToTimeElement($ctx) {
        return $ctx.find('[name*="toTime"]');
    }

    function getFromTimeElement($ctx) {
        return $ctx.find('[name*="fromTime"]');
    }

    function getLastToTime() {
        var result = getLastSessionRow();
        return getToTime(result);
    }

    function getLastSessionRow() {
        return $(sessionRowClass).last();
    }

    function parseTime($input) {
        var time = $input.val();
        return teamrynkebyse.timehelper.getTimeFromString(time);
    }

    function getRange(sessionRow) {
        var toTime = getToTime(sessionRow);
        var fromTime = getFromTime(sessionRow);
        return teamrynkebyse.timehelper.makeRange(fromTime, toTime);
    }

    function getNbr($row) {
        var v = $row.find('[name*="volunteersNeeded"]');
        return v.val();
    }

    function validate() {
        var isInvalid = false;
        var allSessions = [];

        $(sessionRowClass).each(function () {
            var range = getRange($(this));
            var nbr = getNbr($(this));
            
            if (nbr < 1 || nbr > 100000) {
                isInvalid = true;
                return false;
            }
            
            if (range.toTime.isGreaterThan(range.fromTime) == false) {
                isInvalid = true;
                return false;
            }
            
            if (isOverlap(range, allSessions)) {
                isInvalid = true;
                return false;
            }

            allSessions.push(range);
            return true;
        });
        
        var $warning = $(warningId);
        if (isInvalid) $warning.show();
        else $warning.hide();
    }

    function isOverlap(range, allSessions) {

        for (var i = 0; i < allSessions.length; i++) {
            if (allSessions[i].isBetween(range.fromTime)
                || allSessions[i].isBetween(range.toTime)) {
                return true;
            }
            else if (range.fromTime.isEqualOrLessThan(allSessions[i].fromTime)
                && range.toTime.isEqualOrGreaterThan(allSessions[i].toTime)) {
                return true;
            }
        }

        return false;
    }

    function addSession() {
        var lasttoTime = getLastToTime();

        count++;
        
        var viewData = {};
        viewData.fromTime = lasttoTime.toString();
        viewData.toTime = lasttoTime.addHours(2).toString();
        viewData.index = count;
        
        var result = $(sessioncontainerId)
                        .mustache(sessionTemplateId, viewData)
                        .children()
                        .last();
        
        result.find(textboxClass).on('change', validate);
        
        result.find(undoClass).on('click', function () {
            result.remove();
            validate();
            count--;
        });
        
        hockTimePicker(result.find(timePickerClass));
        validate();
    }

    return {
        init: function () {
            
            $(addMoreSessionsId).on('click', addSession);
            $(volunteer0Id).on('change', validate);
            
            hockTimePicker($(timePickerClass));
            var picker = $(datePickerClass);
            var calendar = picker.pickadate({
                format: 'd mmmm, yyyy',
                formatSubmit: 'yyyy-mm-dd',
                dateMin: 1    
            }).data('pickadate');

            picker.val(calendar.getDateLimit(false, "d mmmm, yyyy"));
            $.Mustache.addFromDom();
        },
        
        isValid: function() {
            return $(warningId).is(':visible') == false; 
        }
    };
})();
