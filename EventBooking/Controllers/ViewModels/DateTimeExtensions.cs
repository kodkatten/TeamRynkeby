using System;
using System.Globalization;

namespace EventBooking.Controllers.ViewModels
{
    public static class DateTimeExtensions
    {
        public static string ToSwedishShortDateString(this DateTime self)
        {
            return self.ToString(new CultureInfo("sv-SE").DateTimeFormat.ShortDatePattern);
        }
    }
}