namespace MyWallet.WebUI
{
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;

	#region Class: MvcApplication

	public class MvcApplication : HttpApplication
	{

		#region Methods: Protected

		protected void Application_Start() {
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
		}

		#endregion

	}

	#endregion

}