using System.Security.Cryptography;
using System.Text;

namespace MyWallet.Domain.Entities {
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	#region Class: Transaction

	/// <inheritdoc />
	/// <summary>
	/// Class represent transaction.
	/// </summary>
	/// <seealso cref="T:MyWallet.Domain.Entities.BaseEntity" />
	public class Transaction : BaseEntity {

		#region Properties: Public

		/// <summary>
		/// Gets or sets the amount of the transaction.
		/// </summary>
		/// <value>
		/// The amount of the transaction.
		/// </value>
		public decimal Amount { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string Comment { get; set; }

		/// <summary>
		/// Gets or sets the category identifier.
		/// </summary>
		/// <value>
		/// The category identifier.
		/// </value>
		public Guid CategoryId { get; set; }

		/// <summary>
		/// Gets or sets the category.
		/// </summary>
		/// <value>
		/// The category.
		/// </value>
		public virtual Category Category { get; set; }

		/// <summary>
		/// Gets or sets the account identifier.
		/// </summary>
		/// <value>
		/// The account identifier.
		/// </value>
		public Guid AccountId { get; set; }

		/// <summary>
		/// Gets or sets the account.
		/// </summary>
		/// <value>
		/// The account.
		/// </value>
		public virtual Account Account { get; set; }

		/// <summary>
		/// Gets or sets date of transaction.
		/// </summary>
		/// <value>
		/// The date of transaction.
		/// </value>
		[NotMapped]
		public DateTime DateIn {
			get { return CreatedOn; }
			set { CreatedOn = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether transaction need be confirmed.
		/// </summary>
		/// <value>
		///   <c>true</c> if transaction need confirm; otherwise, <c>false</c>.
		/// </value>
		public bool NeedConfirm { get; set; }

		/// <summary>
		/// Gets or sets the hash of transaction.
		/// </summary>
		/// <value>
		/// The hash of transaction.
		/// </value>
		[NotMapped]
		public Guid Hash {
			get {
				var str = $"{Amount}{DateIn:u}{Comment}";
				using (var md = MD5.Create()) {
					var hash = md.ComputeHash(Encoding.Default.GetBytes(str));
					return new Guid(hash);
				}
			}
		}

		#endregion

	}

	#endregion

}
