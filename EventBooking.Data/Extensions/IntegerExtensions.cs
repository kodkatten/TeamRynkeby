using System;

namespace EventBooking.Data.Extensions
{
    public static class IntegerExtensions
    {
         public static TimeSpan Days(this int days)
         {
             return TimeSpan.FromDays(days);
         }
    }
}