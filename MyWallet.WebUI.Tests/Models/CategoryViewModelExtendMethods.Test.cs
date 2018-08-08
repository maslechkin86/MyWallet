namespace MyWallet.WebUI.Tests.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Domain.Entities;
	using Domain.Type;
	using FluentAssertions;
	using WebUI.Models;
	using Xunit;

	#region Class: CategoryViewModelExtendMethodsTestCase

	public class CategoryViewModelExtendMethodsTestCase
	{

		[Fact]
		public void ToCategoryViewModel_CreteExpectedObject_WhenCall() {
			// Arrange
			var category = new Category {
				Id = Guid.NewGuid(),
				Name = "subAccountName",
				DirectionType = DirectionType.Incoming
			};

			// Act
			var viewModel = category.ToCategoryViewModel();

			// Assert
			viewModel.Id.Should().Be(category.Id);
			viewModel.Name.Should().Be($"{category.Name}");
			viewModel.DirectionType.Should().Be(category.DirectionType);
		}

		[Fact]
		public void ToCategoryViewModelList_CreteExpectedCollection_WhenCall() {
			// Arrange
			var categories = new List<Category> {
				new Category {
					Id = Guid.NewGuid(),
					Name = "subAccountName",
					DirectionType = DirectionType.Incoming
				},
				new Category {
					Id = Guid.NewGuid(),
					Name = "subAccountName",
					DirectionType = DirectionType.Incoming
				}
			};

			// Act
			var viewModel = categories.ToCategoryViewModelList();

			// Assert
			viewModel.Should()
				.NotBeNull().And
				.HaveCount(2);
			var expectedItem = categories[0];
			var item1 = viewModel.First(x => x.Id == expectedItem.Id);
			item1.Id.Should().Be(expectedItem.Id);
			item1.Name.Should().Be(expectedItem.Name);
			expectedItem = categories[1];
			var item2 = viewModel.First(x => x.Id == expectedItem.Id);
			item2.Id.Should().Be(expectedItem.Id);
			item2.Name.Should().Be(expectedItem.Name);
		}

	}

	#endregion

}
