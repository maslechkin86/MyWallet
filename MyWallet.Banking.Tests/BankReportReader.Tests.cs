namespace MyWallet.Banking.Tests
{
	using FluentAssertions;
	using System;
	using Xunit;

	#region Class: BankReportReaderWrapper

	public class BankReportReaderWrapper : BankReportReader
	{

		#region Methods: Public

		public override ReadReportResponse ReadReport(ReadReportRequest request) {
			throw new NotImplementedException();
		}

		#endregion

	}

	#endregion

	#region Class: BankReportReaderTestCase

	public class BankReportReaderTestCase
	{

		#region Methods: Public

		[Theory]
		[InlineData("")]
		[InlineData(null)]
		public void ParseAmount_ThrowFormatException_IfAmountEmptyOrNull(object amount) {
			// Arrange
			var reader = new BankReportReaderWrapper();

			// Act
			Action a = () => { reader.ParseAmount(amount); };

			// Assert
			a.ShouldThrow<FormatException>();
		}

		[Theory]
		[InlineData(-1, -1)]
		[InlineData(48, 48)]
		[InlineData("13", 13)]
		[InlineData("-17", -17)]
		[InlineData("14.0", 14)]
		[InlineData("11.2", 11.2f)]
		public void ParseAmount_ReturnExpectedValue_IfAmount(object amount, decimal expectedAmmount) {
			// Arrange
			var reader = new BankReportReaderWrapper();

			// Act
			var result = reader.ParseAmount(amount);

			// Assert
			result.Should().Be(expectedAmmount);
		}

		[Theory]
		[InlineData(" 10", 10)]
		[InlineData("77 ", 77)]
		[InlineData(" 14 78.0", 1478)]
		[InlineData(" 12.3 ", 12.3f)]
		public void ParseAmount_ReturnExpectedValue_IfAmountContainsSpace(object amount, decimal expectedAmmount) {
			// Arrange
			var reader = new BankReportReaderWrapper();

			// Act
			var result = reader.ParseAmount(amount);

			// Assert
			result.Should().Be(expectedAmmount);
		}

		#endregion

	}

	#endregion

}