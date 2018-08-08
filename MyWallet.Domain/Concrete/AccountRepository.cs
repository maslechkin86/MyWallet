namespace MyWallet.Domain.Concrete
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Abstract;
	using Entities;
	using Type;

	#region Class: AccountRepository

	/// <inheritdoc />
	/// <summary>
	/// Represents database abstraction layer for work with account
	/// entity <see cref="T:MyWallet.Domain.Entities.Account" />.
	/// </summary>
	/// <seealso cref="T:MyWallet.Domain.Abstract.IAccountRepository" />
	public class AccountRepository : IAccountRepository
	{

		#region Fields: Private

		private readonly IMyWalletDbContext _context;

		private bool _disposed;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="AccountRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public AccountRepository(IMyWalletDbContext context) {
			_context = context;
		}

		#endregion

		#region Methods: Protected

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
		/// Gets all collection of accounts.
		/// </summary>
		/// <returns>Collection of accounts.</returns>
		public IEnumerable<Account> GetAll() {
			return _context.Accounts.Where(x => x.RowState == (int)RowState.Existing);
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the account by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Instance of the <see cref="T:MyWallet.Domain.Entities.Account" /> class.</returns>
		public Account GetById(Guid id) {
			return _context.Accounts.FirstOrDefault(x => x.Id == id);
		}

		/// <inheritdoc />
		/// <summary>
		/// Saves the specified account.
		/// </summary>
		/// <param name="accountId">The account identifier.</param>
		/// <param name="name">The name.</param>
		/// <param name="parentAccountId">The parent account identifier.</param>
		/// <param name="currencyId">The currency identifier.</param>
		/// <param name="iconPath">The icon path.</param>
		/// <param name="rowState">State of the row.</param>
		public void Save(Guid accountId, string name, Guid? parentAccountId, Guid currencyId, string iconPath, byte rowState) {
			if(accountId == Guid.Empty) {
				accountId = Guid.NewGuid();
				var transaction = new Account {
					Id = accountId,
					IconPath = iconPath,
					Name = name,
					ParentAccountId = parentAccountId,
					CurrencyId = currencyId,
					CreatedOn = DateTime.Now,
					RowState = rowState
				};
				_context.Accounts.Add(transaction);
			} else {
				var dbEntry = _context.Accounts.SingleOrDefault(x => x.Id == accountId);
				if(dbEntry != null) {
					dbEntry.Name = name;
					dbEntry.IconPath = iconPath;
					dbEntry.ParentAccountId = parentAccountId;
					dbEntry.CurrencyId = currencyId;
					dbEntry.RowState = rowState;
					dbEntry.ModifiedOn = DateTime.Now;
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
