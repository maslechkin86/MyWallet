namespace MyWallet.Domain.Entities
{
	using System;

	#region Class: Account

	/// <inheritdoc />
	/// <summary>
	/// Represent money account.
	/// </summary>
	/// <seealso cref="T:MyWallet.Domain.Entities.BaseEntity" />
	public class Account : BaseLookup
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the name of the account.
		/// </summary>
		/// <value>
		/// The name of the account.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the parent account identifier.
		/// </summary>
		/// <value>
		/// The parent account identifier.
		/// </value>
		public Guid? ParentAccountId { get; set; }

		/// <summary>
		/// Gets or sets the parent account.
		/// </summary>
		/// <value>
		/// The parent account.
		/// </value>
		public virtual Account ParentAccount { get; set; }

		/// <summary>
		/// Gets or sets the account currency identifier.
		/// </summary>
		/// <value>
		/// The account currency identifier.
		/// </value>
		public Guid CurrencyId { get; set; }

		/// <summary>
		/// Gets or sets the account currency.
		/// </summary>
		/// <value>
		/// The account currency.
		/// </value>
		public virtual Currency Currency { get; set; }

		#endregion

	}

	#endregion

}