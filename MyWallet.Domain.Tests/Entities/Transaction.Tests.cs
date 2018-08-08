using System;
using FluentAssertions;
using MyWallet.Domain.Entities;
using Xunit;

namespace MyWallet.Domain.Tests.Entities {

	public class TransactionTestCase {

		[Fact]
		public void Hash_ShouldBeDifferent_IfTransactionIsDifferentTransaction() {
			// Arrange
			var transaction1 = new Transaction {
				Amount = 100,
				Comment = "Comment",
				DateIn = DateTime.Now
			};
			var transaction2 = new Transaction {
				Amount = 101,
				Comment = "Comment",
				DateIn = DateTime.Now
			};

			// Act
			var hash1 = transaction1.Hash;
			var hash2 = transaction2.Hash;

			// Assert
			hash1.Should().NotBe(hash2);
		}

		[Fact]
		public void Hash_ShouldSame_IfTransactionAmountDescriptionAndDateIsSame() {
			// Arrange
			var transaction1 = new Transaction {
				Amount = 100,
				Comment = "Comment",
				DateIn = DateTime.Now,
				AccountId = Guid.NewGuid()
			};
			var transaction2 = new Transaction {
				Amount = 100,
				Comment = "Comment",
				DateIn = DateTime.Now,
				AccountId = Guid.NewGuid()
			};

			// Act
			var hash1 = transaction1.Hash;
			var hash2 = transaction2.Hash;

			// Assert
			hash1.Should().Be(hash2);
		}

	}

}
