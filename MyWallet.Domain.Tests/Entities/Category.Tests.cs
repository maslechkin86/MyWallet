namespace MyWallet.Domain.Tests.Entities {
	using Domain.Entities;
	using FluentAssertions;
	using Type;
	using Xunit;

	#region Class: CategoryTestCase

	public class CategoryTestCase {

		#region Methods: Public

		[Fact]
		public void DirectionType_SetDirectionTypeId_WhenSet() {
			// Arrange
			var category = new Category();

			// Act
			category.DirectionType = DirectionType.Expense;

			// Assert
			category.DirectionTypeId.Should().Be((int)DirectionType.Expense);
		}

		[Fact]
		public void DirectionTypeId_SetDirectionType_WhenSet() {
			// Arrange
			var category = new Category();

			// Act
			category.DirectionTypeId = 0;

			// Assert
			category.DirectionType.Should().Be(DirectionType.Expense);
		}

		[Fact]
		public void DirectionTypeId_SetDirectionTypeAsExpense_WhenSetValueIsNotInEnumRange() {
			// Arrange
			var category = new Category();

			// Act
			category.DirectionTypeId = 5;

			// Assert
			category.DirectionType.Should().Be(DirectionType.Expense);
		}

		#endregion

	}

	#endregion

}
