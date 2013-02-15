using System;

namespace EventBooking.Data.Extensions
{
    public static class TimeSpanExtensions
    {
         public static DateTime FromNow(this TimeSpan span)
         {
             return DateTime.Now + span;
         }
    }
}