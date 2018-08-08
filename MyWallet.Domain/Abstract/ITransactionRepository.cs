namespace MyWallet.Domain.Abstract
{
	using System;
	using System.Collections.Generic;
	using Entities;

	#region Interface: ITransactionRepository

	/// <summary>
	/// Represents database abstraction layer for work with transaction
	/// entity <see cref="Transaction"/>.
	/// </summary>
	public interface ITransactionRepository
	{

		#region Methods: Public

		/// <summary>
		/// Gets all collection of transactions.
		/// </summary>
		/// <returns>Collection of transactions.</returns>
		IEnumerable<Transaction> GetAll();

		/// <summary>
		/// Gets the transaction by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Instance of the <see cref="Transaction"/> class.</returns>
		Transaction GetById(Guid id);

		/// <summary>
		/// Saves the specified transaction identifier.
		/// </summary>
		/// <param name="transactionId">The transaction identifier.</param>
		/// <param name="comment">The comment.</param>
		/// <param name="amount">The amount.</param>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="categoryId">The category identifier.</param>
		/// <param name="rowState">State of the row.</param>
		/// <param name="needConfirm">if set to <c>true</c> [need confirm].</param>
		void Save(Guid transactionId, string comment, decimal amount, Guid accountId, Guid categoryId, byte rowState,
			bool needConfirm);

		#endregion

	}

	#endregion

}
