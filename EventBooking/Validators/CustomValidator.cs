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

			if (!currentSessions.Any())
			{
				return false;
			}

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

        private bool IsTimeBetween(TimeSpan time, TimeSpan startTime, TimeSpan endTime)
        {
            if (startTime < endTime)
                return (time > startTime && time < endTime);
            else
                return !(time > endTime && time < startTime);
        }
    }
}