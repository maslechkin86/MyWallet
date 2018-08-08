namespace MyWallet.WebUI.Tests.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Web.Mvc;
	using System.Web;
	using System.Web.Routing;
	using Domain.Abstract;
	using Domain.Entities;
	using Domain.Tests.Common;
	using Domain.Type;
	using FluentAssertions;
	using Moq;
	using WebUI.Controllers;
	using WebUI.Models;
	using Xunit;

	#region Class: AccountControllerTestCase

	public class AccountControllerTestCase
	{

		#region Setup/Teardown

		public AccountControllerTestCase() {
			MockAccountRepository = new Mock<IAccountRepository>();
			Controller = new AccountController(MockAccountRepository.Object);
		}

		#endregion

		protected readonly AccountController Controller;

		protected readonly Mock<IAccountRepository> MockAccountRepository;

		[Fact]
		public void List_ReturnsExpectedCollectionOfItems_WhenCall() {
			// Arrange
			var fakeList = new[] {
				TestObjectCreator.CreateAccount(name: "C"),
				TestObjectCreator.CreateAccount(name: "B"),
				TestObjectCreator.CreateAccount(name: "A")
			};
			MockAccountRepository
				.Setup(m => m.GetAll())
				.Returns(fakeList);

			// Act
			var model = (List<AccountViewModel>)Controller.List().Model;

			// Assert
			model.Should()
				.HaveCount(3).And
				.BeInAscendingOrder(x => x.Name);
			var expectedTransaction = fakeList[0];
			var item1 = model.First(x => x.Id == expectedTransaction.Id);
			item1.Name.Should().Be(expectedTransaction.Name);
			expectedTransaction = fakeList[1];
			var item2 = model.First(x => x.Id == expectedTransaction.Id);
			item2.Name.Should().Be(expectedTransaction.Name);
			expectedTransaction = fakeList[2];
			var item3 = model.First(x => x.Id == expectedTransaction.Id);
			item3.Name.Should().Be(expectedTransaction.Name);
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

			// Act
			var model = (Account)Controller.Create().Model;

			// Assert
			model.Should()
				.NotBeNull();
			model.Id.Should().BeEmpty();
			SelectList accounts = Controller.ViewBag.Accounts;
			accounts.Should().NotBeNull();
			var expectedAcountsCount = fakeAccounts.Length + 1;
			accounts.Items.Should().HaveCount(expectedAcountsCount);
		}

		[Fact]
		public void Edit_CanEdit_WhenAccountExists() {
			// Arrange
			var accountId = Guid.NewGuid();
			var account = TestObjectCreator.CreateAccount(accountId);
			var fakeAccounts = new[] {
				account,
				TestObjectCreator.CreateAccount()
			};
			MockAccountRepository
				.Setup(m => m.GetAll())
				.Returns(fakeAccounts);
			MockAccountRepository
				.Setup(m => m.GetById(accountId))
				.Returns(account);

			// Act
			var model = ((ViewResult)Controller.Edit(accountId)).Model;

			// Assert
			model.Should()
				.NotBeNull().And
				.Be(account);
			SelectList accounts = Controller.ViewBag.Accounts;
			accounts.Should().NotBeNull();
			var expectedAcountsCount = fakeAccounts.Length + 1;
			accounts.Items.Should().HaveCount(expectedAcountsCount);
		}

		[Fact]
		public void Edit_CanNotEdit_WhenAccountNotExists() {
			// Arrange
			var unknownAccountId = Guid.NewGuid();
			MockAccountRepository
				.Setup(m => m.GetById(unknownAccountId))
				.Returns((Account)null);

			// Act
			var model = Controller.Edit(unknownAccountId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<HttpNotFoundResult>();
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndSave_WhenAccountIsValid() {
			// Arrange
			var account = TestObjectCreator.CreateAccount();
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var files = new Mock<HttpFileCollectionBase>();
			context.Setup(x => x.Request).Returns(request.Object);
			request.Setup(x => x.Files).Returns(files.Object);
			files.Setup(x => x["item-icon"]).Returns((HttpPostedFileBase)null);
			Controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), Controller);

			// Act
			var model = Controller.Edit(account, Guid.Empty);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			MockAccountRepository
				.Verify(x => x.Save(account.Id, account.Name, account.ParentAccountId, account.CurrencyId, account.IconPath,
					account.RowState));
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndSave_WhenChangeParentAccount() {
			// Arrange
			var parrentAccount = TestObjectCreator.CreateAccount();
			var account = TestObjectCreator.CreateAccount();
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var files = new Mock<HttpFileCollectionBase>();
			context.Setup(x => x.Request).Returns(request.Object);
			request.Setup(x => x.Files).Returns(files.Object);
			files.Setup(x => x["item-icon"]).Returns((HttpPostedFileBase)null);
			Controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), Controller);
			MockAccountRepository
				.Setup(m => m.GetById(parrentAccount.Id))
				.Returns(parrentAccount);

			// Act
			var model = Controller.Edit(account, parrentAccount.Id);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			MockAccountRepository
				.Verify(x => x.Save(account.Id, account.Name, account.ParentAccountId, account.CurrencyId, account.IconPath,
					account.RowState));
			account.ParentAccount.Should().Be(parrentAccount);
			account.ParentAccountId.Should().Be(parrentAccount.Id);
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndSave_WhenResetParentAccount() {
			// Arrange
			var parrentAccountId = Guid.Empty;
			var account = TestObjectCreator.CreateAccount();
			var context = new Mock<HttpContextBase>();
			var request = new Mock<HttpRequestBase>();
			var files = new Mock<HttpFileCollectionBase>();
			context.Setup(x => x.Request).Returns(request.Object);
			request.Setup(x => x.Files).Returns(files.Object);
			files.Setup(x => x["item-icon"]).Returns((HttpPostedFileBase)null);
			Controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), Controller);
			MockAccountRepository
				.Setup(m => m.GetById(parrentAccountId))
				.Returns((Account)null);

			// Act
			var model = Controller.Edit(account, parrentAccountId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			MockAccountRepository
				.Verify(x => x.Save(account.Id, account.Name, account.ParentAccountId, account.CurrencyId, account.IconPath,
					account.RowState));
			account.ParentAccount.Should().BeNull();
			account.ParentAccountId.Should().BeNull();
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndNotSave_WhenAccountIsNotValid() {
			// Arrange
			var account = TestObjectCreator.CreateAccount();
			Controller.ModelState.AddModelError("error", "error");

			// Act
			var model = Controller.Edit(account, Guid.Empty);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<ViewResult>();
			MockAccountRepository
				.Verify(x => x.Save(account.Id, account.Name, account.ParentAccountId, account.CurrencyId, account.IconPath,
					account.RowState), Times.Never);
		}

		[Fact]
		public void Delete_ReturnExpectedResultAndSaveWithDeletedState_WhenAccountIsExists() {
			// Arrange
			var accountId = Guid.NewGuid();
			var account = TestObjectCreator.CreateAccount(accountId);
			MockAccountRepository
				.Setup(m => m.GetById(accountId))
				.Returns(account);

			// Act
			var model = Controller.Delete(accountId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			MockAccountRepository
				.Verify(x => x.Save(account.Id, account.Name, account.ParentAccountId, account.CurrencyId, account.IconPath,
					account.RowState), Times.Once);
			((RowState)account.RowState).Should().Be(RowState.Deleted);
		}

		[Fact]
		public void Delete_ReturnExpectedResult_WhenAccountIsNotExists() {
			// Arrange
			var unknownAccountId = Guid.NewGuid();
			MockAccountRepository
				.Setup(m => m.GetById(unknownAccountId))
				.Returns((Account)null);

			// Act
			var result = Controller.Delete(unknownAccountId);

			// Assert
			result.Should()
				.NotBeNull().And
				.BeOfType<HttpNotFoundResult>();
			MockAccountRepository
				.Verify(
					x => x.Save(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<Guid>(), It.IsAny<string>(),
						It.IsAny<byte>()), Times.Never);
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
			MockAccountRepository
				.Verify(
					x => x.Save(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid?>(), It.IsAny<Guid>(), It.IsAny<string>(),
						It.IsAny<byte>()), Times.Never);
		}

	}

	#endregion

}
