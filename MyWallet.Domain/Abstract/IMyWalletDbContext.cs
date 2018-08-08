namespace MyWallet.Domain.Abstract
{
	using System;
	using System.Data.Entity;
	using Entities;

	#region Interface: IMyWalletDbContext

	public interface IMyWalletDbContext : IDisposable
	{

		#region Properties: Public

		/// <summary>
		/// Gets or sets the collections of transactions.
		/// </summary>
		/// <value>
		/// The collections of transactions.
		/// </value>
		IDbSet<Transaction> Transactions { get; set; }

		/// <summary>
		/// Gets or sets the collections of accounts.
		/// </summary>
		/// <value>
		/// The collections of accounts.
		/// </value>
		IDbSet<Account> Accounts { get; set; }

		/// <summary>
		/// Gets or sets the collections of categories.
		/// </summary>
		/// <value>
		/// The collections of categories.
		/// </value>
		IDbSet<Category> Categories { get; set; }

		/// <summary>
		/// Gets or sets the collections of currencies.
		/// </summary>
		/// <value>
		/// The collections of currencies.
		/// </value>
		IDbSet<Currency> Currencies { get; set; }

		/// <summary>
		/// Gets or sets the collections of currency rates.
		/// </summary>
		/// <value>
		/// The collections of  currency rates.
		/// </value>
		IDbSet<CurrencyRate> CurrencyRates { get; set; }

		#endregion

		#region Methods: Public

		/// <summary>
		/// Saves the changes.
		/// </summary>
		/// <returns></returns>
		int SaveChanges();

		#endregion

	}

	#endregion

}