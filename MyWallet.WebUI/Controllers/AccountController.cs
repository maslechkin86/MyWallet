namespace MyWallet.WebUI.Controllers {
	using System;
	using System.Linq;
	using System.Net;
	using System.Web.Mvc;
	using Domain.Abstract;
	using Domain.Entities;
	using Domain.Type;
	using Models;

	#region Class: AccountController

	public class AccountController : Controller {

		#region Fields: Private

		private readonly IAccountRepository _accountRepository;

		#endregion

		#region Constructors: Public

		public AccountController(IAccountRepository accountRepository) {
			_accountRepository = accountRepository;
		}

		#endregion

		#region Methods: Protected

		protected SelectList GetAccountsList(Guid id) {
			var list = _accountRepository.GetAll().ToList();
			list.Add(new Account {Id = Guid.Empty, Name = "-"});
			return new SelectList(
				list,
				"Id",
				"Name",
				id);
		}

		#endregion

		#region Methods: Public

		public ViewResult List() {
			var transactions = _accountRepository.GetAll()
				.OrderBy(p => p.Name);
			return View(transactions.ToList().ToAccountViewModelList());
		}

		public ViewResult Create() {
			ViewBag.Accounts = GetAccountsList(Guid.Empty);
			return View("Edit", new Account {Id = Guid.Empty});
		}

		public ActionResult Edit(Guid accountId) {
			var account = _accountRepository.GetById(accountId);
			if (account == null) {
				return HttpNotFound();
			}
			var parentAccountId = account.ParentAccount?.Id ?? Guid.Empty;
			ViewBag.Accounts = GetAccountsList(parentAccountId);
			return View(account);
		}

		[HttpPost]
		public ActionResult Edit(Account account, Guid? accounts) {
			if (ModelState.IsValid) {
				var parentAccount = _accountRepository.GetById(accounts ?? Guid.Empty);
				if (parentAccount != null) {
					account.ParentAccount = parentAccount;
					account.ParentAccountId = parentAccount.Id;
				} else {
					account.ParentAccount = null;
					account.ParentAccountId = null;
				}
				_accountRepository.Save(account.Id, account.Name, account.ParentAccountId, account.CurrencyId, account.IconPath,
					account.RowState);
				TempData["message"] = $"{account.Name} has been saved";
				return RedirectToAction("List");
			}
			return View(account);
		}

		public ActionResult Delete(Guid? accountId) {
			if (accountId == null) {
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			var account = _accountRepository.GetById((Guid) accountId);
			if (account == null) {
				return HttpNotFound();
			}
			account.RowState = (int) RowState.Deleted;
			_accountRepository.Save(account.Id, account.Name, account.ParentAccountId, account.CurrencyId, account.IconPath,
				account.RowState);
			TempData["message"] = $"{account.Name} has been deleted";
			return RedirectToAction("List");
		}

		#endregion

	}

	#endregion

}