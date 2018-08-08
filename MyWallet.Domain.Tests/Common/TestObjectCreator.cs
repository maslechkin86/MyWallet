namespace MyWallet.Domain.Tests.Common
{
	using System;
	using Domain.Entities;
	using Type;

	#region Class: TestObjectCreator

	public static class TestObjectCreator
	{

		#region Methods: Public

		public static Account CreateAccount(Guid? id = null, string name = null) {
			var account = new Account {
				Id = id ?? Guid.NewGuid(),
				IconPath = "IconPath",
				Name = name ?? "name",
				RowState = (int) RowState.Existing
			};
			return account;
		}

		public static Category CreateCategory(Guid? id = null, string name = null) {
			var category = new Category {
				Id = id ?? Guid.NewGuid(),
				Name = name ?? "name",
				IconPath = "IconPath",
				DirectionType = DirectionType.Incoming,
				RowState = (int)RowState.Existing
			};
			return category;
		}

		public static Transaction CreateTransaction(Guid? id = null) {
			var transaction = new Transaction {
				Id = id ?? Guid.NewGuid(),
				Amount = 100,
				Comment = "comment",
				Account = CreateAccount(),
				Category = CreateCategory()
			};
			return transaction;
		}

		#endregion

	}

	#endregion

}