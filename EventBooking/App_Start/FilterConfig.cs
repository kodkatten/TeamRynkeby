using System.Web;
using System.Web.Mvc;
using EventBooking.Filters;

namespace EventBooking
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}