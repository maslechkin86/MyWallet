namespace MyWallet.WebUI.Models
{
	using System;
	using System.Web.Mvc;
	using Domain.Type;

	#region Class: CategoryViewModel

	public class CategoryViewModel
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>
		/// The unique identifier.
		/// </value>
		[HiddenInput(DisplayValue = false)]
		public Guid Id {
			get; set;
		}

		/// <summary>
		/// Gets or sets the name of the category.
		/// </summary>
		/// <value>
		/// The name of the category.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the direction.
		/// </summary>
		/// <value>The type of the direction.</value>
		public DirectionType DirectionType { get; set; }

		#endregion

	}

	#endregion

}