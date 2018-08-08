namespace MyWallet.Domain.Concrete
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Abstract;
	using Entities;
	using Type;

	#region Class: CategoryRepository

	/// <inheritdoc />
	/// <summary>
	/// Represents database abstraction layer for work with category
	/// entity <see cref="T:MyWallet.Domain.Entities.Category" />.
	/// </summary>
	/// <seealso cref="T:MyWallet.Domain.Abstract.ICategoryRepository" />
	public class CategoryRepository : ICategoryRepository
	{

		#region Fields: Private

		private readonly IMyWalletDbContext _context;

		private bool _disposed;

		#endregion

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="CategoryRepository"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public CategoryRepository(IMyWalletDbContext context) {
			_context = context;
		}

		#endregion

		#region Methods: Protected

		protected virtual void Dispose(bool disposing) {
			if (!_disposed) {
				if (disposing) {
					_context.Dispose();
				}
			}
			_disposed = true;
		}

		#endregion

		#region Methods: Public

		/// <inheritdoc />
		/// <summary>
		/// Gets all collection of categories.
		/// </summary>
		/// <returns>Collection of categories.</returns>
		public IEnumerable<Category> GetAll() {
			return _context.Categories.Where(x => x.RowState == (int)RowState.Existing);
		}

		/// <inheritdoc />
		/// <summary>
		/// Gets the category by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Instance of the <see cref="T:MyWallet.Domain.Entities.Category" /> class.</returns>
		public Category GetById(Guid id) {
			return _context.Categories.SingleOrDefault(x => x.Id == id);
		}

		/// <inheritdoc />
		/// <summary>
		/// Saves the specified category.
		/// </summary>
		/// <param name="categoryId">The category identifier.</param>
		/// <param name="name">The name.</param>
		/// <param name="directionType">The direction type.</param>
		/// <param name="iconPath">The icon path.</param>
		/// <param name="rowState">State of the row.</param>
		public void Save(Guid categoryId, string name, int directionType, string iconPath, byte rowState) {
			if(categoryId == Guid.Empty) {
				categoryId = Guid.NewGuid();
				var transaction = new Category {
					Id = categoryId,
					IconPath = iconPath,
					Name = name,
					DirectionTypeId = directionType,
					CreatedOn = DateTime.Now,
					RowState = rowState
				};
				_context.Categories.Add(transaction);
			} else {
				var dbEntry = _context.Categories.SingleOrDefault(x => x.Id == categoryId);
				if(dbEntry != null) {
					dbEntry.Name = name;
					dbEntry.DirectionTypeId = directionType;
					dbEntry.IconPath = iconPath;
					dbEntry.RowState = rowState;
					dbEntry.ModifiedOn = DateTime.Now;
				}
			}
			_context.SaveChanges();
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

	}

	#endregion
}
