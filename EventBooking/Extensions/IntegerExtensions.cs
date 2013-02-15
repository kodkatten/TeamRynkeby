using System;

namespace EventBooking.Extensions
{
    public static class IntegerExtensions
    {
         public static TimeSpan Days(this int days)
         {
             return TimeSpan.FromDays(days);
         }
    }
}