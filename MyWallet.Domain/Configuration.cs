namespace MyWallet.Domain.Migrations
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity.Migrations;
	using System.Linq;
	using Concrete;
	using Entities;
	using Type;

	#region Class: Configuration

	internal sealed class Configuration : DbMigrationsConfiguration<MyWalletDbContext>
	{

		#region Constructors: Public

		public Configuration() {
			AutomaticMigrationsEnabled = true;
		}

		#endregion

		#region Methods: Protected

		protected override void Seed(MyWalletDbContext context) {
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
			var privatBank = new Account {
				Name = "PrivatBank",
				Currency = uah
			};
			var accounts = new List<Account> {
				new Account {
					Name = "Wallet(PLN)",
					Icon = Convert.FromBase64String(@"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAADXklEQVR42m1Ta2xTZRh+vnNO23Paba3d2OrosAwz8DLDRYWoPzRmRv5pgol4YQkxGi/JJF5iNsYPExKTQTTI/qDNxOGikEgEFkM0KkPn4lgkZYOmMh12tevanp1u7ek537n5tVS8xDd5/nzf+z3f87wXctt36ubmoLizvR6b6nmyjrMhlmwnn6aIz63Y37vz6ide08y66wOI9T6O3A9f4J9Buk4vKAql/mxRR9EkMG12aFsQOA4UBOt9dYMdfv8r84kLGN//JGgu9W+CV6PzS6qpBVY0EyVNR5k6MB0bvOBBUHLjllDL0B8z53ePHt4NRcngv0HeOJnJcjzfZBiAQ2z2pwNCOPCcA170IJWYjp7ofey58koW/xfkwbOLX0eCzsMRvwSpwQ3esmDYBIpMMacJ+Pbtl/Ytjn58hOV6GUyGEoPCYFcJth9PLSTTuRZlXoZZ0gGDwHATUCZfaGuHODl8PH2sbx/LrWPgGCpSfr9BsOeDVN4UnKBJTRR1HZRa0B03PCKwtrEOs+OfnTlxuOe1GgH7AXM1FdctvD6aTXoCQpi3OFisBZaLh6Vb6Fy+gA2tIhpXhfX+/r6BkZGRYZafr+HvGmw+tfBNC0cfWhMS0Rz0QWCF/DVrYad6GVtubcWiUsI7AweODR8deovllxm0Gq5beOrTlJzMyDdd+0WGpqqgZQ86gudw6OltuGvLo7g0E8fnQwcm3x2MvkDt6qNyzQatEvQczSzBcQK8wGGZ2Sg4HmzX38Qj7deA0C5cjV+GPP0Vet6f2pGU6UX2psCQu2HhxS/TyQaJhCkbpKCewAMrvRDNEqTmm9EUMpCe+g3akgaxORvfP+z76OLVVRNLSi5eKBSqU0X8HRu7hgbfG40EfS6v5IM8eQh33H4K0up1rGkuaHIRkqRBaFShLN6HS5k9zo/nz0719vXtsCwrSzZ2dq7/MHrkSlvbWuL1BzA7cQYNDVFnzZ0hthIWCCeAOGy8LQrLjiAjP4/pWMx6dtczXUzFONl679bQ3v69CdM065dVE67cObS3zMDbFKi29a9w8wZypTAS+fsxO3tl/uDAwbsNw8iQymV3d/e21nBkdXLm9IYnNpVfLqqBykpU1pKp4MCxvXARh0ykxEHaeE8s9vNPc2NjY5WC4k8Fe40xy7Y11gAAAABJRU5ErkJggg=="),
					Currency = pln
				},
				new Account {
					Name = "Wallet(EUR)",
					Icon = Convert.FromBase64String(@"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAADXklEQVR42m1Ta2xTZRh+vnNO23Paba3d2OrosAwz8DLDRYWoPzRmRv5pgol4YQkxGi/JJF5iNsYPExKTQTTI/qDNxOGikEgEFkM0KkPn4lgkZYOmMh12tevanp1u7ek537n5tVS8xDd5/nzf+z3f87wXctt36ubmoLizvR6b6nmyjrMhlmwnn6aIz63Y37vz6ide08y66wOI9T6O3A9f4J9Buk4vKAql/mxRR9EkMG12aFsQOA4UBOt9dYMdfv8r84kLGN//JGgu9W+CV6PzS6qpBVY0EyVNR5k6MB0bvOBBUHLjllDL0B8z53ePHt4NRcngv0HeOJnJcjzfZBiAQ2z2pwNCOPCcA170IJWYjp7ofey58koW/xfkwbOLX0eCzsMRvwSpwQ3esmDYBIpMMacJ+Pbtl/Ytjn58hOV6GUyGEoPCYFcJth9PLSTTuRZlXoZZ0gGDwHATUCZfaGuHODl8PH2sbx/LrWPgGCpSfr9BsOeDVN4UnKBJTRR1HZRa0B03PCKwtrEOs+OfnTlxuOe1GgH7AXM1FdctvD6aTXoCQpi3OFisBZaLh6Vb6Fy+gA2tIhpXhfX+/r6BkZGRYZafr+HvGmw+tfBNC0cfWhMS0Rz0QWCF/DVrYad6GVtubcWiUsI7AweODR8deovllxm0Gq5beOrTlJzMyDdd+0WGpqqgZQ86gudw6OltuGvLo7g0E8fnQwcm3x2MvkDt6qNyzQatEvQczSzBcQK8wGGZ2Sg4HmzX38Qj7deA0C5cjV+GPP0Vet6f2pGU6UX2psCQu2HhxS/TyQaJhCkbpKCewAMrvRDNEqTmm9EUMpCe+g3akgaxORvfP+z76OLVVRNLSi5eKBSqU0X8HRu7hgbfG40EfS6v5IM8eQh33H4K0up1rGkuaHIRkqRBaFShLN6HS5k9zo/nz0719vXtsCwrSzZ2dq7/MHrkSlvbWuL1BzA7cQYNDVFnzZ0hthIWCCeAOGy8LQrLjiAjP4/pWMx6dtczXUzFONl679bQ3v69CdM065dVE67cObS3zMDbFKi29a9w8wZypTAS+fsxO3tl/uDAwbsNw8iQymV3d/e21nBkdXLm9IYnNpVfLqqBykpU1pKp4MCxvXARh0ykxEHaeE8s9vNPc2NjY5WC4k8Fe40xy7Y11gAAAABJRU5ErkJggg=="),
					Currency = eur
				},
				new Account {
					Name = "Wallet",
					Icon = Convert.FromBase64String(@"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAADXklEQVR42m1Ta2xTZRh+vnNO23Paba3d2OrosAwz8DLDRYWoPzRmRv5pgol4YQkxGi/JJF5iNsYPExKTQTTI/qDNxOGikEgEFkM0KkPn4lgkZYOmMh12tevanp1u7ek537n5tVS8xDd5/nzf+z3f87wXctt36ubmoLizvR6b6nmyjrMhlmwnn6aIz63Y37vz6ide08y66wOI9T6O3A9f4J9Buk4vKAql/mxRR9EkMG12aFsQOA4UBOt9dYMdfv8r84kLGN//JGgu9W+CV6PzS6qpBVY0EyVNR5k6MB0bvOBBUHLjllDL0B8z53ePHt4NRcngv0HeOJnJcjzfZBiAQ2z2pwNCOPCcA170IJWYjp7ofey58koW/xfkwbOLX0eCzsMRvwSpwQ3esmDYBIpMMacJ+Pbtl/Ytjn58hOV6GUyGEoPCYFcJth9PLSTTuRZlXoZZ0gGDwHATUCZfaGuHODl8PH2sbx/LrWPgGCpSfr9BsOeDVN4UnKBJTRR1HZRa0B03PCKwtrEOs+OfnTlxuOe1GgH7AXM1FdctvD6aTXoCQpi3OFisBZaLh6Vb6Fy+gA2tIhpXhfX+/r6BkZGRYZafr+HvGmw+tfBNC0cfWhMS0Rz0QWCF/DVrYad6GVtubcWiUsI7AweODR8deovllxm0Gq5beOrTlJzMyDdd+0WGpqqgZQ86gudw6OltuGvLo7g0E8fnQwcm3x2MvkDt6qNyzQatEvQczSzBcQK8wGGZ2Sg4HmzX38Qj7deA0C5cjV+GPP0Vet6f2pGU6UX2psCQu2HhxS/TyQaJhCkbpKCewAMrvRDNEqTmm9EUMpCe+g3akgaxORvfP+z76OLVVRNLSi5eKBSqU0X8HRu7hgbfG40EfS6v5IM8eQh33H4K0up1rGkuaHIRkqRBaFShLN6HS5k9zo/nz0719vXtsCwrSzZ2dq7/MHrkSlvbWuL1BzA7cQYNDVFnzZ0hthIWCCeAOGy8LQrLjiAjP4/pWMx6dtczXUzFONl679bQ3v69CdM065dVE67cObS3zMDbFKi29a9w8wZypTAS+fsxO3tl/uDAwbsNw8iQymV3d/e21nBkdXLm9IYnNpVfLqqBykpU1pKp4MCxvXARh0ykxEHaeE8s9vNPc2NjY5WC4k8Fe40xy7Y11gAAAABJRU5ErkJggg=="),
					Currency = uah
				},
				privatBank,
				new Account {
					Name = "PBCreditCard",
					ParentAccount = privatBank,
					Currency = uah
				},
				new Account {
					Name = "PBDebitCard",
					ParentAccount = privatBank,
					Currency = uah
				}
			};
			context.Accounts.ToList().ForEach(x => context.Accounts.Remove(x));
			accounts.ForEach(account => context.Accounts.AddOrUpdate(p => p.Id, account));
			context.SaveChanges();
			var categories = new List<Category> {
				new Category {
					Name = "Salary",
					DirectionType = DirectionType.Incoming
				},
				new Category {
					Name = "Flat",
					DirectionType = DirectionType.Expense
				},
				new Category {
					Name = "Food",
					DirectionType = DirectionType.Expense
				}
			};
			context.Categories.ToList().ForEach(x => context.Categories.Remove(x));
			categories.ForEach(category => context.Categories.AddOrUpdate(p => p.Id, category));
			context.SaveChanges();
			var transactions = new List<Transaction> {
				new Transaction {
					Comment = "Salary",
					Category = categories.First(x => x.Name == "Salary"),
					Amount = 1000,
					Account = accounts.First(x => x.Name == "Wallet")
				},
				new Transaction {
					Comment = "bigPocket",
					Category = categories.First(x => x.Name == "Food"),
					Amount = 500,
					Account = accounts.First(x => x.Name == "Wallet")
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