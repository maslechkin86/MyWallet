using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MyWallet.Banking.Model;
using MyWallet.Domain.Type;

namespace MyWallet.Banking {
	public class MilleniumReportReader : BankReportReader {

		private const int MaxRecipRowsCount = 20;

		private const string TransactionAmount = "Amount";

		private const string TransactionAmountPosted = "Amount posted";

		private const string TransactionBegin = "Confirmation of transaction performance";

		private const string TransactionDate = "Transaction date";

		private const string TransactionDescription = "Description";

		private const string TransactionEnd = "Date of the document:";

		private const string TransactionNumber = "Daily transaction number";

		/// <summary>
		/// Gets or sets the collection of rules for determining source of transaction.
		/// </summary>
		/// <value>
		/// The collection of rules.
		/// </value>
		protected List<BaseDetermineRule> SourceDetermineRules { get; set; }

		public MilleniumReportReader() {
			ReportSource = new PdfFileReportSource();
			SourceDetermineRules = new List<BaseDetermineRule> {
				new SourceDetermineRule("MilleniumAccount"),
				new SourceDetermineRule("MilleniumCard")
			};
		}

		private string DetermineSourceId(string line) {
			var sourceId = string.Empty;
			var rule = SourceDetermineRules.FirstOrDefault(_ => line.Contains(_.Pattern));
			if(rule != null) {
				sourceId = rule.ObjectId;
			}
			return sourceId;
		}

		private bool ParseAmmount(string line, string pattern, out decimal amount) {
			amount = 0;
			if(line.StartsWith(pattern)) {
				var amountStr = line
					.Replace(pattern, "")
					.Replace("PLN", "")
					.Replace(",", "")
					.Replace(" ", "");
				var numberFormatInfo = new NumberFormatInfo {
					NumberDecimalSeparator = "."
				};
				amount = decimal.Parse(amountStr, numberFormatInfo);
				return true;
			}
			return false;
		}

		public override ReadReportResponse ReadReport(ReadReportRequest request) {
			var response = new ReadReportResponse {
				FilePath = request.FilePath,
				Transactions = new List<ReadTransaction>()
			};
			var source = ReportSource.GetLines(request.FilePath);
			var list = ((string[])source).ToList();
			for (var row = 0; row < list.Count; row++) {
				var line = list[row];
				if (string.IsNullOrEmpty(response.SourceId)) {
					response.SourceId = DetermineSourceId(line);
				}
				if (line == TransactionBegin) {
					var transaction = new ReadTransaction();
					for (var i = 1; i < MaxRecipRowsCount && row + i < list.Count; i++) {
						line = list[row + i];
						decimal amount;
						if (ParseAmmount(line, TransactionAmountPosted, out amount) ||
						    ParseAmmount(line, TransactionAmount, out amount)) {
							transaction.Amount = amount;
							transaction.DirectionType = amount < 0
								? DirectionType.Expense
								: DirectionType.Incoming;
						}
						if (line.StartsWith(TransactionDate)) {
							transaction.DateIn = DateTime.Parse(line.Replace(TransactionDate, ""));
						}
						if(line.StartsWith(TransactionNumber)) {
							var number = line.Replace(TransactionNumber, "");
							transaction.Comment += $"#{number}: ";
						}
						if (line.StartsWith(TransactionDescription)) {
							transaction.Comment += line.Replace(TransactionDescription, "");
						}
						if (line.StartsWith(TransactionEnd)) {
							response.Transactions.Add(transaction);
							row += i;
							break;
						}
					}
				}
			}
			return response;
		}

	}

}
