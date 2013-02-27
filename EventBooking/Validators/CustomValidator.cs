using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventBooking.Controllers.ViewModels;
using System.Linq;

namespace EventBooking.Validators
{
    public class CustomValidator
    {

        public bool IsSessionIntrudingOnOtherSessionsTimeframe(SessionModel newSession, IEnumerable<SessionModel> currentSessions)
        {
            bool isInConflict = true;
            foreach (var session in currentSessions)
            {
                //Checks for exact match
                if (session.FromTime == newSession.FromTime && session.ToTime == newSession.ToTime)
                {
                    return isInConflict;
                }

                //Checks that newSessions FromTime is not inbetween the current sessions timeframe.
                isInConflict = IsTimeBetween(newSession.FromTime, session.FromTime, session.ToTime);

                if (isInConflict)
                {
                    return isInConflict;
                }

                //Checks that newSessions toTime is not inbetween the current sessions timeframe.
                isInConflict = IsTimeBetween(newSession.ToTime, session.FromTime, session.ToTime);

                if (isInConflict)
                {
                    return isInConflict;
                }
            }
            return isInConflict;
        }

        private bool IsTimeBetween(DateTime time, DateTime startTime, DateTime endTime)
        {
            if (startTime.TimeOfDay < endTime.TimeOfDay)
                return (time.TimeOfDay > startTime.TimeOfDay && time.TimeOfDay < endTime.TimeOfDay);
            else
                return !(time.TimeOfDay > endTime.TimeOfDay && time.TimeOfDay < startTime.TimeOfDay);
        }
    }
}