using System.ComponentModel.DataAnnotations.Schema;

namespace MyWallet.Domain.Entities
{
	using System;
	using Type;

	#region Class: Category

	/// <inheritdoc />
	/// <summary>
	/// Class represent money category.
	/// </summary>
	/// <seealso cref="T:MyWallet.Domain.Entities.BaseEntity" />
	public class Category : BaseLookup
	{

		#region Fields: Private

		private int _directionTypeId;

		private DirectionType _directionType;

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets the category name.
		/// </summary>
		/// <value>The category name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the direction.
		/// </summary>
		/// <value>The type of the direction.</value>
		public int DirectionTypeId {
			get { return _directionTypeId; }
			set {
				_directionTypeId = value;
				if (Enum.IsDefined(typeof(DirectionType), _directionTypeId)) {
					_directionType = (DirectionType)_directionTypeId;
				} else {
					_directionType = DirectionType.Expense;
				}
			}
		}
		/// <summary>
		/// Gets or sets the type of the direction.
		/// </summary>
		/// <value>The type of the direction.</value>
		[NotMapped]
		public DirectionType DirectionType {
			get { return _directionType; }
			set {
				_directionType = value;
				_directionTypeId = (byte)_directionType;
			}
		}

		#endregion

	}

	#endregion

}