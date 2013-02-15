using System;

namespace EventBooking.Extensions
{
    public static class TimeSpanExtensions
    {
         public static DateTime FromNow(this TimeSpan span)
         {
             return DateTime.Now + span;
         }
    }
}