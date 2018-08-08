namespace MyWallet.WebUI.Tests.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Mvc;
	using Domain.Abstract;
	using Domain.Entities;
	using Domain.Tests.Common;
	using Domain.Type;
	using FluentAssertions;
	using Moq;
	using WebUI.Controllers;
	using WebUI.Models;
	using Xunit;

	#region Class: TransactionControllerTestCase

	public class TransactionControllerTestCase
	{

		#region Setup/Teardown

		public TransactionControllerTestCase() {
			MockTransactionRepository = new Mock<ITransactionRepository>();
			MockCategoryRepository = new Mock<ICategoryRepository>();
			MockAccountRepository = new Mock<IAccountRepository>();
			Controller = new TransactionController(MockCategoryRepository.Object, MockAccountRepository.Object,
				MockTransactionRepository.Object);
		}

		#endregion

		protected readonly TransactionController Controller;

		protected readonly Mock<ITransactionRepository> MockTransactionRepository;

		protected readonly Mock<ICategoryRepository> MockCategoryRepository;

		protected readonly Mock<IAccountRepository> MockAccountRepository;

		[Fact]
		public void List_ReturnsExpectedCollectionOfItems_WhenCall() {
			// Arrange
			const string dateFormat = "yyyy.MM.dd";
			var fakeList = new[] {
				TestObjectCreator.CreateTransaction(),
				TestObjectCreator.CreateTransaction(),
				TestObjectCreator.CreateTransaction()
			};
			MockTransactionRepository
				.Setup(m => m.GetAll())
				.Returns(fakeList);

			// Act
			var model = (List<TransactionViewModel>)Controller.List().Model;

			// Assert
			model.Should().HaveCount(3);
			var expectedTransaction = fakeList[0];
			var item1 = model.First(x => x.Id == expectedTransaction.Id);
			item1.Amount.Should().Be(expectedTransaction.Amount);
			item1.AccountName.Should().Be(expectedTransaction.Account.Name);
			item1.CategoryName.Should().Be(expectedTransaction.Category.Name);
			item1.Comment.Should().Be(expectedTransaction.Comment);
			item1.DateIn.Should().Be(expectedTransaction.DateIn.ToString(dateFormat));
			expectedTransaction = fakeList[1];
			var item2 = model.First(x => x.Id == expectedTransaction.Id);
			item2.Amount.Should().Be(expectedTransaction.Amount);
			item2.AccountName.Should().Be(expectedTransaction.Account.Name);
			item2.CategoryName.Should().Be(expectedTransaction.Category.Name);
			item2.Comment.Should().Be(expectedTransaction.Comment);
			item2.DateIn.Should().Be(expectedTransaction.DateIn.ToString(dateFormat));
			expectedTransaction = fakeList[2];
			var item3 = model.First(x => x.Id == expectedTransaction.Id);
			item3.Amount.Should().Be(expectedTransaction.Amount);
			item3.AccountName.Should().Be(expectedTransaction.Account.Name);
			item3.CategoryName.Should().Be(expectedTransaction.Category.Name);
			item3.Comment.Should().Be(expectedTransaction.Comment);
			item3.DateIn.Should().Be(expectedTransaction.DateIn.ToString(dateFormat));
		}

		[Fact]
		public void Create_ReturnsExpectedResult_WhenCall() {
			// Arrange
			var fakeAccounts = new[] {
				TestObjectCreator.CreateAccount(),
				TestObjectCreator.CreateAccount()
			};
			MockAccountRepository
				.Setup(m => m.GetAll())
				.Returns(fakeAccounts);
			var fakeCategories = new[] {
				TestObjectCreator.CreateCategory(),
				TestObjectCreator.CreateCategory()
			};
			MockCategoryRepository
				.Setup(m => m.GetAll())
				.Returns(fakeCategories);

			// Act
			var model = (Transaction)Controller.Create().Model;

			// Assert
			model.Should()
				.NotBeNull();
			model.Id.Should().BeEmpty();
			SelectList accounts = Controller.ViewBag.Accounts;
			accounts.Should().NotBeNull();
			var expectedAcountsCount = fakeAccounts.Length + 1;
			accounts.Items.Should().HaveCount(expectedAcountsCount);
			SelectList categories = Controller.ViewBag.Categories;
			categories.Should().NotBeNull();
			var expectedCategoriesCount = fakeCategories.Length + 1;
			categories.Items.Should().HaveCount(expectedCategoriesCount);
		}

		[Fact]
		public void Edit_CanEdit_WhenTransactionExists() {
			// Arrange
			var transactionId = Guid.NewGuid();
			var transaction = TestObjectCreator.CreateTransaction(transactionId);
			MockTransactionRepository
				.Setup(m => m.GetById(transactionId))
				.Returns(transaction);
			var fakeAccounts = new[] {
				TestObjectCreator.CreateAccount(),
				TestObjectCreator.CreateAccount()
			};
			MockAccountRepository
				.Setup(m => m.GetAll())
				.Returns(fakeAccounts);
			var fakeCategories = new[] {
				TestObjectCreator.CreateCategory(),
				TestObjectCreator.CreateCategory()
			};
			MockCategoryRepository
				.Setup(m => m.GetAll())
				.Returns(fakeCategories);

			// Act
			var model = ((ViewResult)Controller.Edit(transactionId)).Model;

			// Assert
			model.Should()
				.NotBeNull().And
				.Be(transaction);
			SelectList accounts = Controller.ViewBag.Accounts;
			accounts.Should().NotBeNull();
			var expectedAcountsCount = fakeAccounts.Length + 1;
			accounts.Items.Should().HaveCount(expectedAcountsCount);
			SelectList categories = Controller.ViewBag.Categories;
			categories.Should().NotBeNull();
			var expectedCategoriesCount = fakeCategories.Length + 1;
			categories.Items.Should().HaveCount(expectedCategoriesCount);
		}

		[Fact]
		public void Edit_CanNotEdit_WhenTransactionNotExists() {
			// Arrange
			var unknownTransactionId = Guid.NewGuid();
			var fakeList = new[] {
				TestObjectCreator.CreateTransaction(),
				TestObjectCreator.CreateTransaction()
			};
			MockTransactionRepository
				.Setup(m => m.GetAll())
				.Returns(fakeList);

			// Act
			var model = Controller.Edit(unknownTransactionId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<HttpNotFoundResult>();
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndSave_WhenTransactionIsValid() {
			// Arrange
			var transaction = TestObjectCreator.CreateTransaction();
			var savedTransaction = new Transaction();
			MockTransactionRepository
				.Setup(h => h.Save(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<Guid>(), It.IsAny<Guid>(),
					It.IsAny<byte>(), It.IsAny<bool>()))
				.Callback<Guid, string, decimal, Guid, Guid, byte, bool>((i, c, a, ai, ci, s, nc) =>
					savedTransaction = new Transaction {
						AccountId = ai,
						Amount = a,
						CategoryId = ci,
						Comment = c,
						Id = i,
						RowState = s,
						NeedConfirm = nc
					});

			// Act
			var model = Controller.Edit(transaction, transaction.CategoryId, transaction.AccountId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			MockTransactionRepository
				.Verify(x => x.Save(transaction.Id, transaction.Comment, transaction.Amount, transaction.AccountId,
					transaction.CategoryId, transaction.RowState, transaction.NeedConfirm));
			savedTransaction.AccountId.Should().Be(transaction.AccountId);
			savedTransaction.CategoryId.Should().Be(transaction.CategoryId);
			savedTransaction.Amount.Should().Be(transaction.Amount);
			savedTransaction.Comment.Should().Be(transaction.Comment);
			savedTransaction.Id.Should().Be(transaction.Id);
			savedTransaction.RowState.Should().Be(transaction.RowState);
			savedTransaction.NeedConfirm.Should().BeFalse();
		}

		[Fact]
		public void Edit_SaveTransactionWithStateNeedConfirm_WhenTransactionIsWithDateInFuture() {
			// Arrange
			var transaction = TestObjectCreator.CreateTransaction();
			transaction.DateIn = DateTime.Now.AddDays(2);
			var savedTransaction = new Transaction();
			MockTransactionRepository
				.Setup(h => h.Save(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<Guid>(), It.IsAny<Guid>(),
					It.IsAny<byte>(), It.IsAny<bool>()))
				.Callback<Guid, string, decimal, Guid, Guid, byte, bool>((i, c, a, ai, ci, s, nc) =>
					savedTransaction = new Transaction {
						AccountId = ai,
						Amount = a,
						CategoryId = ci,
						Comment = c,
						Id = i,
						RowState = s,
						NeedConfirm = nc
					});

			// Act
			var model = Controller.Edit(transaction, transaction.CategoryId, transaction.AccountId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			savedTransaction.NeedConfirm.Should().BeTrue();
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndSetAccountAndSave_WhenChangeAccount() {
			// Arrange
			var transaction = TestObjectCreator.CreateTransaction();
			var account = TestObjectCreator.CreateAccount();
			MockAccountRepository
				.Setup(m => m.GetById(account.Id))
				.Returns(account);

			// Act
			var model = Controller.Edit(transaction, transaction.CategoryId, account.Id);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			MockTransactionRepository
				.Verify(x => x.Save(transaction.Id, transaction.Comment, transaction.Amount, account.Id,
					transaction.CategoryId, transaction.RowState, transaction.NeedConfirm));
			transaction.Account.Should().Be(account);
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndResetAccountAndSave_WhenResetAccount() {
			// Arrange
			var transaction = TestObjectCreator.CreateTransaction();
			var accountId = Guid.Empty;
			MockAccountRepository
				.Setup(m => m.GetById(accountId))
				.Returns((Account)null);

			// Act
			var model = Controller.Edit(transaction, transaction.CategoryId, accountId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			MockTransactionRepository
				.Verify(x => x.Save(transaction.Id, transaction.Comment, transaction.Amount, transaction.AccountId,
					transaction.CategoryId, transaction.RowState, transaction.NeedConfirm));
			transaction.Account.Should().BeNull();
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndSetCategoryAndSave_WhenChangeCategory() {
			// Arrange
			var transaction = TestObjectCreator.CreateTransaction();
			var category = TestObjectCreator.CreateCategory();
			MockCategoryRepository
				.Setup(m => m.GetById(category.Id))
				.Returns(category);

			// Act
			var model = Controller.Edit(transaction, category.Id, transaction.AccountId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			MockTransactionRepository
				.Verify(x => x.Save(transaction.Id, transaction.Comment, transaction.Amount, transaction.AccountId,
					category.Id, transaction.RowState, transaction.NeedConfirm));
			transaction.Category.Should().Be(category);
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndResetCategoryAndSave_WhenResetCategory() {
			// Arrange
			var transaction = TestObjectCreator.CreateTransaction();
			var categoryId = Guid.Empty;
			MockCategoryRepository
				.Setup(m => m.GetById(categoryId))
				.Returns((Category)null);

			// Act
			var model = Controller.Edit(transaction, categoryId, transaction.AccountId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			MockTransactionRepository
				.Verify(x => x.Save(transaction.Id, transaction.Comment, transaction.Amount, transaction.AccountId,
					transaction.CategoryId, transaction.RowState, transaction.NeedConfirm));
			transaction.Category.Should().BeNull();
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndNotSave_WhenTransactionIsNotValid() {
			// Arrange
			var transaction = TestObjectCreator.CreateTransaction();
			Controller.ModelState.AddModelError("error", @"error");

			// Act
			var model = Controller.Edit(transaction, transaction.CategoryId, transaction.AccountId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<ViewResult>();
			MockTransactionRepository
				.Verify(x => x.Save(transaction.Id, transaction.Comment, transaction.Amount, transaction.AccountId,
					transaction.CategoryId, transaction.RowState, transaction.NeedConfirm), Times.Never);
		}

		[Fact]
		public void Delete_ReturnExpectedResultAndSaveWithDeletedState_WhenTransactionIsExists() {
			// Arrange
			var transactionId = Guid.NewGuid();
			var transaction = TestObjectCreator.CreateTransaction(transactionId);
			MockTransactionRepository
				.Setup(m => m.GetById(transactionId))
				.Returns(transaction);

			// Act
			var model = Controller.Delete(transactionId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			MockTransactionRepository
				.Verify(x => x.Save(transaction.Id, transaction.Comment, transaction.Amount, transaction.AccountId,
					transaction.CategoryId, transaction.RowState, transaction.NeedConfirm), Times.Once);
			((RowState)transaction.RowState).Should().Be(RowState.Deleted);
		}

		[Fact]
		public void Delete_ReturnExpectedResult_WhenTransactionIsNotExists() {
			// Arrange
			var unknownTransactionId = Guid.NewGuid();
			MockTransactionRepository
				.Setup(m => m.GetById(unknownTransactionId))
				.Returns((Transaction)null);

			// Act
			var result = Controller.Delete(unknownTransactionId);

			// Assert
			result.Should()
				.NotBeNull().And
				.BeOfType<HttpNotFoundResult>();
			MockTransactionRepository
				.Verify(x => x.Save(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<Guid>(), It.IsAny<Guid>(),
					It.IsAny<byte>(), It.IsAny<bool>()), Times.Never);
		}

		[Fact]
		public void Delete_ReturnExpectedResult_WhenIdIsWrong() {
			// Act
			var result = Controller.Delete(null);

			// Assert
			result.Should()
				.NotBeNull().And
				.BeOfType<HttpStatusCodeResult>();
			((HttpStatusCodeResult)result).StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
			MockTransactionRepository
				.Verify(x => x.Save(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<Guid>(), It.IsAny<Guid>(),
					It.IsAny<byte>(), It.IsAny<bool>()), Times.Never);
		}

		[Fact]
		public void GetListOfPopularAccounts_ReturnsFourLastTransactionAccounts_WhenCall() {
			// Arrange
			var transaction1 = TestObjectCreator.CreateTransaction();
			transaction1.ModifiedOn = DateTime.Now.AddDays(-1);
			var transaction2 = TestObjectCreator.CreateTransaction();
			transaction2.ModifiedOn = DateTime.Now.AddDays(-2);
			var transaction3 = TestObjectCreator.CreateTransaction();
			transaction3.ModifiedOn = DateTime.Now.AddDays(-3);
			var fakeList = new[] {
				transaction1, transaction2, transaction3
			};
			MockTransactionRepository
				.Setup(m => m.GetAll())
				.Returns(fakeList);

			// Act
			var result = Controller.GetListOfPopularAccounts().Data;
			var model = (List<Guid>)result;

			// Assert
			model.Should()
				.HaveCount(2).And
				.Contain(transaction1.AccountId).And
				.Contain(transaction2.AccountId);
		}

		[Fact]
		public void GetListOfPopularAccounts_ReturnsEmptyList_WhenNoData() {
			// Arrange
			MockTransactionRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Transaction>());

			// Act
			var result = Controller.GetListOfPopularAccounts().Data;
			var model = (List<Guid>)result;

			// Assert
			model.Should().BeEmpty();
		}


		[Fact]
		public void GetListOfPopularCategories_ReturnsFourLastTransactionCategories_WhenCall() {
			// Arrange
			var transaction1 = TestObjectCreator.CreateTransaction();
			transaction1.ModifiedOn = DateTime.Now.AddDays(-1);
			var transaction2 = TestObjectCreator.CreateTransaction();
			transaction2.ModifiedOn = DateTime.Now.AddDays(-2);
			var transaction3 = TestObjectCreator.CreateTransaction();
			transaction3.ModifiedOn = DateTime.Now.AddDays(-3);
			var transaction4 = TestObjectCreator.CreateTransaction();
			transaction4.ModifiedOn = DateTime.Now.AddDays(-4);
			var transaction5 = TestObjectCreator.CreateTransaction();
			transaction5.ModifiedOn = DateTime.Now.AddDays(-5);
			var fakeList = new[] {
				transaction1, transaction2, transaction3, transaction4, transaction5
			};
			MockTransactionRepository
				.Setup(m => m.GetAll())
				.Returns(fakeList);

			// Act
			var result = Controller.GetListOfPopularCategories().Data;
			var model = (List<Guid>) result;

			// Assert
			model.Should()
				.HaveCount(4).And
				.Contain(transaction1.CategoryId).And
				.Contain(transaction2.CategoryId).And
				.Contain(transaction3.CategoryId).And
				.Contain(transaction4.CategoryId);
		}

		[Fact]
		public void GetListOfPopularCategories_ReturnsEmptyList_WhenNoData() {
			// Arrange
			MockTransactionRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Transaction>());

			// Act
			var result = Controller.GetListOfPopularCategories().Data;
			var model = (List<Guid>)result;

			// Assert
			model.Should().BeEmpty();
		}

		[Fact]
		public void GetLastModifiedTransactionDateIn_ReturnsTransactionDateInWhichWasLastModified_WhenCall() {
			// Arrange
			var transaction1 = TestObjectCreator.CreateTransaction();
			transaction1.DateIn = DateTime.Now.AddDays(-1);
			transaction1.ModifiedOn = DateTime.Now.AddDays(-2);
			var transaction2 = TestObjectCreator.CreateTransaction();
			transaction2.DateIn = DateTime.Now.AddDays(-2);
			transaction2.ModifiedOn = DateTime.Now.AddDays(-1);
			var fakeList = new[] {
				transaction1, transaction2
			};
			MockTransactionRepository
				.Setup(m => m.GetAll())
				.Returns(fakeList);

			// Act
			var result = Controller.GetLastModifiedTransactionDateIn().Data;
			var model = (DateTime)result;

			// Assert
			model.Should().Be(transaction2.DateIn);
		}

		[Fact]
		public void GetLastModifiedTransactionDateIn_ReturnsNull_WhenNoData() {
			// Arrange
			MockTransactionRepository
				.Setup(m => m.GetAll())
				.Returns(new List<Transaction>());

			// Act
			var result = Controller.GetLastModifiedTransactionDateIn().Data;
			var model = (DateTime?)result;

			// Assert
			model.Should().BeNull();
		}

	}

	#endregion

}