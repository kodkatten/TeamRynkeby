using System.Web.Optimization;

namespace EventBooking
{
	public class BundleConfig
	{
		public static void RegisterBundles( BundleCollection bundles )
		{
			// Keep this separe since we donwload from CDN
			jQuery(bundles);

			// Common over all pages
			SiteBaseCss(bundles);
			SiteBaseJS(bundles);

			// Common for all pages that authenticated users see.
			MemberBaseCss(bundles); 
			MembersBaseJS(bundles);

			LandingPageCss(bundles);

			AdminAreaCss(bundles);

			CreateEventAreaJS(bundles);
			CreateEventAreaCSS(bundles);
		}
		
		private static void CreateEventAreaJS(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/Scripts/CreateEvent")
				.Include("~/scripts/pickadate.js")
				.Include("~/scripts/pickadate.sv_SE.js")
				.Include("~/scripts/bootstrap-timepicker.js")
				.Include("~/scripts/mustache.js")
				.Include("~/scripts/jquery.mustache.js")
				.Include("~/scripts/createevent.js"));
		}
		
		private static void CreateEventAreaCSS(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/CreateEvent")
				            .Include("~/content/styles/bootstrap-timepicker.css")
				            .Include("~/content/styles/pickadate.css")
				            .Include("~/content/styles/createevent.css"));
		}

		private static void SiteBaseJS(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/Scripts/SiteBase")
				.Include("~/Scripts/bootstrap.js")
				.Include("~/Scripts/upcomingEvents.js"));
		}

		private static void MembersBaseJS(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/scripts/MembersBase")
				.Include("~/Scripts/bootstrap.js")
				.Include("~/Scripts/members.js"));
		}

		private static void jQuery(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/scripts/jQuery", "http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js")
				            .Include("~/Scripts/jquery-{version}.js"));
		}

		private static void AdminAreaCss(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/Admin")
				.Include("~/content/styles/admin.css"));
		}

		private static void SiteBaseCss(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/SiteBase")
							.Include("~/content/styles/bootstrap.css")
							.Include("~/content/styles/bootstrap-responsive.css")
							.Include("~/content/styles/site.css"));
		}
		
		private static void MemberBaseCss(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/MemberBase")
				.Include("~/content/styles/font-awesome.css"));
		}

		private static void LandingPageCss(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/LandingPage")
				.Include("~/content/styles/landingpage.css")
				.Include("~/content/styles/landingpage-phone.css")
				.Include("~/content/styles/landingpage-tablet.css"));
		}
	}
}
