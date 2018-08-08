namespace MyWallet.WebUI.Controllers
{
	using System;
	using System.Linq;
	using System.Net;
	using System.Web.Mvc;
	using Domain.Abstract;
	using Domain.Entities;
	using Domain.Type;
	using Models;

	#region Class: CategoryController

	public class CategoryController : Controller
	{

		#region Fields: Private

		private readonly ICategoryRepository _categoryRepository;

		#endregion

		#region Constructors: Public

		public CategoryController(ICategoryRepository categoryRepository) {
			_categoryRepository = categoryRepository;
		}

		#endregion

		#region Methods: Public

		public ViewResult List() {
			var transactions = _categoryRepository.GetAll()
				.OrderBy(p => p.Name);
			return View(transactions.ToList().ToCategoryViewModelList());
		}
		public ViewResult Create() {
			return View("Edit", new Category { Id = Guid.Empty });
		}

		public ViewResult Edit(Guid categoryId) {
			var category = _categoryRepository.GetById(categoryId);
			return View(category);
		}

		[HttpPost]
		public ActionResult Edit(Category category) {
			if (ModelState.IsValid) {
				_categoryRepository.Save(category.Id, category.Name, category.DirectionTypeId, category.IconPath, category.RowState);
				TempData["message"] = $"{category.Name} has been saved";
				return RedirectToAction("List");
			}
			return View(category);
		}

		public ActionResult Delete(Guid? categoryId) {
			if (categoryId == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var category = _categoryRepository.GetById((Guid)categoryId);
			if (category == null) {
				return HttpNotFound();
			}
			category.RowState = (int)RowState.Deleted;
			_categoryRepository.Save(category.Id, category.Name, category.DirectionTypeId, category.IconPath, category.RowState);
			TempData["message"] = $"{category.Name} has been deleted";
			return RedirectToAction("List");
		}

		#endregion

	}

	#endregion

}