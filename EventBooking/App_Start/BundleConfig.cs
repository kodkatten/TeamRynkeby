using System.Web.Optimization;

namespace EventBooking
{
	public class BundleConfig
	{
		public static void RegisterBundles(BundleCollection bundles)
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
				.Include("~/Scripts/pickadate.js")
				.Include("~/Scripts/pickadate.sv_SE.js")
				.Include("~/Scripts/bootstrap-timepicker.js")
				.Include("~/Scripts/mustache.js")
				.Include("~/Scripts/jquery.mustache.js")
				.Include("~/Scripts/teamrynkebyse.wizard.js")
                .Include("~/Scripts/teamrynkebyse.timehelper.js")
                .Include("~/Scripts/teamrynkebyse.sessionbuilder.js")
                .Include("~/Scripts/teamrynkebyse.itemsbuilder.js")
				.Include("~/Scripts/createevent.js"));
		}
		
		private static void CreateEventAreaCSS(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/CreateEvent")
				            .Include("~/Content/styles/bootstrap-timepicker.css")
				            .Include("~/Content/styles/pickadate.css")
				            .Include("~/Content/styles/createevent.css"));
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
			bundles.Add(new ScriptBundle("~/Scripts/jQuery", "http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js")
				            .Include("~/Scripts/jquery-{version}.js"));
		}

		private static void AdminAreaCss(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/Admin")
				.Include("~/Content/styles/admin.css"));
		}

		private static void SiteBaseCss(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/SiteBase")
							.Include("~/Content/styles/bootstrap.css")
							.Include("~/Content/styles/bootstrap-responsive.css")
							.Include("~/Content/styles/site.css"));
		}
		
		private static void MemberBaseCss(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/MemberBase")
				.Include("~/Content/Styles/font-awesome.css"));
		}

		private static void LandingPageCss(BundleCollection bundles)
		{
			bundles.Add(new StyleBundle("~/Styles/LandingPage")
				.Include("~/Content/styles/landingpage.css")
				.Include("~/Content/styles/landingpage-phone.css")
				.Include("~/Content/styles/landingpage-tablet.css"));
		}
	}
}
