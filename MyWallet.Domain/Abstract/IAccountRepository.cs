namespace MyWallet.Domain.Abstract {
	using System;
	using System.Collections.Generic;
	using Entities;

	#region Interface: IAccountRepository

	/// <summary>
	/// Represents database abstraction layer for work with account
	/// entity <see cref="Account"/>.
	/// </summary>
	public interface IAccountRepository {

		#region Methods: Public

		/// <summary>
		/// Gets all collection of accounts.
		/// </summary>
		/// <returns>Collection of accounts.</returns>
		IEnumerable<Account> GetAll();

		/// <summary>
		/// Gets the account by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Instance of the <see cref="Account"/> class.</returns>
		Account GetById(Guid id);

		/// <summary>
		/// Saves the specified account.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="name">The name.</param>
		/// <param name="parentAccountId">The parent account identifier.</param>
		/// <param name="currencyId">The currency identifier.</param>
		/// <param name="iconPath">The icon path.</param>
		/// <param name="rowState">State of the row.</param>
		void Save(Guid accountId, string name, Guid? parentAccountId, Guid currencyId, string iconPath, byte rowState);

		#endregion

	}

	#endregion

}