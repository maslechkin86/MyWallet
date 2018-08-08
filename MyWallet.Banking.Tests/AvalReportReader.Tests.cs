namespace MyWallet.Banking.Tests
{
	using FluentAssertions;
	using NSubstitute;
	using Xunit;

	public class AvalReportReaderTestCase
	{

		#region Setup/Teardown

		public AvalReportReaderTestCase() {
			ReportSource = Substitute.For<IReportSource>();
			ReportReader = new AvalReportReader {
				ReportSource = ReportSource
			};
		}

		#endregion

		protected AvalReportReader ReportReader;

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
			var request = new ReadReportRequest {FilePath = path};

			// Act
			var result = ReportReader.ReadReport(request);

			// Assert
			result.FilePath.Should().Be(path);
			result.BalanceIsRead.Should().BeFalse();
			result.Balance.Should().Be(0);
			result.Transactions.Should().BeNull();
		}

	}
}