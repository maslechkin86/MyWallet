namespace MyWallet.Banking.Model {
	using System;
	using Domain.Type;

	#region Class: ReadTransaction

	[Serializable]
	public class ReadTransaction : ICloneable {

		#region Properties: Public

		/// <summary>
		/// Gets or sets the transaction date.
		/// </summary>
		/// <value>The transaction date.</value>
		public DateTime DateIn { get; set; }

		/// <summary>
		/// Gets or sets the transaction comment.
		/// </summary>
		/// <value>The transaction comment.</value>
		public string Comment { get; set; }

		/// <summary>
		/// Gets or sets the transaction amount.
		/// </summary>
		/// <value>The transaction amount.</value>
		public decimal Amount { get; set; }

		/// <summary>
		/// Gets or sets the type of the direction.
		/// </summary>
		/// <value>The type of the direction.</value>
		public DirectionType DirectionType { get; set; }

		/// <summary>
		/// Категория транзакции
		/// </summary>
		public string Category { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is transfer.
		/// </summary>
		/// <value><c>true</c> if this instance is transfer; otherwise, <c>false</c>.</value>
		public bool IsTransfer { get; set; }

		#endregion

		#region Methods: Public

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone() {
			return new ReadTransaction {
				Amount = Amount,
				Comment = Comment,
				DateIn = DateIn,
				DirectionType = DirectionType,
				Category = Category
			};
		}

		#endregion

	}

	#endregion

}
