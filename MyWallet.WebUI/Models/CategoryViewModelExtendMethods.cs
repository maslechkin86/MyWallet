namespace MyWallet.WebUI.Models
{
	using System.Collections.Generic;
	using System.Linq;
	using Domain.Entities;

	#region Class: CategoryViewModelExtendMethods

	public static class CategoryViewModelExtendMethods
	{
		#region Methods: Public

		public static CategoryViewModel ToCategoryViewModel(this Category source) {
			var viewModel = new CategoryViewModel {
				Id = source.Id,
				Name = $"{source.Name}",
				DirectionType = source.DirectionType
			};
			return viewModel;
		}

		public static List<CategoryViewModel> ToCategoryViewModelList(this List<Category> list) {
			return list.Select(item => item.ToCategoryViewModel()).ToList();
		}

		#endregion

	}

	#endregion

}