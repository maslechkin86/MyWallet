namespace MyWallet.Banking
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Domain.Type;
	using Model;

	#region Class: AvalReportReader

	public class AvalReportReader : BankReportReader
	{

		#region Constants: Protected

		/// <summary>
		/// The maximum column count in report.
		/// </summary>
		protected const int MaxColumnCount = 9;

		/// <summary>
		/// The column index which store transaction description.
		/// </summary>
		protected const int DescriptionColumnIndex = 4;

		/// <summary>
		/// The column index which store transaction amount.
		/// </summary>
		protected const int TransactionAmountColumnIndex = 6;

		/// <summary>
		/// The column index which store transaction date.
		/// </summary>
		protected const int TransactionDateColumnIndex = 0;

		/// <summary>
		/// The detail standard pattern.
		/// </summary>
		protected const string DetailStandardPattern = "205 - Безготівковий платіж.";

		/// <summary>
		/// The current balance pattern.
		/// </summary>
		protected const string CurrentBalancePattern = "Доступний баланс";

		/// <summary>
		/// The block balance pattern.
		/// </summary>
		protected const string BlockBalancePattern = "Поточна заблокована сума";

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="AvalReportReader"/> class.
		/// </summary>
		public AvalReportReader() {
			SourceDetermineRules = new List<BaseDetermineRule> {
				new SourceDetermineRule("AvalCreditCard"),
				new SourceDetermineRule("AvalDebitCard")
			};
			TransferSignDetermineRules = new List<BaseDetermineRule> {
				new BaseDetermineRule {
					Pattern = "Зняття готівки"
				},
				new BaseDetermineRule {
					Pattern = "Платежі на рахунок"
				}
			};
		}

		#endregion

		#region Properties: Protected

		/// <summary>
		/// Gets or sets the collection of rules for determining sign of transfer.
		/// </summary>
		/// <value>
		/// The collection of rules.
		/// </value>
		protected List<BaseDetermineRule> TransferSignDetermineRules { get; set; }

		/// <summary>
		/// Gets or sets the collection of rules for determining source of transaction.
		/// </summary>
		/// <value>
		/// The collection of rules.
		/// </value>
		protected List<BaseDetermineRule> SourceDetermineRules { get; set; }

		#endregion

		#region Methods: Private

		private bool FindData(string line, out decimal result, int position, char separator = ',') {
			result = 0;
			var data = line.Split(separator);
			if (data.Length >= position + 1) {
				result = ParseAmount(data[position]);
				return true;
			}
			return false;
		}

		private string DetermineSourceId(string line) {
			var sourceId = string.Empty;
			var rule = SourceDetermineRules.FirstOrDefault(_ => line.Contains(_.Pattern));
			if (rule != null) {
				sourceId = rule.ObjectId;
			}
			return sourceId;
		}

		private bool DetermineBalance(string line, out decimal balance) {
			const int balanceColumnIndex = 1;
			var sign = 0;
			balance = 0;
			if (line.Contains(CurrentBalancePattern)) {
				sign = 1;
			} else if (line.Contains(BlockBalancePattern)) {
				sign = -1;
			}
			decimal amount;
			if (FindData(line, out amount, balanceColumnIndex)) {
				balance += amount * sign;
				return true;
			}
			return false;
		}

		private ReadTransaction DetermineTransaction(string[] columns) {
			ReadTransaction transaction = null;
			try {
				var amount = ParseAmount(columns[TransactionAmountColumnIndex].Replace("UAH", ""));
				transaction = new ReadTransaction {
					Amount = Math.Abs(amount),
					DateIn = DateTime.Parse(columns[TransactionDateColumnIndex]),
					Comment = columns[DescriptionColumnIndex],
					DirectionType = amount < 0 ? DirectionType.Expense : DirectionType.Incoming,
					Category = null
				};
				if (TransferSignDetermineRules.Any(_ => transaction.Comment.Contains(_.Pattern))) {
					transaction.IsTransfer = true;
				}
				transaction.Comment = transaction.Comment.Replace(DetailStandardPattern, "");
			} catch {
			}
			return transaction;
		}

		private string[] GetReportColumns(string line) {
			return line.Replace("\",\"", "|").Replace("\"", "").Split('|');
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc />
		/// <summary>
		/// Reads the bank report.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>Response.</returns>
		public override ReadReportResponse ReadReport(ReadReportRequest request) {
			var response = new ReadReportResponse {
				FilePath = request.FilePath
			};
			var lines = ReportSource.GetLines(request.FilePath);
			foreach (var line in lines) {
				var row = line.ToString();
				if (!response.BalanceIsRead) {
					decimal balance;
					if (DetermineBalance(row, out balance)) {
						response.Balance = balance;
					}
				}
				if (string.IsNullOrEmpty(response.SourceId)) {
					response.SourceId = DetermineSourceId(row);
				}
				var columns = GetReportColumns(row);
				if (columns.Length >= MaxColumnCount) {
					var transaction = DetermineTransaction(columns);
					if (transaction != null) {
						response.Transactions.Add(transaction);
					}
				}
			}
			return response;
		}

		#endregion

	}

	#endregion

}