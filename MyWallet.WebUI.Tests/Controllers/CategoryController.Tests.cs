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

	#region Class: CategoryControllerTestCase

	public class CategoryControllerTestCase
	{

		#region Setup/Teardown

		public CategoryControllerTestCase() {
			MockCategoryRepository = new Mock<ICategoryRepository>();
			Controller = new CategoryController(MockCategoryRepository.Object);
		}

		#endregion

		protected readonly CategoryController Controller;

		protected readonly Mock<ICategoryRepository> MockCategoryRepository;

		[Fact]
		public void List_ReturnsExpectedCollectionOfItems_WhenCall() {
			// Arrange
			var fakeList = new[] {
				TestObjectCreator.CreateCategory(name: "C"),
				TestObjectCreator.CreateCategory(name: "B"),
				TestObjectCreator.CreateCategory(name: "A")
			};
			MockCategoryRepository
				.Setup(m => m.GetAll())
				.Returns(fakeList);

			// Act
			var model = (List<CategoryViewModel>)Controller.List().Model;

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
			// Act
			var model = (Category)Controller.Create().Model;

			// Assert
			model.Should()
				.NotBeNull();
			model.Id.Should().BeEmpty();
		}

		[Fact]
		public void Edit_CanEdit_WhenCategoryExists() {
			// Arrange
			var categoryId = Guid.NewGuid();
			var category = TestObjectCreator.CreateCategory(categoryId);
			MockCategoryRepository
				.Setup(m => m.GetById(categoryId))
				.Returns(category);

			// Act
			var model = (Category)Controller.Edit(categoryId).Model;

			// Assert
			model.Should()
				.NotBeNull().And
				.Be(category);
		}

		[Fact]
		public void Edit_CanNotEdit_WhenCategoryNotExists() {
			// Arrange
			var unknownCategoryId = Guid.NewGuid();
			MockCategoryRepository
				.Setup(m => m.GetById(unknownCategoryId))
				.Returns((Category)null);

			// Act
			var model = (Transaction)Controller.Edit(unknownCategoryId).Model;

			// Assert
			model.Should()
				.BeNull();
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndSave_WhenCategoryIsValid() {
			// Arrange
			var category = TestObjectCreator.CreateCategory();

			// Act
			var model = Controller.Edit(category);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			Controller.TempData["message"].Should().NotBeNull();
			MockCategoryRepository
				.Verify(x => x.Save(category.Id, category.Name, category.DirectionTypeId, category.IconPath, category.RowState));
		}

		[Fact]
		public void Edit_ReturnExpectedResultAndNotSave_WhenCategoryIsNotValid() {
			// Arrange
			var category = TestObjectCreator.CreateCategory();
			Controller.ModelState.AddModelError("error", "error");

			// Act
			var model = Controller.Edit(category);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<ViewResult>();
			MockCategoryRepository
				.Verify(x => x.Save(category.Id, category.Name, category.DirectionTypeId, category.IconPath, category.RowState),
					Times.Never);
		}

		[Fact]
		public void Delete_ReturnExpectedResultAndSaveWithDeletedState_WhenCategoryIsExists() {
			// Arrange
			var categoryId = Guid.NewGuid();
			var category = TestObjectCreator.CreateCategory(categoryId);
			MockCategoryRepository
				.Setup(m => m.GetById(categoryId))
				.Returns(category);

			// Act
			var model = Controller.Delete(categoryId);

			// Assert
			model.Should()
				.NotBeNull().And
				.BeOfType<RedirectToRouteResult>();
			MockCategoryRepository
				.Verify(x => x.Save(category.Id, category.Name, category.DirectionTypeId, category.IconPath, category.RowState),
					Times.Once);
			((RowState)category.RowState).Should().Be(RowState.Deleted);
		}

		[Fact]
		public void Delete_ReturnExpectedResult_WhenCategoryIsNotExists() {
			// Arrange
			var unknownCategoryId = Guid.NewGuid();
			MockCategoryRepository
				.Setup(m => m.GetById(unknownCategoryId))
				.Returns((Category)null);

			// Act
			var result = Controller.Delete(unknownCategoryId);

			// Assert
			result.Should()
				.NotBeNull().And
				.BeOfType<HttpNotFoundResult>();
			MockCategoryRepository
				.Verify(x => x.Save(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<byte>()), Times.Never);
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
			MockCategoryRepository
				.Verify(x => x.Save(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<byte>()), Times.Never);
		}

	}

	#endregion

}
