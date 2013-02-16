using System;

namespace EventBooking.Data
{
    internal class SystemTime
    {
        public static Func<DateTime> Now = () => DateTime.Now;
    }
}
