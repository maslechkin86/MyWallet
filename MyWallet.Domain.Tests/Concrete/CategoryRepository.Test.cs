namespace MyWallet.Domain.Tests.Concrete {
	using System;
	using Abstract;
	using Common;
	using Domain.Concrete;
	using Domain.Entities;
	using FluentAssertions;
	using Moq;
	using Type;
	using Xunit;

	#region Class: CategoryRepositoryTestCase

	public class CategoryRepositoryTestCase {

		#region Setup/Teardown

		public CategoryRepositoryTestCase() {
			MockDbContext = new Mock<IMyWalletDbContext>();
			Repository = new CategoryRepository(MockDbContext.Object);
		}

		#endregion

		protected Mock<IMyWalletDbContext> MockDbContext;

		protected CategoryRepository Repository;

		[Fact]
		public void Dispose_CallDisposeOfDbContext_WhenCall() {
			// Act
			Repository.Dispose();

			// Assert
			MockDbContext.Verify(x => x.Dispose());
		}

		[Fact]
		public void GetById_ReturnExpectedItem_WhenCategoryExists() {
			// Arrange
			var category = TestObjectCreator.CreateCategory();
			MockDbContext
				.Setup(x => x.Categories)
				.Returns(new FakeDbSet<Category> {category});

			// Act
			var items = Repository.GetById(category.Id);

			// Assert
			items.Should()
				.NotBeNull().And
				.Be(category);
		}

		[Fact]
		public void GetById_ReturnNull_WhenCategoryNotExists() {
			// Arrange
			var unknownCategoryId = Guid.NewGuid();
			MockDbContext
				.Setup(x => x.Categories)
				.Returns(new FakeDbSet<Category>());

			// Act
			var items = Repository.GetById(unknownCategoryId);

			// Assert
			items.Should()
				.BeNull();
		}

		[Fact]
		public void GetAll_ReturnExpectedItems_WhenCall() {
			// Arrange
			var category1 = TestObjectCreator.CreateCategory();
			var category2 = TestObjectCreator.CreateCategory();
			MockDbContext
				.Setup(x => x.Categories)
				.Returns(new FakeDbSet<Category> {
					category1,
					category2
				});

			// Act
			var items = Repository.GetAll();

			// Assert
			items.Should()
				.HaveCount(2).And
				.Contain(category1).And
				.Contain(category2);
		}

		[Fact]
		public void GetAll_ReturnOnlyItemsWithStateExisting_WhenExistsDeletedItemsCall() {
			// Arrange
			var category1 = TestObjectCreator.CreateCategory();
			var category2 = TestObjectCreator.CreateCategory();
			category2.RowState = (int) RowState.Deleted;
			var category3 = TestObjectCreator.CreateCategory();
			var category4 = TestObjectCreator.CreateCategory();
			MockDbContext
				.Setup(x => x.Categories)
				.Returns(new FakeDbSet<Category> {
					category1,
					category2,
					category3,
					category4
				});

			// Act
			var items = Repository.GetAll();

			// Assert
			items.Should()
				.HaveCount(3).And
				.Contain(category1).And
				.Contain(category3).And
				.Contain(category4);
		}

		[Fact]
		public void Save_AddAccountAndSaveChanges_WhenItIsNewItem() {
			// Arrange
			var category = TestObjectCreator.CreateCategory(Guid.Empty);
			var fakeDbSet = new FakeDbSet<Category>();
			MockDbContext
				.Setup(x => x.Categories)
				.Returns(fakeDbSet);

			// Act
			Repository.Save(category.Id, category.Name, category.DirectionTypeId, category.IconPath, category.RowState);

			// Assert
			fakeDbSet.Local.Should().HaveCount(1);
			var item = fakeDbSet.Local[0];
			item.Name.Should().Be(category.Name);
			item.IconPath.Should().Be(category.IconPath);
			item.RowState.Should().Be(category.RowState);
			item.DirectionType.Should().Be(category.DirectionType);
			item.ModifiedOn.Date.Should().Be(DateTime.Now.Date);
			MockDbContext.Verify(x => x.SaveChanges());
		}

		[Fact]
		public void Save_UpdateAccountAndSaveChanges_WhenItIsExistsItem() {
			// Arrange
			var categoryId = Guid.NewGuid();
			var fakeDbSet = new FakeDbSet<Category> {
				new Category {
					Id = categoryId,
					Name = "name1",
					RowState = (int) RowState.Existing,
					ModifiedOn = DateTime.MinValue,
					DirectionType = DirectionType.Incoming
				}
			};
			MockDbContext
				.Setup(x => x.Categories)
				.Returns(fakeDbSet);
			var updateCategory = new Category {
				Id = categoryId,
				Name = "updayteName",
				RowState = (int) RowState.Deleted,
				DirectionType = DirectionType.Expense
			};

			// Act
			Repository.Save(updateCategory.Id, updateCategory.Name, updateCategory.DirectionTypeId, updateCategory.IconPath,
				updateCategory.RowState);

			// Assert
			fakeDbSet.Local.Should()
				.HaveCount(1);
			var item = fakeDbSet.Local[0];
			item.Name.Should().Be(updateCategory.Name);
			item.RowState.Should().Be(updateCategory.RowState);
			item.DirectionType.Should().Be(updateCategory.DirectionType);
			item.ModifiedOn.Date.Should().Be(DateTime.Now.Date);
			MockDbContext.Verify(x => x.SaveChanges());
		}

	}

	#endregion

}
