namespace MyWallet.Domain.Entities
{
	using System;
	using System.ComponentModel.DataAnnotations;

	#region Class: BaseEntity

	/// <summary>
	/// Class represent base entity.
	/// </summary>
	[Serializable]
	public class BaseEntity
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseEntity"/> class.
		/// </summary>
		public BaseEntity() {
			Id = Guid.NewGuid();
			CreatedOn = DateTime.Now;
			ModifiedOn = DateTime.Now;
			RowState = (int)Domain.Type.RowState.Existing;
		}

		#endregion

		#region Properties: Public

		/// <summary>
		/// Gets or sets the unique identifier.
		/// </summary>
		/// <value>
		/// The unique identifier.
		/// </value>
		[Key]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the state of the record <see cref="Domain.Type.RowState"/>.
		/// </summary>
		/// <value>
		/// The state of the record <see cref="Domain.Type.RowState"/>.
		/// </value>
		public byte RowState { get; set; }

		/// <summary>
		/// Gets or sets the created on date/time.
		/// </summary>
		/// <value>
		/// The created on date/time.
		/// </value>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the modified on date/time.
		/// </summary>
		/// <value>
		/// The modified on date/time.
		/// </value>
		public DateTime ModifiedOn { get; set; }

		#endregion

	}

	#endregion

}