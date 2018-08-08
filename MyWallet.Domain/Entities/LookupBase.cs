namespace MyWallet.Domain.Entities
{

	#region Class: BaseLookup

	/// <inheritdoc />
	/// <summary>
	/// Class represent base lookup.
	/// </summary>
	public class BaseLookup : BaseEntity
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the path to the icon.
		/// </summary>
		/// <value>
		/// The path to the icon.
		/// </value>
		public string IconPath { get; set; }

		#endregion

	}

	#endregion

}