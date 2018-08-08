[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MyWallet.WebUI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MyWallet.WebUI.App_Start.NinjectWebCommon), "Stop")]

namespace MyWallet.WebUI.App_Start
{
	using System;
	using System.Web;
	using Domain.Abstract;
	using Domain.Concrete;
	using Microsoft.Web.Infrastructure.DynamicModuleHelper;
	using Ninject;
	using Ninject.Web.Common;

	#region Class: NinjectWebCommon

	public static class NinjectWebCommon
	{

		#region Fields: Private

		private static readonly Bootstrapper bootstrapper = new Bootstrapper();

		#endregion

		#region Methods: Private

		/// <summary>
		/// Creates the kernel that will manage your application.
		/// </summary>
		/// <returns>The created kernel.</returns>
		private static IKernel CreateKernel() {
			var kernel = new StandardKernel();
			try {
				kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
				kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
				RegisterServices(kernel);
				return kernel;
			} catch {
				kernel.Dispose();
				throw;
			}
		}

		/// <summary>
		/// Load your modules or register your services here!
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		private static void RegisterServices(IKernel kernel) {
			kernel.Bind<IMyWalletDbContext>().To<MyWalletDbContext>().InRequestScope();
			kernel.Bind<IAccountRepository>().To<AccountRepository>();
			kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
			kernel.Bind<ITransactionRepository>().To<TransactionRepository>();
		}

		#endregion

		#region Methods: Public

		/// <summary>
		/// Starts the application
		/// </summary>
		public static void Start() {
			DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
			DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
			bootstrapper.Initialize(CreateKernel);
		}

		/// <summary>
		/// Stops the application.
		/// </summary>
		public static void Stop() {
			bootstrapper.ShutDown();
		}

		#endregion

	}

#endregion

}
