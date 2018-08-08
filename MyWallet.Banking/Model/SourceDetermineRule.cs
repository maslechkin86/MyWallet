namespace MyWallet.Banking.Model
{
	using Common;

	/// <summary>
	/// Represent base rule for determine source of transaction.
	/// </summary>
	internal class SourceDetermineRule : BaseDetermineRule
	{

		#region Constructors: Public

		/// <summary>
		/// Initializes a new instance of the <see cref="SourceDetermineRule"/> class.
		/// </summary>
		/// <param name="sectionName">Name of the section in configuration file.</param>
		public SourceDetermineRule(string sectionName) {
			var creditCardConfig = ConfigurationHelper.GetConfigSection(sectionName);
			Pattern = creditCardConfig["Pattern"];
			ObjectId = creditCardConfig["ObjectId"];
			Description = creditCardConfig["Description"];
		}

		#endregion

	}
}