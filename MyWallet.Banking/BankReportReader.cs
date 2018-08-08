namespace MyWallet.Banking
{
	using System.Collections.Generic;
	using System.Globalization;
	using Model;

	#region Class: BankReportReader

	public abstract class BankReportReader
	{

		#region Properties: Public

		/// <summary>
		/// The report source.
		/// </summary>
		public IReportSource ReportSource { get; set; }

		#endregion

		#region Methods: Public

		/// <summary>
		/// Reads the bank report.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>Response.</returns>
		public abstract ReadReportResponse ReadReport(ReadReportRequest request);

		/// <summary>
		/// Parses the amount.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns>Amount System.Decimal.</returns>
		public decimal ParseAmount(object value) {
			var numberFormatInfo = new NumberFormatInfo {
				NumberDecimalSeparator = "."
			};
			var result = decimal.Parse((value ?? string.Empty).ToString().Replace(" ", ""), numberFormatInfo);
			return result;
		}

		#endregion

	}

	#endregion

	#region Class: ReadBankReportRequest

	public class ReadReportRequest
	{

		#region Properties: Public

		public string FilePath { get; set; }

		#endregion

	}

	#endregion

	#region Class: ReadBankReportResponse

	public class ReadReportResponse
	{

		#region Properties: Public

		public decimal Balance { get; internal set; }
		public bool BalanceIsRead { get; internal set; }
		public string FilePath { get; internal set; }
		public string SourceId { get; internal set; }
		public List<ReadTransaction> Transactions { get; internal set; }

		#endregion

	}

	#endregion

}