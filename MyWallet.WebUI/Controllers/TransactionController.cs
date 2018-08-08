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

	#region Class: TransactionController

	public class TransactionController : Controller
	{

		#region Fields: Private

		private readonly ITransactionRepository _transactionRepository;

		private readonly ICategoryRepository _categoryRepository;

		private readonly IAccountRepository _accountRepository;

		#endregion

		#region Constructors: Public

		public TransactionController(ICategoryRepository categoryRepository, IAccountRepository accountRepository,
				ITransactionRepository transactionRepository) {
			_categoryRepository = categoryRepository;
			_accountRepository = accountRepository;
			_transactionRepository = transactionRepository;
		}

		#endregion

		#region Methods: Protected

		protected SelectList GetCategoriesList(Guid id) {
			var list = _categoryRepository.GetAll().ToList();
			list.Add(new Category { Id = Guid.Empty, Name = "-" });
			return new SelectList(
				list,
				"Id",
				"Name",
				id);
		}

		protected SelectList GetAccountsList(Guid id) {
			var list = _accountRepository.GetAll().ToList();
			list.Add(new Account { Id = Guid.Empty, Name = "-" });
			return new SelectList(
				list,
				"Id",
				"Name",
				id);
		}

		#endregion

		#region Methods: Public

		public ViewResult List() {
			var transactions = _transactionRepository.GetAll()
				.OrderBy(p => p.CreatedOn);
			return View(transactions.ToList().ToTransactionViewModelList());
		}

		public ViewResult Create() {
			ViewBag.Accounts = GetAccountsList(Guid.Empty);
			ViewBag.Categories = GetCategoriesList(Guid.Empty);
			return View("Edit", new Transaction { Id = Guid.Empty });
		}

		public ActionResult Edit(Guid transactionId) {
			var transaction = _transactionRepository.GetById(transactionId);
			if (transaction == null) {
				return HttpNotFound();
			}
			ViewBag.Accounts = GetAccountsList(transaction.AccountId);
			ViewBag.Categories = GetCategoriesList(transaction.CategoryId);
			return View(transaction);
		}

		[HttpPost]
		public ActionResult Edit(Transaction transaction, Guid categories, Guid accounts) {
			if (ModelState.IsValid) {
				transaction.Category = _categoryRepository.GetById(categories);
				transaction.Account = _accountRepository.GetById(accounts);
				transaction.NeedConfirm = transaction.DateIn > DateTime.Now;
				_transactionRepository.Save(transaction.Id, transaction.Comment, transaction.Amount, accounts,
					categories, transaction.RowState, transaction.NeedConfirm);
				TempData["message"] = $"{transaction.Comment} has been saved";
				return RedirectToAction("List");
			}
			return View(transaction);
		}

		public ActionResult Delete(Guid? transactionId) {
			if (transactionId == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var transaction = _transactionRepository.GetById((Guid)transactionId);
			if (transaction == null) {
				return HttpNotFound();
			}
			transaction.RowState = (int)RowState.Deleted;
			_transactionRepository.Save(transaction.Id, transaction.Comment, transaction.Amount, transaction.AccountId,
				transaction.CategoryId, transaction.RowState, transaction.NeedConfirm);
			TempData["message"] = $"{transaction.Comment} has been deleted";
			return RedirectToAction("List");
		}

		[HttpGet]
		public JsonResult GetListOfPopularAccounts() {
			const int defaultAccountsCount = 2;
			var list = _transactionRepository
				.GetAll()
				.OrderBy(x => x.ModifiedOn)
				.Take(defaultAccountsCount)
				.Select(x => x.AccountId).ToList();
			return Json(list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetListOfPopularCategories() {
			const int defaultCategoriesCount = 4;
			var list = _transactionRepository
				.GetAll()
				.OrderBy(x => x.ModifiedOn)
				.Take(defaultCategoriesCount)
				.Select(x => x.CategoryId).ToList();
			return Json(list, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult GetLastModifiedTransactionDateIn() {
			var item = _transactionRepository
				.GetAll()
				.OrderByDescending(x => x.ModifiedOn)
				.FirstOrDefault();
			return Json(item?.DateIn, JsonRequestBehavior.AllowGet);
		}

		#endregion

	}

	#endregion

}