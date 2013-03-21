var teamrynkebyse = teamrynkebyse === undefined ? {} : teamrynkebyse;

teamrynkebyse.timehelper = (function () {

    return {
        getTimeFromString: function (time) {
            return {
                hours: parseInt(time.substring(0, 2)),
                minutes: parseInt(time.substring(3, 5)),

                toString: function () {
                    return this.hours + ':' + this.minutes;
                },

                addHours: function (hoursToAdd) {
                    this.hours = (this.hours + hoursToAdd) % 24;
                    return this;
                },

                isGreaterThan: function (other) {
                    if (this.hours > other.hours) return true;
                    if (this.hours === other.hours) return this.minutes > other.minutes;
                    return false;
                },

                isEqualOrGreaterThan: function (other) {
                    if (this.hours > other.hours) return true;
                    if (this.hours === other.hours) return this.minutes >= other.minutes;
                    return false;
                },

                isLessThan: function (other) {
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
        },
        
        makeRange: function(startTimeInput, endTimeInput) {
            return {
                startTime: startTimeInput,
                endTime: endTimeInput,
                isBetween: function (time) {
                    return time.isGreaterThan(this.startTime) && time.isLessThan(this.endTime);
                }
            };
        }
    };
})();
