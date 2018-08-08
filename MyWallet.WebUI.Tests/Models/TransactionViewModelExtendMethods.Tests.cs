namespace MyWallet.WebUI.Tests.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Domain.Entities;
	using Domain.Tests.Common;
	using FluentAssertions;
	using WebUI.Models;
	using Xunit;

	#region Class: TransactionViewModelExtendMethodsTestsCase

	public class TransactionViewModelExtendMethodsTestsCase
	{

		[Fact]
		public void ToTransactionViewModel_CreteExpectedObject_WhenCall() {
			// Arrange
			const string dateFormat = "yyyy.MM.dd";
			var transaction = new Transaction {
				Id = Guid.NewGuid(),
				Amount = 100,
				Comment = "comment",
				Account = TestObjectCreator.CreateAccount(),
				Category = TestObjectCreator.CreateCategory(),
				DateIn = DateTime.Today
			};

			// Act
			var viewModel = transaction.ToTransactionViewModel();

			// Assert
			viewModel.Id.Should().Be(transaction.Id);
			viewModel.Amount.Should().Be(transaction.Amount);
			viewModel.Comment.Should().Be(transaction.Comment);
			viewModel.AccountName.Should().Be(transaction.Account.Name);
			viewModel.CategoryName.Should().Be(transaction.Category.Name);
			viewModel.DateIn.Should().Be(transaction.DateIn.ToString(dateFormat));
		}

		[Fact]
		public void ToTransactionViewModelList_CreteExpectedCollection_WhenCall() {
			// Arrange
			var transactions = new List<Transaction> {
				new Transaction {
					Id = Guid.NewGuid(),
					Amount = 100,
					Comment = "comment1",
					Account = TestObjectCreator.CreateAccount(),
					Category = TestObjectCreator.CreateCategory()
				},
				new Transaction {
					Id = Guid.NewGuid(),
					Amount = 200,
					Comment = "comment2",
					Account = TestObjectCreator.CreateAccount(),
					Category = TestObjectCreator.CreateCategory()
				}
			};

			// Act
			var viewModel = transactions.ToTransactionViewModelList();

			// Assert
			viewModel.Should()
				.NotBeNull().And
				.HaveCount(2);
			var item1 = viewModel.First(x => x.Id == transactions[0].Id);
			item1.Id.Should().Be(transactions[0].Id);
			item1.Amount.Should().Be(transactions[0].Amount);
			item1.Comment.Should().Be(transactions[0].Comment);
			item1.AccountName.Should().Be(transactions[0].Account.Name);
			item1.CategoryName.Should().Be(transactions[0].Category.Name);
			var item2 = viewModel.First(x => x.Id == transactions[1].Id);
			item2.Id.Should().Be(transactions[1].Id);
			item2.Amount.Should().Be(transactions[1].Amount);
			item2.Comment.Should().Be(transactions[1].Comment);
			item2.AccountName.Should().Be(transactions[1].Account.Name);
			item2.CategoryName.Should().Be(transactions[1].Category.Name);
		}

	}

	#endregion

}
