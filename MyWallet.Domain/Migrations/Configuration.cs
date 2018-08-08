namespace MyWallet.Domain.Migrations {
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using Concrete;
	using Entities;
	using Type;

	#region Class: Configuration

	internal sealed class Configuration : DbMigrationsConfiguration<MyWalletDbContext> {

		#region Constructors: Public

		public Configuration() {
			AutomaticMigrationsEnabled = true;
		}

		#endregion

		#region Methods: Protected

		protected override void Seed(MyWalletDbContext context) { }

		protected void InitialSeed(MyWalletDbContext context) {
			var uah = new Currency {
				Name = "UAH",
				Symbol = "uah"
			};
			var eur = new Currency {
				Name = "EUR",
				Symbol = "€"
			};
			var pln = new Currency {
				Name = "PLN",
				Symbol = "zł"
			};
			var currencies = new List<Currency> {
				uah,
				eur,
				pln
			};
			context.Currencies.ToList().ForEach(x => context.Currencies.Remove(x));
			currencies.ForEach(currency => context.Currencies.AddOrUpdate(p => p.Id, currency));
			context.SaveChanges();
			var accounts = new List<Account> {
				new Account {
					Name = "Wallet(PLN)",
					IconPath = "~/Content/Images/salary.png",
					Currency = pln
				},
				new Account {
					Name = "Wallet(EUR)",
					IconPath = "/Content/Images/salary.png",
					Currency = eur
				},
				new Account {
					Name = "Wallet(UAH)",
					IconPath = "/Content/Images/salary.png",
					Currency = uah
				},
				new Account {
					Name = "AvalBank",
					IconPath = "/Content/Images/balance.png",
					Currency = uah
				},
				new Account {
					Name = "PrivatBank",
					IconPath = "/Content/Images/balance.png",
					Currency = uah
				},
				new Account {
					Name = "MillenniumBank",
					IconPath = "/Content/Images/balance.png",
					Currency = pln
				}
			};
			context.Accounts.ToList().ForEach(x => context.Accounts.Remove(x));
			accounts.ForEach(account => context.Accounts.AddOrUpdate(p => p.Id, account));
			context.SaveChanges();
			var categories = new List<Category> {
				new Category { DirectionType = DirectionType.Incoming, Name = "Salary", IconPath = "/Content/Images/salary.png" },
				new Category { DirectionType = DirectionType.Expense, Name = "Flat", IconPath = "/Content/Images/flat.JPG" },
				new Category { DirectionType = DirectionType.Expense, Name = "Food", IconPath = "/Content/Images/food.JPG" },
				new Category { DirectionType = DirectionType.Expense, Name = "Transport", IconPath = "/Content/Images/bus.png" },
				new Category { DirectionType = DirectionType.Expense, Name = "Clothes", IconPath = "/Content/Images/clothes.JPG" },
				new Category { DirectionType = DirectionType.Expense, Name = "Fun", IconPath = "/Content/Images/fun.JPG" },
				new Category { DirectionType = DirectionType.Expense, Name = "Health", IconPath = "/Content/Images/health.JPG" },
				new Category { DirectionType = DirectionType.Expense, Name = "Mobile", IconPath = "/Content/Images/mobile.JPG" },
				new Category { DirectionType = DirectionType.Expense, Name = "Restoran", IconPath = "/Content/Images/restorun.JPG" },
				new Category { DirectionType = DirectionType.Expense, Name = "Technique", IconPath = "/Content/Images/technique.JPG" },
				new Category { DirectionType = DirectionType.Expense, Name = "Transfer", IconPath = "/Content/Images/transfer.JPG" },
				new Category { DirectionType = DirectionType.Expense, Name = "Travel", IconPath = "/Content/Images/travel.JPG" },
			};
			context.Categories.ToList().ForEach(x => context.Categories.Remove(x));
			categories.ForEach(category => context.Categories.AddOrUpdate(p => p.Id, category));
			context.SaveChanges();
			var transactions = new List<Transaction> {
				new Transaction {
					Comment = "(test)Salary",
					Category = categories.First(x => x.Name == "Salary"),
					Amount = 1000,
					Account = accounts.First(x => x.Name == "Wallet(UAH)")
				},
				new Transaction {
					Comment = "(test)bigPocket",
					Category = categories.First(x => x.Name == "Food"),
					Amount = 500,
					Account = accounts.First(x => x.Name == "Wallet(UAH)")
				}
			};
			context.Transactions.ToList().ForEach(x => context.Transactions.Remove(x));
			transactions.ForEach(transaction => context.Transactions.AddOrUpdate(p => p.Id, transaction));
			context.SaveChanges();
		}

		#endregion

	}

	#endregion

}