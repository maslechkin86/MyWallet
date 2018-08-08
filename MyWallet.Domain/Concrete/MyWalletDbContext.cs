namespace MyWallet.Domain.Concrete
{
	using System.Data.Entity;
	using Abstract;
	using Entities;
	//using Migrations;

	#region Class: MyWalletDbContext

	public class MyWalletDbContext : DbContext, IMyWalletDbContext
	{

		#region Constructors: Public

		/// <inheritdoc />
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MyWallet.Domain.Concrete.MyWalletDbContext" /> class.
		/// </summary>
		public MyWalletDbContext() : base("MyWalletDbConnectionString") {
			Configuration.LazyLoadingEnabled = true;
			//Database.SetInitializer(
			//	new MigrateDatabaseToLatestVersion<MyWalletDbContext, Configuration>("MyWalletDbConnectionString")
			//);
		}

		#endregion

		#region Properties: Private

		/// <inheritdoc />
		/// <summary>
		/// Gets or sets the collections of transactions.
		/// </summary>
		/// <value>
		/// The collections of transactions.
		/// </value>
		public IDbSet<Transaction> Transactions { get; set; }

		/// <inheritdoc />
		/// <summary>
		/// Gets or sets the collections of accounts.
		/// </summary>
		/// <value>
		/// The collections of accounts.
		/// </value>
		public IDbSet<Account> Accounts { get; set; }

		/// <inheritdoc />
		/// <summary>
		/// Gets or sets the collections of categories.
		/// </summary>
		/// <value>
		/// The collections of categories.
		/// </value>
		public IDbSet<Category> Categories { get; set; }

		/// <inheritdoc />
		/// <summary>
		/// Gets or sets the collections of currencies.
		/// </summary>
		/// <value>
		/// The collections of currencies.
		/// </value>
		public IDbSet<Currency> Currencies { get; set; }

		/// <inheritdoc />
		/// <summary>
		/// Gets or sets the collections of currency rates.
		/// </summary>
		/// <value>
		/// The collections of  currency rates.
		/// </value>
		public IDbSet<CurrencyRate> CurrencyRates { get; set; }

		#endregion

		#region Methods: Protected

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.Entity<Transaction>().HasRequired(t => t.Category).WithMany().HasForeignKey(t => t.CategoryId);
			modelBuilder.Entity<Transaction>().HasRequired(t => t.Account).WithMany().HasForeignKey(t => t.AccountId);
			modelBuilder.Entity<Account>().HasOptional(t => t.ParentAccount).WithMany().HasForeignKey(t => t.ParentAccountId);
			modelBuilder.Entity<Account>().HasRequired(t => t.Currency).WithMany().HasForeignKey(t => t.CurrencyId);
			modelBuilder.Entity<CurrencyRate>().HasRequired(t => t.DestinationCurrency).WithMany().HasForeignKey(t => t.DestinationCurrencyId);
			modelBuilder.Entity<CurrencyRate>().HasRequired(t => t.SourceCurrency).WithMany().HasForeignKey(t => t.SourceCurrencyId);
		}

		#endregion

	}

	#endregion

}