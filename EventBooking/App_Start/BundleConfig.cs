using System.Web.Optimization;

namespace EventBooking
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            
			bundles.Add(new StyleBundle("~/Content/bootstrap")
				.Include("~/Content/bootstrap.css")
				.Include("~/Content/site.css")
				.Include("~/Content/landingpage.css")
				.Include("~/Content/bootstrap-responsive.css")
				.Include("~/Content/bootstrap-timepicker.css")
				.Include("~/Content/landingpage-phone.css")
				.Include("~/Content/landingpage-tablet.css"));

            bundles.Add(new StyleBundle("~/bundles/admincss").Include("~/Content/admin.css"));

            bundles.Add(new ScriptBundle("~/bundles/general")
                .Include("~/Scripts/general.js")
                .Include("~/Scripts/upcomingEvents.js"));

			bundles.Add( new ScriptBundle( "~/bundles/pickadate" ).Include( "~/Scripts/pickadate.js" ) );
			bundles.Add( new ScriptBundle( "~/bundles/timepicker" ).Include( "~/Scripts/bootstrap-timepicker.js" ) );

            bundles.Add(new ScriptBundle("~/bundles/adminscripts").Include("~/Scripts/admin.js"));

        }
    }
}
