namespace MyWallet.Domain.Tests.Entities {
	using System;
	using Domain.Entities;
	using FluentAssertions;
	using Xunit;

	#region Class: BaseEntityTestCase
	public class BaseEntityTestCase {

		#region Methods: Public

		[Fact]
		public void Constructor_ClassContainsExpectedProperties_IfCallBaseConstructor() {
			// Act
			var entity = new BaseEntity();

			// Assert
			entity.CreatedOn.Date.Should().Be(DateTime.Now.Date);
			entity.ModifiedOn.Date.Should().Be(DateTime.Now.Date);
			entity.Id.Should().NotBeEmpty();
		}

		#endregion

	}

	#endregion

}
