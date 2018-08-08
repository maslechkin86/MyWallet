namespace MyWallet.WebUI
{
	using System.Web.Mvc;
	using System.Web.Routing;

	#region Class: RouteConfig

	public class RouteConfig
	{

		#region Methods: Public

		public static void RegisterRoutes(RouteCollection routes) {
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default",
				"{controller}/{action}/{id}",
				new {controller = "Home", action = "Index", id = UrlParameter.Optional}
			);
		}

		#endregion

	}

	#endregion

}