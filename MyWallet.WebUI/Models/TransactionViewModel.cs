namespace MyWallet.WebUI.Models
{
	using System;
	using System.Web.Mvc;

	#region Class: TransactionViewModel

	public class TransactionViewModel
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
		/// Gets or sets the date and time of the transaction.
		/// </summary>
		/// <value>
		/// The date and time of the transaction.
		/// </value>
		public string DateIn { get; set; }

		/// <summary>
		/// Gets or sets the amount of the transaction.
		/// </summary>
		/// <value>
		/// The amount of the transaction.
		/// </value>
		public decimal Amount { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Comment { get; set; }

		/// <summary>
		/// Gets or sets the category name.
		/// </summary>
		/// <value>
		/// The category.
		/// </value>
		public string CategoryName { get; set; }

		/// <summary>
		/// Gets or sets the category icon.
		/// </summary>
		/// <value>
		/// The category icon.
		/// </value>
		public string CategoryIco { get; set; }

		/// <summary>
		/// Gets or sets the account name.
		/// </summary>
		/// <value>
		/// The account identifier.
		/// </value>
		public string AccountName { get; set; }

		/// <summary>
		/// Gets or sets the account icon.
		/// </summary>
		/// <value>
		/// The account icon.
		/// </value>
		public string AccountIco { get; set; }

		#endregion

	}

	#endregion

}