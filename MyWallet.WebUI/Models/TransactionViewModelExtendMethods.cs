namespace MyWallet.WebUI.Models
{
	using System.Collections.Generic;
	using System.Linq;
	using Domain.Entities;

	#region Class: TransactionViewModelExtendMethods

	public static class TransactionViewModelExtendMethods
	{

		#region Methods: Public

		public static TransactionViewModel ToTransactionViewModel(this Transaction source) {
			var viewModel = new TransactionViewModel {
				Id = source.Id,
				Comment = source.Comment,
				Amount = source.Amount,
				AccountName = source.Account.Name,
				AccountIco = source.Account.IconPath,
				CategoryName = source.Category.Name,
				CategoryIco = source.Category.IconPath,
				DateIn = source.DateIn.ToString("yyyy.MM.dd")
			};
			return viewModel;
		}

		public static List<TransactionViewModel> ToTransactionViewModelList(this List<Transaction> list) {
			return list.Select(item => item.ToTransactionViewModel()).ToList();
		}

		#endregion

	}

	#endregion

}