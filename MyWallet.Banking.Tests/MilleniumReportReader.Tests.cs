namespace MyWallet.Banking.Tests {
	using System;
	using System.Globalization;
	using FluentAssertions;
	using MyWallet.Domain.Type;
	using NSubstitute;
	using Xunit;

	public class MilleniumReportReaderTestCase {

		#region Setup/Tear down

		public MilleniumReportReaderTestCase() {
			ReportSource = Substitute.For<IReportSource>();
			ReportReader = new MilleniumReportReader {
				ReportSource = ReportSource
			};
		}

		#endregion

		protected MilleniumReportReader ReportReader;

		protected IReportSource ReportSource;

		[Fact]
		public void ReadReport_ReturnExpectedResult_IfReportDoseNotContainData() {
			// Arrange
			var path = "";
			var report = new[] {
				"row1",
				"row2"
			};
			ReportSource.GetLines(path).Returns(report);
			var request = new ReadReportRequest { FilePath = path };

			// Act
			var result = ReportReader.ReadReport(request);

			// Assert
			result.FilePath.Should().Be(path);
			result.BalanceIsRead.Should().BeFalse();
			result.Balance.Should().Be(0);
			result.Transactions.Should().BeEmpty();
		}

		[Fact]
		public void ReadReport_ReadTransaction_IfReportContainData() {
			// Arrange
			var path = "";
			var number = 1;
			var amount = 521.34M;
			var numberFormatInfo = new NumberFormatInfo {
				NumberDecimalSeparator = "."
			};
			var amountStr = amount.ToString(numberFormatInfo);
			var description = "WYNAGRODZENIESIERPIEŃ2017";
			var date = new DateTime(2017, 1, 2);
			var report = new[] {
				"www.bankmillennium.pl",
				"Confirmation of transaction performance",
				"Transaction typeINCOMINGTRANSFER",
				$"Daily transaction number{number}",
				$"Transaction date{date:yyyy-MM-dd}",
				$"Effective date{date:yyyy-MM-dd}",
				"Source account19103000190109850300147080",
				"Originator bankBHSektorBankowościDetalicznej",
				"BCFSOFTWARESP.ZO.O.TECHNOLOGICZNA245-837",
				"Originator",
				"OPOLE",
				"Destination account42116022020000000332191098",
				"BeneficiaryIURIIMASLECHKIN",
				$"Transaction amount{amountStr}PLN",
				$"Amount posted{amountStr}PLN",
				$"Description{description}",
				"Date of the document:  2017-08-30"
			};
			ReportSource.GetLines(path).Returns(report);
			var request = new ReadReportRequest { FilePath = path };

			// Act
			var result = ReportReader.ReadReport(request);

			// Assert
			result.FilePath.Should().Be(path);
			result.BalanceIsRead.Should().BeFalse();
			result.Balance.Should().Be(0);
			result.Transactions.Should().NotBeEmpty().And.HaveCount(1);
			result.Transactions[0].Amount.Should().Be(amount);
			result.Transactions[0].Comment.Should().Be($"#{number}: {description}");
			result.Transactions[0].DateIn.ToShortDateString().Should().Be(date.ToShortDateString());
			result.Transactions[0].DirectionType.Should().Be(DirectionType.Incoming);
		}

		[Theory]
		[InlineData("-123.5", -123.5, DirectionType.Expense)]
		[InlineData("123.5", 123.5, DirectionType.Incoming)]
		[InlineData("1,223.5", 1223.5, DirectionType.Incoming)]
		[InlineData("-1 , 223.5 ", -1223.5, DirectionType.Expense)]
		public void ReadReport_ReadTransactionWithExpectedAmmount_IfAmount(string ammount, decimal expectedAmount, DirectionType expectedType) {
			// Arrange
			var path = "";
			var report = new[] {
				"Confirmation of transaction performance",
				$"Amount posted{ammount}PLN",
				"Date of the document:  2017-08-30"
			};
			ReportSource.GetLines(path).Returns(report);
			var request = new ReadReportRequest { FilePath = path };

			// Act
			var result = ReportReader.ReadReport(request);

			// Assert
			result.Transactions.Should().HaveCount(1);
			result.Transactions[0].Amount.Should().Be(expectedAmount);
			result.Transactions[0].DirectionType.Should().Be(expectedType);
		}

		[Fact]
		public void ReadReport_ReadTransaction_IfTransactionIsTransferForPhone() {
			// Arrange
			var path = "";
			var number = 1;
			var amount = -15M;
			var numberFormatInfo = new NumberFormatInfo {
				NumberDecimalSeparator = "."
			};
			var amountStr = amount.ToString(numberFormatInfo);
			var description = "48733694472";
			var date = new DateTime(2017, 08, 30);
			var report = new[] {
				"Confirmation of transaction performance",
				"Transaction type FUNDS TRANSFER FOR PHONE TOP-UP",
				$"Daily transaction number{number}",
				$"Transaction date {date:yyyy-MM-dd}",
				"Effective date 2017-08-30",
				"Source account 42116022020000000332191098 Originator MASLECHKIN YURII UL ZDOBUŃSKA 9M73 02081 KIJÓW Destination account 97116022020000000102390027",
				"Beneficiary bank Millennium - Centrum Rozliczeniowe",
				"Beneficiary Blue Media S.A. ul. Haffnera 6 81-717 SOPOT",
				$"Amount{amountStr}PLN",
				"ID number 170830111728033",
				$"Description{description}",
				"Date of the document:  2017-08-30"
			};
			ReportSource.GetLines(path).Returns(report);
			var request = new ReadReportRequest { FilePath = path };

			// Act
			var result = ReportReader.ReadReport(request);

			// Assert
			result.Transactions.Should().NotBeEmpty().And.HaveCount(1);
			result.Transactions[0].Amount.Should().Be(amount);
			result.Transactions[0].Comment.Should().Be($"#{number}: {description}");
			result.Transactions[0].DateIn.ToShortDateString().Should().Be(date.ToShortDateString());
			result.Transactions[0].DirectionType.Should().Be(DirectionType.Expense);
		}

		[Fact]
		public void ReadReport_DidNotReadTransaction_IfTransactionRecipWithoutEndLine() {
			// Arrange
			var path = "";
			var amount = 521.34M;
			var numberFormatInfo = new NumberFormatInfo {
				NumberDecimalSeparator = "."
			};
			var amountStr = amount.ToString(numberFormatInfo);
			var description = "WYNAGRODZENIESIERPIEŃ2017";
			var date = new DateTime(2017, 1, 2);
			var report = new[] {
				"www.bankmillennium.pl",
				"Confirmation of transaction performance",
				"Transaction typeINCOMINGTRANSFER",
				"Daily transaction number1",
				$"Transaction date{date:yyyy-MM-dd}",
				$"Effective date{date:yyyy-MM-dd}",
				"Source account19103000190109850300147080",
				"Originator bankBHSektorBankowościDetalicznej",
				"BCFSOFTWARESP.ZO.O.TECHNOLOGICZNA245-837",
				"Originator",
				"OPOLE",
				"Destination account42116022020000000332191098",
				"BeneficiaryIURIIMASLECHKIN",
				$"Transaction amount{amountStr}PLN",
				$"Amount posted{amountStr}PLN",
				$"Description{description}"
			};
			ReportSource.GetLines(path).Returns(report);
			var request = new ReadReportRequest { FilePath = path };

			// Act
			var result = ReportReader.ReadReport(request);

			// Assert
			result.FilePath.Should().Be(path);
			result.BalanceIsRead.Should().BeFalse();
			result.Balance.Should().Be(0);
			result.Transactions.Should().BeEmpty();
		}


	}

}
