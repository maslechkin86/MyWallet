namespace MyWallet.WebUI.Models
{
	using System;
	using System.Web.Mvc;

	#region Class: AccountViewModel

	public class AccountViewModel
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>
		/// The unique identifier.
		/// </value>
		[HiddenInput(DisplayValue = false)]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the account.
		/// </summary>
		/// <value>
		/// The name of the account.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the icon.
		/// </summary>
		/// <value>
		/// The icon.
		/// </value>
		public string Icon { get; set; }

		#endregion

	}

	#endregion

}