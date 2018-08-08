namespace MyWallet.WebUI.Models
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Domain.Entities;

	#region Class: AccountViewModelExtendMethods

	public static class AccountViewModelExtendMethods
	{

		#region Methods: Public

		public static AccountViewModel ToAccountViewModel(this Account source) {
			var namePrefix = source.ParentAccount != null ? $"{source.ParentAccount.Name}: " : string.Empty;
			var viewModel = new AccountViewModel {
				Id = source.Id,
				Name = $"{namePrefix}{source.Name}",
				Icon = source.IconPath
			};
			//if (source.Icon != null) {
			//	var imageBase64 = Convert.ToBase64String(source.Icon);
			//	var imageSrc = $"data:image/gif;base64,{imageBase64}";
			//	viewModel.Icon = imageSrc;
			//}
			return viewModel;
		}

		public static List<AccountViewModel> ToAccountViewModelList(this List<Account> list) {
			return list.Select(item => item.ToAccountViewModel()).ToList();
		}

		#endregion

	}

	#endregion

}