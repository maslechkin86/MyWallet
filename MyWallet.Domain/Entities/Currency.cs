namespace MyWallet.Domain.Entities {

	/// <inheritdoc />
	/// <summary>
	/// Represent currency.
	/// </summary>
	/// <seealso cref="T:MyWallet.Domain.Entities.BaseEntity" />
	public class Currency : BaseLookup {

		/// <summary>
		/// Gets or sets the name of currency.
		/// </summary>
		/// <value>
		/// The name of currency.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the symbol of currency.
		/// </summary>
		/// <value>
		/// The symbol of currency.
		/// </value>
		public string Symbol { get; set; }

	}

}
