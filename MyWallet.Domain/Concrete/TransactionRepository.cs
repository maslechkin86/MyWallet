namespace MyWallet.Domain.Concrete
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Abstract;
	using Entities;
	using Type;

	#region Class: TransactionRepository

	/// <inheritdoc />
	/// <summary>
	/// Represents database abstraction layer for work with category
	/// entity <see cref="T:MyWallet.Domain.Entities.Category" />.
	/// </summary>
	/// <seealso cref="T:MyWallet.Domain.Abstract.ICategoryRepository" />
	public class TransactionRepository : ITransactionRepository
	{

		#region Fields: Private

		private readonly IMyWalletDbContext _context;

		private bool _disposed;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="TransactionRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public TransactionRepository(IMyWalletDbContext context) {
			_context = context;
		}

		#endregion

		#region Methods: Protected

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing) {
			if (!_disposed) {
				if (disposing) {
					_context.Dispose();
				}
			}
			_disposed = true;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc />
		/// <summary>
		/// Gets all collection of transactions.
		/// </summary>
		/// <returns>Collection of transactions.</returns>
		public IEnumerable<Transaction> GetAll() {
			return _context.Transactions.Where(x => x.RowState == (int)RowState.Existing);
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the transaction by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Instance of the <see cref="T:MyWallet.Domain.Entities.Transaction" /> class.</returns>
		public Transaction GetById(Guid id) {
			return _context.Transactions.SingleOrDefault(x => x.Id == id);
		}

		public void Save(Guid transactionId, string comment, decimal amount, Guid accountId, Guid categoryId, byte rowState, bool needConfirm) {
			if(transactionId == Guid.Empty) {
				transactionId = Guid.NewGuid();
				var transaction = new Transaction {
					Id = transactionId,
					AccountId = accountId,
					CategoryId = categoryId,
					Amount = amount,
					Comment = comment,
					DateIn = DateTime.Now,
					CreatedOn = DateTime.Now,
					RowState = rowState,
					NeedConfirm = needConfirm
				};
				_context.Transactions.Add(transaction);
			} else {
				var dbEntry = _context.Transactions.SingleOrDefault(x => x.Id == transactionId);
				if(dbEntry != null) {
					dbEntry.Comment = comment;
					dbEntry.Account = _context.Accounts.Single(x => x.Id == accountId);
					dbEntry.AccountId = accountId;
					dbEntry.Category = _context.Categories.Single(x => x.Id == categoryId);
					dbEntry.CategoryId = categoryId;
					dbEntry.Amount = amount;
					dbEntry.RowState = rowState;
					dbEntry.ModifiedOn = DateTime.Now;
					dbEntry.NeedConfirm = needConfirm;
				}
			}
			_context.SaveChanges();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion

}
