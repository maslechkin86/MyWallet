namespace MyWallet.Domain.Abstract {
	using System;
	using System.Collections.Generic;
	using Entities;

	#region Interface: ICategoryRepository

	/// <summary>
	/// Represents database abstraction layer for work with category
	/// entity <see cref="Category"/>.
	/// </summary>
	public interface ICategoryRepository {

		#region Methods: Public

		/// <summary>
		/// Gets all collection of categories.
		/// </summary>
		/// <returns>Collection of categories.</returns>
		IEnumerable<Category> GetAll();

		/// <summary>
		/// Gets the category by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>Instance of the <see cref="Category"/> class.</returns>
		Category GetById(Guid id);

		/// <summary>
		/// Saves the specified category.
		/// </summary>
		/// <param name="categoryId">The category identifier.</param>
		/// <param name="name">The name.</param>
		/// <param name="directionType">The direction type.</param>
		/// <param name="iconPath">The icon path.</param>
		/// <param name="rowState">State of the row.</param>
		void Save(Guid categoryId, string name, int directionType, string iconPath, byte rowState);

		#endregion

	}

	#endregion

}
