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

	#region Class: TransactionRepositoryTestCase

	public class TransactionRepositoryTestCase
	{

		#region Setup/Teardown

		public TransactionRepositoryTestCase() {
			MockDbContext = new Mock<IMyWalletDbContext>();
			Repository = new TransactionRepository(MockDbContext.Object);
		}

		#endregion

		protected Mock<IMyWalletDbContext> MockDbContext;

		protected TransactionRepository Repository;

		[Fact]
		public void Dispose_CallDisposeOfDbContext_WhenCall() {
			// Act
			Repository.Dispose();

			// Assert
			MockDbContext.Verify(x => x.Dispose());
		}

		[Fact]
		public void GetById_ReturnExpectedItem_WhenTransactionExists() {
			// Arrange
			var transaction = new Transaction {
				Id = Guid.NewGuid(),
				Comment = "comment",
				Amount = 100,
				Account = TestObjectCreator.CreateAccount(),
				Category = TestObjectCreator.CreateCategory()
			};
			MockDbContext
				.Setup(x => x.Transactions)
				.Returns(new FakeDbSet<Transaction> { transaction });

			// Act
			var items = Repository.GetById(transaction.Id);

			// Assert
			items.Should()
				.NotBeNull()
				.And
				.Be(transaction);
		}

		[Fact]
		public void GetById_ReturnNull_WhenTransactionNotExists() {
			// Arrange
			var unknownTransactionId = Guid.NewGuid();
			MockDbContext
				.Setup(x => x.Transactions)
				.Returns(new FakeDbSet<Transaction>());

			// Act
			var items = Repository.GetById(unknownTransactionId);

			// Assert
			items.Should()
				.BeNull();
		}

		[Fact]
		public void GetAll_ReturnExpectedItems_WhenCall() {
			// Arrange
			var transaction1 = new Transaction {
				Id = Guid.NewGuid(),
				Comment = "comment1",
				Amount = 100,
				Account = TestObjectCreator.CreateAccount(),
				Category = TestObjectCreator.CreateCategory()
			};
			var transaction2 = new Transaction {
				Id = Guid.NewGuid(),
				Comment = "comment2",
				Amount = 200,
				Account = TestObjectCreator.CreateAccount(),
				Category = TestObjectCreator.CreateCategory()
			};
			MockDbContext
				.Setup(x => x.Transactions)
				.Returns(new FakeDbSet<Transaction> {
					transaction1,
					transaction2
				});

			// Act
			var items = Repository.GetAll();

			// Assert
			items.Should()
				.HaveCount(2).And
				.Contain(transaction1).And
				.Contain(transaction2);
		}

		[Fact]
		public void GetAll_ReturnOnlyItemsWithStateExisting_WhenExistsDeletedItemsCall() {
			// Arrange
			var transaction1 = TestObjectCreator.CreateTransaction();
			var transaction2 = TestObjectCreator.CreateTransaction();
			transaction2.RowState = (int)RowState.Deleted;
			var transaction3 = TestObjectCreator.CreateTransaction();
			var transaction4 = TestObjectCreator.CreateTransaction();
			MockDbContext
				.Setup(x => x.Transactions)
				.Returns(new FakeDbSet<Transaction> {
					transaction1,
					transaction2,
					transaction3,
					transaction4
				});

			// Act
			var items = Repository.GetAll();

			// Assert
			items.Should()
				.HaveCount(3).And
				.Contain(transaction1).And
				.Contain(transaction3).And
				.Contain(transaction4);
		}

		[Fact]
		public void Save_AddTransactionAndSaveChanges_WhenItIsNewItem() {
			// Arrange
			var transaction = new Transaction {
				Id = Guid.Empty,
				Comment = "comment1",
				Amount = 100,
				Account = TestObjectCreator.CreateAccount(),
				Category = TestObjectCreator.CreateCategory()
			};
			var fakeDbSet = new FakeDbSet<Transaction>();
			MockDbContext
				.Setup(x => x.Transactions)
				.Returns(fakeDbSet);

			// Act
			Repository.Save(transaction.Id, transaction.Comment, transaction.Amount, transaction.AccountId,
				transaction.CategoryId, transaction.RowState, transaction.NeedConfirm);

			// Assert
			fakeDbSet.Local.Should()
				.HaveCount(1);
			fakeDbSet.Local[0].AccountId.Should().Be(transaction.AccountId);
			fakeDbSet.Local[0].CategoryId.Should().Be(transaction.CategoryId);
			fakeDbSet.Local[0].Amount.Should().Be(transaction.Amount);
			fakeDbSet.Local[0].Comment.Should().Be(transaction.Comment);
			fakeDbSet.Local[0].Id.Should().NotBeEmpty();
			fakeDbSet.Local[0].RowState.Should().Be(transaction.RowState);
			fakeDbSet.Local[0].NeedConfirm.Should().Be(transaction.NeedConfirm);
			MockDbContext.Verify(x => x.SaveChanges());
		}

		[Fact]
		public void Save_UpdateTransactionAndSaveChanges_WhenItIsExistsItem() {
			// Arrange
			var transactionId = Guid.NewGuid();
			var fakeDbAccount = new FakeDbSet<Account> {
				TestObjectCreator.CreateAccount(),
				TestObjectCreator.CreateAccount()
			};
			MockDbContext
				.Setup(x => x.Accounts)
				.Returns(fakeDbAccount);
			var fakeDbCategory = new FakeDbSet<Category> {
				TestObjectCreator.CreateCategory(),
				TestObjectCreator.CreateCategory()
			};
			MockDbContext
				.Setup(x => x.Categories)
				.Returns(fakeDbCategory);
			var fakeDbSet = new FakeDbSet<Transaction> {
				new Transaction {
					Id = transactionId,
					Comment = "comment",
					Amount = 100,
					AccountId = fakeDbAccount.Local[0].Id,
					CategoryId = fakeDbCategory.Local[0].Id
				}
			};
			MockDbContext
				.Setup(x => x.Transactions)
				.Returns(fakeDbSet);
			var updateTransaction = new Transaction {
				Id = transactionId,
				Comment = "updayteComment",
				Amount = 200,
				AccountId = fakeDbAccount.Local[1].Id,
				CategoryId = fakeDbCategory.Local[1].Id,
				RowState = (int)RowState.Deleted
			};

			// Act
			Repository.Save(updateTransaction.Id, updateTransaction.Comment, updateTransaction.Amount,
				updateTransaction.AccountId, updateTransaction.CategoryId, updateTransaction.RowState, updateTransaction.NeedConfirm);

			// Assert
			fakeDbSet.Local.Should()
				.HaveCount(1);
			var item = fakeDbSet.Local[0];
			item.Comment.Should().Be(updateTransaction.Comment);
			item.Amount.Should().Be(updateTransaction.Amount);
			item.AccountId.Should().Be(updateTransaction.AccountId);
			item.CategoryId.Should().Be(updateTransaction.CategoryId);
			item.RowState.Should().Be(updateTransaction.RowState);
			item.ModifiedOn.Date.Should().Be(DateTime.Now.Date);
			MockDbContext.Verify(x => x.SaveChanges());
		}

	}

	#endregion

}
