$(function() {
    function hockTimePicker($selection) {
        $selection.timepicker({
            minuteStep: 15,
            showSeconds: false,
            showMeridian: false
        }).on('changeTime.timepicker', function(e) { validate(); });
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
        return $('.sessionRow').last();
    }
    
    function validate() {
        var isInvalid = false;
        var allSessions = [];
        
        $('.sessionRow').each(function (index, value) {
            var range = getRange($(this));
            
            if (range.endTime.isGreaterThan(range.startTime) == false) {
                isInvalid = true;
                return false;
            }

            console.log(allSessions);
            if(isOverlap(range, allSessions)) {
                isInvalid = true;
                return false;
            }

            allSessions.push(range);
            return true;
        });

        var $warning = $('#overlappingWarning');
        if (isInvalid) $warning.show();
        else $warning.hide();
    }

    function isOverlap(range, allSessions) {
        
        for (var i = 0; i < allSessions.length; i++) {
            if (allSessions[i].isBetween(range.startTime) 
                || allSessions[i].isBetween(range.endTime)) {
                console.log('between');
                return true;
            }
            else if(range.startTime.isEqualOrLessThan(allSessions[i].startTime) 
                && range.endTime.isEqualOrGreaterThan(allSessions[i].endTime)) {
                console.log('other');
                return true;
            }
            

        }

        return false;
    }

    function parseTime($input) {
        var time = $input.val();
        return {
            hours: parseInt(time.substring(0, 2)),
            minutes: parseInt(time.substring(3, 5)),

            toString: function() {
                return this.hours + ':' + this.minutes;
            },
            
            addHours: function(hoursToAdd) {
                this.hours = (this.hours + hoursToAdd)%24;
                return this;
            },
            
            isGreaterThan: function(other) {
                if (this.hours > other.hours) return true;
                if (this.hours === other.hours) return this.minutes > other.minutes;
                return false;
            },
            
            isEqualOrGreaterThan: function (other) {
                if (this.hours > other.hours) return true;
                if (this.hours === other.hours) return this.minutes >= other.minutes;
                return false;
            },
            
            isLessThan: function(other) {
                if (this.hours < other.hours) return true;
                if (this.hours === other.hours) return this.minutes < other.minutes;
                return false;
            },
            
            isEqualOrLessThan: function (other) {
                if (this.hours < other.hours) return true;
                if (this.hours === other.hours) return this.minutes <= other.minutes;
                return false;
            },
        };
    }
    
    function getRange(sessionRow) {
        return {
            endTime: getEndTime(sessionRow),
            startTime: getStartTime(sessionRow),
            
            isBetween: function (time) {
                return time.isGreaterThan(this.startTime) && time.isLessThan(this.endTime);
            }
        };
    }

    function gotoPage(pageName) {
        $('.active').removeClass('active');
        $('[data-wizard-header="'+ pageName +'"]').addClass('active');
        
        $('[data-wizard-page]').hide();
        $('[data-wizard-page="'+ pageName +'"]').show();
    }
    
    function addSession() {
        var lastEndTime = getLastEndTime();
        
        var viewData = {};
        viewData.startTime = lastEndTime.toString();
        viewData.endTime = lastEndTime.addHours(2).toString();
        
        var result = $('#sessionContainer')
                        .mustache('sessionTemplate', viewData)
                        .children()
                        .last();
        
        result.find('.undo').on('click', function() {
            result.remove();
            validate();
        });
        
        hockTimePicker(result.find('.timepicker'));
        validate();
    }

    /**  Init **/
    $('[data-wizard-link]').on('click', function () { gotoPage($(this).data('wizard-link')); });
    $('[data-wizard-page]').next().hide();
    $('#addMoreSessions').on('click', addSession);
    
    hockTimePicker($('.timepicker'));

    $('.datepicker').pickadate({
        format: 'd mmmm, yyyy',
        formatSubmit: 'yyyy-mm-dd'
    });

    $.Mustache.addFromDom();
});