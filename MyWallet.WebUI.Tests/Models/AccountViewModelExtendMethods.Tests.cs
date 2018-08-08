namespace MyWallet.WebUI.Tests.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Domain.Entities;
	using FluentAssertions;
	using WebUI.Models;
	using Xunit;

	#region Class: AccountViewModelExtendMethodsTestCase

	public class AccountViewModelExtendMethodsTestCase
	{

		[Fact]
		public void ToAccountViewModel_CreteExpectedObject_WhenAccountHaveParent() {
			// Arrange
			var account = new Account {
				Id = Guid.NewGuid(),
				Name = "subAccountName",
				ParentAccount = new Account {
					Id = Guid.NewGuid(),
					Name = "AccountName"
				}
			};

			// Act
			var viewModel = account.ToAccountViewModel();

			// Assert
			viewModel.Id.Should().Be(account.Id);
			viewModel.Name.Should().Be($"{account.ParentAccount.Name}: {account.Name}");
		}

		[Fact]
		public void ToAccountViewModel_CreteExpectedObject_WhenAccountHaveNotParent() {
			// Arrange
			var account = new Account {
				Id = Guid.NewGuid(),
				Name = "subAccountName",
				ParentAccount = null
			};

			// Act
			var viewModel = account.ToAccountViewModel();

			// Assert
			viewModel.Id.Should().Be(account.Id);
			viewModel.Name.Should().Be($"{account.Name}");
		}

		[Fact]
		public void ToAccountViewModelList_CreteExpectedCollection_WhenCall() {
			// Arrange
			var accounts = new List<Account> {
				new Account {
					Id = Guid.NewGuid(),
					Name = "subAccountName",
					ParentAccount = null
				},
				new Account {
					Id = Guid.NewGuid(),
					Name = "subAccountName",
					ParentAccount = null
				}
			};

			// Act
			var viewModel = accounts.ToAccountViewModelList();

			// Assert
			viewModel.Should()
				.NotBeNull().And
				.HaveCount(2);
			var expectedItem = accounts[0];
			var item1 = viewModel.First(x => x.Id == expectedItem.Id);
			item1.Id.Should().Be(expectedItem.Id);
			item1.Name.Should().Be(expectedItem.Name);
			expectedItem = accounts[1];
			var item2 = viewModel.First(x => x.Id == expectedItem.Id);
			item2.Id.Should().Be(expectedItem.Id);
			item2.Name.Should().Be(expectedItem.Name);
		}

	}

	#endregion

}
