namespace MyWallet.Banking.Model {

	#region Class: BaseDetermineRule

	/// <summary>
	/// Represent base rule for determine object by pattern.
	/// </summary>
	public class BaseDetermineRule {

		#region Properties: Public

		/// <summary>
		/// Gets or sets the identifier of the object.
		/// </summary>
		/// <value>The name of the object.</value>
		public string ObjectId { get; set; }

		/// <summary>
		/// Gets or sets the find pattern.
		/// </summary>
		/// <value>The pattern.</value>
		public string Pattern { get; set; }

		/// <summary>
		/// Gets or sets the description of the object.
		/// </summary>
		/// <value>The name of the object.</value>
		public string Description { get; set; }

		#endregion

	}

	#endregion

}
