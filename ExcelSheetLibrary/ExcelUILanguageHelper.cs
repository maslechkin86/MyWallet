namespace ExcelLibrary {
	using System;
	using System.Threading;
	using System.Globalization;

	#region Class: ExcelUILanguageHelper

	public class ExcelUILanguageHelper : IDisposable {

		#region Fieleds: Private

		private CultureInfo currentCulture;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="ExcelUILanguageHelper"/> class.
		/// </summary>
		public ExcelUILanguageHelper() {
			currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
		}


		#endregion

		#region Methods: Public

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose() {
			Thread.CurrentThread.CurrentCulture = currentCulture;
		}

		#endregion

	}

	#endregion

}
