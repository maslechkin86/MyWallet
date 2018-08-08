using System;

namespace MyWallet.Domain.Entities {

	/// <inheritdoc />
	/// <summary>
	/// Represent currency rate.
	/// </summary>
	/// <seealso cref="T:MyWallet.Domain.Entities.BaseEntity" />
	public class CurrencyRate : BaseEntity {

		/// <summary>
		/// Gets or sets the source currency identifier.
		/// </summary>
		/// <value>
		/// The source currency identifier.
		/// </value>
		public Guid SourceCurrencyId { get; set; }

		/// <summary>
		/// Gets or sets the source currency.
		/// </summary>
		/// <value>
		/// The source currency.
		/// </value>
		public virtual Currency SourceCurrency { get; set; }

		/// <summary>
		/// Gets or sets the destination currency identifier.
		/// </summary>
		/// <value>
		/// The destination currency identifier.
		/// </value>
		public Guid DestinationCurrencyId { get; set; }

		/// <summary>
		/// Gets or sets the destination currency.
		/// </summary>
		/// <value>
		/// The destination currency.
		/// </value>
		public virtual Currency DestinationCurrency { get; set; }

		/// <summary>
		/// Gets or sets the currency rate.
		/// </summary>
		/// <value>
		/// The currency rate.
		/// </value>
		public decimal Rate { get; set; }

	}

}
