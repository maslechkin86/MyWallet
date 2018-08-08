namespace MyWallet.Domain.Tests.Concrete
{
	using System;
	using Abstract;
	using Common;
	using Domain.Concrete;
	using Domain.Entities;
	using FluentAssertions;
	using Moq;
	using Type;
	using Xunit;

	#region Class: AccountRepositoryTestCase

	public class AccountRepositoryTestCase
	{

		#region Setup/Teardown

		public AccountRepositoryTestCase() {
			MockDbContext = new Mock<IMyWalletDbContext>();
			Repository = new AccountRepository(MockDbContext.Object);
		}

		#endregion

		protected Mock<IMyWalletDbContext> MockDbContext;

		protected AccountRepository Repository;

		[Fact]
		public void Dispose_CallDisposeOfDbContext_WhenCall() {
			// Act
			Repository.Dispose();

			// Assert
			MockDbContext.Verify(x => x.Dispose());
		}

		[Fact]
		public void GetById_ReturnExpectedItem_WhenAccountExists() {
			// Arrange
			var account = TestObjectCreator.CreateAccount();
			MockDbContext
				.Setup(x => x.Accounts)
				.Returns(new FakeDbSet<Account> { account });

			// Act
			var items = Repository.GetById(account.Id);

			// Assert
			items.Should()
				.NotBeNull()
				.And
				.Be(account);
		}

		[Fact]
		public void GetById_ReturnNull_WhenAccountNotExists() {
			// Arrange
			var unknownAccountId = Guid.NewGuid();
			MockDbContext
				.Setup(x => x.Accounts)
				.Returns(new FakeDbSet<Account>());

			// Act
			var items = Repository.GetById(unknownAccountId);

			// Assert
			items.Should()
				.BeNull();
		}

		[Fact]
		public void GetAll_ReturnExpectedItems_WhenCall() {
			// Arrange
			var account1 = TestObjectCreator.CreateAccount();
			var account2 = TestObjectCreator.CreateAccount();
			MockDbContext
				.Setup(x => x.Accounts)
				.Returns(new FakeDbSet<Account> {
					account1,
					account2
				});

			// Act
			var items = Repository.GetAll();

			// Assert
			items.Should()
				.HaveCount(2).And
				.Contain(account1).And
				.Contain(account2);
		}

		[Fact]
		public void GetAll_ReturnOnlyItemsWithStateExisting_WhenExistsDeletedItemsCall() {
			// Arrange
			var account1 = TestObjectCreator.CreateAccount();
			var account2 = TestObjectCreator.CreateAccount();
			account2.RowState = (int)RowState.Deleted;
			var account3 = TestObjectCreator.CreateAccount();
			var account4 = TestObjectCreator.CreateAccount();
			MockDbContext
				.Setup(x => x.Accounts)
				.Returns(new FakeDbSet<Account> {
					account1,
					account2,
					account3,
					account4
				});

			// Act
			var items = Repository.GetAll();

			// Assert
			items.Should()
				.HaveCount(3).And
				.Contain(account1).And
				.Contain(account3).And
				.Contain(account4);
		}

		[Fact]
		public void Save_AddAccountAndSaveChanges_WhenItIsNewItem() {
			// Arrange
			var account = TestObjectCreator.CreateAccount(Guid.Empty);
			var fakeDbSet = new FakeDbSet<Account>();
			MockDbContext
				.Setup(x => x.Accounts)
				.Returns(fakeDbSet);

			// Act
			Repository.Save(account.Id, account.Name, account.ParentAccountId, account.CurrencyId, account.IconPath,
				account.RowState);

			// Assert
			fakeDbSet.Local.Should()
				.HaveCount(1);
			var item = fakeDbSet.Local[0];
			item.Name.Should().Be(account.Name);
			item.ParentAccountId.Should().Be(account.ParentAccountId);
			item.RowState.Should().Be(account.RowState);
			item.ModifiedOn.Date.Should().Be(DateTime.Now.Date);
			item.IconPath.Should().Be(account.IconPath);
			item.CurrencyId.Should().Be(account.CurrencyId);
			MockDbContext.Verify(x => x.SaveChanges());
		}

		[Fact]
		public void Save_UpdateAccountAndSaveChanges_WhenItIsExistsItem() {
			// Arrange
			var accountId = Guid.NewGuid();
			var fakeDbSet = new FakeDbSet<Account> {
				new Account {
					Id = accountId,
					Name = "name1",
					RowState = (int) RowState.Existing,
					CurrencyId = Guid.NewGuid(),
					IconPath = "IconPath",
					ModifiedOn = DateTime.MinValue
				}
			};
			MockDbContext
				.Setup(x => x.Accounts)
				.Returns(fakeDbSet);
			var parentAccount = TestObjectCreator.CreateAccount();
			var updateAccount = new Account {
				Id = accountId,
				Name = "updayteName",
				RowState = (int)RowState.Deleted,
				CurrencyId = Guid.NewGuid(),
				IconPath = "updayteIconPath",
				ParentAccount = parentAccount,
				ParentAccountId = parentAccount.Id
			};

			// Act
			Repository.Save(updateAccount.Id, updateAccount.Name, updateAccount.ParentAccountId, updateAccount.CurrencyId,
				updateAccount.IconPath, updateAccount.RowState);

			// Assert
			fakeDbSet.Local.Should()
				.HaveCount(1);
			var item = fakeDbSet.Local[0];
			item.Name.Should().Be(updateAccount.Name);
			item.ParentAccountId.Should().Be(updateAccount.ParentAccount.Id);
			item.RowState.Should().Be(updateAccount.RowState);
			item.ModifiedOn.Date.Should().Be(DateTime.Now.Date);
			item.IconPath.Should().Be(updateAccount.IconPath);
			item.CurrencyId.Should().Be(updateAccount.CurrencyId);
			MockDbContext.Verify(x => x.SaveChanges());
		}

	}

	#endregion

}
